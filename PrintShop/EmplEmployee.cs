/*TASK RP. EMPLOYEE*/
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.JsonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowaStandard;
using Odyssey2Backend.XJDF;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.Job;
using Microsoft.AspNetCore.SignalR;
using Odyssey2Backend.Alert;
using Odyssey2Backend.Customer;
using System.Text.Json;
using Odyssey2Backend.Process.SRP;
//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 22, 2020.

namespace Odyssey2Backend.PrintShop
{
    //==================================================================================================================
    public class EmplEmployee
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetFinalStart(
            //                                              //Update the period, add the final start date and time.
            //                                              //Validate if there are no dependencies.
            //                                              //If it is the first period that start for a process, the
            //                                              //      process should be mark as started.

            int intPkPeriod_I,
            String strPrintshopId_I,
            int intContactId_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            ref bool boolAskEmailNeedsToBeSent_IO,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            PerentityPeriodEntityDB perentity = context.Period.FirstOrDefault(per => per.intPk == intPkPeriod_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "The period was not found in the database.";
            if (
                perentity != null
                )
            {
                bool boolIsAbleToStart;
                bool boolIsAbleToEnd;
                EmplEmployee.subGetBoolIsAbleToStartOrEnd(perentity, out boolIsAbleToStart, out boolIsAbleToEnd,
                    ref strUserMessage_IO);

                intStatus_IO = 402;
                strDevMessage_IO = "";
                if (
                    boolIsAbleToStart
                    )
                {
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                    int intPkProcessInWorkflow = context.ProcessInWorkflow.FirstOrDefault(piw =>
                        piw.intPkWorkflow == perentity.intPkWorkflow &&
                        piw.intProcessInWorkflowId == perentity.intProcessInWorkflowId).intPk;

                    PiwjentityProcessInWorkflowForAJobEntityDB piwjentity =
                        context.ProcessInWorkflowForAJob.FirstOrDefault(piwj =>
                        piwj.intPkProcessInWorkflow == intPkProcessInWorkflow &&
                        piwj.intPkPrintshop == ps.intPk &&
                        piwj.intJobId == perentity.intJobId);

                    //                                      //Update final start date and final start time.
                    perentity.strFinalStartDate = Date.Now(ZonedTimeTools.timezone).ToString();
                    perentity.strFinalStartTime = Time.Now(ZonedTimeTools.timezone).ToString();
                    context.Period.Update(perentity);

                    if (
                        //                                  //ProcessInWorkflow has not been started.
                        piwjentity == null
                        )
                    {
                        int intStage = JobJob.intProcessInWorkflowStarted;
                        JobJob.subUpdateProcessStage(ps, intContactId_I, intPkProcessInWorkflow, perentity.intJobId, intStage,
                            context, configuration_I, iHubContext_I, ref boolAskEmailNeedsToBeSent_IO,
                            ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                    }

                    //                                      //Find alert type related to periods.
                    AlerttypeentityAlertTypeEntityDB alerttypeentity = context.AlertType.FirstOrDefault(alerttype =>
                        alerttype.strType == AlerttypeentityAlertTypeEntityDB.strPeriod);
                    //                                      //Find alert.
                    AlertentityAlertEntityDB alertentity = context.Alert.FirstOrDefault(alert =>
                        alert.intnPkPeriod == perentity.intPk && alert.intPkAlertType == alerttypeentity.intPk);
                    if (
                        alertentity != null
                        )
                    {
                        //                                  //Delete alert.
                        context.Alert.Remove(alertentity);
                    }

                    context.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetFinalEnd(
            //                                              //Update the period, add the final end date and time.
            //                                              //When all periods of a process are finished, the process is
            //                                              //      is marked as completed.

            int intPkPeriod_I,
            String strPrintshopId_I,
            int intContactId_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            ref bool boolAskEmailNeedsToBeSent_IO,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            PerentityPeriodEntityDB perentity = context.Period.FirstOrDefault(per => per.intPk == intPkPeriod_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "The period was not found in the database.";
            if (
                perentity != null
                )
            {
                bool boolIsAbleToStart;
                bool boolIsAbleToEnd;
                EmplEmployee.subGetBoolIsAbleToStartOrEnd(perentity, out boolIsAbleToStart, out boolIsAbleToEnd,
                    ref strUserMessage_IO);

                intStatus_IO = 402;
                strDevMessage_IO = "";
                if (
                    boolIsAbleToEnd
                    )
                {
                    //                                      //Update final end date and final end time.
                    perentity.strFinalEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                    perentity.strFinalEndTime = Time.Now(ZonedTimeTools.timezone).ToString();

                    context.Period.Update(perentity);
                    context.SaveChanges();

                    //                                      //Get not finished periods for procces.
                    List<PerentityPeriodEntityDB> darrperentityForProcess = context.Period.Where(per =>
                        per.intPkWorkflow == perentity.intPkWorkflow &&
                        per.intProcessInWorkflowId == perentity.intProcessInWorkflowId &&
                        per.intJobId == perentity.intJobId &&
                        per.strFinalEndDate == null &&
                        per.strFinalEndTime == null &&
                        per.intPk != perentity.intPk).ToList();

                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                    int intPkProcessInWorkflow = context.ProcessInWorkflow.FirstOrDefault(piw =>
                        piw.intPkWorkflow == perentity.intPkWorkflow &&
                        piw.intProcessInWorkflowId == perentity.intProcessInWorkflowId).intPk;

                    if (
                        //                                  //All periods are completed.
                        darrperentityForProcess.Count == 0
                        )
                    {
                        int intStage = JobJob.intProcessInWorkflowCompleted;
                        JobJob.subUpdateProcessStage(ps, intContactId_I, intPkProcessInWorkflow, perentity.intJobId,
                            intStage, context, configuration_I, iHubContext_I, ref boolAskEmailNeedsToBeSent_IO,
                            ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                    }

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetTask(
           //                                              //Method to add or edit a task in the DB.

           //                                              //Data to add.
           int? intnPkTask_I,
           String strDescription_I,
           String strStartDate_I,
           String strStartTime_I,
           String strEndDate_I,
           String strEndTime_I,
           int intContactId_I,
           String strPrintshopId_I,
           int intMinutesForNotification_I,
           bool boolIsNotifiedable_I,
           int? intnCustomerId_I,
           IHubContext<ConnectionHub> iHubContext_I,
           ref int intStatus_IO,
           ref String strUserMessage_IO,
           ref String strDevMessage_IO
           )
        {
            //                              //Find PkPrintshop.
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
            int intPkPrintshop = ps.intPk;

            ZonedTime ztimeStartTask = new ZonedTime();
            ZonedTime ztimeEndTask = new ZonedTime();

            intStatus_IO = 401;
            strUserMessage_IO = "Add a description.";
            strDevMessage_IO = "";
            if (
                strDescription_I.Length > 0
                )
            {
                intStatus_IO = 402;
                if (
                    ZonedTimeTools.boolIsValidStartDateTimeAndEndDateTime(strStartDate_I, strStartTime_I, strEndDate_I,
                        strEndTime_I, ps.strTimeZone, out ztimeStartTask, out ztimeEndTask, ref strUserMessage_IO,
                        ref strDevMessage_IO)
                    )
                {
                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Minutes for notification cannot be less to 0.";
                    if (
                        intMinutesForNotification_I >= 0
                        )
                    {
                        //                                  //Task start time consering minutes before notification.
                        ZonedTime ztimeStartTaskWithNotif = ztimeStartTask - (intMinutesForNotification_I * 60000);

                        //                                  //Create ztime now.
                        ZonedTime ztimeDateNow = ZonedTimeTools.NewZonedTime(Date.Now(ZonedTimeTools.timezone),
                                Time.Now(ZonedTimeTools.timezone));

                        intStatus_IO = 404;
                        strUserMessage_IO = "Date is in the past considering minutes for notification.";
                        strDevMessage_IO = "";
                        if (
                            boolIsNotifiedable_I &&
                            ztimeStartTaskWithNotif > ztimeDateNow ||
                            !boolIsNotifiedable_I
                            )
                        {
                            //                              //Establish connection.
                            Odyssey2Context context = new Odyssey2Context();

                            //                              //Find task.
                            TaskentityTaskEntityDB taskentity = context.Task.FirstOrDefault(task =>
                                task.intPk == intnPkTask_I);

                            //                              //Start time divided in hours, minutes and seconds.
                            String strStartHour = ztimeStartTask.Time.ToString();
                            strStartHour = strStartHour.Substring(0, 2);
                            String strStartMinute = ztimeStartTask.Time.ToString();
                            strStartMinute = strStartMinute.Substring(3, 2);
                            String strStartSecond = ztimeStartTask.Time.ToString();
                            strStartSecond = strStartSecond.Substring(6, 2);

                            if (
                                taskentity != null
                                )
                            {
                                intStatus_IO = 405;
                                strUserMessage_IO = "Cannot edit this task.";
                                strDevMessage_IO = "";
                                if (
                                    taskentity.intContactId == intContactId_I
                                    )
                                {
                                    intStatus_IO = 406;
                                    strUserMessage_IO = "Something is wrong.";
                                    strDevMessage_IO = "Cannot edit a completed task.";
                                    if (
                                        taskentity.boolIsCompleted == false
                                        )
                                    {
                                        taskentity.strDescription = strDescription_I;
                                        taskentity.strStartDate = ztimeStartTask.Date.ToString();
                                        taskentity.strStartHour = strStartHour;
                                        taskentity.strStartMinute = strStartMinute;
                                        taskentity.strStartSecond = strStartSecond;
                                        taskentity.strEndDate = ztimeEndTask.Date.ToString();
                                        taskentity.strEndTime = ztimeEndTask.Time.ToString();
                                        taskentity.intContactId = intContactId_I;
                                        taskentity.intMinsBeforeNotify = intMinutesForNotification_I;
                                        taskentity.boolIsNotifiedable = boolIsNotifiedable_I;
                                        taskentity.intnCustomerId = intnCustomerId_I;

                                        //                  //Calculate time before notification.
                                        String strNotificationDate;
                                        String strNotificationTime;
                                        EmplEmployee.subCalculateTimeBeforeNotify(ztimeStartTask, boolIsNotifiedable_I,
                                            intMinutesForNotification_I, out strNotificationDate, 
                                            out strNotificationTime);
                                        taskentity.strNotificationDate = strNotificationDate;

                                        if (
                                            strNotificationTime != null
                                            )
                                        {
                                            taskentity.strNotificationHour = strNotificationTime.Substring(0, 2);
                                            taskentity.strNotificationMinute = strNotificationTime.Substring(3, 2);
                                        }
                                        else
                                        {
                                            taskentity.strNotificationHour = null;
                                            taskentity.strNotificationMinute = null;
                                        }

                                        //                  //Find alert type related to tasks.
                                        AlerttypeentityAlertTypeEntityDB alerttypeentity =
                                            context.AlertType.FirstOrDefault(alerttype =>
                                            alerttype.strType == AlerttypeentityAlertTypeEntityDB.strTask);

                                        //                  //Find alert.
                                        AlertentityAlertEntityDB alertentity = context.Alert.FirstOrDefault(alert =>
                                            alert.intnPkTask == taskentity.intPk &&
                                            alert.intPkAlertType == alerttypeentity.intPk);

                                        if (
                                            alertentity != null
                                            )
                                        {
                                            if (
                                                //          //Notification not read by contact.
                                                !PsPrintShop.boolNotificationReadByUser(alertentity, intContactId_I)
                                                )
                                            {
                                                //          //Support method to reduce notifications in 
                                                //          //      front.
                                                AlnotAlertNotification.subReduceToOne(taskentity.intContactId,
                                                    iHubContext_I);
                                            }

                                            context.Alert.Remove(alertentity);
                                        }

                                        context.Task.Update(taskentity);
                                        context.SaveChanges();

                                        intStatus_IO = 200;
                                        strUserMessage_IO = "";
                                        strDevMessage_IO = "";
                                    }
                                }
                            }
                            else
                            {
                                taskentity = new TaskentityTaskEntityDB
                                {
                                    strDescription = strDescription_I,
                                    strStartDate = ztimeStartTask.Date.ToString(),
                                    strStartHour = strStartHour,
                                    strStartMinute = strStartMinute,
                                    strStartSecond = strStartSecond,
                                    strEndDate = ztimeEndTask.Date.ToString(),
                                    strEndTime = ztimeEndTask.Time.ToString(),
                                    intContactId = intContactId_I,
                                    intMinsBeforeNotify = intMinutesForNotification_I,
                                    boolIsNotifiedable = boolIsNotifiedable_I,
                                    intPkPrintshop = intPkPrintshop,
                                    intnCustomerId = intnCustomerId_I
                                };

                                //                          //Calculate time before notification.
                                String strNotificationDate;
                                String strNotificationTime;
                                EmplEmployee.subCalculateTimeBeforeNotify(ztimeStartTask, boolIsNotifiedable_I,
                                    intMinutesForNotification_I, out strNotificationDate, out strNotificationTime);
                                taskentity.strNotificationDate = strNotificationDate;

                                if (
                                    strNotificationTime != null
                                    )
                                {
                                    taskentity.strNotificationHour = strNotificationTime.Substring(0, 2);
                                    taskentity.strNotificationMinute = strNotificationTime.Substring(3, 2);
                                }
                                else
                                {
                                    taskentity.strNotificationHour = null;
                                    taskentity.strNotificationMinute = null;
                                }

                                context.Task.Add(taskentity);
                                context.SaveChanges();

                                intStatus_IO = 200;
                                strUserMessage_IO = "";
                                strDevMessage_IO = "";
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subCalculateTimeBeforeNotify(
            //                                              //Calculates new date and time when there's a value for 
            //                                              //      minutesfornotification.

            ZonedTime ztimeStartTask_I,
            bool boolIsNotifiedable_I,
            int intMinutesForNotification_I,
            out String strNotificationDate_O,
            out String strNotificationTime_O
            )
        {
            strNotificationDate_O = null;
            strNotificationTime_O = null;

            if (
                boolIsNotifiedable_I
                )
            {
                if (
                    //                          //There are not minutes before notify.
                    intMinutesForNotification_I == 0
                    )
                {
                    //                          //Task notification date and time remains the same than
                    //                          //      task's.

                    strNotificationDate_O = ztimeStartTask_I.Date.ToString();

                    //                          //Consider only hours and minutes.
                    strNotificationTime_O = ztimeStartTask_I.Time.ToString();
                    strNotificationTime_O = strNotificationTime_O.Substring(0, 5);
                }
                else
                {
                    //                          //User added minutes before notify.

                    //                          //Calculate notification time considering minutes 
                    //                          //      before notify.
                    int intMiliseconds = intMinutesForNotification_I * 60000;
                    ztimeStartTask_I = ztimeStartTask_I - intMiliseconds;

                    strNotificationDate_O = ztimeStartTask_I.Date.ToString();

                    //                          //Consider only hours and minutes.
                    strNotificationTime_O = ztimeStartTask_I.Time.ToString();
                    strNotificationTime_O = strNotificationTime_O.Substring(0, 5);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeleteTask(
            //                                              //Deletes one task from DB.
            //                                              //Only the owner of the task is allowed to delete it.

            //                                              //Connection object that will reduce the notification.
            IHubContext<ConnectionHub> iHubContext_I,
            int intPkTask_I,
            String strPrintshopId_I,
            int intContactId_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Invalid employee.";
            if (
                ResResource.boolEmployeeOrOwnerIsFromPrintshop(strPrintshopId_I, intContactId_I)
                )
            {
                //                                          //Find task.
                TaskentityTaskEntityDB taskentity = context.Task.FirstOrDefault(task => (task.intPk == intPkTask_I) &&
                    (task.intContactId == intContactId_I));

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Task not found or not allowed to delete it.";
                if (
                    taskentity != null &&
                    taskentity.boolIsCompleted == false
                    )
                {
                    //                                      //Find alert type related to tasks.
                    AlerttypeentityAlertTypeEntityDB alerttypeentity = context.AlertType.FirstOrDefault(
                        alerttypeentity => alerttypeentity.strType == AlerttypeentityAlertTypeEntityDB.strTask);

                    //                                      //Find alert.
                    AlertentityAlertEntityDB alertentity = context.Alert.FirstOrDefault(alert =>
                        alert.intnPkTask == taskentity.intPk && alert.intPkAlertType == alerttypeentity.intPk);

                    if (
                        alertentity != null
                        )
                    {
                        if (
                            //                              //Notification not read by user.
                            !PsPrintShop.boolNotificationReadByUser(alertentity, intContactId_I)
                            )
                        {
                            //                              //Support method to reduce notifications in 
                            //                              //      front.
                            AlnotAlertNotification.subReduceToOne(taskentity.intContactId,
                                iHubContext_I);
                        }

                        context.Alert.Remove(alertentity);
                    }

                    context.Task.Remove(taskentity);
                    context.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subCompleteTask(
            //                                              //Complete one task from DB.
            //                                              //Only the owner of the task is allowed to complete it.
            //                                              //Delete task's alert if exists.

            //                                              //Connection object that will reduce the notification.
            IHubContext<ConnectionHub> iHubContext_I,
            int intPkTask_I,
            String strPrintshopId_I,
            int intContactId_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Invalid employee.";
            if (
                ResResource.boolEmployeeOrOwnerIsFromPrintshop(strPrintshopId_I, intContactId_I)
                )
            {
                //                                          //Find task.
                TaskentityTaskEntityDB taskentity = context.Task.FirstOrDefault(task => (task.intPk == intPkTask_I) &&
                    (task.intContactId == intContactId_I));

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Task not found or not allowed to complete it.";
                if (
                    taskentity != null
                    )
                {
                    intStatus_IO = 403;
                    strUserMessage_IO = "Cannot complete a task already completed.";
                    strDevMessage_IO = "";
                    if (
                        taskentity.boolIsCompleted == false
                        )
                    {
                        //                                  //Remove alert if it exists.

                        //                                  //Find alert type related to tasks.
                        AlerttypeentityAlertTypeEntityDB alerttypeentity =
                            context.AlertType.FirstOrDefault(alerttype =>
                            alerttype.strType == AlerttypeentityAlertTypeEntityDB.strTask);

                        //                                  //Find alert.
                        AlertentityAlertEntityDB alertentity = context.Alert.FirstOrDefault(alert =>
                            alert.intnPkTask == taskentity.intPk && alert.intPkAlertType == alerttypeentity.intPk);
                        if (
                            alertentity != null
                            )
                        {
                            if (
                                //                          //Notification not read by user.
                                !PsPrintShop.boolNotificationReadByUser(alertentity, intContactId_I)
                                )
                            {
                                //                          //Support method to reduce notifications in 
                                //                          //      front.
                                AlnotAlertNotification.subReduceToOne(taskentity.intContactId, iHubContext_I);
                            }

                            context.Alert.Remove(alertentity);
                        }

                        //                                  //Complete the task.
                        taskentity.boolIsCompleted = true;
                        context.Task.Update(taskentity);

                        context.SaveChanges();

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetRole(
            //                                              //Add or remove an employee as supervisor or accountant.

            String strPrintshopId_I,
            int intContactId_I,
            bool? boolnIsSupervisor_I,
            bool? boolnIsAccountant_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            intStatus_IO = 402;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Invalid employee.";
            if (
                ResResource.boolEmployeeIsFromPrintshop(strPrintshopId_I, intContactId_I)
                )
            {
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                int intPkPrintshop = ps.intPk;

                //                                          //Find a supervisor employee.
                RolentityRoleEntityDB roleentity = context.Role.FirstOrDefault(role =>
                    role.intContactId == intContactId_I && role.intPkPrintshop == intPkPrintshop);

                if (
                    //                                      //The employee has role current.
                    roleentity != null
                    )
                {
                    UpdateRole.subUpdate(boolnIsSupervisor_I, boolnIsAccountant_I, roleentity, context);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
                else
                {
                    if (
                        boolnIsAccountant_I == false ||
                        boolnIsSupervisor_I == false
                        )
                    {
                        intStatus_IO = 403;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "it can not add new record with supervisor or accountant in false.";
                    }
                    else
                    {
                        //                                  //Add employee as supervisor or accountant.
                        SaveEmployeeToRoleTable.subSave(intContactId_I, intPkPrintshop, boolnIsSupervisor_I,
                            boolnIsAccountant_I, context);

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetDay(
            //                                              //Find periods and tasks related to an employee on a 
            //                                              //      specific date.

            String strPrintshopId_I,
            int intContactId_I,
            //                                              //Date to search for.
            String strDay_I,
            IConfiguration configuration_I,
            out Dayjson2DayJson2 dayjson2_O,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            dayjson2_O = null;

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No employee found.";
            if (
                ResResource.boolEmployeeOrOwnerIsFromPrintshop(strPrintshopId_I, intContactId_I)
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                //                                          //Find periods excluding temporary ones.
                List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per =>
                    per.intnContactId == intContactId_I && per.intnEstimateId == null).ToList();

                //                                          //List that will hold periods and tasks.
                List<PerortaskjsonPeriodOrTaskJson> darrperortaskjson2 = new List<PerortaskjsonPeriodOrTaskJson>();

                Date date = strDay_I.ParseToDate();

                foreach (PerentityPeriodEntityDB perentity in darrperentity)
                {
                    ZonedTime ztimeStartPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        perentity.strStartDate.ParseToDate(), perentity.strStartTime.ParseToTime(), ps.strTimeZone);

                    ZonedTime ztimeEndPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        perentity.strEndDate.ParseToDate(), perentity.strEndTime.ParseToTime(), ps.strTimeZone);

                    if (
                        //                                  //Starts in the date.
                        (ztimeStartPeriod.Date == date) ||
                        //                                  //Ends in the date.
                        ((ztimeEndPeriod.Date == date) && (ztimeEndPeriod.Time > Time.MinValue)) ||
                        //                                  //The period is over the date.
                        ((ztimeStartPeriod.Date < date) && (ztimeEndPeriod.Date > date))
                        )
                    {
                        bool boolIsByResource = (perentity.intnPkElementElementType != null) ||
                            (perentity.intnPkElementElement != null);

                        bool boolIsAbleToStart;
                        bool boolIsAbleToEnd;
                        EmplEmployee.subGetBoolIsAbleToStartOrEnd(perentity, out boolIsAbleToStart,
                            out boolIsAbleToEnd, ref strUserMessage_IO);

                        PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(
                            piw => piw.intPkWorkflow == perentity.intPkWorkflow &&
                            piw.intProcessInWorkflowId == perentity.intProcessInWorkflowId);

                        ProProcess pro = ProProcess.proFromDB(piwentity.intPkProcess);

                        String strProcessName = pro.strName + ((piwentity.intnId != null) ?
                            ("(" + piwentity.intnId + ")") : "");

                        String strA = "", strB = "";
                        JobjsonJobJson jobjson;
                        if (
                            JobJob.boolIsValidJobId(perentity.intJobId, strPrintshopId_I, configuration_I, out jobjson,
                            ref strA, ref strB)                            
                            )
                        {
                            //                                  //Get strJobNumber
                            String strJobNumber = JobJob.strGetJobNumber(null, perentity.intJobId, strPrintshopId_I,
                                context);

                            //                                  //Period is completed.
                            bool boolPeriodCompleted = perentity.strEndDate != null ? true : false;

                            PerortaskjsonPeriodOrTaskJson perortaskjson2 = new PerortaskjsonPeriodOrTaskJson(
                                perentity.intPk, null, ztimeStartPeriod.Date.ToString(), 
                                ztimeStartPeriod.Time.ToString(), ztimeEndPeriod.Date.ToString(), 
                                ztimeEndPeriod.Time.ToString(), perentity.intJobId + "", strJobNumber,
                                boolIsByResource, boolIsAbleToStart, boolIsAbleToEnd, perentity.intMinsBeforeDelete,
                                strProcessName, null, 0, false, boolPeriodCompleted, jobjson.strJobTicket, null, null);

                            darrperortaskjson2.Add(perortaskjson2);
                        }                        
                    }
                }

                //                                          //Find tasks.
                List<TaskentityTaskEntityDB> darrtaskentity = context.Task.Where(task =>
                    task.intContactId == intContactId_I).ToList();

                //                                          //Customer info if exists.
                CusjsonCustomerJson cusjson;
                String strCustomerName = "";
                String strCustomerLastName = "";

                foreach (TaskentityTaskEntityDB taskentity in darrtaskentity)
                {
                    String strStartTime = taskentity.strStartHour + ":" + taskentity.strStartMinute + ":" +
                            taskentity.strStartSecond;
                    //                                      //To easy code.
                    ZonedTime ztimeStartTask =  ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        taskentity.strStartDate.ParseToDate(), strStartTime.ParseToTime(), ps.strTimeZone);

                    ZonedTime ztimeEndTask = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        taskentity.strEndDate.ParseToDate(), taskentity.strEndTime.ParseToTime(), ps.strTimeZone);

                    if (
                        //                                  //Starts in the date.
                        (ztimeStartTask.Date == date) ||
                        //                                  //Ends in the date.
                        ((ztimeEndTask.Date == date) && (ztimeEndTask.Time > Time.MinValue)) ||
                        //                                  //The period is over the date.
                        ((ztimeStartTask.Date < date) && (ztimeEndTask.Date > date))
                        )
                    {
                        if (
                            //                              //Task related to a customer.
                            taskentity.intnCustomerId != null
                            )
                        {
                            //                              //Find customer.
                            CusCustomer.subGetOneCustomerFromPrintshop(ps, (int)taskentity.intnCustomerId,
                                out cusjson, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                            if (
                                //                          //Customer data.
                                cusjson != null
                                )
                            {
                                strCustomerName = cusjson.strFirstName;
                                strCustomerLastName = cusjson.strLastName;
                            }
                        }

                        PerortaskjsonPeriodOrTaskJson perortaskjson2 = new PerortaskjsonPeriodOrTaskJson(null,
                            taskentity.intPk, ztimeStartTask.Date.ToString(), ztimeStartTask.Time.ToString(),
                            ztimeEndTask.Date.ToString(), ztimeEndTask.Time.ToString(), null, "", false, false, false,
                            0, null, taskentity.strDescription, taskentity.intMinsBeforeNotify,
                            taskentity.boolIsNotifiedable, taskentity.boolIsCompleted, null, strCustomerName,
                            strCustomerLastName);

                        darrperortaskjson2.Add(perortaskjson2);

                        //                                  //Reset values
                        strCustomerName = "";
                        strCustomerLastName = "";
                    }
                }

                darrperortaskjson2.Sort();

                dayjson2_O = new Dayjson2DayJson2(ResResource.arrstrWeekdays[(int)date.DayOfWeek],
                    date.ToText(), darrperortaskjson2.ToArray());

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetBoolIsAbleToStartOrEnd(
            PerentityPeriodEntityDB perentity_I,
            out bool boolIsAbleToStart_O,
            out bool boolIsAbleToEnd_O,
            ref String strUserMessage_IO
            )
        {
            boolIsAbleToStart_O = false;
            boolIsAbleToEnd_O = false;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Job must be in progress.
            JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job =>
                job.intJobID == perentity_I.intJobId);
            if (
                jobentity != null
                )
            {
                /*CASE*/
                if (
                    //                                      //Period not started.
                    perentity_I.strFinalStartDate == null &&
                    perentity_I.strFinalEndDate == null
                    )
                {
                    //                                      //Get the previous periods in current job.
                    List<PerentityPeriodEntityDB> darrperentityForJob = context.Period.Where(
                        per => per.intJobId == perentity_I.intJobId).ToList();
                    darrperentityForJob.Sort();

                    int intI = 0;
                    bool boolFinished = true;
                    /*UNTIL-DO*/
                    while (!(
                        //                                  //The next period is this period (is the first period for 
                        //                                  //      the process).
                        (darrperentityForJob[intI].intPk == perentity_I.intPk) ||
                        //                                  //The process is the process of this period.
                        (darrperentityForJob[intI].intProcessInWorkflowId == perentity_I.intProcessInWorkflowId) ||
                        //                                  //The previous period was not finished.
                        !boolFinished
                        ))
                    {
                        boolFinished = darrperentityForJob[intI].strFinalEndDate != null;

                        intI = intI + 1;
                    }

                    if (
                        //                                  //All previous periods were finished in the job.
                        boolFinished
                        )
                    {
                        //                                  //Verify if periods of the same resource have been started
                        //                                  //      and not finished yet.

                        //                                  //Get all periods related to the same resource.
                        List<PerentityPeriodEntityDB> darrperentityResource = context.Period.Where(
                            per => per.intPkElement == perentity_I.intPkElement &&
                            (per.intnPkElementElementType != null || per.intnPkElementElement != null)).ToList();

                        //                                  //Period's job id that was started but not finished.
                        int intBusyPeriodJobId = 0;

                        int intJ = 0;
                        /*WHILE-DO*/
                        while (
                            intJ < darrperentityResource.Count() &&
                            boolFinished
                            )
                        {
                            if (
                                //                          //Period started but not finished.
                                darrperentityResource[intJ].strFinalStartDate != null &&
                                darrperentityResource[intJ].strFinalEndDate == null
                                )
                            {
                                intBusyPeriodJobId = darrperentityResource[intJ].intJobId;

                                boolFinished = false;
                            }

                            intJ = intJ + 1;
                        }

                        if (
                            boolFinished == false
                            )
                        {
                            strUserMessage_IO = "This period cannot be started. Its resource is being used in job: " +
                                intBusyPeriodJobId;
                        }

                        boolIsAbleToStart_O = boolFinished;
                    }
                }
                else if (
                    //                                      //Period started but not finished
                    perentity_I.strStartDate != null &&
                    perentity_I.strFinalEndDate == null
                    )
                {
                    boolIsAbleToEnd_O = true;
                }
                /*END-CASE*/
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetAlerts(
             //                                              //Get all the alerts for the printshop and add the contactId
             //                                              //      if it does not exist in the ReadBy string.

            int intContactId_I,
            String strPrintshopId_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,            
            out AlertjsonAlertJson[] arralertjson_O,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            //                                              //Get the printshop.
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

            //                                              //Get complement description to new order or estimate.
            AlerttypeentityAlertTypeEntityDB alerttypeentityItemsToAnswer = context_M.AlertType.FirstOrDefault(
                at => at.strType == AlerttypeentityAlertTypeEntityDB.strItemsToAnswer);

            AlerttypeentityAlertTypeEntityDB alerttypeentityReadyToGo = context_M.AlertType.FirstOrDefault(
                at => at.strType == AlerttypeentityAlertTypeEntityDB.strReadyToGo);

            EmplEmployee.subCleanNotRequestedEstimatesAnyMoreAlertTable(ps_I, context_M);

            //                                              //Get all the alerts for the printshop or the specic 
            //                                              //      contact.
            List<AlertentityAlertEntityDB> darralertentity = context_M.Alert.Where(alert => 
                (alert.intPkPrintshop == ps.intPk) &&
                ((alert.intnContactId == null) || (alert.intnContactId == intContactId_I))
                ).ToList();
            darralertentity.Reverse();

            //                                              //Get all Wisnet jobs and estimates to avoid go to Wisnet
            //                                              //      to many times.
            List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfoPendingInEstimatingAndEstimates = 
                EmplEmployee.darrjobbasicinfoGetPendingInEstimatingAndEstimates(ps_I, configuration_I, context_M);

            List<AlertjsonAlertJson> darralertjson = new List<AlertjsonAlertJson>();
            foreach (AlertentityAlertEntityDB alertentity in darralertentity)
            {
                //                                          //To get who has read the alert.
                String strReadBy = (alertentity.strReadBy == null) ? "" : alertentity.strReadBy;

                if (
                //                                          //The contactId has not been added.
                !strReadBy.Contains("|" + intContactId_I)
                )
                {
                    //                                      //Add the contactId to the string.
                    strReadBy = alertentity.strReadBy + "|" + intContactId_I;

                    //                                      //Update the alert.
                    alertentity.strReadBy = strReadBy;
                    context_M.Alert.Update(alertentity);
                    context_M.SaveChanges();
                }

                //                                          //Get the type.
                AlerttypeentityAlertTypeEntityDB alerttypeentity = context_M.AlertType.FirstOrDefault(alerttype =>
                    alerttype.intPk == alertentity.intPkAlertType);

                /*CASE*/
                if (
                    //                                      //Alert related to availability.
                    alerttypeentity.strType == AlerttypeentityAlertTypeEntityDB.strAvailability
                    )
                {
                    ResResource res = ResResource.resFromDB(alertentity.intnPkResource, false);
                    res = (res == null) ? ResResource.resFromDB(alertentity.intnPkResource, true) : res;

                    //                                      //Create the alertjson.
                    AlertjsonAlertJson alertjson = new AlertjsonAlertJson(alerttypeentity.strType, res.strName +
                        alerttypeentity.strDescription, null, false, null);
                    darralertjson.Add(alertjson);
                }
                else if (
                    //                                      //Alert related to periods.
                    alerttypeentity.strType == AlerttypeentityAlertTypeEntityDB.strPeriod
                    )
                {
                    JobJob job = JobJob.jobFromDB((int)alertentity.intnJobId);

                    String strJobNumber = alertentity.intnJobId != null ? JobJob.strGetJobNumber(null,
                        (int)alertentity.intnJobId, strPrintshopId_I, context_M) : "";

                    //                                      //Create the alertjson.
                    AlertjsonAlertJson alertjson = new AlertjsonAlertJson(alerttypeentity.strType, 
                        alerttypeentity.strDescription + strJobNumber + ".", null, false, null);
                    darralertjson.Add(alertjson);
                }
                else if (
                    //                                      //Alert related to tasks.
                    alerttypeentity.strType == AlerttypeentityAlertTypeEntityDB.strTask
                    )
                {
                    //                                      //Find task.
                    TaskentityTaskEntityDB taskentity = context_M.Task.FirstOrDefault(task =>
                        task.intPk == alertentity.intnPkTask);

                    if (
                        taskentity != null
                        )
                    {
                        //                                  //Task description.
                        String strTaskDescription = taskentity.strDescription;

                        //                                  //Create the alertjson.
                        AlertjsonAlertJson alertjson = new AlertjsonAlertJson(alerttypeentity.strType,
                            alerttypeentity.strDescription + "" + strTaskDescription, null, false, null);
                        darralertjson.Add(alertjson);
                    }
                }
                else if (
                     //                                      //Alert related to new order or new estimate.
                     alerttypeentity.strType == AlerttypeentityAlertTypeEntityDB.strNewEstimate ||
                     alerttypeentity.strType == AlerttypeentityAlertTypeEntityDB.strNewOrder
                    )
                {
                    if (
                        //                                  //CUANDO ESTO SE CUMPLA, SIGNIFICA QUE ES UNA ALERTA QUE YA
                        //                                  //      EXISTIA EN LA DB ANTES DE LA NUEVA FUNCIONALIDAD
                        alertentity.strMessage == null
                        )
                    {
                        //                                  //Get customer name and job/estimate number.

                        //                                  //To know if is an alert for an order or estimate.
                        bool boolEstimate = alerttypeentity.strType ==
                            AlerttypeentityAlertTypeEntityDB.strNewEstimate ? true : false;

                        bool boolJob = boolEstimate ? false : true;
                        int intJobId = (int)alertentity.intnJobId;

                    //                                      //To get the customer's name and Order/Estimate number.
                    String[] strOrderOrEstimInfo = 
                        EmplEmployee.arrstrGetOrderIdOrEstimateNumber((int)alertentity.intnJobId, boolEstimate,
                        ps_I, darrjobbasicinfoPendingInEstimatingAndEstimates, configuration_I, context_M,
                        ref intStatus_IO, ref strDevMessage_IO, ref strUserMessage_IO);

                        if (
                            //                              //If there is not OrderNumber or EstimateNumber, means that
                            //                              //      the job is not valid in Wisnet anymore. We do not
                            //                              //      need to send this alert.
                            strOrderOrEstimInfo[1].Length > 0
                            )
                        {
                            //                              //There is a customer name, add description, othewise just
                            //                              //      an empty description.
                            String strCustomerName = strOrderOrEstimInfo[0].Length > 0 ? "from " +
                                strOrderOrEstimInfo[0] : strOrderOrEstimInfo[0];

                            //                              //Assign Order/Estimate number.
                            String strJobNumber = strOrderOrEstimInfo[1];

                            //                              //Get date.
                            String strOrderOrEstimateDate = strOrderOrEstimInfo[2].Length > 0 ? "requested at " +
                            strOrderOrEstimInfo[2] : "";

                            //                              //Assing the complement description.
                            String strDescription = alertentity.intnOtherAttributes.HasValue &&
                                alertentity.intnOtherAttributes.Value > 0 ?
                                alerttypeentity.strDescription + strJobNumber + " " + strOrderOrEstimateDate + " " +
                                strCustomerName + " " + ". " + alertentity.intnOtherAttributes.Value +
                                    alerttypeentityItemsToAnswer.strDescription :
                                alerttypeentity.strDescription + strJobNumber + " " + strOrderOrEstimateDate + " " +
                                strCustomerName + " " + alerttypeentityReadyToGo.strDescription;

                            //                              //To know if the order is an InEstimating or Pending stage.
                            bool boolInEstimating = false;
                            if (
                                //                          //Only evaluate Orders.
                                !boolEstimate
                                )
                            {
                                JobbasicinfojsonJobBasicInfoJson jobbasicinfo =
                                darrjobbasicinfoPendingInEstimatingAndEstimates.FirstOrDefault(job =>
                                job.intJobId == (int)alertentity.intnJobId);
                                boolInEstimating = (bool)jobbasicinfo.boolInEstimating;
                            }

                            //                              //Create the alertjson.
                            AlertjsonAlertJson alertjson = new AlertjsonAlertJson(alerttypeentity.strType,
                                alertentity.strMessage, boolJob, boolInEstimating, intJobId);
                            darralertjson.Add(alertjson);
                        }

                    }
                    else
                    {
                        //                                  //ALERTA CREADA CON LA NUEVA FUNCIONALIDAD.

                        if (
                        //                                  //Job still existing in wisnet.
                        darrjobbasicinfoPendingInEstimatingAndEstimates.Exists(job =>
                            job.intJobId == (int)alertentity.intnJobId)
                        )
                        {
                            //                              //To know if is an alert for an order or estimate.
                            bool boolEstimate = alerttypeentity.strType ==
                                AlerttypeentityAlertTypeEntityDB.strNewEstimate ? true : false;

                            bool boolJob = boolEstimate ? false : true;
                            int intJobId = (int)alertentity.intnJobId;

                            //                              //To know if the order is an InEstimating or Pending stage.
                            bool boolInEstimating = false;
                            if (
                                //                          //Only evaluate Orders.
                                !boolEstimate
                                )
                            {
                                JobbasicinfojsonJobBasicInfoJson jobbasicinfo =
                                darrjobbasicinfoPendingInEstimatingAndEstimates.FirstOrDefault(job =>
                                job.intJobId == (int)alertentity.intnJobId);
                                boolInEstimating = (bool)jobbasicinfo.boolInEstimating;
                            }

                            //                                  //Create the alertjson.
                            AlertjsonAlertJson alertjson = new AlertjsonAlertJson(alerttypeentity.strType,
                                alertentity.strMessage, boolJob, boolInEstimating, intJobId);
                            darralertjson.Add(alertjson);
                        }
                    }                    
                }
                else if (
                     //                                     //Alert related to due date at risk.
                     alerttypeentity.strType == AlerttypeentityAlertTypeEntityDB.strDueDateAtRisk
                    )
                {
                    String strJobNumber = alertentity.intnJobId != null ? JobJob.strGetJobNumber(null,
                        (int)alertentity.intnJobId, strPrintshopId_I, context_M) : "";

                    //                                      //Create the alertjson.
                    AlertjsonAlertJson alertjson = new AlertjsonAlertJson(alerttypeentity.strType,
                        alerttypeentity.strDescription + strJobNumber + ".", null, false, null);
                    darralertjson.Add(alertjson);
                }
                else if (
                     //                                     //Alert related to due date in the past.
                     alerttypeentity.strType == AlerttypeentityAlertTypeEntityDB.strDueDateInThePast
                    )
                {
                    String strJobNumber = alertentity.intnJobId != null ? JobJob.strGetJobNumber(null,
                        (int)alertentity.intnJobId, strPrintshopId_I, context_M) : "";

                    //                                      //Create the alertjson.
                    AlertjsonAlertJson alertjson = new AlertjsonAlertJson(alerttypeentity.strType,
                        alerttypeentity.strDescription + strJobNumber + ".", null, false, null);
                    darralertjson.Add(alertjson);
                }
                else if (
                   //                                      //Alert related to mentions.
                   alerttypeentity.strType == AlerttypeentityAlertTypeEntityDB.strMentioned
                   )
                {
                    PiwentityProcessInWorkflowEntityDB piwentityMention = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == alertentity.intnOtherAttributes);
                    //                                      //Get process's name.
                    String strProcessName = (from piwentity in context_M.ProcessInWorkflow
                                             join eleentity in context_M.Element on
                                             piwentity.intPkProcess equals eleentity.intPk
                                             where piwentity.intPkWorkflow == piwentityMention.intPkWorkflow &&
                                             piwentity.intProcessInWorkflowId == piwentityMention.intProcessInWorkflowId
                                             select eleentity).FirstOrDefault().strElementName;

                    //                                      //Get workflow's name.
                    String strWorkflowName = (from piwentity in context_M.ProcessInWorkflow
                                              join wfentity in context_M.Workflow on
                                              piwentity.intPkWorkflow equals wfentity.intPk
                                              where piwentity.intPkWorkflow == piwentityMention.intPkWorkflow &&
                                              piwentity.intProcessInWorkflowId == piwentityMention.intProcessInWorkflowId
                                              select wfentity).FirstOrDefault().strName;

                    String strJobNumber = alertentity.intnJobId != null ? JobJob.strGetJobNumber(null,
                        (int)alertentity.intnJobId, strPrintshopId_I, context_M) : "";

                    //                                      //Create message to notify.
                    String strMessage = alerttypeentity.strDescription + strProcessName + " in job " + strJobNumber +
                        " " + strWorkflowName + " workflow.";

                    //                                      //Create the alertjson.
                    AlertjsonAlertJson alertjson = new AlertjsonAlertJson(alerttypeentity.strType,
                        strMessage, null, false, null);
                    darralertjson.Add(alertjson);
                }
                /*END-CASE*/

                if (
                    //                                      //It is a mention alert.
                    alerttypeentity.strType == AlerttypeentityAlertTypeEntityDB.strMentioned
                    )
                {
                    //                                      //Delete the alert from the DB.
                    context_M.Alert.Remove(alertentity);
                }
            }

            context_M.SaveChanges();

            arralertjson_O = darralertjson.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subCleanNotRequestedEstimatesAnyMoreAlertTable(
            //                                              //Delete alerts from jobs not existing in Wisnet and
            //                                              //      estimates not requested stage.

            PsPrintShop ps_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Get alertType New Estimate.
            AlerttypeentityAlertTypeEntityDB alerttypeentityEstimate = context_M.AlertType.FirstOrDefault(atype =>
                atype.strType == AlerttypeentityAlertTypeEntityDB.strNewEstimate);
         
            //                                              //Get job's ids from estimates.
            List<int> darrintEstimatesIds =
                (from alertentity in context_M.Alert
                 where alertentity.intPkPrintshop == ps_I.intPk &&
                 alertentity.intPkAlertType == alerttypeentityEstimate.intPk &&
                 alertentity.intnJobId != null
                 select (int)alertentity.intnJobId).ToList();

            List<int> darrintEstimatesIdsDistinct = darrintEstimatesIds.Distinct().ToList();

            if (
                darrintEstimatesIdsDistinct.Count > 0
                )
            {
                EmplEmployee.subDeleteEstimatesAlertNotRequestedStage(darrintEstimatesIdsDistinct.ToArray(), ps_I,
                    context_M);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subDeleteJobsAlertsNotExistsInWisnet(
            //                                              //Delete all job's alerts from alert table from those jobs
            //                                              //      not exists in wisnet anymore

            int[] arrintJobsIds_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Get jobs deleted in Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<DeljobsjsonDeletedJobsJson> Task_darrcatjsonFromWisnet =
                HttpTools<DeljobsjsonDeletedJobsJson>.GetListAsyncJobsDeleted(strUrlWisnet +
                "/PrintShopData/GetJobsDeletedOnWisnet", arrintJobsIds_I);
            Task_darrcatjsonFromWisnet.Wait();

            if (
                 //                                         //If the list has elements means those elements were
                 //                                         //      deleted in Wisnet.
                 Task_darrcatjsonFromWisnet.Result.darrintJobsDeleted.Count() > 0
                )
            {
                DeljobsjsonDeletedJobsJson deljobsjson = Task_darrcatjsonFromWisnet.Result;

                //                                          //Set as deleted those jobs deleted in Wisnet.
                foreach (int intJobId in deljobsjson.darrintJobsDeleted)
                {
                    IEnumerable<AlertentityAlertEntityDB> darralertentity = context_M.Alert.Where(alert =>
                        alert.intPkPrintshop == ps_I.intPk && alert.intnJobId == intJobId);
                    context_M.Alert.RemoveRange(darralertentity);
                }
                context_M.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subDeleteEstimatesAlertNotRequestedStage(
            //                                              //Delete all job's alerts from alert table from those 
            //                                              //      estimates not requested stage anymore.

            int[] arrintEstimatesIds_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Get jobs deleted in wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<DeljobsjsonDeletedJobsJson> Task_darrcatjsonFromWisnet =
                HttpTools<DeljobsjsonDeletedJobsJson>.GetListAsyncEstimatesNotRequestedStage(strUrlWisnet +
                "/Estimates/GetEstimatesIdsNotInRequestedStage", ps_I.strPrintshopId, arrintEstimatesIds_I);
            Task_darrcatjsonFromWisnet.Wait();

            if (
                 //                                         //If the list has elements means those elements weere
                 //                                         //      deleted in wisnet.
                 Task_darrcatjsonFromWisnet.Result.darrintJobsDeleted.Count() > 0
                )
            {
                DeljobsjsonDeletedJobsJson deljobsjson = Task_darrcatjsonFromWisnet.Result;

                //                                          //Set as deleted those jobs deleted in wisnet.
                foreach (int intJobId in deljobsjson.darrintJobsDeleted)
                {
                    IEnumerable<AlertentityAlertEntityDB> darralertentity = context_M.Alert.Where(alert =>
                        alert.intPkPrintshop == ps_I.intPk && alert.intnJobId == intJobId);
                    context_M.Alert.RemoveRange(darralertentity);
                }
                context_M.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfoGetPendingInEstimatingAndEstimates(
            //                                              //Get pending, inEstimating and estimates printshop's
            //                                              //      jobs' basic info.

            PsPrintShop ps_I,
            IConfiguration configuration_I,
            Odyssey2Context contex_M
            )
        {
            //                                              //To store jobs (inEstimating and Pending) and Estimates.
            List<JobbasicinfojsonJobBasicInfoJson> darrjobasicinfoPendingInEstimatingAndEstimates =
                new List<JobbasicinfojsonJobBasicInfoJson>();

            //                                              //Json that specifies pending stage.
            StagesjsonStagesJsonInternal stagesjsonPending = 
                new StagesjsonStagesJsonInternal(ps_I.strPrintshopId.ParseToInt(), null, null, null, true, true,
                null, null, null);            

            //                                              //Get jobs from Wisnet. 
            Task<List<JobbasicinfojsonJobBasicInfoJson>> Task_darrjobbasicinfojson = HttpTools<
                JobbasicinfojsonJobBasicInfoJson>.GetListAsyncWithBodyToEndPoint(stagesjsonPending,
                configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                "/PrintshopData/printshopJobsToFillList/");
            Task_darrjobbasicinfojson.Wait();

            if (
                Task_darrjobbasicinfojson.Result != null
                )
            {
                List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfoPending = Task_darrjobbasicinfojson.Result;
                darrjobasicinfoPendingInEstimatingAndEstimates.AddRange(
                    EmplEmployee.darrjobbsicinfoGetPendingStageJobs(ps_I, darrjobbasicinfoPending, contex_M));
            }

            //                                              //Json that specifies inEstimating stage.
            StagesjsonStagesJsonInternal stagesjsonInEstimating = 
                new StagesjsonStagesJsonInternal(ps_I.strPrintshopId.ParseToInt(), null, true, null, null, null,
                null, null, null);

            //                                              //Get jobs from Wisnet (InEstimating stage.). 
            Task<List<JobbasicinfojsonJobBasicInfoJson>> Task_darrjobbasicinfojsonInEstimating = HttpTools<
                JobbasicinfojsonJobBasicInfoJson>.GetListAsyncWithBodyToEndPoint(stagesjsonInEstimating,
                configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                "/PrintshopData/printshopJobsToFillList/");
            Task_darrjobbasicinfojsonInEstimating.Wait();

            if (
                Task_darrjobbasicinfojsonInEstimating.Result != null
                )
            {
                darrjobasicinfoPendingInEstimatingAndEstimates.AddRange(Task_darrjobbasicinfojsonInEstimating.Result);
            }

            //                                              //Get Jobs ids from Wisnet (Estimates). 
            Task<List<JobbasicinfojsonJobBasicInfoJson>> Task_darrjobsidsjsonFromWisnet =
                HttpTools<JobbasicinfojsonJobBasicInfoJson>.GetListAsyncToEndPoint(
                    configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                    "/PrintshopData/getAllPrintshopEstimates/" + ps_I.strPrintshopId);
            Task_darrjobsidsjsonFromWisnet.Wait();

            if (
                Task_darrjobsidsjsonFromWisnet.Result != null
                )
            {
                darrjobasicinfoPendingInEstimatingAndEstimates.AddRange(Task_darrjobsidsjsonFromWisnet.Result);                
            }

            return darrjobasicinfoPendingInEstimatingAndEstimates;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<JobbasicinfojsonJobBasicInfoJson> darrjobbsicinfoGetPendingStageJobs(
            //                                              //Get pending stage's jobs.

            PsPrintShop ps_I,
            List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfojson_I,
            Odyssey2Context context_M
            )
        {
            List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfo = new List<JobbasicinfojsonJobBasicInfoJson>();

            //                                              //Jobs InProgress in Odyssey2.
            List<JobentityJobEntityDB> darrjobentity = context_M.Job.Where(job =>
                job.intPkPrintshop == ps_I.intPk && job.intStage == JobJob.intInProgressStage
                 && job.boolDeleted == false).ToList();

            //                                              //Init the list for reduce notification.
            List<int> darrintRepsOrSuperviserToReduce = new List<int>();

            foreach (JobbasicinfojsonJobBasicInfoJson jobbasicinfojson in darrjobbasicinfojson_I)
            {
                if (
                    //                                      //It is not InProgress or Completed in Odyssey2.
                    darrjobentity.FirstOrDefault(job => job.intJobID == jobbasicinfojson.intJobId) == null
                    )
                {
                    darrjobbasicinfo.Add(jobbasicinfojson);
                }
            }

            return darrjobbasicinfo;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static String[]  arrstrGetOrderIdOrEstimateNumber(
            //                                              //Return an array with the next information.
            //                                              //1.-Array[0] = Customer's name.
            //                                              //2.-Array[1] = Estimate/Order number.
            //                                              //3.-Array[2] = Estimate/Order date.

            int intJobId_I,
            bool boolEstimate_I,
            PsPrintShop ps_I,
            List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfoPendingInEstimatingAndEstimates_I,
            IConfiguration configuration_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            String[] arrOrderOrEstimateInfo = new String[3];
            arrOrderOrEstimateInfo[0] = "";
            arrOrderOrEstimateInfo[1] = "";
            arrOrderOrEstimateInfo[2] = "";

            if (
                (darrjobbasicinfoPendingInEstimatingAndEstimates_I != null && darrjobbasicinfoPendingInEstimatingAndEstimates_I.Exists(job => 
                job.intJobId == intJobId_I))
                )
            {
                JobbasicinfojsonJobBasicInfoJson jobbasicinfo = darrjobbasicinfoPendingInEstimatingAndEstimates_I.FirstOrDefault(job =>
                    job.intJobId == intJobId_I);

                //                              //Get customer's name.
                arrOrderOrEstimateInfo[0] = jobbasicinfo.strCustomerName;

                //                              //Get order/estimate number.
                arrOrderOrEstimateInfo[1] = EmplEmployee.strGetOrderOrEstimateNumber(intJobId_I,
                    jobbasicinfo.intOrderId, boolEstimate_I, ps_I.strPrintshopId, configuration_I, context_M,
                    ref intStatus_IO, ref strDevMessage_IO, ref strUserMessage_IO);

                //                              //Get order-estima date.
                arrOrderOrEstimateInfo[2] = EmplEmployee.strGetOrderOrEstimateDate(jobbasicinfo.dateLastUpdate);
            }
            else
            {
                IEnumerable<AlertentityAlertEntityDB> darralertentity = context_M.Alert.Where(alert =>
                        alert.intPkPrintshop == ps_I.intPk && alert.intnJobId == intJobId_I);
                context_M.Alert.RemoveRange(darralertentity);
                context_M.SaveChanges();
            }

            return arrOrderOrEstimateInfo;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static String strGetOrderOrEstimateDate(
            //                                              //Get order or estimate date.

            String strDateLastUpdate_I
            )
        {
            //                                              //Get position of char T.
            int intTPos = strDateLastUpdate_I.IndexOf('T');
            String strOrderOrEstimateDate = (intTPos >= 0) ? strDateLastUpdate_I.Substring(0, intTPos) : "";

            return strOrderOrEstimateDate;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static String strGetOrderOrEstimateNumber(
            //                                              //Get order or estimate number.

            int intJobId_I,
            int intOrderId_I,
            //                                              //To know is if we requested order or estimate number.
            bool boolEstimate_I,

            String strPrintshopId_I,
            IConfiguration configuration_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            String strOrderOrEstimateNumber = "";

            if (
                //                                          //Estimate's number requested.
                boolEstimate_I
                )
            {
                //                                          //Verify an update/add(if needed) estimates to db.
                bool boolEstimatesWereUpdated;
                JobJob.subUpdateOrAddEstimNumberForAPrintshop(strPrintshopId_I, out boolEstimatesWereUpdated,
                    configuration_I, context_M);

                //                                          //Estimation were not updated.
                //                                          //Due to there was estimated already in DB.
                //                                          //1.-Add the alerted estimation and get the EstimateNumber.
                //                                          //2.-Get EstimateNumber from DB.
                if (
                    //                                      //1.
                    !boolEstimatesWereUpdated
                    )
                {
                    //                                      //Update the alerted estimation.
                    int intEstimateNumber;
                    EmplEmployee.subUpdateEstimateAlerted(intJobId_I, intOrderId_I, strPrintshopId_I,
                        out intEstimateNumber, context_M);
                    strOrderOrEstimateNumber = intEstimateNumber.ToString();
                }
                else
                {
                    //                                      //2.
                    strOrderOrEstimateNumber = EmplEmployee.intGetEstimateNumber(intJobId_I,
                        strPrintshopId_I, context_M).ToString();
                }
            }
            else
            {
                //                                          //Order's number requested.
                strOrderOrEstimateNumber = EmplEmployee.strGetOrderIdForAPrintshop(intJobId_I, intOrderId_I,
                    strPrintshopId_I, configuration_I, context_M, ref intStatus_IO, ref strDevMessage_IO,
                    ref strUserMessage_IO);
            }

            return strOrderOrEstimateNumber;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subUpdateEstimateAlerted(
            //                                              //Look if the estimate alert exist in jobjson table.
            //                                              //1.-Exists, return EstimateNumber.
            //                                              //2.-No exists, add it and return EstimateNumber.

            int intJobId_I,
            int intOrderId_I,
            String strPrintshopId_I,
            out int intEstimaNumber_O,
            Odyssey2Context context_M
            )
        {
            JobjsonentityJobJsonEntityDB jobjsonentity = context_M.JobJson.FirstOrDefault(json =>
                json.intJobID == intJobId_I && json.strPrintshopId == strPrintshopId_I && json.intOrderId <= 0);

            if (
                jobjsonentity != null
                )
            {
                intEstimaNumber_O = (int)jobjsonentity.intnEstimateNumber;
            }
            else
            {
                //                                          //Get the next estimateNumber to add new one.
                int intNextEstimateNumber = EmplEmployee.intGetNextEstimateNumberForAPrintshop(strPrintshopId_I, context_M);
                //                                          //Add to DB.
                JobJob.subAddIntEstimateNumber(intJobId_I, intOrderId_I, intNextEstimateNumber, strPrintshopId_I,
                    context_M);
                intEstimaNumber_O = intNextEstimateNumber;
            }  
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static int intGetEstimateNumber(
            //                                              //Get the estimate number for and especific estimate.

            int intJobId_I,
            String strPrintshopId_I,
            Odyssey2Context context_M
            )
        {
            JobjsonentityJobJsonEntityDB jobjsonentity = context_M.JobJson.FirstOrDefault(json =>
                json.intJobID == intJobId_I && json.strPrintshopId == strPrintshopId_I && json.intOrderId <= 0);

            return (int)jobjsonentity.intnEstimateNumber;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static int intGetNextEstimateNumberForAPrintshop(
            //                                              //Get the max estimate number for an especific printshop.

            String strPrintshopId_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Get all printshop's estimates.
            List<JobjsonentityJobJsonEntityDB> darrjobjsonentity = context_M.JobJson.Where(jjson =>
                jjson.strPrintshopId == strPrintshopId_I && (jjson.intOrderId == 0 || jjson.intOrderId == -1) &&
                jjson.intnOrderNumber == null && jjson.intnJobNumber == null).ToList();

            int intMaxEstimateNumber = (darrjobjsonentity.Count > 0) ?
                                ((int)darrjobjsonentity.Max(job => job.intnEstimateNumber)) + 1 : 1;

            return intMaxEstimateNumber;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strGetOrderIdForAPrintshop(
            //                                              //Get the orderId for an especific order.
            //                                              //1.-If the printshop does not have orders in DB, add them
            //                                              //      all and return th OrderId
            //                                              //2.-Verify if the order exists in DB.
            //                                              //      2.1-Exists, take it from db.
            //                                              //      2.2-Not exists, add it and then take it.

            int intJobId_I,
            int intOrderId_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            String strOrderId = "";

            if (
                //                                          //1.                                          
                context_M.JobJson.FirstOrDefault(job =>
                    //                                      //There are no orders-jobs in the jobjson table.
                    job.strPrintshopId == strPrintshopId_I &&
                    //                                      //To be a order-job, OrderId must be greater than 0.
                    job.intOrderId > 0
                    ) == null
                )
            {
                //                                          //Get all the jobs to save the correct order number.
                JobJob.subGetAllJobsToSetOrderNumber(strPrintshopId_I, configuration_I, 1, context_M, ref intStatus_IO,
                    ref strUserMessage_IO, ref strDevMessage_IO);

                //                                          //After add all jobs to db, get orderId for the given
                //                                          //      order.
                JobjsonentityJobJsonEntityDB jobjsonentity = context_M.JobJson.FirstOrDefault(json =>
                json.intJobID == intJobId_I && json.strPrintshopId == strPrintshopId_I);
                strOrderId = ((int)jobjsonentity.intnOrderNumber).ToString();
            }
            else
            {
                //                                          //2.
                JobjsonentityJobJsonEntityDB jobjsonentity = context_M.JobJson.FirstOrDefault(json =>
                json.intJobID == intJobId_I && json.strPrintshopId == strPrintshopId_I);
                if (
                    jobjsonentity != null && jobjsonentity.intnOrderNumber != null
                    )
                {
                    //                                      //2.1
                    strOrderId = ((int)jobjsonentity.intnOrderNumber).ToString();
                }
                else
                {
                    //                                      //2.2
                    //context_M.JobJson.Remove(jobjsonentity);
                    //context_M.SaveChanges();
                    EmplEmployee.subAddNewOrderTOoDB(intJobId_I, intOrderId_I, strPrintshopId_I, context_M);
                    JobjsonentityJobJsonEntityDB jobjsonentityAfterAdd = context_M.JobJson.FirstOrDefault(json =>
                    json.intJobID == intJobId_I && json.strPrintshopId == strPrintshopId_I);
                    strOrderId = ((int)jobjsonentityAfterAdd.intnOrderNumber).ToString();
                }                
            }            

            return strOrderId;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subAddNewOrderTOoDB(
            //                                              //Add new order to db.

            int intJobId_I,
            int intOrderId_I,
            String strPrintshopId_I,
            Odyssey2Context context_M
            )
        {

            //                                          //
            JobbasicinfojsonJobBasicInfoJson jobbasicinfojson = new JobbasicinfojsonJobBasicInfoJson();
            jobbasicinfojson.intJobId = intJobId_I;
            jobbasicinfojson.intOrderId = intOrderId_I;
            List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfojson = new List<JobbasicinfojsonJobBasicInfoJson>();
            darrjobbasicinfojson.Add(jobbasicinfojson);

            JobjsonentityJobJsonEntityDB jobjsonentity = context_M.JobJson.FirstOrDefault(jobjson => 
                jobjson.intJobID == intJobId_I && jobjson.intOrderId == intOrderId_I);

            if (
                jobjsonentity != null &&
                jobjsonentity.intnJobNumber == null
                )
            {
                //                                          //Remove the jobjson for after will added to the jobjson with
                //                                          //    job.number.
                context_M.JobJson.Remove(jobjsonentity);
                context_M.SaveChanges();
            }

            JobJob.subVerifyJobIsInDB(strPrintshopId_I, 1, darrjobbasicinfojson, context_M);

        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetAllTasks(
            //                                              //Get an employee's uncompleted tasks from the DB.

            int intContactId_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            out PerortaskjsonPeriodOrTaskJson[] arrperortaskjson_O,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            arrperortaskjson_O = null;

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No employee found.";
            if (
                ResResource.boolEmployeeOrOwnerIsFromPrintshop(strPrintshopId_I, intContactId_I)
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                //                                          //Find periods excluding temporary ones.
                List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per =>
                    per.intnContactId == intContactId_I && per.intnEstimateId == null).ToList();

                List<JobjsonentityJobJsonEntityDB> darrjobjsonentity = context.JobJson.Where(job =>
                    job.strPrintshopId == strPrintshopId_I).ToList();

                //                                          //List that will hold periods and tasks.
                List<PerortaskjsonPeriodOrTaskJson> darrperortaskjson = new List<PerortaskjsonPeriodOrTaskJson>();

                foreach (PerentityPeriodEntityDB perentity in darrperentity)
                {
                    if (
                        //                                  //The period is not completed
                        perentity.strFinalEndDate == null && perentity.strFinalEndTime == null
                        )
                    {
                        bool boolIsByResource = (perentity.intnPkElementElementType != null) ||
                            (perentity.intnPkElementElement != null);

                        bool boolIsAbleToStart;
                        bool boolIsAbleToEnd;
                        EmplEmployee.subGetBoolIsAbleToStartOrEnd(perentity, out boolIsAbleToStart,
                            out boolIsAbleToEnd, ref strUserMessage_IO);

                        PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(
                            piw => piw.intPkWorkflow == perentity.intPkWorkflow &&
                            piw.intProcessInWorkflowId == perentity.intProcessInWorkflowId);

                        EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(eleentity =>
                            eleentity.intPk == piwentity.intPkProcess);

                        String strProcessName = eleentity.strElementName + ((piwentity.intnId != null) ?
                            ("(" + piwentity.intnId + ")") : "");

                        String strA = "", strB = "";
                        JobjsonJobJson jobjson;
                        if (
                            JobJob.boolIsValidJobId(perentity.intJobId, strPrintshopId_I, configuration_I, out jobjson,
                                ref strA, ref strB)
                            )
                        {
                            JobjsonentityJobJsonEntityDB jobjsonentity = darrjobjsonentity.FirstOrDefault(job =>
                                job.intJobID == perentity.intJobId &&
                                job.strPrintshopId == strPrintshopId_I);

                            if (
                                jobjsonentity != null
                                )
                            {
                                //                          //Get strJobNumber
                                String strJobNumber = jobjsonentity != null ?
                                    (jobjsonentity.intnOrderNumber.ToString() + " - " +
                                    jobjsonentity.intnJobNumber.ToString()) : "";

                                ZonedTime ztimeStartPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                    perentity.strStartDate.ParseToDate(), perentity.strStartTime.ParseToTime(), 
                                    ps.strTimeZone);
                                ZonedTime ztimeEndPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                    perentity.strEndDate.ParseToDate(), perentity.strEndTime.ParseToTime(),
                                    ps.strTimeZone);

                                PerortaskjsonPeriodOrTaskJson perortaskjson2 = new PerortaskjsonPeriodOrTaskJson(
                                    perentity.intPk, null, ztimeStartPeriod.Date.ToString(), 
                                    ztimeStartPeriod.Time.ToString(), ztimeEndPeriod.Date.ToString(), 
                                    ztimeEndPeriod.Time.ToString(), perentity.intJobId + "", strJobNumber,
                                    boolIsByResource, boolIsAbleToStart, boolIsAbleToEnd, perentity.intMinsBeforeDelete,
                                    strProcessName, null, 0, false, false, jobjson.strJobTicket, null, null);

                                darrperortaskjson.Add(perortaskjson2);
                            }
                        }
                    }
                }

                //                                          //Find uncompleted tasks.
                List<TaskentityTaskEntityDB> darrtaskentity = context.Task.Where(task =>
                    task.intContactId == intContactId_I && task.boolIsCompleted == false).ToList();

                //                                          //Customer info if exists.
                CusjsonCustomerJson cusjson;
                String strCustomerName = "";
                String strCustomerLastName = "";

                List<CusjsonCustomerJson> darrcusjson;
                CusCustomer.subGetAllForAPrintshop(ps, out darrcusjson, ref intStatus_IO,
                    ref strUserMessage_IO, ref strDevMessage_IO);

                foreach (TaskentityTaskEntityDB taskentity in darrtaskentity)
                {
                    if (
                        //                                  //Task related to a customer.
                        taskentity.intnCustomerId != null
                        )
                    {
                        //                                  //Find customer.
                        cusjson = darrcusjson.FirstOrDefault(customer => 
                            customer.intContactId == taskentity.intnCustomerId);

                        if (
                            //                              //Customer data.
                            cusjson != null
                            )
                        {
                            strCustomerName = cusjson.strFirstName;
                            strCustomerLastName = cusjson.strLastName;
                        }
                    }

                    String strStartTime = taskentity.strStartHour + ":" + taskentity.strStartMinute + ":" +
                        taskentity.strStartSecond;

                    ZonedTime ztimeStartTask = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        taskentity.strStartDate.ParseToDate(), strStartTime.ParseToTime(), ps.strTimeZone);
                    ZonedTime ztimeEndTask = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                         taskentity.strEndDate.ParseToDate(), taskentity.strEndTime.ParseToTime(),
                        ps.strTimeZone);

                    PerortaskjsonPeriodOrTaskJson perortaskjson2 = new PerortaskjsonPeriodOrTaskJson(null, 
                        taskentity.intPk, ztimeStartTask.Date.ToString(), ztimeStartTask.Time.ToString(), 
                        ztimeEndTask.Date.ToString(), ztimeEndTask.Time.ToString(), null, "", false, false, false, 0,
                        null, taskentity.strDescription, taskentity.intMinsBeforeNotify, taskentity.boolIsNotifiedable,
                        taskentity.boolIsCompleted, null, strCustomerName, strCustomerLastName);

                    darrperortaskjson.Add(perortaskjson2);

                    //                                      //Reset values
                    strCustomerName = "";
                    strCustomerLastName = "";
                }

                darrperortaskjson.Sort();
                arrperortaskjson_O = darrperortaskjson.ToArray();

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetTask(
            //                                              //Get one task from the DB.
            //                                              //If the task is related to a customer then find data 
            //                                              //      related to him/her.

            int intPkTask_I,
            String strPrintshopId_I,
            int intContactId_I,
            IConfiguration configuration_I,
            out TaskjsonTaskJson taskjson_O,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            taskjson_O = null;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Invalid employee.";
            if (
                ResResource.boolEmployeeOrOwnerIsFromPrintshop(strPrintshopId_I, intContactId_I)
                )
            {
                //                                          //Find task.
                TaskentityTaskEntityDB taskentity = context.Task.FirstOrDefault(task => (task.intPk == intPkTask_I));

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Task not found.";
                if (
                    taskentity != null
                    )
                {
                    //                                      //Customer info if exists.
                    CusjsonCustomerJson cusjson;
                    String strCustomerName = "";
                    String strCustomerLastName = "";

                    if (
                        //                                  //Task related to a customer.
                        taskentity.intnCustomerId != null
                        )
                    {
                        //                                  //Find customer.
                        PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                        CusCustomer.subGetOneCustomerFromPrintshop(ps, (int)taskentity.intnCustomerId, out cusjson,
                            ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                        if (
                            //                              //Customer data.
                            cusjson != null
                            )
                        {
                            strCustomerName = cusjson.strFirstName;
                            strCustomerLastName = cusjson.strLastName;
                        }
                    }

                    String strStartTime = taskentity.strStartHour + ":" + taskentity.strStartMinute + ":" +
                        taskentity.strStartSecond;

                    //                                      //Json object to return.
                    taskjson_O = new TaskjsonTaskJson(intPkTask_I, taskentity.strDescription, taskentity.strStartDate,
                        strStartTime, taskentity.strEndDate, taskentity.strEndTime,
                        taskentity.intMinsBeforeNotify, taskentity.boolIsNotifiedable, taskentity.boolIsCompleted,
                        taskentity.intnCustomerId, strCustomerName, strCustomerLastName);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSendTasksAlertsAndDeleteOld(
            //                                              //Send notifications to all the employees who have 
            //                                              //      tasks about to start.
            //                                              //Delete task's notification when the task's start time
            //                                              //      is the same than current time.

            //                                              //Connection object that will send the notifications.
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get current time.
            String strCurrentTime = Time.Now(ZonedTimeTools.timezone).ToString();
            //                                              //Consider only hours and minutes.
            strCurrentTime = strCurrentTime.Substring(0, 5);
            //                                              //Current time.
            Time timeCurrentTime = strCurrentTime.ParseToTime();
            //                                              //Create ztime now.
            ZonedTime ztimeDateNow = ZonedTimeTools.NewZonedTime(Date.Now(ZonedTimeTools.timezone),
                    timeCurrentTime);

            String strCurrentHour = ztimeDateNow.Time.ToString();
            strCurrentHour = strCurrentHour.Substring(0, 2);
            String strCurrentMinute = ztimeDateNow.Time.ToString();
            strCurrentMinute = strCurrentMinute.Substring(3, 2);
            //                                              //Get all tasks that are notifiedable and that must
            //                                              //      must be notified in current date and time.
            List<TaskentityTaskEntityDB> darrtaskentityToNotify = context.Task.Where(task =>
                task.boolIsNotifiedable && task.strNotificationDate == ztimeDateNow.Date.ToString() &&
                task.strNotificationHour == strCurrentHour &&
                task.strNotificationMinute == strCurrentMinute &&
                task.boolIsCompleted == false).ToList();

            //                                              //Contacts who will have notifications
            List<int> darrintContactId = new List<int>();

            //                                              //Find PkAltertType for Tasks.
            AlerttypeentityAlertTypeEntityDB alerttypeentity = context.AlertType.FirstOrDefault(
                type => type.strType == AlerttypeentityAlertTypeEntityDB.strTask);

            foreach (TaskentityTaskEntityDB taskentity in darrtaskentityToNotify)
            {
                //                                      //Create notification.
                AlertentityAlertEntityDB alertentityNew = new AlertentityAlertEntityDB
                {
                    intPkPrintshop = taskentity.intPkPrintshop,
                    intPkAlertType = alerttypeentity.intPk,
                    intnContactId = taskentity.intContactId,
                    intnPkTask = taskentity.intPk
                };
                context.Alert.Add(alertentityNew);
                context.SaveChanges();

                darrintContactId.Add(taskentity.intContactId);
            }

            //                                              //Support method to send notifications to front.
            String strMessage = "You have a task starting soon.";
            AlnotAlertNotification.subSendTaskToAFew(darrintContactId.ToArray(), strMessage, iHubContext_I);

            //                                              //Get all tasks that start on current date and time.
            List<TaskentityTaskEntityDB> darrtaskentityToDelete = context.Task.Where(task =>
                task.boolIsNotifiedable && task.strStartDate == ztimeDateNow.Date.ToString() &&
                task.strStartHour == strCurrentHour && task.strStartMinute == strCurrentMinute).ToList();

            foreach (TaskentityTaskEntityDB taskentity in darrtaskentityToDelete)
            {
                //                                          //Find task's alert.
                AlertentityAlertEntityDB alertToDelete = context.Alert.FirstOrDefault(alert =>
                    (alert.intnContactId == taskentity.intContactId) &&
                    (alert.intPkAlertType == alerttypeentity.intPk) &&
                    (alert.intnPkTask == taskentity.intPk));

                if (
                    alertToDelete != null
                    )
                {
                    if (
                        //                                  //Notification not read by user.
                        !PsPrintShop.boolNotificationReadByUser(alertToDelete, taskentity.intContactId)
                        )
                    {
                        //                                  //Support method to reduce notifications in 
                        //                                  //      front.
                        AlnotAlertNotification.subReduceToOne(taskentity.intContactId, iHubContext_I);
                    }

                    context.Alert.Remove(alertToDelete);
                    context.SaveChanges();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetOverdueTasks(
            //                                              //Find overdue tasks related to an employee

            String strPrintshopId_I,
            int intContactId_I,
            IConfiguration configuration_I,
            out Taskjson2TaskJson2[] arrtaskjson2_O,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            arrtaskjson2_O = null;

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Invalid employee.";
            if (
                ResResource.boolEmployeeOrOwnerIsFromPrintshop(strPrintshopId_I, intContactId_I)
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Find tasks.
                List<TaskentityTaskEntityDB> darrtaskentity = context.Task.Where(task =>
                    task.intContactId == intContactId_I).ToList();

                ZonedTime ztimeDateNow = ZonedTimeTools.NewZonedTime(Date.Now(ZonedTimeTools.timezone),
                    Time.Now(ZonedTimeTools.timezone));

                //                                          //Get the printshop.
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                //                                          //Customer info if exists.
                CusjsonCustomerJson cusjson;
                String strCustomerName = "";
                String strCustomerLastName = "";

                List<Taskjson2TaskJson2> darrtaskjson2 = new List<Taskjson2TaskJson2>();

                foreach (TaskentityTaskEntityDB taskentity in darrtaskentity)
                {
                    //                                      //To easy code.
                    ZonedTime ztimeEndTask = ZonedTimeTools.NewZonedTime(taskentity.strEndDate.ParseToDate(),
                        taskentity.strEndTime.ParseToTime());
                    if (
                        //                                  //The EndTask is in the past and is not completed.
                        (ztimeEndTask < ztimeDateNow) && taskentity.boolIsCompleted == false
                        )
                    {
                        ztimeEndTask = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                            taskentity.strEndDate.ParseToDate(), taskentity.strEndTime.ParseToTime(), ps.strTimeZone);
                        if (
                            //                              //Task related to a customer.
                            taskentity.intnCustomerId != null
                            )
                        {
                            //                              //Find customer.
                            CusCustomer.subGetOneCustomerFromPrintshop(ps, (int)taskentity.intnCustomerId,
                                out cusjson, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                            if (
                                //                          //Customer data.
                                cusjson != null
                                )
                            {
                                strCustomerName = cusjson.strFirstName;
                                strCustomerLastName = cusjson.strLastName;
                            }
                        }

                        String strStartTime = taskentity.strStartHour + ":" + taskentity.strStartMinute + ":" +
                            taskentity.strStartSecond;

                        ZonedTime ztimeStartTask = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                            taskentity.strStartDate.ParseToDate(), strStartTime.ParseToTime(), ps.strTimeZone);

                        Taskjson2TaskJson2 taskjson2 = new Taskjson2TaskJson2(taskentity.intPk,
                            taskentity.strDescription, ztimeStartTask.Date.ToString(), ztimeStartTask.Time.ToString(),
                            ztimeEndTask.Date.ToString(), ztimeEndTask.Time.ToString(), taskentity.boolIsCompleted,
                            strCustomerName, strCustomerLastName);

                        darrtaskjson2.Add(taskjson2);
                    }
                }

                darrtaskjson2.Sort();

                arrtaskjson2_O = darrtaskjson2.ToArray();

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
