/*TASK RP. JOB*/
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.Utilities;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowaStandard;
using System.Xml.XPath;
using Microsoft.AspNetCore.SignalR;
using Odyssey2Backend.Alert;
using Odyssey2Backend.JsonTemplates.Out;
using Odyssey2Backend.Customer;
using System.Text.Json;
using System.Text.RegularExpressions;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 06, 2020.

namespace Odyssey2Backend.Job
{
    //==================================================================================================================
    public class JobJob
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTANTS.

        public const int intPendingStage = 1;
        public const int intInProgressStage = 2;
        public const int intCompletedStage = 3;

        public const String strUnsubmittedStage = "Unsubmitted.";
        public const String strInEstimatingStage = "In Estimating.";
        public const String strWaitingForPriceApprovalStage = "Waiting Approval.";
        public const String strPendingStage = "Pending.";
        public const String strInProgressStage = "In Progress.";
        public const String strCompletedStage = "Completed.";
        public const String strNotPaidStage = "Not Paid.";

        public const String strRequestedEstimates = "Requested.";
        public const String strWaitingForResponseEstimates = "Waiting Response.";
        public const String strRejectedEstimates = "Rejected.";

        public const int intProcessInWorkflowStarted = 1;
        public const int intProcessInWorkflowCompleted = 2;

        //                                                  //Id confirmed estimate.
        public const int intIdConfirmedEstimate = 0;

        public const int intTimeOffset = 10;

        private const int intTemporalJobId = -999;

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //                                                  //Job Pk.
        private readonly int intPk_Z;
        public int intPk { get { return this.intPk_Z; } }
        //                                                  //Job Id.
        private readonly int intId_Z;
        public int intId { get { return this.intId_Z; } }
        //                                                  //Stage of a Job.
        private readonly int intStage_Z;
        public int intStage { get { return this.intStage_Z; } }

        //                                                  //Job Pk.
        private readonly int intPkPrintshop_Z;
        public int intPkPrintshop { get { return this.intPkPrintshop_Z; } }


        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTOR.

        //--------------------------------------------------------------------------------------------------------------
        public JobJob(
            //                                              //Pk job.
            int intPk_I,
            //                                              //Job Id.
            int intId_I,
            //                                              //Stage a job.
            int intStage_I,
            //                                              //PkPrintshop.
            int intPkPrintshop_I
            )
        {
            this.intPk_Z = intPk_I;
            this.intId_Z = intId_I;
            this.intStage_Z = intStage_I;
            this.intPkPrintshop_Z = intPkPrintshop_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static void subModifyJobStagePendingToProgress(
            //                                              //Starts a job.

            PsPrintShop ps_I,
            int intJobId_I,
            int intPkWorkflow_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Verify job.
            JobjsonJobJson jobjsonJob;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjsonJob,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "This job cannot be started.";
                if (
                    jobjsonJob.intnOrderId > 0
                    )
                {
                    //                                      //Create the connection.
                    Odyssey2Context context = new Odyssey2Context();

                    //                                      //Find job.
                    JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(jobentity =>
                        jobentity.intJobID == intJobId_I);

                    if (
                        //                                  //Job exist en table in the DB.
                        jobentity != null
                        )
                    {
                        //                                  //Find if the job is in Database App.
                        /*CASE*/
                        if (
                            //                              //Current stage is InProgress.
                            jobentity.intStage == JobJob.intInProgressStage
                            )
                        {
                            intStatus_IO = 401;
                            strUserMessage_IO = "The job current stage is \"In Progress\".";
                            strDevMessage_IO = "Error, The job current stage is \"In Progress\".";
                        }
                        else if (
                            //                              //Current stage is completed.
                            jobentity.intStage == JobJob.intCompletedStage
                            )
                        {
                            intStatus_IO = 402;
                            strUserMessage_IO = "The job current stage is \"Completed\".";
                            strDevMessage_IO = "Error, The job current stage is \"Completed\".";
                        }
                        /*END-CASE*/
                    }
                    else
                    {
                        //                                  //Get workflow.
                        WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                            wf.intPk == intPkWorkflow_I);

                        intStatus_IO = 403;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Workflow not found.";
                        if (
                            //                              //Workflow exists.
                            wfentity != null
                            )
                        {
                            //                              //Add Job to table job DB.
                            JobentityJobEntityDB jobentityAdd = new JobentityJobEntityDB
                            {
                                intPkPrintshop = ps_I.intPk,
                                intJobID = intJobId_I,
                                intStage = JobJob.intInProgressStage,
                                strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                intPkWorkflow = intPkWorkflow_I
                            };
                            context.Job.Add(jobentityAdd);
                            context.SaveChanges();

                            //                              //Delete Resorce'periods and Process'periods
                            //                              //    that are not from this workflow.
                            JobJob.deleteResourcePeriodsAndProcessPeriodThatNotApplyInThisWFOfThisJob(intJobId_I,
                                intPkWorkflow_I, iHubContext_I);

                            //                              //Delete entries from IOJ from others WF.
                            JobJob.subDeleteIOJentriesFromOthersWorkflows(intJobId_I, intPkWorkflow_I);

                            //                              //Delete others' workflows prices.
                            JobJob.subDeleteOthersWorkflowsPrices(intJobId_I, intPkWorkflow_I);

                            //                              //Delete others' workflows estimates.
                            JobJob.subDeleteOthersWorkflowsEstimates(intJobId_I);

                            //                              //Delete due date in the past alerts.
                            JobJob.subDeleteDueDateInThePastAlert(intJobId_I, ps_I.intPk, null, null, iHubContext_I);

                            intStatus_IO = 200;
                            strUserMessage_IO = "Job stage was changed from \"Pending\" to \"In Progress\".";
                            strDevMessage_IO = "";

                            //                              //Periods in the past.

                            List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per =>
                                per.intJobId == intJobId_I && per.intnEstimateId == null).ToList();

                            int intPeriodsEliminated = 0;
                            foreach (PerentityPeriodEntityDB perentity in darrperentity)
                            {
                                String strDeleteTime = perentity.strDeleteHour + ":" +
                                    perentity.strDeleteMinute + ":00";

                                ZonedTime ztimeDeletePeriod = ZonedTimeTools.NewZonedTime(
                                    perentity.strDeleteDate.ParseToDate(), strDeleteTime.ParseToTime());

                                if (
                                    //                      //The period is in the past
                                    ztimeDeletePeriod <= ZonedTimeTools.ztimeNow
                                    )
                                {
                                    context.Period.Remove(perentity);
                                    intPeriodsEliminated++;
                                }
                            }

                            context.SaveChanges();
                            if (
                               intPeriodsEliminated > 0
                                )
                            {
                                intStatus_IO = 200;
                                strUserMessage_IO = "Job stage was changed from \"Pending\" to \"In Progress\". " +
                                    "And some periods in the past were deleted.";
                                strDevMessage_IO = "";
                            }

                            //                              //Period notifications.

                            PiwentityProcessInWorkflowEntityDB piwentityFirst;
                            JobJob.subGetFirstProcessForTheJob(intJobId_I, intPkWorkflow_I, out piwentityFirst);

                            //                              //Find periods in the first piw.
                            List<PerentityPeriodEntityDB> darrperentityFirstPiw = context.Period.Where(per =>
                                per.intJobId == intJobId_I && per.intPkWorkflow == intPkWorkflow_I &&
                                per.intProcessInWorkflowId == piwentityFirst.intProcessInWorkflowId).ToList();

                            //                              //Find period type related to period.
                            AlerttypeentityAlertTypeEntityDB alerttypeentity = context.AlertType.FirstOrDefault(at =>
                                at.strType == AlerttypeentityAlertTypeEntityDB.strPeriod);


                            foreach (PerentityPeriodEntityDB perentity in darrperentityFirstPiw)
                            {
                                //                          //This notification will be sent to the employee assigned 
                                //                          //      to the period.
                                if (
                                    //                      //It is a period related to an employee.
                                    perentity.intnContactId != null
                                    )
                                {
                                    //                      //List that will contain the contact id to notify.
                                    List<int> darrintContactToNotify = new List<int>();
                                    darrintContactToNotify.Add((int)perentity.intnContactId);

                                    AlertentityAlertEntityDB alertentity = new AlertentityAlertEntityDB
                                    {
                                        intPkAlertType = alerttypeentity.intPk,
                                        intPkPrintshop = ps_I.intPk,
                                        intnJobId = intJobId_I,
                                        intnContactId = perentity.intnContactId,
                                        intnPkPeriod = perentity.intPk
                                    };
                                    context.Alert.Add(alertentity);
                                    context.SaveChanges();

                                    //                      //Get strJobNumber.
                                    String strJobNumber = JobJob.strGetJobNumber(null, intJobId_I, ps_I.strPrintshopId,
                                        context);

                                    //                      //Send the notification to the corresponding contact.
                                    AlnotAlertNotification.subSendToAFew(ps_I.strPrintshopId,
                                        darrintContactToNotify.ToArray(), alerttypeentity.strDescription +
                                        strJobNumber + ".", iHubContext_I);
                                }
                            }

                            //                              //Section where we delete New Order or Estimate 
                            //                              //      Alerts.

                            //                              //Get the Alert types for the Order.
                            AlerttypeentityAlertTypeEntityDB alerttypeentityNewOrder = context.AlertType.FirstOrDefault(
                                alerttype => alerttype.strType == AlerttypeentityAlertTypeEntityDB.strNewOrder);
                            //                                  //Get the Alert types for the Estimate.
                            AlerttypeentityAlertTypeEntityDB alerttypeentityNewEstimate =
                                context.AlertType.FirstOrDefault(alerttype =>
                                alerttype.strType == AlerttypeentityAlertTypeEntityDB.strNewEstimate);

                            //                              //Get Alerts related to orders and estimates.
                            List<AlertentityAlertEntityDB> darralertentityToDelete = context.Alert.Where(alert =>
                                alert.intPkPrintshop == ps_I.intPk && alert.intnJobId == intJobId_I &&
                                (alert.intPkAlertType == alerttypeentityNewOrder.intPk ||
                                alert.intPkAlertType == alerttypeentityNewEstimate.intPk)).ToList();
                            if (
                                 darralertentityToDelete.Count > 0
                                )
                            {
                                foreach (AlertentityAlertEntityDB alertentity in darralertentityToDelete)
                                {
                                    if (
                                        //                  //******Only reduce if notification not read by contact.
                                        alertentity.intnContactId != null &&
                                        !PsPrintShop.boolNotificationReadByUser(alertentity,
                                        (int)alertentity.intnContactId)
                                        )
                                    {
                                        //                  //Here we delete the alerts and get the Reps Ids to be
                                        //                  //      reduced the notifications.
                                        List<int> darrintRepsToReduce = new List<int>();
                                        darrintRepsToReduce.Add((int)alertentity.intnContactId);

                                        //                  //Send reduce notifications to the Reps.
                                        AlnotAlertNotification.subReduceToAFew(ps_I.strPrintshopId,
                                            darrintRepsToReduce.ToArray(), iHubContext_I);
                                    }

                                    context.Alert.Remove(alertentity);
                                }
                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subGetFirstProcessForTheJob(
            int intJobId_I,
            int intPkWorkflow_I,
            out PiwentityProcessInWorkflowEntityDB piwentityFirst
            )
        {
            piwentityFirst = null;

            Odyssey2Context context = new Odyssey2Context();

            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity = context.ProcessInWorkflow.Where(piw =>
                piw.intPkWorkflow == intPkWorkflow_I).ToList();

            int intI = 0;
            /*UNTIL-DO*/
            while (!(
                (intI >= darrpiwentity.Count) ||
                piwentityFirst != null
                ))
            {
                List<IoentityInputsAndOutputsEntityDB> darrioentityInputEleetWithLink = (
                    from ioentity in context.InputsAndOutputs
                    join eleetentity in context.ElementElementType
                    on ioentity.intnPkElementElementType equals eleetentity.intPk
                    where ioentity.strLink != null && ioentity.intPkWorkflow == intPkWorkflow_I &&
                    ioentity.intnProcessInWorkflowId == darrpiwentity[intI].intProcessInWorkflowId &&
                    eleetentity.boolUsage == true
                    select ioentity).ToList();

                List<IoentityInputsAndOutputsEntityDB> darrioentityInputEleeleWithLink = (
                    from ioentity in context.InputsAndOutputs
                    join eleeleentity in context.ElementElement
                    on ioentity.intnPkElementElement equals eleeleentity.intPk
                    where ioentity.strLink != null && ioentity.intPkWorkflow == intPkWorkflow_I &&
                    ioentity.intnProcessInWorkflowId == darrpiwentity[intI].intProcessInWorkflowId &&
                    eleeleentity.boolUsage == true
                    select ioentity).ToList();

                if (
                    darrioentityInputEleetWithLink.Count == 0 &&
                    darrioentityInputEleeleWithLink.Count == 0
                    )
                {
                    piwentityFirst = darrpiwentity[intI];
                }

                intI = intI + 1;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void deleteResourcePeriodsAndProcessPeriodThatNotApplyInThisWFOfThisJob(
            int intJobId_I,
            int intPkWorkflow_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            //                                              //Connect to DB.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find Resource'periods and Process´periods 
            //                                              //    in this Job that no apply for this WF. 
            List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per => per.intJobId == intJobId_I &&
                per.intPkWorkflow != intPkWorkflow_I).ToList();

            //                                              //Delete periods that are not from this workflow.
            foreach (PerentityPeriodEntityDB perentity in darrperentity)
            {
                //                                          //Find alerts about this period.
                List<AlertentityAlertEntityDB> darralertentity =
                    context.Alert.Where(alert =>
                    alert.intnPkPeriod == perentity.intPk).ToList();

                foreach (AlertentityAlertEntityDB alertentity in darralertentity)
                {
                    //                                      //Delete alerts about this period.

                    if (
                        //                                  //Notification not read.
                        !PsPrintShop.boolNotificationReadByUser(alertentity,
                            (int)alertentity.intnContactId)
                        )
                    {
                        AlnotAlertNotification.subReduceToOne(
                            (int)alertentity.intnContactId, iHubContext_I);
                    }

                    context.Alert.Remove(alertentity);
                }

                context.Period.Remove(perentity);
            }
            context.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subDeleteIOJentriesFromOthersWorkflows(
            int intJobId_I,
            int intPkWorkflow_I
            )
        {
            //                                              //Connect to DB.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get entries to delete from IOJ.
            List<IojentityInputsAndOutputsForAJobEntityDB> darriojentity =
                    (from iojentity in context.InputsAndOutputsForAJob
                     join piwentity in context.ProcessInWorkflow
                     on iojentity.intPkProcessInWorkflow equals piwentity.intPk
                     where iojentity.intJobId == intJobId_I && piwentity.intPkWorkflow != intPkWorkflow_I
                     select iojentity).ToList();

            //                                              //Delete IOJ's entries.
            foreach (IojentityInputsAndOutputsForAJobEntityDB iojentity in darriojentity)
            {
                context.InputsAndOutputsForAJob.Remove(iojentity);
            }
            context.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteOthersWorkflowsEstimates(
            int intJobId_I
            )
        {
            //                                              //Connect to DB.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //EstimatesData.
            List<EstdataentityEstimationDataEntityDB> darrestdataentityEstimatesData =
            (
                from estdata in context.EstimationData
                where estdata.intJobId == intJobId_I
                select estdata
            ).ToList();
            foreach (EstdataentityEstimationDataEntityDB estdata in darrestdataentityEstimatesData)
            {
                context.EstimationData.Remove(estdata);
            }

            //                                              //Estimates.
            List<EstentityEstimateEntityDB> darrestentityEstimates =
            (
                from est in context.Estimate
                where est.intJobId == intJobId_I
                select est
            ).ToList();
            foreach (EstentityEstimateEntityDB est in darrestentityEstimates)
            {
                context.Estimate.Remove(est);
            }

            //                                              //Find Resource'periods and Process´periods 
            //                                              //      in this Job that no apply for this WF (temporary)
            //                                              //      periods. 
            List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per => per.intJobId == intJobId_I &&
                per.intnEstimateId != null).ToList();

            //                                              //Delete periods that are not from this workflow.
            foreach (PerentityPeriodEntityDB perentity in darrperentity)
            {
                context.Period.Remove(perentity);
            }
            context.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteOthersWorkflowsPrices(
            int intJobId_I,
            int intPkWorkflow_I
            )
        {
            //                                              //Connect to DB.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find all prices except the ones for current wf.
            List<PriceentityPriceEntityDB> darrpriceentity = context.Price.Where(price =>
                price.intJobId == intJobId_I && price.intnPkWorkflow != intPkWorkflow_I).ToList();

            foreach (PriceentityPriceEntityDB priceentity in darrpriceentity)
            {
                context.Price.Remove(priceentity);
            }

            context.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subUpdateProcessStage(
            //                                              //Update process's stage in a job.
            PsPrintShop ps_I,
            int intContactId_I,
            int intPkProcessInWorkflow_I,
            int intJobId_I,
            //                                              //Can be:
            //                                              //JobJob.intProcessInWorkflowStarted
            //                                              //JobJob.intProcessInWorkflowCompleted
            int intStage_I,
            Odyssey2Context context_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            ref bool boolAskEmailNeedsToBeSent_IO,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            JobjsonJobJson jobjsonJob;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjsonJob,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Find PIW.
                PiwentityProcessInWorkflowEntityDB piwentity = context_I.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == intPkProcessInWorkflow_I);

                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Invalid Piw.";
                if (
                    piwentity != null
                    )
                {
                    //                                      //Get correct processes based on specific job.
                    int intPkWorkflow = piwentity.intPkWorkflow;
                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentity;
                    List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                    ProdtypProductType.subGetWorkflowValidWay(intPkWorkflow, jobjsonJob, out darrpiwentity,
                        out darrdynlkjson);

                    if (
                        JobJob.boolIsValidJobAndPiw(intPkProcessInWorkflow_I, intJobId_I, darrpiwentity, context_I,
                        ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO)
                        )
                    {
                        //                                  //Get current register from piwj table.
                        PiwjentityProcessInWorkflowForAJobEntityDB piwjentity =
                            context_I.ProcessInWorkflowForAJob.FirstOrDefault(piwj =>
                            piwj.intJobId == intJobId_I &&
                            piwj.intPkPrintshop == ps_I.intPk &&
                            piwj.intPkProcessInWorkflow == intPkProcessInWorkflow_I);

                        if (
                            //                              //The printers wants to start the process.
                            intStage_I == JobJob.intProcessInWorkflowStarted
                            )
                        {
                            intStatus_IO = 401;
                            strUserMessage_IO = "Cannot set a process as completed before being started.";
                            strDevMessage_IO = "";
                            if (
                                //                          //The process has not been started yet.
                                piwjentity == null
                            )
                            {
                                intStatus_IO = 401;
                                strUserMessage_IO = "This process has dependency on starting process.";
                                strDevMessage_IO = "";
                                if (
                                    //                      //Links only on outputs or 
                                    //                      //other link side is completed.
                                    JobJob.boolIsAbleToStart(intJobId_I, ps_I, piwentity, darrpiwentity,
                                        darrdynlkjson, context_I)
                                    )
                                {
                                    //                      //Create Register.
                                    piwjentity = new PiwjentityProcessInWorkflowForAJobEntityDB
                                    {
                                        intJobId = intJobId_I,
                                        intPkPrintshop = ps_I.intPk,
                                        intStage = intStage_I,
                                        strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                        strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                        intPkProcessInWorkflow = intPkProcessInWorkflow_I
                                    };
                                    context_I.ProcessInWorkflowForAJob.Add(piwjentity);

                                    intStatus_IO = 200;
                                    strUserMessage_IO = "";
                                    strDevMessage_IO = "";
                                }
                            }
                        }
                        else if (
                            //                              //The printers wants to complete the process.
                            intStage_I == JobJob.intProcessInWorkflowCompleted
                            )
                        {
                            intStatus_IO = 404;
                            strUserMessage_IO = "Process is not started or already completed.";
                            strDevMessage_IO = "";
                            if (
                                piwjentity != null &&
                                piwjentity.intStage == JobJob.intProcessInWorkflowStarted
                                )
                            {
                                //                          //All periods for this process.
                                List<PerentityPeriodEntityDB> darrperentity = context_I.Period.Where(per =>
                                   per.intPkWorkflow == piwentity.intPkWorkflow &&
                                   per.intProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
                                   per.intJobId == intJobId_I).ToList();

                                //                          //Periods with contact id, started and not finished.
                                List<PerentityPeriodEntityDB> darrperentityStartedNotFinishedWithContactId =
                                   darrperentity.Where(per =>
                                   per.strFinalEndDate == null &&
                                   per.strFinalEndTime == null &&
                                   per.strFinalStartDate != null &&
                                   per.strFinalStartTime != null &&
                                   per.intnContactId != null).ToList();

                                intStatus_IO = 405;
                                strUserMessage_IO = "All started periods must be completed.";
                                strDevMessage_IO = "";
                                if (
                                    //                      //All started periods are completed.
                                    darrperentityStartedNotFinishedWithContactId.Count == 0
                                    )
                                {
                                    String strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                                    String strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();

                                    //                      //Update register.
                                    //                      //Update table.
                                    piwjentity.intStage = intStage_I;
                                    piwjentity.strEndDate = strEndDate;
                                    piwjentity.strEndTime = strEndTime;
                                    context_I.ProcessInWorkflowForAJob.Update(piwjentity);
                                    //                      //This save is necessary to verify how many processes 
                                    //                      //      are completed in order to use 
                                    //                      //      subUpdateJobStageToCompleted correctly.
                                    context_I.SaveChanges();

                                    //                      //Periods Not started.
                                    List<PerentityPeriodEntityDB> darrperentityNotStarted = darrperentity.Where(per =>
                                        (per.strFinalStartDate == null) && (per.strFinalStartTime == null)).ToList();
                                    List<int> darrintContactIdToReduce = new List<int>();
                                    foreach (PerentityPeriodEntityDB perentity in darrperentityNotStarted)
                                    {
                                        perentity.strFinalEndDate = strEndDate;
                                        perentity.strFinalEndTime = strEndTime;
                                        context_I.Period.Update(perentity);

                                        //                  //Get the alert for this period and delete it.
                                        AlertentityAlertEntityDB alertentityToDelete =
                                            context_I.Alert.FirstOrDefault(alert =>
                                            alert.intnPkPeriod == perentity.intPk);
                                        if (
                                            alertentityToDelete != null
                                            )
                                        {
                                            context_I.Alert.Remove(alertentityToDelete);

                                            darrintContactIdToReduce.Add((int)alertentityToDelete.intnContactId);
                                        }
                                    }

                                    //                      //Method to update job stage.
                                    JobJob.subUpdateJobStageToCompleted(context_I, intJobId_I, ps_I, intContactId_I,
                                        darrpiwentity, configuration_I, ref boolAskEmailNeedsToBeSent_IO);

                                    //                      //Get the piw that are linked with this.
                                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityNext =
                                        JobJob.darrpiwentityGetNextProcessInWorkflow(piwentity, darrpiwentity,
                                        darrdynlkjson, context_I);

                                    //                      //Get the periods for the following piw.
                                    List<PerentityPeriodEntityDB> darrperentityFollowing =
                                        new List<PerentityPeriodEntityDB>();
                                    foreach (PiwentityProcessInWorkflowEntityDB piwentityNext in darrpiwentityNext)
                                    {
                                        List<PerentityPeriodEntityDB> darrperentityFromPIW = context_I.Period.Where(
                                            per =>
                                        per.intJobId == intJobId_I && per.intPkWorkflow ==
                                        piwentityNext.intPkWorkflow &&
                                        per.intProcessInWorkflowId == piwentityNext.intProcessInWorkflowId).ToList();

                                        darrperentityFollowing.AddRange(darrperentityFromPIW);
                                    }

                                    //                      //Find alert type related to periods.
                                    AlerttypeentityAlertTypeEntityDB alerttypeentity =
                                        context_I.AlertType.FirstOrDefault(at =>
                                        at.strType == AlerttypeentityAlertTypeEntityDB.strPeriod);

                                    List<int> darrintContactToNotify = new List<int>();
                                    foreach (PerentityPeriodEntityDB perentity in darrperentityFollowing)
                                    {
                                        if (
                                            perentity.intnContactId != null
                                            )
                                        {
                                            darrintContactToNotify.Add((int)perentity.intnContactId);
                                            AlertentityAlertEntityDB alertentity = new AlertentityAlertEntityDB
                                            {
                                                intPkAlertType = alerttypeentity.intPk,
                                                intPkPrintshop = ps_I.intPk,
                                                intnJobId = intJobId_I,
                                                intnContactId = perentity.intnContactId,
                                                intnPkPeriod = perentity.intPk
                                            };
                                            context_I.Alert.Add(alertentity);
                                        }
                                    }

                                    if (
                                        darrintContactToNotify.Count > 0
                                        )
                                    {
                                        //                      //Get strJobNumber.
                                        String strJobNumber = JobJob.strGetJobNumber(null, intJobId_I, ps_I.strPrintshopId,
                                            context_I);

                                        //                  //Send the notification to the corresponding contacts.
                                        AlnotAlertNotification.subSendToAFew(ps_I.strPrintshopId,
                                            darrintContactToNotify.ToArray(), alerttypeentity.strDescription +
                                            strJobNumber + ".", iHubContext_I);
                                    }

                                    if (
                                        darrintContactIdToReduce.Count > 0
                                        )
                                    {
                                        //                  //Reduce the notification to the corresponding contacts.           
                                        AlnotAlertNotification.subReduceToAFew(ps_I.strPrintshopId,
                                            darrintContactIdToReduce.ToArray(), iHubContext_I);
                                    }


                                    //                      //Add first movement.
                                    JobJob.subAddFirstMovement(jobjsonJob, ps_I.strPrintshopId, piwentity,
                                        intContactId_I, configuration_I, context_I, ref intStatus_IO,
                                        ref strUserMessage_IO, ref strDevMessage_IO);

                                    if (
                                        //                  //It is OK.
                                        intStatus_IO == 200
                                        )
                                    {
                                        intStatus_IO = 200;
                                        strUserMessage_IO = "";
                                        strDevMessage_IO = "";
                                    }
                                }
                            }
                        }
                        context_I.SaveChanges();
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subAddFirstMovement(
            //                                              //Add first movements.

            JobjsonJobJson jobjsonJob_I,
            String strPrintshopId_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            int intContactId_I,
            IConfiguration configuration_I,
            Odyssey2Context context_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Process is completed.
            PiwjentityProcessInWorkflowForAJobEntityDB piwjentity =
                context_I.ProcessInWorkflowForAJob.FirstOrDefault(piwj => piwj.intStage == 2 &&
                piwj.intPkProcessInWorkflow == piwentity_I.intPk && piwj.intJobId == jobjsonJob_I.intJobId);

            intStatus_IO = 404;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Process in workflow is not completed.";
            if (
                piwjentity != null
                )
            {
                int intPkWorkflow = piwentity_I.intPkWorkflow;
                int intPkProcess = piwentity_I.intPkProcess;
                int intPkProduct = (int)context_I.Workflow.FirstOrDefault(wf => wf.intPk == intPkWorkflow).intnPkProduct;

                //                                          //Get the product.
                ProdtypProductType prodtyp = (ProdtypProductType)EtElementTypeAbstract.etFromDB(intPkProduct);

                //                                          //Find Job.
                JobentityJobEntityDB jobentity = context_I.Job.FirstOrDefault(job =>
                    job.intJobID == jobjsonJob_I.intJobId && job.intPkWorkflow == intPkWorkflow);

                String strProcess = context_I.Element.FirstOrDefault(pro => pro.intPk == intPkProcess).strElementName;

                //                                          //Find base costs by process.
                JobJob.subAddFirstMovementBaseByProcess(intPkProcess, strProcess, intPkWorkflow, piwentity_I.intPk,
                    jobentity, jobjsonJob_I, prodtyp, intContactId_I, context_I);

                //                                          //Find per quantity costs by process.
                JobJob.subAddFirstMovementPQByProcess(intPkProcess, strProcess, intPkWorkflow, piwentity_I.intPk,
                    jobentity, jobjsonJob_I, prodtyp, intContactId_I, context_I);

                //                                          //Find per quantity costs by resource.
                JobJob.subAddFirstMovementByResource(piwentity_I, jobentity, jobjsonJob_I, strPrintshopId_I, prodtyp,
                    intContactId_I, configuration_I, context_I);

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subAddFirstMovementBaseByProcess(
            int intPkProcess_I,
            String strProcess_I,
            int intPkWorkflow_I,
            int intPkProcessInWorkflow_I,
            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjsonJob_I,
            ProdtypProductType prodtyp_I,
            int intContactId_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Json to return.
            List<BscostjsonBaseCostJson> darrbscostjson = new List<BscostjsonBaseCostJson>();

            int intPkJob = jobentity_I.intPk;
            //                                              //Get calculations in this wf and job.
            List<CalCalculation> darrcalentity = prodtyp_I.darrcalGetCalculationsCurrentByJobsStageAndWFFromDB(intPkJob,
                intPkWorkflow_I);
            //                                              //Filter them by cost, process and base.
            darrcalentity = darrcalentity.Where(cal => cal.boolIsEnable &&
                cal.strEndDate == null && cal.strEndTime == null &&
                cal.intnPkProcessElementBelongsTo == intPkProcess_I && cal.numnCost != null &&
                cal.strCalculationType == CalCalculation.strBase &&
                cal.strByX == CalCalculation.strByProcess).ToList();

            if (
                //                                         //Calculations found.
                darrcalentity.Count > 0
                )
            {
                foreach (CalCalculation cal in darrcalentity)
                {
                    if (
                        //                                  //Calculation applies.
                        Tools.boolCalculationOrLinkApplies(cal.intPk, null, null, null, jobjsonJob_I)
                        )
                    {
                        double numBaseCost = ((double)cal.numnCost).Round(2);

                        //                                  //Add first record to AccountMovement.
                        AccmoventityAccountMovementEntityDB accmoventityBaseByProcess =
                            new AccmoventityAccountMovementEntityDB
                            {
                                strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                strConcept = strProcess_I + " - " + cal.strDescription,
                                numnIncrease = numBaseCost,
                                intnJobId = jobentity_I.intJobID,
                                intPkAccount = (int)cal.intnPkAccount
                            };
                        context_I.AccountMovement.Add(accmoventityBaseByProcess);
                        context_I.SaveChanges();

                        //                                  //Generate an increase amount to the account's balance
                        AccAccounting.subUpdateAccountBalance((int)cal.intnPkAccount, null, numBaseCost,
                            context_I);

                        //                                  //Add first record to FinalCost
                        //                                  //    for not again to calculate.
                        FnlcostentityFinalCostEntityDB fnlcostentityBaseByProcess = new FnlcostentityFinalCostEntityDB
                        {
                            numnCost = numBaseCost,
                            numnQuantity = null,
                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                            strDescription = "Final Cost",
                            intContactId = intContactId_I,
                            intPkJob = intPkJob,
                            intPkProcessInWorkflow = intPkProcessInWorkflow_I,
                            intnPkCalculation = cal.intPk,
                            intPkAccountMovement = accmoventityBaseByProcess.intPk
                        };

                        context_I.FinalCost.Add(fnlcostentityBaseByProcess);
                    }
                    context_I.SaveChanges();
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subAddFirstMovementPQByProcess(
            int intPkProcess_I,
            String strProcess_I,
            int intPkWorkflow_I,
            int intPkProcessInWorkflow_I,
            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjsonJob_I,
            ProdtypProductType prodtyp_I,
            int intContactId_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Array of Jsons which will contain each calculation.
            List<PqcostjsonPerQuantityCostJson> darrpqcostjson = new List<PqcostjsonPerQuantityCostJson>();

            int intPkJob = jobentity_I.intPk;
            //                                              //Get calculations in this wf and job.
            List<CalCalculation> darrcalentity = prodtyp_I.darrcalGetCalculationsCurrentByJobsStageAndWFFromDB(intPkJob,
                intPkWorkflow_I);

            //                                              //Filter them by wf, by cost, process and perquantity.
            darrcalentity = darrcalentity.Where(cal => cal.boolIsEnable &&
                cal.strEndDate == null && cal.strEndTime == null &&
                cal.intnPkProcessElementBelongsTo == intPkProcess_I && cal.numnCost != null &&
                cal.strCalculationType == CalCalculation.strPerQuantity &&
                cal.strByX == CalCalculation.strByProcess).ToList();

            if (
                darrcalentity.Count > 0
                )
            {
                foreach (CalCalculation cal in darrcalentity)
                {
                    if (
                        //                                  //Calculation applies.
                        Tools.boolCalculationOrLinkApplies(cal.intPk, null, null, null, jobjsonJob_I)
                        )
                    {
                        //                                  //Variables to return.
                        double numQuantityWithoutWasteNotUsed;
                        double numWasteCalculatedNotUsed;
                        double numFactorNotUsed;
                        double numQuantity;
                        bool boolJobWorkflowIsReadyNotUsed = true;
                        int intStatus = 200; String strUserMessage = ""; String strDevMessage = "";
                        double numCost = (ProdtypProductType.numGetPerQuantityCost(cal, jobjsonJob_I,
                            jobjsonJob_I.intnQuantity, true, null, intPkProcessInWorkflow_I, null,
                            out numQuantityWithoutWasteNotUsed, out numQuantity, out numWasteCalculatedNotUsed,
                            out numFactorNotUsed, ref boolJobWorkflowIsReadyNotUsed,
                            ref intStatus, ref strUserMessage, ref strDevMessage)).Round(2);

                        //                                  //Add first record to AccountMovement.
                        AccmoventityAccountMovementEntityDB accmoventityBaseByProcess =
                            new AccmoventityAccountMovementEntityDB
                            {
                                strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                strConcept = strProcess_I + " - " + cal.strDescription,
                                numnIncrease = numCost,
                                intnJobId = jobentity_I.intJobID,
                                intPkAccount = (int)cal.intnPkAccount
                            };
                        context_I.AccountMovement.Add(accmoventityBaseByProcess);
                        context_I.SaveChanges();

                        //                                  //Generate an increase amount to the account's balance
                        AccAccounting.subUpdateAccountBalance((int)cal.intnPkAccount, null, numCost,
                            context_I);

                        //                                  //Add first record to FinalCost
                        //                                  //    for not again to calculate.
                        FnlcostentityFinalCostEntityDB fnlcostentityBaseByProcess = new FnlcostentityFinalCostEntityDB
                        {
                            numnCost = numCost,
                            numnQuantity = numQuantity,
                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                            strDescription = "Final Cost",
                            intContactId = intContactId_I,
                            intPkJob = intPkJob,
                            intPkProcessInWorkflow = intPkProcessInWorkflow_I,
                            intnPkCalculation = cal.intPk,
                            intPkAccountMovement = accmoventityBaseByProcess.intPk
                        };

                        context_I.FinalCost.Add(fnlcostentityBaseByProcess);
                    }
                }
                context_I.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subAddFirstMovementByResource(
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjsonJob_I,
            String strPrintshopId_I,
            ProdtypProductType prodtyp_I,
            int intContactId_I,
            IConfiguration configuration_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Get all the processes.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
            List<DynLkjsonDynamicLinkJson> darrdynlkjson;
            ProdtypProductType.subGetWorkflowValidWay(piwentity_I.intPkWorkflow, jobjsonJob_I,
                out darrpiwentityAllProcesses, out darrdynlkjson);

            //                                              //Dictionary to store inputs and outputs of a process.
            prodtyp_I.dicProcessIOs = new Dictionary<int, List<Iofrmpiwjson2IOFromPIWJson2>>();
            //                                              //List to store resource thickness.
            prodtyp_I.darrresthkjsonResThickness = new List<ResthkjsonResourceThicknessJson>();
            //
            if (
                //                                          //It is Postprocess.
                piwentity_I.boolIsPostProcess
                )
            {
                //                                          //It is PostProcess..
                JobJob.subAddFirstMovementByResourceByPostProcess(darrpiwentityAllProcesses, piwentity_I,
                    darrdynlkjson, prodtyp_I, jobjsonJob_I, strPrintshopId_I, jobentity_I, intContactId_I,
                    configuration_I, context_I);
            }
            else
            {
                //                                          //It is normal process.
                JobJob.subAddFirstMovementByResourceByNormalProcess(darrpiwentityAllProcesses, piwentity_I,
                    darrdynlkjson, prodtyp_I, jobjsonJob_I, strPrintshopId_I, jobentity_I, intContactId_I,
                    configuration_I, context_I);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subAddFirstMovementByResourceByNormalProcess(
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcess_I,
            PiwentityProcessInWorkflowEntityDB piwentityNormalProcess_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            ProdtypProductType prodtyp_I,
            JobjsonJobJson jobjsonJob_I,
            String strPrintshopId_I,
            JobentityJobEntityDB jobentity_I,
            int intContactId_I,
            IConfiguration configuration_I,
            Odyssey2Context context_I
            )
        {
            prodtyp_I.subAddCalculationsBasedOnJobStatus(jobentity_I, darrpiwentityAllProcess_I, context_I);

            //                                              //List of quantityInputs and quantityOutputs.
            //                                              //    for optimization.
            List<IoqytjsonIOQuantityJson> darrioqytjsonIOQuantity = new List<IoqytjsonIOQuantityJson>();

            //                                              //List of waste to propagate.                          
            List<WstpropjsonWasteToPropagateJson> darrwstpropjson = new List<WstpropjsonWasteToPropagateJson>();

            //                                              //The lists are for optimization

            //                                              //Get eleet-s.
            List<EleetentityElementElementTypeEntityDB> darreleetentityAllEleEt = context_I.ElementElementType.Where(
                eleet => eleet.intPkElementDad == piwentityNormalProcess_I.intPkProcess).ToList();

            //                                              //Get eleele-s.
            List<EleeleentityElementElementEntityDB> darreleeleentityAllEleEle = context_I.ElementElement.Where(
                eleele => eleele.intPkElementDad == piwentityNormalProcess_I.intPkProcess).ToList();

            //                                              //Get io-s.
            List<IoentityInputsAndOutputsEntityDB> darrioentityAllIO = context_I.InputsAndOutputs.Where(io =>
                io.intPkWorkflow == piwentityNormalProcess_I.intPkWorkflow &&
                io.intnProcessInWorkflowId == piwentityNormalProcess_I.intProcessInWorkflowId).ToList();

            //                                              //Get ioj-s.
            List<IojentityInputsAndOutputsForAJobEntityDB> darriojentityAllIOJ = 
                context_I.InputsAndOutputsForAJob.Where(ioj => 
                ioj.intPkProcessInWorkflow == piwentityNormalProcess_I.intPk &&
                ioj.intJobId == jobjsonJob_I.intJobId).ToList();

            if (
                !prodtyp_I.dicProcessIOs.ContainsKey(piwentityNormalProcess_I.intPk)
                )
            {
                List<Iofrmpiwjson2IOFromPIWJson2> darrioinfrmpiwjson2IosFromPIW;
                ProdtypProductType.subGetProcessInputsAndOutputs(jobjsonJob_I, piwentityNormalProcess_I, prodtyp_I,
                    darreleeleentityAllEleEle, darreleetentityAllEleEt, out darrioinfrmpiwjson2IosFromPIW);

                prodtyp_I.dicProcessIOs.Add(piwentityNormalProcess_I.intPk, darrioinfrmpiwjson2IosFromPIW);
            }

            //                                              //Get the process.
            EleentityElementEntityDB eleentityProcess = context_I.Element.
                FirstOrDefault(ele => ele.intPk == piwentityNormalProcess_I.intPkProcess);

            double numJobFinalCostNotused = 0;

            //                                              //List to Add IO Inputs.
            List<Iojson1InputOrOutputJson1> darriojson1Input = new List<Iojson1InputOrOutputJson1>();

            bool boolWorkflowJobIsReadyNotUsed = true;
            
            darriojson1Input.AddRange(prodtyp_I.arriojson1GetTypes(true, jobentity_I, jobjsonJob_I, 
                piwentityNormalProcess_I, darrdynlkjson_I, darreleetentityAllEleEt, darrioentityAllIO, 
                darriojentityAllIOJ, darrpiwentityAllProcess_I, darrioqytjsonIOQuantity, darrwstpropjson,
                ref numJobFinalCostNotused, ref boolWorkflowJobIsReadyNotUsed));

            //                                              //Get the input templates.
            darriojson1Input.AddRange(prodtyp_I.arriojson1GetTemplates(true, jobentity_I, jobjsonJob_I,
                piwentityNormalProcess_I, darrdynlkjson_I, darreleeleentityAllEleEle, darrioentityAllIO, 
                darriojentityAllIOJ, darrpiwentityAllProcess_I, darrioqytjsonIOQuantity, darrwstpropjson,
                ref numJobFinalCostNotused, ref boolWorkflowJobIsReadyNotUsed));

            IoqytjsonIOQuantityJson ioqytjsonWasPropagate = darrioqytjsonIOQuantity.FirstOrDefault(
                ioqyt => ioqyt.intPkProcessInWorkflow == piwentityNormalProcess_I.intPk);

            //                                              //This PIW was not analized or is the first PIW.
            ProdtypProductType.subPropagateWaste(jobjsonJob_I, piwentityNormalProcess_I, prodtyp_I,
                darrwstpropjson, configuration_I, strPrintshopId_I, null, ref darriojson1Input);

            ProdtypProductType.CalculateTime(jobjsonJob_I, piwentityNormalProcess_I, configuration_I, strPrintshopId_I,
                ref darriojson1Input, null);

            //                                              //Only resources without link have calculations.
            darriojson1Input = darriojson1Input.Where(res => res.strLink == null).ToList();

            //                                              //Total extra cost per process.
            //                                              //This variable must be use for inputs and outputs.
            double numProcessExtraCost_IO = 0.0;

            //                                              //Increase cost to input resources that contain an hourly 
            //                                              //      rate.
            ProdtypProductType.subCalculateResourcesHourlyRates(context_I, ref darriojson1Input, 
                ref numProcessExtraCost_IO);

            foreach (Iojson1InputOrOutputJson1 iojson1Res in darriojson1Input)
            {
                if (
                    iojson1Res.intnPkResource != null &&
                    iojson1Res.intnPkResource > 0 &&
                    iojson1Res.boolIsPhysical
                    )
                {
                    double numQuantity = iojson1Res.numQuantity;
                    double numCost = iojson1Res.numCostByResource.Round(2);

                    int? intnPkEleetOrEleele = iojson1Res.intPkEleetOrEleele;
                    int? intnPkEleet = null;
                    int? intnPkEleEle = null;
                    bool boolIsEleet = iojson1Res.boolIsEleet;
                    if (
                        boolIsEleet
                        )
                    {
                        intnPkEleet = iojson1Res.intPkEleetOrEleele;
                    }
                    else
                    {
                        intnPkEleEle = iojson1Res.intPkEleetOrEleele;
                    }

                    //                                      //The resource'cost always should have
                    //                                      //    an account associate.
                    CostentityCostEntityDB costentityByResource = context_I.Cost.FirstOrDefault(cost =>
                        cost.intPkResource == (int)iojson1Res.intnPkResource);

                    //                                      //Add first record to AccountMovement.
                    AccmoventityAccountMovementEntityDB accmoventityBaseByProcess =
                        new AccmoventityAccountMovementEntityDB
                        {
                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                            strConcept = iojson1Res.strResource + " - Final Cost",
                            numnIncrease = numCost,
                            intnJobId = jobentity_I.intJobID,
                            intPkAccount = (int)costentityByResource.intPkAccount
                        };
                    context_I.AccountMovement.Add(accmoventityBaseByProcess);
                    context_I.SaveChanges();

                    //                                  //Generate an increase amount to the account's balance
                    AccAccounting.subUpdateAccountBalance((int)costentityByResource.intPkAccount, null, numCost,
                        context_I);

                    //                                      //Add first record to FinalCost
                    //                                      //    for not again to calculate.
                    FnlcostentityFinalCostEntityDB fnlcostentityBaseByProcess = new FnlcostentityFinalCostEntityDB
                    {
                        numnCost = numCost,
                        numnQuantity = numQuantity,
                        strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                        strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                        strDescription = "Final Cost",
                        intContactId = intContactId_I,
                        intPkJob = jobentity_I.intPk,
                        intPkProcessInWorkflow = piwentityNormalProcess_I.intPk,
                        intnPkElementElementType = intnPkEleet,
                        intnPkElementElement = intnPkEleEle,
                        intnPkResource = (int)iojson1Res.intnPkResource,
                        intnPkCalculation = null,
                        intPkAccountMovement = accmoventityBaseByProcess.intPk
                    };

                    context_I.FinalCost.Add(fnlcostentityBaseByProcess);
                }
                context_I.SaveChanges();
            }
        }


        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subAddFirstMovementByResourceByPostProcess(
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcess_I,
            PiwentityProcessInWorkflowEntityDB piwentityPostProcess_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            ProdtypProductType prodtyp_I,
            JobjsonJobJson jobjsonJob_I,
            String strPrintshopId_I,
            JobentityJobEntityDB jobentity_I,
            int intContactId_I,
            IConfiguration configuration_I,
            Odyssey2Context context_I
            )
        {
            //                                              //List of quantityInputs and quantityOutputs
            //                                              //    for optimization.
            List<IoqytjsonIOQuantityJson> darrioqytjsonIOQuantity = new List<IoqytjsonIOQuantityJson>();

            //                                              //Get eleet-s.
            List<EleetentityElementElementTypeEntityDB> darreleetentityAll = context_I.ElementElementType.Where(
                eleet => eleet.intPkElementDad == piwentityPostProcess_I.intPkProcess).ToList();

            //                                              //Get eleele-s.
            List<EleeleentityElementElementEntityDB> darreleeleentityAll = context_I.ElementElement.Where(
                eleele => eleele.intPkElementDad == piwentityPostProcess_I.intPkProcess).ToList();

            //                                              //Get io-s.
            List<IoentityInputsAndOutputsEntityDB> darrioentityAllIO = context_I.InputsAndOutputs.Where(io =>
                io.intPkWorkflow == piwentityPostProcess_I.intPkWorkflow &&
                io.intnProcessInWorkflowId == piwentityPostProcess_I.intProcessInWorkflowId).ToList();

            //                                              //Get ioj-s.
            List<IojentityInputsAndOutputsForAJobEntityDB> darriojentityAllIOJ = 
                context_I.InputsAndOutputsForAJob.Where(ioj => 
                ioj.intPkProcessInWorkflow == piwentityPostProcess_I.intPk &&
                ioj.intJobId == jobjsonJob_I.intJobId).ToList();

            if (
                !prodtyp_I.dicProcessIOs.ContainsKey(piwentityPostProcess_I.intPk)
                )
            {
                List<Iofrmpiwjson2IOFromPIWJson2> darrioinfrmpiwjson2IosFromPIW;
                ProdtypProductType.subGetProcessInputsAndOutputs(jobjsonJob_I, piwentityPostProcess_I, prodtyp_I,
                    darreleeleentityAll, darreleetentityAll, out darrioinfrmpiwjson2IosFromPIW);

                prodtyp_I.dicProcessIOs.Add(piwentityPostProcess_I.intPk, darrioinfrmpiwjson2IosFromPIW);
            }

            //                                              //Get the inputs and outputs for the process.
            //                                              //Get the process.
            EleentityElementEntityDB eleentityProcess = context_I.Element.
                FirstOrDefault(ele => ele.intPk == piwentityPostProcess_I.intPkProcess);

            //                                              //List to Add IO Inputs.
            List<Iojson1InputOrOutputJson1> darriojson1Input = new List<Iojson1InputOrOutputJson1>();

            double numJobFinalCost = 0.0;

            ////                                              //Get the input types.
            //darriojson1Input.AddRange(prodtyp_I.arriojson1GetTypesPostProcess(piwentityPostProcess_I, jobjsonJob_I,
            //    strPrintshopId_I, true, true, darrpiwentityAllProcess_I, darrdynlkjson_I, configuration_I,
            //    ref numJobFinalCost, ref darrioqytjsonIOQuantity));

            ////                                              //Get the input templates.
            //darriojson1Input.AddRange(prodtyp_I.arriojson1GetTemplatesPostProcess(piwentityPostProcess_I, jobjsonJob_I,
            //    strPrintshopId_I, true, true, darrpiwentityAllProcess_I, darrdynlkjson_I, configuration_I,
            //    ref numJobFinalCost, ref darrioqytjsonIOQuantity));

            //                                          //Get the input types.
            darriojson1Input.AddRange(prodtyp_I.arriojson1GetTypesPostProcess(true, strPrintshopId_I, jobentity_I, 
                jobjsonJob_I, piwentityPostProcess_I, darrdynlkjson_I, darreleetentityAll, darrioentityAllIO,
                darriojentityAllIOJ, darrpiwentityAllProcess_I, configuration_I, darrioqytjsonIOQuantity,
                ref numJobFinalCost));

            //                                          //Get the input templates.
            darriojson1Input.AddRange(prodtyp_I.arriojson1GetTemplatesPostProcess(true, strPrintshopId_I, jobentity_I,
                jobjsonJob_I, piwentityPostProcess_I, darrdynlkjson_I, darreleeleentityAll, darrioentityAllIO,
                darriojentityAllIOJ, darrpiwentityAllProcess_I, configuration_I, darrioqytjsonIOQuantity,
                ref numJobFinalCost));

            //                                              //Only resources without link have calculations.
            darriojson1Input = darriojson1Input.Where(res => res.strLink == null).ToList();

            ProdtypProductType.CalculateTime(jobjsonJob_I, piwentityPostProcess_I, configuration_I, strPrintshopId_I,
                 ref darriojson1Input, prodtyp_I.darriojsoninInputsCombinationsAndInputsSelected);

            //                                              //Total extra cost per process.
            //                                              //This variable must be use for inputs and outputs.
            double numProcessExtraCost_IO = 0.0;

            //                                              //Increase cost to input resources that contain an hourly 
            //                                              //      rate.
            ProdtypProductType.subCalculateResourcesHourlyRates(context_I, ref darriojson1Input, ref numProcessExtraCost_IO);

            foreach (Iojson1InputOrOutputJson1 iojson1Res in darriojson1Input)
            {
                if (
                    iojson1Res.intnPkResource != null &&
                    iojson1Res.intnPkResource > 0 &&
                    iojson1Res.boolIsPhysical
                    )
                {
                    double numQuantity = iojson1Res.numQuantity;
                    double numCost = iojson1Res.numCostByResource.Round(2);

                    int? intnPkEleetOrEleele = iojson1Res.intPkEleetOrEleele;
                    int? intnPkEleet = null;
                    int? intnPkEleEle = null;
                    bool boolIsEleet = iojson1Res.boolIsEleet;
                    if (
                        boolIsEleet
                        )
                    {
                        intnPkEleet = iojson1Res.intPkEleetOrEleele;
                    }
                    else
                    {
                        intnPkEleEle = iojson1Res.intPkEleetOrEleele;
                    }

                    //                                      //The resource'cost always should have
                    //                                      //    an account associate.
                    CostentityCostEntityDB costentityByResource = context_I.Cost.FirstOrDefault(cost =>
                        cost.intPkResource == (int)iojson1Res.intnPkResource);

                    //                                      //Add first record to AccountMovement.
                    AccmoventityAccountMovementEntityDB accmoventityBaseByProcess =
                        new AccmoventityAccountMovementEntityDB
                        {
                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                            strConcept = iojson1Res.strResource + " - Final Cost",
                            numnIncrease = numCost,
                            intnJobId = jobentity_I.intJobID,
                            intPkAccount = (int)costentityByResource.intPkAccount
                        };
                    context_I.AccountMovement.Add(accmoventityBaseByProcess);
                    context_I.SaveChanges();

                    //                                      //Generate an increase amount to the account's balance
                    AccAccounting.subUpdateAccountBalance((int)costentityByResource.intPkAccount, null, numCost,
                        context_I);

                    //                                      //Add first record to FinalCost
                    //                                      //    for not again to calculate.
                    FnlcostentityFinalCostEntityDB fnlcostentityBaseByProcess = new FnlcostentityFinalCostEntityDB
                    {
                        numnCost = numCost,
                        numnQuantity = numQuantity,
                        strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                        strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                        strDescription = "Final Cost",
                        intContactId = intContactId_I,
                        intPkJob = jobentity_I.intPk,
                        intPkProcessInWorkflow = piwentityPostProcess_I.intPk,
                        intnPkElementElementType = intnPkEleet,
                        intnPkElementElement = intnPkEleEle,
                        intnPkResource = (int)iojson1Res.intnPkResource,
                        intnPkCalculation = null,
                        intPkAccountMovement = accmoventityBaseByProcess.intPk
                    };

                    context_I.FinalCost.Add(fnlcostentityBaseByProcess);
                }
                context_I.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static List<PiwentityProcessInWorkflowEntityDB> darrpiwentityGetNextProcessInWorkflow(
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            Odyssey2Context context_I
            )
        {
            //                                              //List to return.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity = new List<PiwentityProcessInWorkflowEntityDB>();

            //                                              //Eleet outputs with link.
            List<IoentityInputsAndOutputsEntityDB> darrioentityOutputEleetWithLink = (
                    from ioentity in context_I.InputsAndOutputs
                    join eleetentity in context_I.ElementElementType
                    on ioentity.intnPkElementElementType equals eleetentity.intPk
                    where ioentity.strLink != null && ioentity.intPkWorkflow == piwentity_I.intPkWorkflow &&
                    ioentity.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId &&
                    eleetentity.boolUsage == false
                    select ioentity).ToList();

            foreach (IoentityInputsAndOutputsEntityDB ioentityEleet in darrioentityOutputEleetWithLink)
            {
                String strLink = ioentityEleet.strLink;
                PiwentityProcessInWorkflowEntityDB piwentityNext;
                IoentityInputsAndOutputsEntityDB ioentity;

                ProdtypProductType.subGetOtherSideOfTheLink(piwentity_I, ioentityEleet.intnPkElementElementType,
                    null, darrpiwentity_I, darrdynlkjson_I, context_I, ref strLink, out piwentityNext, out ioentity);

                if (
                    piwentityNext != null
                    )
                {
                    darrpiwentity.Add(piwentityNext);
                }
            }

            //                                              //Eleele outputs with link.
            List<IoentityInputsAndOutputsEntityDB> darrioentityOutputEleeleWithLink = (
                from ioentity in context_I.InputsAndOutputs
                join eleeleentity in context_I.ElementElement
                on ioentity.intnPkElementElement equals eleeleentity.intPk
                where ioentity.strLink != null && ioentity.intPkWorkflow == piwentity_I.intPkWorkflow &&
                ioentity.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId &&
                eleeleentity.boolUsage == false
                select ioentity).ToList();

            foreach (IoentityInputsAndOutputsEntityDB ioentityEleele in darrioentityOutputEleeleWithLink)
            {
                String strLink = ioentityEleele.strLink;
                PiwentityProcessInWorkflowEntityDB piwentityNext;
                IoentityInputsAndOutputsEntityDB ioentity;

                ProdtypProductType.subGetOtherSideOfTheLink(piwentity_I, null, ioentityEleele.intnPkElementElement,
                    darrpiwentity_I, darrdynlkjson_I, context_I, ref strLink, out piwentityNext, out ioentity);

                if (
                    piwentityNext != null
                    )
                {
                    darrpiwentity.Add(piwentityNext);
                }
            }

            return darrpiwentity;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolIsValidJobAndPiw(
            //                                              //Verify if it is a valid process in order to 
            //                                              //  update the stage.
            //                                              //Verify that PIW exists in job's workflow.

            int intPkProcessInWorkflow_I,
            int intJobId_I,
            //                                              //List containing job's PIWs.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity_I,
            Odyssey2Context context_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Get job from job table.
            JobentityJobEntityDB jobentity = context_I.Job.FirstOrDefault(job => job.intJobID == intJobId_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Job not found.";
            bool bollIsValidData = false;
            if (
                jobentity != null
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Job already completed.";
                if (
                    jobentity.intStage != JobJob.intCompletedStage
                    )
                {
                    int intI = 0;
                    bool boolPiwExistsInList = false;
                    /*WHILE-DO*/
                    while (
                        intI < darrpiwentity_I.Count &&
                        !boolPiwExistsInList
                        )
                    {
                        if (
                            //                              //PIW exists in list.
                            darrpiwentity_I[intI].intPk == intPkProcessInWorkflow_I
                            )
                        {
                            boolPiwExistsInList = true;
                        }

                        intI = intI + 1;
                    }

                    if (
                        boolPiwExistsInList
                        )
                    {
                        bollIsValidData = true;
                    }
                    else
                    {
                        intStatus_IO = 403;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Piw does not exist in current job's workflow.";
                    }
                }
            }
            return bollIsValidData;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolIsAbleToStart(
            int intJobId_I,
            PsPrintShop ps_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Get all IO with links for the process.
            List<IoentityInputsAndOutputsEntityDB> darrioentity = context_I.InputsAndOutputs.Where(io =>
                io.intPkWorkflow == piwentity_I.intPkWorkflow &&
                io.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId && io.strLink != null).ToList();

            bool boolIsAbleToStart;
            if (
                !JobJob.boolLinksOnInputs(darrioentity, context_I)
                )
            {
                boolIsAbleToStart = true;
            }
            else
            {
                boolIsAbleToStart = true;
                int intI = 0;
                /*WHILE-DO*/
                while (
                    (boolIsAbleToStart == true) &&
                    (intI < darrioentity.Count())
                    )
                {
                    if (
                        darrioentity[intI].intnPkElementElementType != null
                        )
                    {
                        EleetentityElementElementTypeEntityDB eleetentity =
                            context_I.ElementElementType.FirstOrDefault(eleet =>
                            eleet.intPk == darrioentity[intI].intnPkElementElementType);
                        if (
                            //                                      //Verify if is Input.
                            eleetentity.boolUsage
                            )
                        {
                            String strLink = darrioentity[intI].strLink;
                            PiwentityProcessInWorkflowEntityDB piwentityPre;
                            IoentityInputsAndOutputsEntityDB ioentity;

                            ProdtypProductType.subGetOtherSideOfTheLink(piwentity_I,
                                darrioentity[intI].intnPkElementElementType, null, darrpiwentity_I, darrdynlkjson_I,
                                context_I, ref strLink, out piwentityPre, out ioentity);

                            PiwjentityProcessInWorkflowForAJobEntityDB piwjentity =
                                    context_I.ProcessInWorkflowForAJob.FirstOrDefault(piwj =>
                                    piwj.intJobId == intJobId_I &&
                                    piwj.intPkPrintshop == ps_I.intPk &&
                                    piwj.intPkProcessInWorkflow == piwentityPre.intPk);
                            if (
                                piwjentity == null || piwjentity.intStage != JobJob.intProcessInWorkflowCompleted
                                )
                            {
                                //                                  //The process has not been started yet or the 
                                //                                  //      process isnot completed.
                                boolIsAbleToStart = false;
                            }
                        }
                    }
                    else
                    {
                        EleeleentityElementElementEntityDB eleeleentity =
                            context_I.ElementElement.FirstOrDefault(eleele => eleele.intPk ==
                            darrioentity[intI].intnPkElementElement);
                        if (
                            //                                      //Verify if is Input.
                            eleeleentity.boolUsage
                            )
                        {
                            String strLink = darrioentity[intI].strLink;
                            PiwentityProcessInWorkflowEntityDB piwentityPre;
                            IoentityInputsAndOutputsEntityDB ioentity;

                            ProdtypProductType.subGetOtherSideOfTheLink(piwentity_I,
                                null, darrioentity[intI].intnPkElementElement, darrpiwentity_I, darrdynlkjson_I,
                                context_I, ref strLink, out piwentityPre, out ioentity);

                            //                                      //Verify if process is completed.
                            PiwjentityProcessInWorkflowForAJobEntityDB piwjentity =
                                    context_I.ProcessInWorkflowForAJob.FirstOrDefault(piwj =>
                                    piwj.intJobId == intJobId_I &&
                                    piwj.intPkPrintshop == ps_I.intPk &&
                                    piwj.intPkProcessInWorkflow == piwentityPre.intPk);
                            if (
                                piwjentity == null || piwjentity.intStage != JobJob.intProcessInWorkflowCompleted
                                )
                            {
                                //                                  //The process has not been started yet or the process is
                                //                                  //  not completed.
                                boolIsAbleToStart = false;
                            }
                        }
                    }
                    intI = intI + 1;
                }
            }
            return boolIsAbleToStart;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolLinksOnInputs(

            List<IoentityInputsAndOutputsEntityDB> darrioentity_I,
            Odyssey2Context context_I
            )
        {
            //                                          //Verify if the process has just links on outputs.
            //                                          //Means this Piw does not depend on other Piw's.
            bool boolLinksOnInputs = false;
            int intI = 0;
            /*WHILE-DO */
            while (
                (boolLinksOnInputs == false) &&
                (intI < darrioentity_I.Count())
                )
            {
                if (
                    darrioentity_I[intI].intnPkElementElementType != null
                    )
                {
                    EleetentityElementElementTypeEntityDB eleetentity =
                        context_I.ElementElementType.FirstOrDefault(eleet => eleet.intPk ==
                        darrioentity_I[intI].intnPkElementElementType);
                    if (
                        eleetentity.boolUsage
                        )
                    {
                        boolLinksOnInputs = true;
                    }
                }
                else
                {
                    EleeleentityElementElementEntityDB eleeleentity =
                        context_I.ElementElement.FirstOrDefault(eleele => eleele.intPk ==
                        darrioentity_I[intI].intnPkElementElement);
                    if (
                        eleeleentity.boolUsage
                        )
                    {
                        boolLinksOnInputs = true;
                    }
                }
                intI = intI + 1;
            }

            return boolLinksOnInputs;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subUpdateJobStageToCompleted(
            //                                              //Verify and update job stage.

            Odyssey2Context context_I,
            int intJobId_I,
            PsPrintShop ps_I,
            int intContactId_I,
            //                                              //Processes in job's workflow.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity_I,
            IConfiguration configuration_I,
            ref bool boolAskEmailNeedsToBeSent_IO
            )
        {
            //                                              //Get job's total Piw´s.
            int intPiwTotal = darrpiwentity_I.Count;

            //                                              //Get completed process for a specific job.
            int intPiwCompletedTotal = context_I.ProcessInWorkflowForAJob.Where(piwj => piwj.intJobId == intJobId_I &&
                piwj.intPkPrintshop == ps_I.intPk &&
                piwj.intStage == JobJob.intProcessInWorkflowCompleted).ToList().Count;

            if (
                //                                          //Job is finish.
                (intPiwCompletedTotal <= intPiwTotal) &&
                (intPiwCompletedTotal / intPiwTotal == 1)
                )
            {
                //                                          //Get job to update.
                JobentityJobEntityDB jobjob = context_I.Job.FirstOrDefault(job => job.intJobID == intJobId_I);

                //                                          //Update job.
                jobjob.intStage = JobJob.intCompletedStage;
                jobjob.strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                jobjob.strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();
                context_I.Job.Update(jobjob);
                //                                          //Need to know if the order is completed ir order to be
                //                                          //      able to send or not an email.
                context_I.SaveChanges();                

                //                                          //Update InProgress to completed on Wisnet.
                JobJob.subUpdateJobInProgressToCompleteOnWisnet(jobjob, ps_I, intContactId_I, context_I,
                    configuration_I);

                //                                          //To know if we have to ask if a email wants to be sent.
                boolAskEmailNeedsToBeSent_IO = JobJob.boolAskForSendAnEmail(intJobId_I, ps_I);

                //                                          //Get Workflow.
                PiwentityProcessInWorkflowEntityDB piwentity = darrpiwentity_I[0];

                //                                          //Verify if the jobs has already a price.
                List<PriceentityPriceEntityDB> darrpriceentity = context_I.Price.Where(price =>
                    price.intJobId == intJobId_I &&
                    price.intnPkWorkflow == piwentity.intPkWorkflow).ToList();

                if (
                    darrpriceentity.Count > 0
                    )
                {
                    PriceentityPriceEntityDB priceentity = darrpriceentity.Last();

                    if (
                        priceentity.boolIsReset == true
                        )
                    {
                        int intStatus_IO = 200;
                        String strUserMessage_IO = "";
                        String strDevMessage_IO = "";
                        double numprice = JobJob.numGetEstimatedPriceForAJob(intJobId_I, piwentity.intPkWorkflow, ps_I,
                                configuration_I, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                        //                                      //Create register at price table.
                        PriceentityPriceEntityDB priceentityToAdd = new PriceentityPriceEntityDB
                        {
                            numnPrice = (numprice).Round(2),
                            intJobId = intJobId_I,
                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                            strDescription = "Price Estimated",
                            intContactId = intContactId_I,
                            boolIsReset = false,
                            intnPkWorkflow = piwentity.intPkWorkflow,
                            intnPkEstimate = null
                        };
                        context_I.Price.Add(priceentityToAdd);
                    }
                }
                else
                {
                    int intStatus_IO = 200;
                    String strUserMessage_IO = "";
                    String strDevMessage_IO = "";
                    double numprice = JobJob.numGetEstimatedPriceForAJob(intJobId_I, piwentity.intPkWorkflow, ps_I,
                            configuration_I, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                    //                                      //Create register at price table.
                    PriceentityPriceEntityDB priceentityToAdd = new PriceentityPriceEntityDB
                    {
                        numnPrice = (numprice).Round(2),
                        intJobId = intJobId_I,
                        strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                        strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                        strDescription = "Price Estimated",
                        intContactId = intContactId_I,
                        boolIsReset = false,
                        intnPkWorkflow = piwentity.intPkWorkflow,
                        intnPkEstimate = null
                    };
                    context_I.Price.Add(priceentityToAdd);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolAskForSendAnEmail(
            //                                              //Verify if the order for a given job is already completed.

            int intJobId_I,
            PsPrintShop ps_I
            )
        {
            bool boolAskForSendAnEmail = false;

            //                                              //To know if order is already completed.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];

            Task<bool?> Task_boolOrderCompleted =
                HttpTools<TjsonTJson>.boolOrderCompleted(strUrlWisnet +
                "/Job/JobOrderCompleted/" + intJobId_I.ToString() + "/" + ps_I.strPrintshopId);
            Task_boolOrderCompleted.Wait();

            if (
                //                                          //There is access to the service of Wisnet.
                Task_boolOrderCompleted.Result != null
                )
            {
                boolAskForSendAnEmail = (bool)Task_boolOrderCompleted.Result;
            }

            return boolAskForSendAnEmail;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subUpdateJobInProgressToCompleteOnWisnet(
            //                                              //Turn an JobInProgress into an JobCompleted on 
            //                                              //    Wisnet.

            JobentityJobEntityDB jobjob_M,
            PsPrintShop ps_I,
            int intContactId_I,
            Odyssey2Context context_M,
            IConfiguration configuration_I
            )
        {
            //                                          //Send json to turn estimate into rejected.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];

            SendtowisjsoninSendToWisnetJsonInternal sendtowis = new SendtowisjsoninSendToWisnetJsonInternal(
                jobjob_M.intJobID, ps_I.strPrintshopId, intContactId_I);

            Task<String> Task_PostToRejectedFromWisnet =
            HttpTools<TjsonTJson>.PostUpdateJobInProgressToCompleteOnWisnetAsyncToEndPoint(strUrlWisnet +
                "/PrintShopData/SetJobComplete/", sendtowis, configuration_I);
            Task_PostToRejectedFromWisnet.Wait();

            if (
                //                          //There is access to the service of Wisnet.
                Task_PostToRejectedFromWisnet.Result != null
                )
            {
                jobjob_M.boolnOnWisnet = true;
            }
            else
            {
                jobjob_M.boolnOnWisnet = false;
            }

            context_M.Job.Update(jobjob_M);
            context_M.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static double numGetEstimatedPriceForAJob(
            //                                              //Get all the processes with their inputs and outputs for a 
            //                                              //      job.

            int intJobId_I,
            int intPkWorkflow_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            //IHubContext<ConnectionHub> iHubContext_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            double numJobPrice = 0;

            JobjsonJobJson jobjson = new JobjsonJobJson();
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //To get the product updated with Wisnet new info.
                ProdtypProductType prodtyp = ProdtypProductType.GetProductTypeUpdated(ps_I,
                    jobjson.strProductName, (int)jobjson.intnProductKey, true);

                //                                          //Get all the processes.
                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
                List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                ProdtypProductType.subGetWorkflowValidWay(intPkWorkflow_I, jobjson,
                    out darrpiwentityAllProcesses, out darrdynlkjson);

                if (
                    darrpiwentityAllProcesses.Count > 0
                    )
                {
                    ProdtypProductType.subUpdateResourceForAJob(prodtyp, null, darrpiwentityAllProcesses, jobjson);

                    //                                      //List of normal piw.
                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityNormalProcess =
                        darrpiwentityAllProcesses.Where(piw => piw.boolIsPostProcess == false).ToList();

                    //                                      //List of post piw.
                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityPostProcess =
                        darrpiwentityAllProcesses.Where(piw => piw.boolIsPostProcess == true).ToList();

                    //                                      //List to add piws.
                    List<Piwjson1ProcessInWorkflowJson1> darrpiwjson1 =
                        new List<Piwjson1ProcessInWorkflowJson1>();

                    //                                      //To acumulate job final cost.
                    double numJobFinalCost = 0;
                    double numJobExtraCost = 0;

                    bool boolWorkflowJobIsReadyNotUsed = true;
                    ////                          //Add normal processes to List of piw json.
                    //ProdtypProductType.AddNormalProcess(darrpiwentityAllProcesses,
                    //    darrpiwentityNormalProcess, darrdynlkjson, prodtyp, ps_I, intJobId_I, jobjson,
                    //    configuration_I, ref darrpiwjson1, ref numJobFinalCost, ref numJobExtraCost,
                    //    ref boolWorkflowJobIsReadyNotUsed);

                    ////                          //Add post processes to List of piw json.
                    //ProdtypProductType.AddPostProcess(darrpiwentityAllProcesses, darrpiwentityPostProcess,
                    //    darrdynlkjson, prodtyp, ps_I, intJobId_I, jobjson, configuration_I, ref darrpiwjson1,
                    //    ref numJobFinalCost, ref numJobExtraCost, ref boolWorkflowJobIsReadyNotUsed);

                    //                                      //Dictionary to store inputs and outputs of a process.
                    prodtyp.dicProcessIOs = new Dictionary<int, List<Iofrmpiwjson2IOFromPIWJson2>>();
                    //                                      //List to store resource thickness.
                    prodtyp.darrresthkjsonResThickness = new List<ResthkjsonResourceThicknessJson>();
                    //                                      //Get job stage.
                    JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job =>
                        job.intJobID == jobjson.intJobId);
                    //                                      //Add normal processes to List of piw json.
                    ProdtypProductType.AddNormalProcess(jobentity, jobjson, prodtyp, ps_I, darrdynlkjson,
                        darrpiwentityAllProcesses, darrpiwentityNormalProcess, configuration_I, darrpiwjson1,
                        ref numJobExtraCost, ref numJobFinalCost, ref boolWorkflowJobIsReadyNotUsed);

                    //                                      //Add post processes to List of piw json.
                    ProdtypProductType.AddPostProcess(jobentity, jobjson, prodtyp, ps_I, darrdynlkjson,
                        darrpiwentityAllProcesses, darrpiwentityPostProcess, configuration_I, darrpiwjson1,
                        ref numJobExtraCost, ref numJobFinalCost, ref boolWorkflowJobIsReadyNotUsed);

                    //                                      //By product/workflow info.
                    List<CostbycaljsonCostByCalculationJson> darrcostbycaljson;
                    double numCostByProduct = prodtyp.numGetCostByProduct(jobjson, ps_I,
                        out darrcostbycaljson, ref boolWorkflowJobIsReadyNotUsed);
                    numJobFinalCost = numJobFinalCost + numCostByProduct;

                    /*bool boolAllResourcesAreSet;
                    bool boolAllResourcesAreAvailable;
                    bool boolAllResourcesHaveAnAccount;
                    bool boolAllCalculationsByProcessHaveAnAccount;
                    bool boolAllCalculationsByProductHaveAnAccount;
                    String strDeliveryDate;
                    List<String> darrstrResourcesNamesWithoutAccount;
                    List<String> darrstrProcessCalculationsWithoutAccount;
                    ProdtypProductType.subThisWorkflowJobIsReady(darrpiwjson1, jobjson, jobentity,
                        ps_I.intPk, prodtyp.intPk, strStage, iHubContext_I, out boolAllResourcesAreSet,
                        out boolAllResourcesAreAvailable, out boolAllCalculationsByProcessHaveAnAccount,
                        out boolAllCalculationsByProductHaveAnAccount, out boolAllResourcesHaveAnAccount,
                        out strDeliveryDate, out darrstrResourcesNamesWithoutAccount,
                        out darrstrProcessCalculationsWithoutAccount);*/

                    //                                      //Set price, cost and profit for a job.
                    //                                      //Get the job's stage.
                    double numJobCost = 0;
                    double numJobProfit = 0;
                    double numJobFinalProfit = 0;
                    ProdtypProductType.subGetJobPriceCostAndProfit(prodtyp, jobjson, numCostByProduct,
                        darrpiwjson1, intPkWorkflow_I, ref numJobPrice, ref numJobCost, numJobExtraCost,
                        ref numJobProfit, ref numJobFinalCost, ref numJobFinalProfit);

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
            return numJobPrice;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetDueDate(
            int intJobId_I,
            String strDueDate_I,
            String strDueTime_I,
            int intContactId_I,
            String strDescription_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            JobjsonJobJson jobjson = new JobjsonJobJson();
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Invalid date and time format.";
                if (
                    (strDueDate_I != "" && strDueDate_I.IsParsableToDate()) &&
                    (strDueTime_I != "" && strDueTime_I.IsParsableToTime())
                    )
                {
                    //                                      //To easy code.
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                    Date dateDue = strDueDate_I.ParseToDate();
                    Time timeDue = strDueTime_I.ParseToTime();
                    //ZonedTime ztimeDue = ZonedTimeTools.NewZonedTime(dateDue, timeDue);
                    //                                      //Get zonedtime considering printshop's timezone.
                    ZonedTime ztimeDue = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(dateDue, timeDue,
                        ps.strTimeZone);
                    strDueDate_I = ztimeDue.Date.ToString();
                    strDueTime_I = ztimeDue.Time.ToString();

                    intStatus_IO = 403;
                    strUserMessage_IO = "The due date must be greater than the current time.";
                    strDevMessage_IO = "";
                    if (
                        ztimeDue > ZonedTimeTools.ztimeNow
                        )
                    {
                        //                                  //Establish the connection.
                        Odyssey2Context context = new Odyssey2Context();

                        //                                  //Find all job's due dates.
                        List<DuedateentityDueDateEntityDB> darrduedateentity = context.DueDate.Where(due =>
                            due.intJobId == intJobId_I).ToList();
                        darrduedateentity.Sort();
                        darrduedateentity.Reverse();
                        if (
                            darrduedateentity.Count > 0
                            )
                        {
                            //                              //Get current due date.
                            DuedateentityDueDateEntityDB duedateentityCurrent = darrduedateentity.Last();
                            //                              //Make last due date as not current.
                            duedateentityCurrent.boolCurrent = false;
                        }

                        //                                  //To easy code.
                        String strHour = strDueTime_I.Substring(0, 2);
                        String strMinute = strDueTime_I.Substring(3, 2);
                        String strSecond = strDueTime_I.Substring(6, 2);
                        DuedateentityDueDateEntityDB duedateentityNew = new DuedateentityDueDateEntityDB
                        {
                            intJobId = intJobId_I,
                            strDate = strDueDate_I,
                            strHour = strHour,
                            strMinute = strMinute,
                            strSecond = strSecond,
                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                            strDescription = strDescription_I,
                            intContactId = intContactId_I,
                            boolCurrent = true
                        };
                        context.DueDate.Add(duedateentityNew);
                        context.SaveChanges();

                        //PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                        JobJob.subDeleteDueDateInThePastAlert(intJobId_I, ps.intPk, strDueDate_I, strDueTime_I,
                            iHubContext_I);

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subDeleteDueDateInThePastAlert(
            //                                              //Find alert related to due date in the past.
            //                                              //Verify if due date is not before current date, otherwise
            //                                              //      alert won't be deleted.

            int intJobId_I,
            int intPkPrintshop_I,
            String strDueDate_I,
            String strDueTime_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find alert.

            //                                              //Find alert type related to due date in the past.
            AlerttypeentityAlertTypeEntityDB alerttypeentity = context.AlertType.FirstOrDefault(alerttype =>
                alerttype.strType == AlerttypeentityAlertTypeEntityDB.strDueDateInThePast);
            //                                              //Find alert.
            AlertentityAlertEntityDB alertentity = context.Alert.FirstOrDefault(alert =>
                alert.intnJobId == intJobId_I && alert.intPkAlertType == alerttypeentity.intPk);
            if (
                //                                          //Alert exists.
                alertentity != null
                )
            {
                //                                          //Find supervisors.
                List<RolentityRoleEntityDB> darrrolentity = context.Role.Where(role =>
                    role.intPkPrintshop == intPkPrintshop_I && role.boolSupervisor).ToList();

                if (
                    //                                      //Method called from setduedate.
                    strDueDate_I != null &&
                    strDueDate_I.Length > 0 &&
                    strDueTime_I != null &&
                    strDueTime_I.Length > 0
                    )
                {
                    //                                      //New due date is correct.

                    //                                      //Create ztime now.
                    ZonedTime ztimeDateNow = ZonedTimeTools.NewZonedTime(Date.Now(ZonedTimeTools.timezone),
                            Time.Now(ZonedTimeTools.timezone));
                    //                                      //Due date.
                    ZonedTime ztimeDueDate = ZonedTimeTools.NewZonedTime(strDueDate_I.ParseToDate(),
                        strDueTime_I.ParseToTime());
                    if (
                        //                                  //New due date is not in the past.
                        ztimeDueDate > ztimeDateNow
                        )
                    {
                        foreach (RolentityRoleEntityDB rolentity in darrrolentity)
                        {
                            if (
                                //                          //Notification not read.
                                !PsPrintShop.boolNotificationReadByUser(alertentity, rolentity.intContactId)
                                )
                            {
                                AlnotAlertNotification.subReduceToOne(rolentity.intContactId,
                                    iHubContext_I);
                            }
                        }

                        context.Alert.Remove(alertentity);
                        context.SaveChanges();
                    }
                }
                else
                {
                    //                                      //Method called from ModifyJobStagePendingToProgress.

                    foreach (RolentityRoleEntityDB rolentity in darrrolentity)
                    {
                        if (
                            //                              //Notification not read.
                            !PsPrintShop.boolNotificationReadByUser(alertentity, rolentity.intContactId)
                            )
                        {
                            AlnotAlertNotification.subReduceToOne(rolentity.intContactId,
                                iHubContext_I);
                        }
                    }

                    context.Alert.Remove(alertentity);
                    context.SaveChanges();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetNote(
            //                                              //Save or Update a note to an specific job.

            String strPrintshopId_I,
            int intJobId_I,
            int? intnPkNote_I,
            String strOdyssey2Note_I,
            int intContactId_I,
            Odyssey2Context context_M,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            JobjsonJobJson jobjson = new JobjsonJobJson();
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something wrong.";
                strDevMessage_IO = "Note limit exceeded.";
                if (
                    strOdyssey2Note_I.Length <= 470
                    )
                {
                    //                                      //This method can save or update a job's note.
                    //                                      //If we receive a PkNote  means it's an updating,
                    //                                      //      otherwise is a new note.
                    if (
                        intnPkNote_I != null &&
                        intnPkNote_I.HasValue &&
                        intnPkNote_I.Value != -1
                        )
                    {
                        //                                  //Validare PkNote
                        JobnotesJobNotesEntityDB jobnoteEntity = context_M.JobNotes.FirstOrDefault(jobnote =>
                            jobnote.intPk == intnPkNote_I);

                        intStatus_IO = 403;
                        strUserMessage_IO = "Something wrong.";
                        strDevMessage_IO = "Invalid PkNote.";
                        if (
                            jobnoteEntity != null &&
                            jobnoteEntity.intJobID == intJobId_I
                            )
                        {
                            jobnoteEntity.intnContactId = intContactId_I;
                            jobnoteEntity.strOdyssey2Note = strOdyssey2Note_I;
                            jobnoteEntity.strDate = Date.Now(ZonedTimeTools.timezone).ToString();
                            jobnoteEntity.strTime = Time.Now(ZonedTimeTools.timezone).ToString();
                            context_M.JobNotes.Update(jobnoteEntity);
                            context_M.SaveChanges();

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                    }
                    else
                    {
                        //                                  //Validate there is not note for this job.
                        JobnotesJobNotesEntityDB jobnoteEntity = context_M.JobNotes.FirstOrDefault(jobnote =>
                            jobnote.intJobID == intJobId_I);

                        intStatus_IO = 403;
                        strUserMessage_IO = "Something wrong.";
                        strDevMessage_IO = "There is already a note for this job. You have to send the pkNote" +
                            " in order to update the note.";
                        if (
                            jobnoteEntity == null
                            )
                        {
                            //                              //Add note to DB.
                            JobnotesJobNotesEntityDB jobNoteentity = new JobnotesJobNotesEntityDB
                            {
                                intJobID = intJobId_I,
                                intnContactId = intContactId_I,
                                strOdyssey2Note = strOdyssey2Note_I,
                                strDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                strTime = Time.Now(ZonedTimeTools.timezone).ToString()
                            };
                            context_M.JobNotes.Add(jobNoteentity);
                            context_M.SaveChanges();

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }

                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetProcessNote(
           //                                              //Add a note to a process.

           int intJobId_I,
           int intPkProcessInWorkflow_I,
           int intContactId_I,
           String strNote_I,
           List<int> darrintContactsId_I,
           PsPrintShop ps_I,
           IConfiguration configuration_I,
           Odyssey2Context context_M,
           IHubContext<ConnectionHub> iHubContext_I,
           ref int intStatus_IO,
           ref String strUserMessage_IO,
           ref String strDevMessage_IO
           )
        {
            JobjsonJobJson jobjson;
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something wrong.";
                strDevMessage_IO = "Note limit exceeded.";
                if (
                    strNote_I.Length <= 220
                    )
                {
                    //                                      //Validate piw.
                    PiwentityProcessInWorkflowEntityDB piwentity = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == intPkProcessInWorkflow_I);

                    intStatus_IO = 402;
                    strUserMessage_IO = "Something wrong.";
                    strDevMessage_IO = "Invalid PkPiw.";
                    if (
                        piwentity != null
                        )
                    {

                        //                                      //Add note to a process.
                        PronotesentityProcessNotesEntityDB pronotesentity = new PronotesentityProcessNotesEntityDB
                        {
                            intJobID = intJobId_I,
                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                            intContactId = intContactId_I,
                            strText = strNote_I,
                            intPkProcessInworkflow = intPkProcessInWorkflow_I
                        };
                        context_M.ProcessNotes.Add(pronotesentity);
                        context_M.SaveChanges();

                        //                                  //Send an alert to all the contacts in the array.
                        if (
                            darrintContactsId_I != null
                            )
                        {
                            JobJob.subSendAlertToEmployeesTagetAtProcessNote(intJobId_I, ps_I,
                                piwentity, darrintContactsId_I, context_M, iHubContext_I);
                        }

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                }                
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subSendAlertToEmployeesTagetAtProcessNote(
            //                                              //Send alert to employees when they were taged at process
            //                                              //      note.

            int intJobId_I,
            PsPrintShop ps_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            List<int> darrintContactsId_I,
            Odyssey2Context context_M,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            //                                              //Get alertType.
            AlerttypeentityAlertTypeEntityDB alerttypeentity = context_M.AlertType.FirstOrDefault(atype =>
                atype.strType == AlerttypeentityAlertTypeEntityDB.strMentioned);

            //                                              //Get strJobNumber
            String strJobNumber = JobJob.strGetJobNumber(null, intJobId_I, ps_I.strPrintshopId, context_M);

            //                                              //Get process's name.
            String strProcessName = (from piwentity in context_M.ProcessInWorkflow
                                     join eleentity in context_M.Element on
                                     piwentity.intPkProcess equals eleentity.intPk where
                                     piwentity.intPkWorkflow == piwentity_I.intPkWorkflow &&
                                     piwentity.intProcessInWorkflowId == piwentity_I.intProcessInWorkflowId
                                     select eleentity).FirstOrDefault().strElementName;

            //                                              //Get workflow's name.
            String strWorkflowName = (from piwentity in context_M.ProcessInWorkflow
                                      join wfentity in context_M.Workflow on
                                      piwentity.intPkWorkflow equals wfentity.intPk
                                      where
                                      piwentity.intPkWorkflow == piwentity_I.intPkWorkflow &&
                                      piwentity.intProcessInWorkflowId == piwentity_I.intProcessInWorkflowId
                                      select wfentity).FirstOrDefault().strName;

            //                                              //Create message to notify.
            String strMessage = alerttypeentity.strDescription + strProcessName + " in job " + strJobNumber +
                " " + strWorkflowName + " workflow.";

            //                                              //Create the notifications.
            foreach (int intContactId in darrintContactsId_I)
            {
                AlertentityAlertEntityDB alertentity = new AlertentityAlertEntityDB
                {
                    intPkPrintshop = ps_I.intPk,
                    intPkAlertType = alerttypeentity.intPk,
                    intnJobId = intJobId_I,
                    intnContactId = intContactId,
                    intnOtherAttributes = piwentity_I.intPk
                };
                context_M.Add(alertentity);
            }
            context_M.SaveChanges();

            AlnotAlertNotification.subSendToAFew(ps_I.strPrintshopId, darrintContactsId_I.ToArray(), strMessage,
                iHubContext_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subAddEstimates(
            //                                              //Add the estimation data or overwrite the existing one.

            //                                              //Data to add.
            String strPrintshopId_I,
            List<EstjsonEstimationDataJson> darrestjson_I,
            String strBaseDate_I,
            String strBaseTime_I,
            Odyssey2Context context_M,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Job data.
            int intJobId = darrestjson_I[0].intJobId;
            JobjsonJobJson jobjsonJob;

            intStatus_IO = 404;
            if (
                JobJob.boolIsValidJobId(intJobId, strPrintshopId_I, configuration_I, out jobjsonJob,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                if (
                    JobJob.boolResourcesDataValid(darrestjson_I, context_M, ref intStatus_IO, ref strUserMessage_IO,
                        ref strDevMessage_IO)
                    )
                {
                    if (
                    JobJob.boolIsCompleteEstimation(darrestjson_I, context_M, ref intStatus_IO, ref strUserMessage_IO,
                        ref strDevMessage_IO)
                        )
                    {
                        //                                  //To easy code.
                        PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                        intStatus_IO = 405;
                        if (
                            JobJob.boolValidBaseDate(strBaseDate_I, strBaseTime_I, ps.strTimeZone,
                                ref strUserMessage_IO, ref strDevMessage_IO)
                            )
                        {
                            ZonedTime ztimeBaseDate = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(
                                strBaseDate_I.ParseToDate(), strBaseTime_I.ParseToTime(), ps.strTimeZone);

                            String strBaseDate = ztimeBaseDate.Date.ToString();
                            String strBaseTime = ztimeBaseDate.Time.ToString();

                            //                              //Get the old estimation data for the job, before adding the
                            //                              //      new one.
                            int intPkWorkflow = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                                    piw.intPk == darrestjson_I[0].intPkProcessInWorkflow).intPkWorkflow;

                            List<EstdataentityEstimationDataEntityDB> darrestdataentityOldForJob =
                                (from estdata in context_M.EstimationData
                                 join piwentity in context_M.ProcessInWorkflow
                                 on estdata.intPkProcessInWorkflow equals piwentity.intPk
                                 where piwentity.intPkWorkflow == intPkWorkflow &&
                                 estdata.intJobId == intJobId
                                 select estdata).ToList();

                            List<EstentityEstimateEntityDB> darrestentityOldForJob =
                                (from est in context_M.Estimate
                                 where est.intPkWorkflow == intPkWorkflow &&
                                 est.intJobId == intJobId
                                 select est).ToList();

                            //                              //Delete old estimation data.
                            foreach (EstdataentityEstimationDataEntityDB estdata in darrestdataentityOldForJob)
                            {
                                context_M.EstimationData.Remove(estdata);
                            }

                            //                              //Delete old estimate.
                            foreach (EstentityEstimateEntityDB est in darrestentityOldForJob)
                            {
                                //                          //Delete price.
                                List<PriceentityPriceEntityDB> darrprice = context_M.Price.Where(price =>
                                    price.intJobId == intJobId && price.intnPkEstimate == est.intPk).ToList();
                                foreach (PriceentityPriceEntityDB priceentity in darrprice)
                                {
                                    context_M.Price.Remove(priceentity);
                                }
                                context_M.Estimate.Remove(est);
                            }

                            //                              //Add the estimation data no matter if some exists
                            //                              //      for the job.
                            foreach (EstjsonEstimationDataJson estjson in darrestjson_I)
                            {
                                //                          //To easy code.
                                int? intnPkElementElementType = null;
                                int? intnPkElementElement = null;
                                if (
                                    estjson.boolIsEleet == true
                                    )
                                {
                                    intnPkElementElementType = estjson.intPkEleetOrEleele;
                                }
                                else
                                {
                                    intnPkElementElement = estjson.intPkEleetOrEleele;
                                }

                                //                          //Add estimation.
                                EstdataentityEstimationDataEntityDB estdata = new EstdataentityEstimationDataEntityDB
                                {
                                    intJobId = estjson.intJobId,
                                    intPkResource = estjson.intPkResource,
                                    intPkProcessInWorkflow = estjson.intPkProcessInWorkflow,
                                    intnPkElementElementType = intnPkElementElementType,
                                    intnPkElementElement = intnPkElementElement,
                                    intId = estjson.intEstimationId
                                };
                                context_M.EstimationData.Add(estdata);

                                //                          //Look for the estimate in the database.
                                EstentityEstimateEntityDB estentity = context_M.Estimate.FirstOrDefault(est =>
                                    est.intJobId == estjson.intJobId &&
                                    est.intPkWorkflow == intPkWorkflow &&
                                    est.intId == estjson.intEstimationId
                                    );

                                if (
                                    estentity == null
                                    )
                                {
                                    //                      //Add to table estimate the baseDate and baseTime.
                                    EstentityEstimateEntityDB estentityToAdd = new EstentityEstimateEntityDB
                                    {
                                        intJobId = estjson.intJobId,
                                        intId = estjson.intEstimationId,
                                        strBaseDate = strBaseDate,
                                        strBaseTime = strBaseTime,
                                        intPkWorkflow = intPkWorkflow,
                                        strName = "Estimate " + estjson.intEstimationId
                                    };
                                    context_M.Estimate.Add(estentityToAdd);
                                }

                                context_M.SaveChanges();
                            }

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolResourcesDataValid(
            //                                              //Support method to validate if the data sent to set a 
            //                                              //      resource is valid.
            List<EstjsonEstimationDataJson> darrestjson_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            int intPkWorkflow = -1;
            PiwentityProcessInWorkflowEntityDB piwentity = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                piw.intPk == darrestjson_I[0].intPkProcessInWorkflow);

            if (
                 piwentity != null
                )
            {
                //                                          //Get the pk workflow.
                intPkWorkflow = piwentity.intPkWorkflow;
            }

            int intI = 0;
            bool boolValidData = true;
            while (
                (intI < darrestjson_I.Count) &&
                boolValidData
                )
            {
                //                                      //To easy code.
                EstjsonEstimationDataJson estjson = darrestjson_I[intI];
                int intPkProcessInworkflow = estjson.intPkProcessInWorkflow;
                bool boolIsEleet = estjson.boolIsEleet;
                int intPkEleetOrEleele = estjson.intPkEleetOrEleele;
                int intPkResource = estjson.intPkResource;

                if (
                    JobJob.boolResourceDataValid(intPkProcessInworkflow, boolIsEleet, intPkEleetOrEleele,
                        intPkResource, context_M, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO)
                    )
                {
                    if (!(
                        context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                            piw.intPk == intPkProcessInworkflow).intPkWorkflow == intPkWorkflow
                        ))
                    {
                        boolValidData = false;
                        intStatus_IO = 400;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "You sent estimations from different workflows.";
                    }
                }
                else
                {
                    boolValidData = false;
                }

                intI = intI + 1;
            }

            return boolValidData;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolResourceDataValid(
            //                                              //Support method to validate if the data sent to set a 
            //                                              //      resource is valid.
            int intPkProcessInworkflow_I,
            bool boolIsEleet_I,
            int intPkEleetOrEleele_I,
            int intPkResource_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsValid = false;

            //                                              //Get the process in workflow.
            PiwentityProcessInWorkflowEntityDB piwentity = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                piw.intPk == intPkProcessInworkflow_I);

            intStatus_IO = 404;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No process in workflow found.";
            if (
                //                                          //Process in workflow does not exist.
                piwentity != null
                )
            {
                //                                          //Get the type or template.
                EleetentityElementElementTypeEntityDB eleetentity = null;
                EleeleentityElementElementEntityDB eleeleentity = null;
                if (
                    boolIsEleet_I
                    )
                {
                    //                                      //Get the type and the template is null.
                    eleetentity = context_M.ElementElementType.FirstOrDefault(eleet =>
                         eleet.intPk == intPkEleetOrEleele_I);
                }
                else
                {
                    //                                      //Get the template and the type is null.
                    eleeleentity = context_M.ElementElement.FirstOrDefault(eleele =>
                         eleele.intPk == intPkEleetOrEleele_I);
                }

                intStatus_IO = 404;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No type or template found.";
                if (
                    //                                      //The type exist and its process is the process of piw.
                    ((eleetentity != null) && (eleetentity.intPkElementDad == piwentity.intPkProcess)) ||
                    ((eleeleentity != null) && (eleeleentity.intPkElementDad == piwentity.intPkProcess))
                    )
                {
                    ResResource res = ResResource.resFromDB(context_M, intPkResource_I, false);

                    intStatus_IO = 404;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "No resource found.";
                    if (
                        //                                  //Resource exists.
                        res != null
                        )
                    {
                        List<EleentityElementEntityDB> darreleentityResource = new List<EleentityElementEntityDB>();
                        List<TyportempresjsonTypeOrTemplateResourceJson> darrtyportempresjsonResourcesFromTemp =
                            new List<TyportempresjsonTypeOrTemplateResourceJson>();

                        if (
                            eleetentity != null
                            )
                        {
                            darreleentityResource = context_M.Element.Where(ele =>
                                ele.intPkElementType == eleetentity.intPkElementTypeSon &&
                                ele.boolIsTemplate == false).ToList();
                        }
                        else
                        {
                            //                      //Get All resources from template and derivate template resources.
                            ResResource.subGetAllResourcesFromTemplateAndDerivateTempResources(
                                eleeleentity.intPkElementSon, ref darrtyportempresjsonResourcesFromTemp, ref context_M);
                        }

                        intStatus_IO = 400;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Resource is not from the type or template.";
                        if (
                            //                      //The resource is from the correct type.
                            ((eleetentity != null) &&
                            (darreleentityResource.Exists(ele => ele.intPk == res.intPk))) ||
                            //                      //The resource is from the correct template.
                            ((eleeleentity != null) &&
                            (darrtyportempresjsonResourcesFromTemp.Exists(typeortempres =>
                                typeortempres.intPk == res.intPk)))
                            )
                        {
                            IoentityInputsAndOutputsEntityDB ioentity;
                            if (
                                eleetentity != null
                                )
                            {
                                ioentity = context_M.InputsAndOutputs.FirstOrDefault(io =>
                                    io.intPkWorkflow == piwentity.intPkWorkflow &&
                                    io.intnPkElementElementType == eleetentity.intPk &&
                                    io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId);
                            }
                            else
                            {
                                ioentity = context_M.InputsAndOutputs.FirstOrDefault(io =>
                                    io.intPkWorkflow == piwentity.intPkWorkflow &&
                                    io.intnPkElementElement == eleeleentity.intPk &&
                                    io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId);
                            }

                            intStatus_IO = 404;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "Input or Output has not a Group.";
                            if (
                                ioentity != null
                                )
                            {
                                intStatus_IO = 404;
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "The Group doesn't exist.";
                                if (
                                    ioentity.intnGroupResourceId != null &&
                                    context_M.GroupResource.FirstOrDefault(gr =>
                                        gr.intId == ioentity.intnGroupResourceId) != null
                                    )
                                {
                                    List<int> darrintResourcesInTheGroup = context_M.GroupResource
                                        .Where(gr => gr.intId == ioentity.intnGroupResourceId)
                                        .Select(gr => gr.intPkResource)
                                        .ToList();

                                    intStatus_IO = 400;
                                    strUserMessage_IO = "Something is wrong.";
                                    strDevMessage_IO = "The Group doesn't contain the Resource.";
                                    if (
                                        darrintResourcesInTheGroup.Contains(intPkResource_I)
                                        )
                                    {
                                        boolIsValid = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return boolIsValid;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolIsCompleteEstimation(
            //                                              //Support method to validate the estimation has been sent
            //                                              //      is completed.

            List<EstjsonEstimationDataJson> darrestjson_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            int intJobId = darrestjson_I[0].intJobId;

            //                                              //Number of estimation that are been added.
            int intNumberOfEstimations = JobJob.intGetNumberOfEstimations(darrestjson_I);

            //                                              //Get the workflow.
            int intPkWorkflow = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                piw.intPk == darrestjson_I[0].intPkProcessInWorkflow).intPkWorkflow;

            //                                              //List of I (eleet) for this wf with group 
            List<IoentityInputsAndOutputsEntityDB> darrioentityIEleEtsWithGroupsWithoutFilter =
                (from io in context_M.InputsAndOutputs
                 join eleet in context_M.ElementElementType
                 on io.intnPkElementElementType equals eleet.intPk
                 where io.intPkWorkflow == intPkWorkflow &&
                 io.intnGroupResourceId != null &&
                 io.intnPkElementElementType != null &&
                 eleet.boolUsage == true
                 select io).ToList();

            List<IoentityInputsAndOutputsEntityDB> darrioentityIEleEtsWithGroups;
            //                                              //Filter each io to avoid not physical resource groups.
            JobJob.subFilterNotPhysicalIOEleEtGroups(darrioentityIEleEtsWithGroupsWithoutFilter, context_M,
                out darrioentityIEleEtsWithGroups);

            JobJob.subFilterWithoutSetInJobEleEtGroups(intJobId, context_M, ref darrioentityIEleEtsWithGroups);

            //                                              //List of I (eleele) for this wf with group 
            List<IoentityInputsAndOutputsEntityDB> darrioentityIEleElesWithGroupsWithoutFilter =
                (from io in context_M.InputsAndOutputs
                 join eleele in context_M.ElementElement
                 on io.intnPkElementElement equals eleele.intPk
                 where io.intPkWorkflow == intPkWorkflow &&
                 io.intnGroupResourceId != null &&
                 io.intnPkElementElement != null &&
                 eleele.boolUsage == true
                 select io).ToList();

            List<IoentityInputsAndOutputsEntityDB> darrioentityIEleElesWithGroups;
            //                                              //Filter each io to avoid not physical resource groups.
            JobJob.subFilterNotPhysicalIOEleEleGroups(darrioentityIEleElesWithGroupsWithoutFilter, context_M,
                out darrioentityIEleElesWithGroups);

            JobJob.subFilterWithoutSetInJobEleEleGroups(intJobId, context_M, ref darrioentityIEleElesWithGroups);

            //                                              //List of Io´s (only inputs) for this wf with group 
            List<IoentityInputsAndOutputsEntityDB> darrioentityIsWithGroups = new List<IoentityInputsAndOutputsEntityDB>();
            darrioentityIsWithGroups.AddRange(darrioentityIEleEtsWithGroups);
            darrioentityIsWithGroups.AddRange(darrioentityIEleElesWithGroups);

            List<EstjsonEstimationDataJson> darrestjsonTemp = new List<EstjsonEstimationDataJson>();
            darrestjsonTemp.AddRange(darrestjson_I);

            bool boolIsComplete = true;
            if (
                (intNumberOfEstimations * darrioentityIsWithGroups.Count) == darrestjson_I.Count
                )
            {
                foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentityIsWithGroups)
                {
                    int intPkProcessInWorkflow = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                        piw.intPkWorkflow == ioentity.intPkWorkflow &&
                        piw.intProcessInWorkflowId == ioentity.intnProcessInWorkflowId).intPk;

                    int? intPkEleetOrEleele;
                    bool boolIsEleet;
                    if (
                        ioentity.intnPkElementElementType != null
                        )
                    {
                        intPkEleetOrEleele = ioentity.intnPkElementElementType;
                        boolIsEleet = true;
                    }
                    else
                    {
                        intPkEleetOrEleele = ioentity.intnPkElementElement;
                        boolIsEleet = false;
                    }

                    int intI = 0;
                    while (
                        intI < intNumberOfEstimations
                        )
                    {
                        int intIndexEstimateToDelete = darrestjsonTemp.FindIndex(est =>
                            est.intPkProcessInWorkflow == intPkProcessInWorkflow &&
                            est.intPkEleetOrEleele == intPkEleetOrEleele &&
                            est.boolIsEleet == boolIsEleet &&
                            est.intEstimationId == (intI + 1));
                        if (
                            intIndexEstimateToDelete != -1
                            )
                        {
                            darrestjsonTemp.Remove(darrestjsonTemp[intIndexEstimateToDelete]);
                        }
                        intI = intI + 1;
                    }
                }

                if (
                    darrestjsonTemp.Count > 0
                    )
                {
                    boolIsComplete = false;
                    intStatus_IO = 400;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "You have repeated the number of resources in an estimate and" +
                        " have another incomplete.";
                }
            }
            else if (
                (intNumberOfEstimations * darrioentityIsWithGroups.Count) > darrestjson_I.Count
                )
            {
                boolIsComplete = false;
                intStatus_IO = 400;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "The estimation is not complete.";
            }
            else if (
               (intNumberOfEstimations * darrioentityIsWithGroups.Count) < darrestjson_I.Count
               )
            {
                boolIsComplete = false;
                intStatus_IO = 400;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "You are sending more resources to set the estimate.";
            }

            return boolIsComplete;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subFilterNotPhysicalIOEleEtGroups(
            //                                              //Verify if the element type classification belongs to
            //                                              //      not physical resources.
            //                                              //Delete io entity which is not physical from the list.

            List<IoentityInputsAndOutputsEntityDB> darrioentityIEleEtsWithGroupsWithoutFilter_I,
            Odyssey2Context context_M,
            out List<IoentityInputsAndOutputsEntityDB> darrioentityIEleEtsWithGroups_O
            )
        {
            //                                              //Clone list.
            darrioentityIEleEtsWithGroups_O = new List<IoentityInputsAndOutputsEntityDB>
                (darrioentityIEleEtsWithGroupsWithoutFilter_I);

            foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentityIEleEtsWithGroupsWithoutFilter_I)
            {
                //                                          //Filter by physical.

                //                                          //Find element element type.
                EleetentityElementElementTypeEntityDB eleetentity = context_M.ElementElementType.FirstOrDefault(
                    eleetentity => eleetentity.intPk == ioentity.intnPkElementElementType);
                //                                          //Get element type.
                EtElementTypeAbstract eletem = EletemElementType.etFromDB(eleetentity.intPkElementTypeSon);
                if (
                    !RestypResourceType.boolIsPhysical(eletem.strClassification)
                    )
                {
                    darrioentityIEleEtsWithGroups_O.Remove(ioentity);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subFilterNotPhysicalIOEleEleGroups(
            //                                              //Verify if the element type classification belongs to
            //                                              //      not physical resources.
            //                                              //Delete io entity which is not physical from the list.

            List<IoentityInputsAndOutputsEntityDB> darrioentityIEleElesWithGroupsWithoutFilter_I,
            Odyssey2Context context_M,
            out List<IoentityInputsAndOutputsEntityDB> darrioentityIEleElesWithGroups_O
            )
        {
            //                                              //Clone list.
            darrioentityIEleElesWithGroups_O = new List<IoentityInputsAndOutputsEntityDB>
                (darrioentityIEleElesWithGroupsWithoutFilter_I);

            foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentityIEleElesWithGroupsWithoutFilter_I)
            {
                //                                          //Filter by physical.

                //                                          //Find element element.
                EleeleentityElementElementEntityDB eleeleentity = context_M.ElementElement.FirstOrDefault(
                    eleele => eleele.intPk == ioentity.intnPkElementElement);
                //                                          //Find resource.
                EleentityElementEntityDB eleentity = context_M.Element.FirstOrDefault(ele =>
                    ele.intPk == eleeleentity.intPkElementSon);
                //                                          //Find element type.
                EtElementTypeAbstract et = EletemElementType.etFromDB(eleentity.intPkElementType);
                if (
                    !RestypResourceType.boolIsPhysical(et.strClassification)
                    )
                {
                    darrioentityIEleElesWithGroups_O.Remove(ioentity);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subFilterWithoutSetInJobEleEtGroups(
            int intJobId_I,
            Odyssey2Context context_M,
            ref List<IoentityInputsAndOutputsEntityDB> darrioentityIEleEtsWithGroups_M
            )
        {
            //                                              //Clone list.
            List<IoentityInputsAndOutputsEntityDB> darrioentityIEleEtsWithGroupsTemp =
                new List<IoentityInputsAndOutputsEntityDB>(darrioentityIEleEtsWithGroups_M);

            foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentityIEleEtsWithGroupsTemp)
            {
                int intPkProcessInWorkflow = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPkWorkflow == ioentity.intPkWorkflow &&
                    piw.intProcessInWorkflowId == ioentity.intnProcessInWorkflowId).intPk;

                IojentityInputsAndOutputsForAJobEntityDB iojentity = context_M.InputsAndOutputsForAJob.FirstOrDefault(iojentity =>
                    iojentity.intPkProcessInWorkflow == intPkProcessInWorkflow &&
                    iojentity.intnPkElementElementType == ioentity.intnPkElementElementType &&
                    iojentity.intJobId == intJobId_I);

                if (
                    iojentity != null
                    )
                {
                    darrioentityIEleEtsWithGroups_M.Remove(ioentity);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subFilterWithoutSetInJobEleEleGroups(
            int intJobId_I,
            Odyssey2Context context_M,
            ref List<IoentityInputsAndOutputsEntityDB> darrioentityIEleElesWithGroups_M
            )
        {
            //                                              //Clone list.
            List<IoentityInputsAndOutputsEntityDB> darrioentityIEleElesWithGroupsTemp =
                new List<IoentityInputsAndOutputsEntityDB>(darrioentityIEleElesWithGroups_M);

            foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentityIEleElesWithGroupsTemp)
            {
                int intPkProcessInWorkflow = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPkWorkflow == ioentity.intPkWorkflow &&
                    piw.intProcessInWorkflowId == ioentity.intnProcessInWorkflowId).intPk;

                IojentityInputsAndOutputsForAJobEntityDB iojentity = context_M.InputsAndOutputsForAJob.FirstOrDefault(iojentity =>
                    iojentity.intPkProcessInWorkflow == intPkProcessInWorkflow &&
                    iojentity.intnPkElementElement == ioentity.intnPkElementElementType &&
                    iojentity.intJobId == intJobId_I);

                if (
                    iojentity != null
                    )
                {
                    darrioentityIEleElesWithGroups_M.Remove(ioentity);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static int intGetNumberOfEstimations(
            List<EstjsonEstimationDataJson> darrestjson_I
            )
        {
            int maxNumber = 0;
            foreach (EstjsonEstimationDataJson estjson in darrestjson_I)
            {
                if (estjson.intEstimationId > maxNumber)
                {
                    maxNumber = estjson.intEstimationId;
                }
            }
            return maxNumber;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subConfirmResources(
            //                                              //Confirm estimation data in the ioj table.

            //                                              //Printshop.
            PsPrintShop ps_I,
            //                                              //Data to confirm.
            List<EstjsonEstimationDataJson> darrestjson_I,
            IConfiguration configuration_I,
            String strPassword_I,
            Odyssey2Context context_M,
            out bool boolAreAllPeriodsAddables_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {

            boolAreAllPeriodsAddables_O = true;

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Nothing to confirm.";
            if (
                darrestjson_I.Count > 0
                )
            {
                //                                          //Job data.
                int intJobId = darrestjson_I[0].intJobId;
                JobjsonJobJson jobjsonJob;

                intStatus_IO = 402;
                if (
                    JobJob.boolIsValidJobId(intJobId, ps_I.strPrintshopId, configuration_I, out jobjsonJob,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                    )
                {
                    int intPkWorkflow;
                    if (
                        JobJob.boolAllProcessInWorkflowAreFromTheSameWf(darrestjson_I, context_M, out intPkWorkflow)
                        )
                    {
                        //                                  //Get the baseDate and baseTime.

                        //                                  //Same estimation id for all data to confirm.
                        int intEstimationId = darrestjson_I[0].intEstimationId;
                        EstentityEstimateEntityDB estentity = context_M.Estimate.FirstOrDefault(est =>
                            est.intJobId == intJobId &&
                            est.intPkWorkflow == intPkWorkflow &&
                            est.intId == intEstimationId);

                        if (
                            estentity != null
                            )
                        {
                            ZonedTime ztimeBase = ZonedTimeTools.NewZonedTime(estentity.strBaseDate.ParseToDate(),
                                estentity.strBaseTime.ParseToTime());

                            intStatus_IO = 403;
                            strUserMessage_IO = "Select valid date or time.";
                            strDevMessage_IO = "";
                            if (
                                ztimeBase > ZonedTimeTools.ztimeNow
                                )
                            {
                                //                          //List of data in EstimationData table for the job.
                                List<EstdataentityEstimationDataEntityDB> darrestdataentityForJobAndPiwAndId =
                                    (
                                    from estdata in context_M.EstimationData
                                    join piwentity in context_M.ProcessInWorkflow
                                    on estdata.intPkProcessInWorkflow equals piwentity.intPk
                                    where estdata.intId == intEstimationId &&
                                    piwentity.intPkWorkflow == intPkWorkflow &&
                                    estdata.intJobId == intJobId
                                    select estdata).ToList();

                                intStatus_IO = 404;
                                strUserMessage_IO = "Data to confirm has changed, please try another estimation.";
                                strDevMessage_IO = "Data is defferent in database, probably has changed or is invalid.";
                                if (
                                    JobJob.boolValidEstimationDataInDB(darrestdataentityForJobAndPiwAndId, darrestjson_I)
                                    )
                                {
                                    //                      //In case there are periods by exception they will be
                                    //                      //      returned here.
                                    List<int> darrintPkPeriodsByException;
                                    //                      //
                                    boolAreAllPeriodsAddables_O = EstEstimate.boolAreAllPeriodsAddables(ps_I,
                                        darrestjson_I, context_M, configuration_I, out darrintPkPeriodsByException,
                                        ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                                    boolAreAllPeriodsAddables_O = boolAreAllPeriodsAddables_O ? true :
                                        strPassword_I == ps_I.strSpecialPassword;

                                    if (
                                        !boolAreAllPeriodsAddables_O &&
                                        (strPassword_I != null) &&
                                        (strPassword_I != ps_I.strSpecialPassword)
                                        )
                                    {
                                        strUserMessage_IO = "Incorrect password. " + strUserMessage_IO;
                                    }
                                    //                      //Verify intStatus_IO updated by EstEstimate.
                                    //                      //      boolAreAllPeriodsAddables.
                                    if (
                                        intStatus_IO == 200 &&
                                        boolAreAllPeriodsAddables_O
                                        )
                                    {
                                        bool boolDataAlreadyInIo = false;
                                        //                  //Confirm data in ioj table.
                                        foreach (EstjsonEstimationDataJson estjson in darrestjson_I)
                                        {
                                            //              //To easy code.
                                            int? intnPkElementElementType = null;
                                            int? intnPkElementElement = null;
                                            if (
                                                estjson.boolIsEleet == true
                                                )
                                            {
                                                intnPkElementElementType = estjson.intPkEleetOrEleele;
                                            }
                                            else
                                            {
                                                intnPkElementElement = estjson.intPkEleetOrEleele;
                                            }

                                            IojentityInputsAndOutputsForAJobEntityDB iojentityToConfirmIfExists =
                                                context_M.InputsAndOutputsForAJob.FirstOrDefault(iojentity =>
                                                iojentity.intJobId == estjson.intJobId &&
                                                iojentity.intPkProcessInWorkflow == estjson.intPkProcessInWorkflow &&
                                                iojentity.intnPkElementElementType == intnPkElementElementType &&
                                                iojentity.intnPkElementElement == intnPkElementElement &&
                                                iojentity.intPkResource == estjson.intPkResource
                                            );

                                            if (
                                                iojentityToConfirmIfExists == null
                                                )
                                            {
                                                //          //Add the ioj.
                                                IojentityInputsAndOutputsForAJobEntityDB iojentity =
                                                    new IojentityInputsAndOutputsForAJobEntityDB
                                                    {
                                                        intJobId = estjson.intJobId,
                                                        intPkProcessInWorkflow = estjson.intPkProcessInWorkflow,
                                                        intnPkElementElementType = intnPkElementElementType,
                                                        intnPkElementElement = intnPkElementElement,
                                                        intPkResource = estjson.intPkResource,
                                                        boolnWasSetAutomatically = false
                                                    };
                                                context_M.InputsAndOutputsForAJob.Add(iojentity);
                                            }
                                            else
                                            {
                                                boolDataAlreadyInIo = true;
                                            }
                                        }

                                        //                  //List of data in EstimationData table for the job and the 
                                        //                  //      wf.
                                        List<EstdataentityEstimationDataEntityDB> darrestdataentityForJobAndPiw =
                                            (
                                            from estdata in context_M.EstimationData
                                            join piwentity in context_M.ProcessInWorkflow
                                            on estdata.intPkProcessInWorkflow equals piwentity.intPk
                                            where piwentity.intPkWorkflow == intPkWorkflow &&
                                            estdata.intJobId == intJobId
                                            select estdata).ToList();

                                        //                  //Delete estimation data for the job, no matter and wf.
                                        foreach (EstdataentityEstimationDataEntityDB estdata in
                                            darrestdataentityForJobAndPiw)
                                        {
                                            if (
                                                estdata.intId == intEstimationId
                                                )
                                            {
                                                estdata.intId = 0;
                                            }
                                            else
                                            {
                                                context_M.EstimationData.Remove(estdata);
                                            }
                                        }

                                        //                  //Find estimates.
                                        List<EstentityEstimateEntityDB> darrestentityForJobAndWf = context_M.Estimate.Where(
                                            est => est.intJobId == intJobId && est.intPkWorkflow == intPkWorkflow).ToList();

                                        //                  //Last estim price is now the job's price.
                                        JobJob.subAssignLastEstimPriceToJob(intJobId, intPkWorkflow, intEstimationId,
                                            darrestentityForJobAndWf);

                                        //                  //Delete estimate for the job and workflow.
                                        foreach (EstentityEstimateEntityDB est in darrestentityForJobAndWf)
                                        {
                                            if (
                                                est.intId == intEstimationId
                                                )
                                            {
                                                est.intId = 0;
                                            }
                                            else
                                            {
                                                context_M.Estimate.Remove(est);
                                            }
                                        }

                                        //                  //Get all periods for the job and wf.
                                        List<PerentityPeriodEntityDB> darrperentityForJobAndWf = context_M.Period.Where(
                                            per => per.intJobId == intJobId && per.intPkWorkflow == intPkWorkflow
                                            ).ToList();

                                        //                  //Confirm periods for the estimate and delete other ones.
                                        foreach (PerentityPeriodEntityDB perentity in darrperentityForJobAndWf)
                                        {
                                            if (
                                                perentity.intnEstimateId != null &&
                                                perentity.intnEstimateId == intEstimationId
                                                )
                                            {
                                                //          //The period is now the job period for the wf.
                                                perentity.intnEstimateId = null;

                                                //          //Identify the periods that are by Exception.
                                                if (
                                                    darrintPkPeriodsByException.Contains(perentity.intPk)
                                                    )
                                                {
                                                    perentity.boolIsException = true;
                                                }
                                            }
                                            else
                                            {
                                                context_M.Period.Remove(perentity);
                                            }
                                        }

                                        //                  //Persist all.
                                        context_M.SaveChanges();

                                        //                  //Status code and message that will be returned.
                                        intStatus_IO = 200;
                                        strUserMessage_IO = "Resources have been confirmed.";
                                        strDevMessage_IO = "";

                                        if (
                                            boolDataAlreadyInIo
                                            )
                                        {
                                            strDevMessage_IO = "Some data were already in IO.";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static bool boolAllProcessInWorkflowAreFromTheSameWf(
            List<EstjsonEstimationDataJson> darrestjson_I,
            Odyssey2Context context_M,
            out int intPkWorkflow_O
            )
        {
            intPkWorkflow_O = 0;

            int intPkWorkflowAnterior = 0;
            bool boolIsTheSameWf = true;
            int intI = 0;
            while (
                (intI < darrestjson_I.Count) &&
                boolIsTheSameWf
                )
            {
                EstjsonEstimationDataJson estjsonI = darrestjson_I[intI];

                intPkWorkflow_O = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == estjsonI.intPkProcessInWorkflow).intPkWorkflow;

                if (
                    (intI > 0) &&
                    (intPkWorkflow_O != intPkWorkflowAnterior)
                    )
                {
                    boolIsTheSameWf = false;
                }

                intPkWorkflowAnterior = intPkWorkflow_O;
                intI = intI + 1;
            }

            return boolIsTheSameWf;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subAssignLastEstimPriceToJob(

            int intJobId_I,
            int intPkWorkflow_I,
            int intEstimationId_I,
            List<EstentityEstimateEntityDB> darrestentityForJobAndWf_I
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get prices from each estimate. 
            List<PriceentityPriceEntityDB> darrpriceentityToDelete = new List<PriceentityPriceEntityDB>();
            List<PriceentityPriceEntityDB> darrpriceentityFromConfirmedEstimate = new List<PriceentityPriceEntityDB>();
            foreach (EstentityEstimateEntityDB estentity in darrestentityForJobAndWf_I)
            {
                //                                          //Find estim prices.
                List<PriceentityPriceEntityDB> darrpriceentity = context.Price.Where(price =>
                    price.intJobId == intJobId_I && price.intnPkEstimate == estentity.intPk).ToList();

                foreach (PriceentityPriceEntityDB priceentity in darrpriceentity)
                {
                    if (
                        estentity.intId == intEstimationId_I
                        )
                    {
                        darrpriceentityFromConfirmedEstimate.Add(priceentity);
                    }
                    else
                    {
                        darrpriceentityToDelete.Add(priceentity);
                    }
                }
            }

            //                                              //Assign last price to the Job.
            if (
                darrpriceentityFromConfirmedEstimate.Count() > 0
                )
            {
                darrpriceentityFromConfirmedEstimate.Sort();

                PriceentityPriceEntityDB priceentityLastPrice = darrpriceentityFromConfirmedEstimate.First();

                if (
                    priceentityLastPrice.boolIsReset == false
                    )
                {
                    PriceentityPriceEntityDB priceentityPriceToSetInJob = new PriceentityPriceEntityDB
                    {
                        numnPrice = priceentityLastPrice.numnPrice,
                        intJobId = intJobId_I,
                        strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                        strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                        strDescription = priceentityLastPrice.strDescription,
                        intContactId = priceentityLastPrice.intContactId,
                        boolIsReset = priceentityLastPrice.boolIsReset,
                        intnPkWorkflow = intPkWorkflow_I,
                    };
                    context.Price.Add(priceentityPriceToSetInJob);
                }
            }

            //                                              //Delete estim's prices.
            foreach (PriceentityPriceEntityDB priceentity in darrpriceentityToDelete)
            {
                context.Price.Remove(priceentity);
            }

            foreach (PriceentityPriceEntityDB priceentity in darrpriceentityFromConfirmedEstimate)
            {
                context.Price.Remove(priceentity);
            }

            context.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static bool boolValidEstimationDataInDB(
            //                                              //True if both darr have the same data.

            //                                              //EstimationData in the database for this wf and Id.
            List<EstdataentityEstimationDataEntityDB> darrestdataentity_I,
            //                                              //Data to confirm, same Id.
            //                                              //All piw should be of the same wf.
            List<EstjsonEstimationDataJson> darrestjson_I
            )
        {
            bool boolValidData = true;

            //                                              //Working temporary list.
            List<EstdataentityEstimationDataEntityDB> darrestdataentityTemp =
                new List<EstdataentityEstimationDataEntityDB>();
            darrestdataentityTemp.AddRange(darrestdataentity_I);

            int intI = 0;
            while (
                (intI < darrestjson_I.Count) &&
                boolValidData
                )
            {
                EstjsonEstimationDataJson estjsonI = darrestjson_I[intI];

                //                                      //To easy code.
                int? intnPkElementElementType = null;
                int? intnPkElementElement = null;
                if (
                    estjsonI.boolIsEleet == true
                    )
                {
                    intnPkElementElementType = estjsonI.intPkEleetOrEleele;
                }
                else
                {
                    intnPkElementElement = estjsonI.intPkEleetOrEleele;
                }

                //                                      //Look for the I-json in the DB.
                EstdataentityEstimationDataEntityDB estdataentity = darrestdataentityTemp.FirstOrDefault(
                    estdataentity =>
                    estdataentity.intJobId == estjsonI.intJobId &&
                    estdataentity.intId == estjsonI.intEstimationId &&
                    estdataentity.intPkProcessInWorkflow == estjsonI.intPkProcessInWorkflow &&
                    estdataentity.intnPkElementElementType == intnPkElementElementType &&
                    estdataentity.intnPkElementElement == intnPkElementElement &&
                    estdataentity.intPkResource == estjsonI.intPkResource
                    );

                if (
                    //                                      //If this entity match with some I-json.
                    estdataentity != null
                    )
                {
                    //                                      //Remove to keep only the ones that haven´t match.
                    darrestdataentityTemp.Remove(estdataentity);
                }
                else
                {
                    //                                      //I-json is not in DB.
                    boolValidData = false;
                }

                intI = intI + 1;
            }

            if (
                darrestdataentityTemp.Count > 0
                )
            {
                boolValidData = false;
            }

            return boolValidData;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subUpdateCostOrQuantity(
            //                                              //Method to set or update final cost or quantity.

            int intContactId_I,
            int? intnPkCalculation_I,
            int? intnPkResource_I,
            String strDescription_I,
            int? intnPkEleetOrEleele_I,
            bool? boolnIsEleet_I,
            int intJobId_I,
            int intPkProcessInWorkflow_I,
            String strPrintshopId_I,
            double? numnFinalQuantity_I,
            double? numnFinalCost_I,
            int intPkAccountMovement_I,
            IConfiguration configuration_I,
            Odyssey2Context context_M,
            out double numFinalCalculatedCost_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            numFinalCalculatedCost_O = 0.0;
            JobjsonJobJson jobjson;

            intStatus_IO = 401;
            strUserMessage_IO = "You cannot make changes to the final costs.";
            strDevMessage_IO = "";
            if (
                (intContactId_I > 0) &&
                (ResResource.boolEmployeeOrOwnerIsFromPrintshop(strPrintshopId_I, intContactId_I))
                )
            {
                intStatus_IO = 402;
                if (
                    JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjson,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                    )
                {
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                    //                                      //Get job entity.
                    JobentityJobEntityDB jobentity = context_M.Job.FirstOrDefault(job => job.intJobID == intJobId_I &&
                        job.intPkPrintshop == ps.intPk);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Job still pending.";
                    if (
                        jobentity != null
                        )
                    {
                        intStatus_IO = 404;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Cannot set both cost and quantity or value not valid.";
                        if (
                            //                              //Validate cost.
                            (numnFinalCost_I != null && numnFinalCost_I >= 0) && (numnFinalQuantity_I == null) ||
                            //                              //Validate quantity.
                            (numnFinalQuantity_I != null && numnFinalQuantity_I >= 0) && (numnFinalCost_I == null)
                            )
                        {
                            //                              //Get the account movement.
                            AccmoventityAccountMovementEntityDB accmoventity = context_M.AccountMovement.FirstOrDefault(
                                accmov => accmov.intPk == intPkAccountMovement_I && accmov.intnJobId == intJobId_I);

                            intStatus_IO = 406;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "Account movement not found.";
                            if (
                                accmoventity != null
                                )
                            {
                                intStatus_IO = 407;
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "A Calculation or Resource is required, not both.";
                                /*CASE*/
                                if (
                                    //                      //We receive a calculation.
                                    intnPkCalculation_I != null &&
                                    intnPkResource_I == null
                                    )
                                {
                                    //                      //Method to create a register in Finalcost table when we
                                    //                      //      receive a calculation.
                                    JobJob.subCreateCalculationFinalCostOrQuantityRegister(intContactId_I,
                                        intnPkCalculation_I, jobentity, jobjson, intPkProcessInWorkflow_I,
                                        numnFinalQuantity_I, numnFinalCost_I, strDescription_I, accmoventity, context_M,
                                        out numFinalCalculatedCost_O, ref intStatus_IO, ref strUserMessage_IO,
                                        ref strDevMessage_IO);
                                }
                                else if (
                                   //                       //We receive a resource.
                                   intnPkCalculation_I == null &&
                                   intnPkResource_I != null
                                   )
                                {
                                    intStatus_IO = 408;
                                    strUserMessage_IO = "Something is wrong.";
                                    strDevMessage_IO = "The EleetOrEleele does not correspond to resource or data not " +
                                        "valid.";
                                    if (
                                        JobJob.boolIsValidEleetOrEleele(intnPkResource_I, intPkProcessInWorkflow_I,
                                            jobentity.intJobID, intnPkEleetOrEleele_I, boolnIsEleet_I, context_M)
                                        )
                                    {
                                        //                  //Method to create a register in Finalcost table when we
                                        //                  //      receive a resource.
                                        JobJob.subCreateAByResourceFinalCostOrQuantityRegister(intContactId_I,
                                            intnPkResource_I, jobentity, intPkProcessInWorkflow_I, intnPkEleetOrEleele_I,
                                            boolnIsEleet_I, numnFinalQuantity_I, numnFinalCost_I, strDescription_I,
                                            accmoventity, context_M, out numFinalCalculatedCost_O,
                                            ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                                    }
                                }
                                /*END-CASE*/
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subCreateCalculationFinalCostOrQuantityRegister(
            //                                              //Method to create a register in final cost table when
            //                                              //      we receive a calculation.

            int intContactId_I,
            int? intnPkCalculation_I,
            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjson_I,
            int intPkProcessInWorkflow_I,
            double? numnFinalQuantity_I,
            double? numnFinalCost_I,
            String strDescription_I,
            AccmoventityAccountMovementEntityDB accmoventity_I,
            Odyssey2Context context_M,
            out double numFinalCalculatedCost_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            numFinalCalculatedCost_O = 0;
            //                                              //Get calculation.
            CalCalculation cal = CalCalculation.calGetFromDb((int)intnPkCalculation_I);

            intStatus_IO = 408;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Calculation not found.";
            if (
                cal != null
                )
            {
                //                                          //Verify calculation apply to job.
                intStatus_IO = 409;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Calculation not valid for this job.";
                if (
                    JobJob.boolCalculationApplyConsideringJobDate(cal, jobentity_I) &&
                    //                                      //Verify calculations conditions.
                    Tools.boolCalculationOrLinkApplies(cal.intPk, null, null, null, jobjson_I)
                    )
                {
                    //                                      //Get the piw.
                    PiwentityProcessInWorkflowEntityDB piwentity = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                        piw.intPk == intPkProcessInWorkflow_I);

                    intStatus_IO = 410;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Process In Workflow not found.";
                    if (
                        piwentity != null
                        )
                    {
                        intStatus_IO = 411;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "The calculation does not belong to the process.";
                        if (
                            cal.intnPkWorkflowBelongsTo == piwentity.intPkWorkflow &&
                            cal.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId
                            )
                        {
                            //                              //Find calculation account
                            int? intnPkCalAccount = context_M.Calculation.FirstOrDefault(cal =>
                                cal.intPk == intnPkCalculation_I).intnPkAccount;

                            bool boolIsAccountAvailable = context_M.Account.FirstOrDefault(acc =>
                                acc.intPk == accmoventity_I.intPkAccount).boolAvailable;

                            intStatus_IO = 412;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "The account does not correspond to the calculation.";
                            if (
                                !boolIsAccountAvailable ||
                                (intnPkCalAccount != null && intnPkCalAccount == accmoventity_I.intPkAccount)
                                )
                            {
                                //                          //Get FinalCost register.
                                List<FnlcostentityFinalCostEntityDB> darrfnlentity = context_M.FinalCost.Where(fnl =>
                                    fnl.intPkJob == jobentity_I.intPk &&
                                    fnl.intPkProcessInWorkflow == intPkProcessInWorkflow_I &&
                                    fnl.intnPkCalculation == intnPkCalculation_I).ToList();
                                darrfnlentity.Sort();

                                intStatus_IO = 413;
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "The movement is not from the calculation";
                                if (
                                    (darrfnlentity.Count > 0) &&
                                    (!boolIsAccountAvailable ||
                                    (darrfnlentity.Last().intPkAccountMovement == accmoventity_I.intPk))
                                    )
                                {
                                    intStatus_IO = 414;
                                    strUserMessage_IO = "The info is already saved.";
                                    strDevMessage_IO = "";
                                    if (
                                        //                  //The last entry is exactly the same.
                                        !(darrfnlentity.Last().numnCost == numnFinalCost_I &&
                                        darrfnlentity.Last().numnQuantity == numnFinalQuantity_I)
                                        )
                                    {
                                        double? numnCostToAdd;
                                        double? numnQuantityToAdd;
                                        int? intnPkResourceToAdd;
                                        int? intnPkCalculationToAdd;
                                        bool boolSuccess;
                                        numFinalCalculatedCost_O = JobJob.GetFinalCost(cal, null, numnFinalQuantity_I,
                                            numnFinalCost_I, jobentity_I, out numnCostToAdd, out numnQuantityToAdd,
                                            out intnPkResourceToAdd, out intnPkCalculationToAdd, out boolSuccess,
                                            ref strUserMessage_IO, ref strDevMessage_IO);

                                        intStatus_IO = 415;
                                        if (
                                            boolSuccess
                                            )
                                        {
                                            if (
                                                numnCostToAdd != null
                                                )
                                            {
                                                numnCostToAdd = ((double)numnCostToAdd).Round(2);
                                            }

                                            //              //There is not cost, create a register.
                                            FnlcostentityFinalCostEntityDB fnlentityNew =
                                                new FnlcostentityFinalCostEntityDB
                                                {
                                                    numnCost = numnCostToAdd,
                                                    numnQuantity = numnQuantityToAdd,
                                                    strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                                    strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                                    strDescription = strDescription_I,
                                                    intPkJob = jobentity_I.intPk,
                                                    intPkProcessInWorkflow = intPkProcessInWorkflow_I,
                                                    intnPkResource = intnPkResourceToAdd,
                                                    intnPkCalculation = intnPkCalculationToAdd,
                                                    intContactId = intContactId_I,
                                                    intPkAccountMovement = accmoventity_I.intPk
                                                };
                                            context_M.FinalCost.Add(fnlentityNew);

                                            if (
                                                //          //The account is available
                                                boolIsAccountAvailable
                                                )
                                            {
                                                FnlcostentityFinalCostEntityDB fnlcostentityFirst =
                                                    darrfnlentity.First();

                                                double numFinalCost;
                                                if (
                                                    cal.strCalculationType == CalCalculation.strPerQuantity
                                                    )
                                                {
                                                    double numQuantity = (double)fnlcostentityFirst.numnQuantity;
                                                    double numCost = (double)fnlcostentityFirst.numnCost;
                                                    numFinalCost = numCost;

                                                    if (
                                                        //      //Final quantity changed.
                                                        numnFinalQuantity_I != null
                                                        )
                                                    {
                                                        double numFinalQuantity = (double)numnFinalQuantity_I;
                                                        if (
                                                            numCost > 0 &&
                                                            numQuantity > 0 &&
                                                            numFinalQuantity > 0
                                                            )
                                                        {
                                                            numFinalCost = (numCost * numFinalQuantity) / numQuantity;
                                                        }
                                                        else
                                                        {
                                                            numFinalCost = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        numFinalCost = (double)numnCostToAdd;
                                                    }
                                                }
                                                else
                                                {
                                                    numFinalCost = (double)numnCostToAdd;
                                                }

                                                //          //Update movement amount
                                                accmoventity_I.numnIncrease = numFinalCost.Round(2);
                                                accmoventity_I.strConcept = strDescription_I;
                                                accmoventity_I.strStartDate =
                                                    Date.Now(ZonedTimeTools.timezone).ToString();
                                                accmoventity_I.strStartTime =
                                                    Time.Now(ZonedTimeTools.timezone).ToString();

                                                intStatus_IO = 200;
                                                strUserMessage_IO = "";
                                                strDevMessage_IO = "";
                                            }
                                            else
                                            {
                                                intStatus_IO = 200;
                                                strUserMessage_IO = "There is not account releated to this cost.";
                                                strDevMessage_IO = "";
                                            }

                                            context_M.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolIsValidEleetOrEleele(
            //                                              //Method to validate EleetOrEleele.
            int? intnPkResource_I,
            int intPkProcessInWorkflow_I,
            int intJobId_I,
            int? intnPkEleetOrEleele_I,
            bool? boolnIsEleet_I,
            Odyssey2Context context_I
            )
        {
            bool boolIsValidEleetOrEleele = false;

            //                                              //To easy code.
            int? intnPkElementElementType = null;
            int? intnPkElementElement = null;
            /*CASE*/
            if (
                boolnIsEleet_I == true
                )
            {
                intnPkElementElementType = intnPkEleetOrEleele_I;
            }
            else if (
               boolnIsEleet_I == false
               )
            {
                intnPkElementElement = intnPkEleetOrEleele_I;
            }
            /*END-CASE*/

            bool boolIsInIOJ = false;
            IojentityInputsAndOutputsForAJobEntityDB iojentity = context_I.InputsAndOutputsForAJob.FirstOrDefault(
                ioj => ioj.intPkProcessInWorkflow == intPkProcessInWorkflow_I &&
                ioj.intPkResource == intnPkResource_I && ioj.intJobId == intJobId_I &&
                ioj.intnPkElementElementType == intnPkElementElementType &&
                ioj.intnPkElementElement == intnPkElementElement);

            if (
                //                                          //There is a resource set in WFJ.
                iojentity != null
                )
            {
                boolIsInIOJ = true;
                boolIsValidEleetOrEleele = true;
            }

            if (
                boolIsInIOJ == false
                )
            {
                //                                          //To easy code.
                PiwentityProcessInWorkflowEntityDB piwentity = context_I.ProcessInWorkflow.FirstOrDefault(
                    piw => piw.intPk == intPkProcessInWorkflow_I);
                int intPkWorkflow = piwentity.intPkWorkflow;
                int intProcessInWorkflowId = piwentity.intProcessInWorkflowId;

                //                                          //Search in IO.
                List<IoentityInputsAndOutputsEntityDB> darrioentity =
                    (from ioentityRes in context_I.InputsAndOutputs
                     where
                        ioentityRes.intPkWorkflow == intPkWorkflow &&
                        ioentityRes.intnProcessInWorkflowId == intProcessInWorkflowId &&
                        ioentityRes.intnPkResource == intnPkResource_I &&
                        ioentityRes.intnPkElementElementType == intnPkElementElementType &&
                        ioentityRes.intnPkElementElement == intnPkElementElement
                     select ioentityRes).ToList();

                if (
                    //                                      //Verify is resource is set on IO.
                    darrioentity.Count() == 1
                    )
                {
                    boolIsValidEleetOrEleele = true;
                }
            }
            return boolIsValidEleetOrEleele;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subCreateAByResourceFinalCostOrQuantityRegister(
            //                                              //Method to create a register in final cost table when
            //                                              //      we receive a resource.

            int intContactId_I,
            int? intnPkResource_I,
            JobentityJobEntityDB jobentity_I,
            int intPkProcessInWorkflow_I,
            int? intnPkEleetOrEleele_I,
            bool? boolnIsEleet_I,
            double? numnFinalQuantity_I,
            double? numnFinalCost_I,
            String strDescription_I,
            AccmoventityAccountMovementEntityDB accmoventity_I,
            Odyssey2Context context_M,
            out double numFinalCalculatedCost_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            numFinalCalculatedCost_O = 0;

            ResResource res = ResResource.resFromDB(intnPkResource_I, false);

            intStatus_IO = 409;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Resource not found.";
            if (
                res != null
                )
            {
                intStatus_IO = 410;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "PkResource is not for a resource.";
                if (
                    res.restypBelongsTo.strResOrPro == ProdtypProductType.strResource
                    )
                {
                    //                                      //Get the piw.
                    PiwentityProcessInWorkflowEntityDB piwentity = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                        piw.intPk == intPkProcessInWorkflow_I);

                    intStatus_IO = 411;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Process In Workflow not found.";
                    if (
                        piwentity != null
                        )
                    {
                        //                                  //Find resource account
                        int? intnPkResourceAccount = context_M.Cost.FirstOrDefault(cost =>
                            cost.intPkResource == intnPkResource_I).intPkAccount;

                        bool boolIsAccountAvailable = context_M.Account.FirstOrDefault(acc =>
                            acc.intPk == accmoventity_I.intPkAccount).boolAvailable;

                        intStatus_IO = 412;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "The account does not correspond to the resource.";
                        if (
                            !boolIsAccountAvailable ||
                            (intnPkResourceAccount != null && intnPkResourceAccount == accmoventity_I.intPkAccount)
                            )
                        {
                            //                              //To easy code.
                            int? intnPkElementElementType = null;
                            int? intnPkElementElement = null;
                            /*CASE*/
                            if (
                                boolnIsEleet_I == true
                                )
                            {
                                intnPkElementElementType = intnPkEleetOrEleele_I;
                            }
                            else if (
                                boolnIsEleet_I == false
                                )
                            {
                                intnPkElementElement = intnPkEleetOrEleele_I;
                            }
                            /*END-CASE*/

                            //                              //Get FinalCost register.
                            List<FnlcostentityFinalCostEntityDB> darrfnlentity = context_M.FinalCost.Where(fnl =>
                                fnl.intPkJob == jobentity_I.intPk &&
                                fnl.intPkProcessInWorkflow == intPkProcessInWorkflow_I &&
                                fnl.intnPkResource == intnPkResource_I &&
                                ((fnl.intnPkElementElementType == intnPkElementElementType) &&
                                fnl.intnPkElementElement == intnPkElementElement)).ToList();
                            darrfnlentity.Sort();

                            intStatus_IO = 413;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "The movement is not from the resource.";
                            if (
                                (darrfnlentity.Count > 0) &&
                                (!boolIsAccountAvailable ||
                                (darrfnlentity.Last().intPkAccountMovement == accmoventity_I.intPk))
                                )
                            {
                                intStatus_IO = 414;
                                strUserMessage_IO = "The info is already saved.";
                                strDevMessage_IO = "";
                                if (
                                    //                      //The last entry is equal to the current.
                                    !(darrfnlentity.Last().numnCost == numnFinalCost_I &&
                                    darrfnlentity.Last().numnQuantity == numnFinalQuantity_I)
                                    )
                                {
                                    double? numnCostToAdd;
                                    double? numnQuantityToAdd;
                                    int? intnPkResourceToAdd;
                                    int? intnPkCalculationToAdd;
                                    bool boolSuccess;
                                    numFinalCalculatedCost_O = JobJob.GetFinalCost(null, intnPkResource_I,
                                        numnFinalQuantity_I, numnFinalCost_I, jobentity_I, out numnCostToAdd,
                                        out numnQuantityToAdd, out intnPkResourceToAdd, out intnPkCalculationToAdd,
                                        out boolSuccess, ref strUserMessage_IO, ref strDevMessage_IO);

                                    intStatus_IO = 415;
                                    if (
                                        boolSuccess
                                        )
                                    {
                                        if (
                                            numnCostToAdd != null
                                            )
                                        {
                                            numnCostToAdd = ((double)numnCostToAdd).Round(2);
                                        }

                                        //                  //There is not cost, create a register.
                                        FnlcostentityFinalCostEntityDB fnlentityNew = new FnlcostentityFinalCostEntityDB
                                        {
                                            numnCost = numnCostToAdd,
                                            numnQuantity = numnQuantityToAdd,
                                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                            strDescription = strDescription_I,
                                            intPkJob = jobentity_I.intPk,
                                            intPkProcessInWorkflow = intPkProcessInWorkflow_I,
                                            intnPkElementElementType = intnPkElementElementType,
                                            intnPkElementElement = intnPkElementElement,
                                            intnPkResource = intnPkResourceToAdd,
                                            intnPkCalculation = intnPkCalculationToAdd,
                                            intContactId = intContactId_I,
                                            intPkAccountMovement = accmoventity_I.intPk
                                        };
                                        context_M.FinalCost.Add(fnlentityNew);

                                        if (
                                            //              //The account is Available
                                            boolIsAccountAvailable
                                            )
                                        {
                                            FnlcostentityFinalCostEntityDB fnlcostentityFirst =
                                                darrfnlentity.First();

                                            double numQuantity = (double)fnlcostentityFirst.numnQuantity;
                                            double numCost = (double)fnlcostentityFirst.numnCost;
                                            double numFinalCost = numCost;

                                            if (
                                                //          //Final quantity changed.
                                                numnFinalQuantity_I != null
                                                )
                                            {
                                                double numFinalQuantity = (double)numnFinalQuantity_I;
                                                if (
                                                    numCost > 0 &&
                                                    numQuantity > 0 &&
                                                    numFinalQuantity > 0
                                                    )
                                                {
                                                    numFinalCost = (numCost * numFinalQuantity) / numQuantity;
                                                }
                                                else
                                                {
                                                    numFinalCost = 0;
                                                }
                                            }
                                            else
                                            {
                                                numFinalCost = (double)numnCostToAdd;
                                            }

                                            //              //Update movement amount
                                            accmoventity_I.numnIncrease = numFinalCost.Round(2);
                                            accmoventity_I.strConcept = strDescription_I;
                                            accmoventity_I.strStartDate = Date.Now(ZonedTimeTools.timezone).ToString();
                                            accmoventity_I.strStartTime = Time.Now(ZonedTimeTools.timezone).ToString();

                                            intStatus_IO = 200;
                                            strUserMessage_IO = "";
                                            strDevMessage_IO = "";
                                        }
                                        else
                                        {
                                            intStatus_IO = 200;
                                            strUserMessage_IO = "There is not account releated to this cost.";
                                            strDevMessage_IO = "";
                                        }

                                        context_M.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolCalculationApplyConsideringJobDate(
            //                                              //Verify calculation and job's date in order to know if
            //                                              //  calculation apply to job.
            CalCalculation cal_I,
            JobentityJobEntityDB jobentity_I
            )
        {
            //                                              //Get job date.
            ZonedTime ztimeJobDate = ZonedTimeTools.NewZonedTime(jobentity_I.strStartDate.ParseToDate(),
                jobentity_I.strStartTime.ParseToTime());

            //                                              //Get calculation date.
            ZonedTime ztimeStarCalDate = ZonedTimeTools.NewZonedTime(cal_I.strStartDate.ParseToDate(),
                cal_I.strStartTime.ParseToTime());
            //                                              //Verify calculation's endDate.
            bool boolCalculationApply = false;
            if (
                //                                          //Calculation still able.
                cal_I.strEndDate == null
                )
            {
                if (
                    //                                      //Calculation apply.
                    ztimeJobDate >= ztimeStarCalDate
                    )
                {
                    boolCalculationApply = true;
                }
            }
            else
            {
                //                                          //Calculation already finish.
                ZonedTime ztimeEndCalDate = ZonedTimeTools.NewZonedTime(cal_I.strEndDate.ParseToDate(),
                    cal_I.strEndTime.ParseToTime());
                if (
                    ztimeJobDate >= ztimeStarCalDate &&
                    ztimeJobDate < ztimeEndCalDate
                    )
                {
                    boolCalculationApply = true;
                }
            }
            return boolCalculationApply;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static double GetFinalCost(
            //                                              //Method to calculate final cost.
            CalCalculation cal_I,
            int? intnPkResource_I,
            double? numnFinalQuantity_I,
            double? numnFinalCost_I,
            JobentityJobEntityDB jobentity_I,
            out double? numnCostToAdd_O,
            out double? numnQuantityToAdd_O,
            out int? intnPkResource_O,
            out int? intnPkCalculation_O,
            out bool boolSuccess_O,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            double numFinalCalculateCost = 0.0;
            //                                              //To easy code.
            numnCostToAdd_O = null;
            numnQuantityToAdd_O = null;
            intnPkResource_O = null;
            intnPkCalculation_O = null;
            boolSuccess_O = false;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "For a base calculation you need a cost not a quantity.";
            /*CASE*/
            if (
                //                                          //Is a ByProcess PerQuantity calculation.
                cal_I != null &&
                cal_I.strByX == CalCalculation.strByProcess &&
                cal_I.strCalculationType == CalCalculation.strPerQuantity
                )
            {
                if (
                    numnFinalQuantity_I != null
                    )
                {
                    double numnUnitCost = ((double)(cal_I.numnCost / cal_I.numnQuantity)).Round(2);
                    numFinalCalculateCost = ((double)(numnFinalQuantity_I * numnUnitCost)).Round(2);

                    numnQuantityToAdd_O = numnFinalQuantity_I;
                    intnPkCalculation_O = cal_I.intPk;
                    boolSuccess_O = true;
                }
                else
                {
                    numFinalCalculateCost = (double)numnFinalCost_I;

                    numnCostToAdd_O = numnFinalCost_I;
                    intnPkCalculation_O = cal_I.intPk;
                    boolSuccess_O = true;
                }
            }
            else if (
                //                                          //Is a ByProcess Base calculation.
                cal_I != null &&
                cal_I.strByX == CalCalculation.strByProcess &&
                cal_I.strCalculationType == CalCalculation.strBase &&
                numnFinalCost_I != null
                )
            {
                numFinalCalculateCost = (double)numnFinalCost_I;

                numnCostToAdd_O = numnFinalCost_I;
                intnPkCalculation_O = cal_I.intPk;
                boolSuccess_O = true;
            }
            else if (
                //                                          //Is a resource.
                intnPkResource_I != null
               )
            {
                if (
                    numnFinalQuantity_I != null
                    )
                {
                    ResResource res = ResResource.resFromDB(intnPkResource_I, false);
                    ZonedTime ztimeJobDate = ZonedTimeTools.NewZonedTime(jobentity_I.strStartDate.ParseToDate(),
                            jobentity_I.strStartTime.ParseToTime());
                    CostentityCostEntityDB costentity = res.GetCostDependingDate(ztimeJobDate);

                    double numUnitCost = 0;
                    if (
                        costentity != null && costentity.numnCost != null
                        )
                    {
                        double? numCalculationQuantity = 0.0;
                        double? numnCalculationMin = 0.0;
                        double? numnCalculationBlock = 0.0;
                        double? numnUnitCost = 0.0;
                        ProdtypProductType.subGetCostEntityData(costentity, ref numnUnitCost,
                            ref numCalculationQuantity, ref numnCalculationMin,
                            ref numnCalculationBlock);
                        numUnitCost = (double)numnUnitCost;
                    }
                    numFinalCalculateCost = ((double)(numnFinalQuantity_I * numUnitCost)).
                            Round(2);

                    numnQuantityToAdd_O = numnFinalQuantity_I;
                    intnPkResource_O = intnPkResource_I;
                    boolSuccess_O = true;
                }
                else
                {
                    numFinalCalculateCost = (double)numnFinalCost_I;

                    numnCostToAdd_O = numnFinalCost_I;
                    intnPkResource_O = intnPkResource_I;
                    boolSuccess_O = true;
                }
            }
            /*END-CASE*/
            return numFinalCalculateCost;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subConfirmResourceAutomaticallySet(
            String strPrintshopId_I,
            int intJobId_I,
            int intPkProcessInWorkflow_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 400;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No job found.";
            JobjsonJobJson jobjsonJob;
            if (
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjsonJob,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //To easy code.
                int? intnPkElementElementType = null;
                int? intnPkElementElement = intPkEleetOrEleele_I;
                if (
                    boolIsEleet_I
                    )
                {
                    intnPkElementElementType = intPkEleetOrEleele_I;
                    intnPkElementElement = null;
                }

                //                                          //Find Io job.
                IojentityInputsAndOutputsForAJobEntityDB iojentity = context.InputsAndOutputsForAJob.FirstOrDefault(
                    ioj => ioj.intJobId == intJobId_I && ioj.intPkProcessInWorkflow == intPkProcessInWorkflow_I &&
                    ioj.intnPkElementElementType == intnPkElementElementType &&
                    ioj.intnPkElementElement == intnPkElementElement && ioj.boolnWasSetAutomatically == true);

                intStatus_IO = 403;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Invalid data for confirm.";
                if (
                    iojentity != null
                    )
                {
                    iojentity.boolnWasSetAutomatically = false;
                    context.InputsAndOutputsForAJob.Update(iojentity);
                    context.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeleteEstimationDataEntriesForAWorkflow(
            //                                              //Delete new workflow's estimates.
            //                                              //Delete estimates' prices.
            //                                              //Delete new workflow's estimation data.

            Odyssey2Context context_M,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity_I
            )
        {
            foreach (PiwentityProcessInWorkflowEntityDB piwForDeteleEstimation in darrpiwentity_I)
            {
                //                                          //Find estimation data.
                List<EstdataentityEstimationDataEntityDB> darrestdataentity = context_M.EstimationData.Where(est =>
                    est.intPkProcessInWorkflow == piwForDeteleEstimation.intPk).ToList();

                foreach (EstdataentityEstimationDataEntityDB estentityToDelete in darrestdataentity)
                {
                    context_M.EstimationData.Remove(estentityToDelete);
                }
            }

            foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentity_I)
            {
                //                                          //Find Estimates.
                List<EstentityEstimateEntityDB> darrestentity = context_M.Estimate.Where(est =>
                        est.intPkWorkflow == piwentity.intPkWorkflow).ToList();

                foreach (EstentityEstimateEntityDB estentity in darrestentity)
                {
                    //                                      //Find estimate's prices.
                    List<PriceentityPriceEntityDB> darrprice = context_M.Price.Where(price =>
                        price.intJobId == estentity.intJobId && price.intnPkEstimate == estentity.intPk).ToList();

                    foreach (PriceentityPriceEntityDB priceentity in darrprice)
                    {
                        context_M.Price.Remove(priceentity);
                    }
                    context_M.Estimate.Remove(estentity);
                }
            }

            context_M.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subRenameEstimation(
            int intJobId_I,
            int intEstimationId_I,
            int intPkWorkflow_I,
            String strName_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Job is not valid.";
            JobjsonJobJson jobjson;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                EstentityEstimateEntityDB estentityEstimate = context.Estimate.FirstOrDefault(est =>
                    est.intPkWorkflow == intPkWorkflow_I && est.intJobId == intJobId_I && est.intId == intEstimationId_I);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Estimate not found.";
                if (
                    estentityEstimate != null
                    )
                {
                    intStatus_IO = 402;
                    strDevMessage_IO = "Estimation name is not valid.";
                    if (
                        JobJob.boolIsValidEstimationName(strName_I, estentityEstimate,
                        ref strUserMessage_IO)
                        )
                    {
                        //                                      //Find desired estimate.
                        EstentityEstimateEntityDB estentity = context.Estimate.FirstOrDefault(est =>
                            est.intJobId == intJobId_I && est.intPkWorkflow == intPkWorkflow_I &&
                            est.intId == intEstimationId_I);

                        intStatus_IO = 403;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Estimation not found.";
                        if (
                            estentity != null
                            )
                        {
                            estentity.strName = strName_I;
                            context.SaveChanges();

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolIsValidEstimationName(
            //                                              //Verifies that an estimations name doesn't exist in the
            //                                              //      same printshop.

            String strName_I,
            EstentityEstimateEntityDB estentityEstimate_I,
            ref String strUserMessage_IO
            )
        {
            //                                          //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            bool boolValidEstimateName = false;

            strUserMessage_IO = "Name cannot be empty";
            if (
                (strName_I != null) &&
                (strName_I != "")
                )
            {
                strUserMessage_IO = "It's the same name";
                if (
                    //                                      //It is the same name.
                    estentityEstimate_I.strName == strName_I
                    )
                {
                    boolValidEstimateName = false;
                }
                else
                {
                    //                                      //Get all estimate of this job and workflow.
                    List<EstentityEstimateEntityDB> darrestentityEstimate = context.Estimate.Where(est =>
                    est.intPkWorkflow == estentityEstimate_I.intPkWorkflow && est.intJobId == estentityEstimate_I.intJobId &&
                    est.strName == strName_I).ToList();

                    strUserMessage_IO = "Name already exists.";
                    if (
                        darrestentityEstimate.Count == 0
                        )
                    {
                        boolValidEstimateName = true;
                    }
                }
            }

            return boolValidEstimateName;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSendEstimatesPrices(
            //                                              //Estimate price to send.

            int intJobId_I,
            int intContactId_I,
            int intPkWorkflow_I,
            PsPrintShop ps_I,
            bool boolSendEmail_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            JobjsonJobJson jobjson;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                SndestjsoninSendEstimateJsonInternal sndestjsonPricesEstimate = null;

                //                                          //Validate Workflow.
                WfentityWorkflowEntityDB wfentityRecieved = context_M.Workflow.FirstOrDefault(wf =>
                    wf.intPk == intPkWorkflow_I &&
                    wf.boolDeleted == false);

                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong";
                strDevMessage_IO = "Workflow not found.";
                if (
                    wfentityRecieved != null
                    )
                {
                    //                                          //Update current estimate sent to null.
                    List<EstentityEstimateEntityDB> darrestentityJob = context_M.Estimate.Where(est =>
                        est.intJobId == intJobId_I).ToList();

                    foreach (EstentityEstimateEntityDB estentity in darrestentityJob)
                    {
                        estentity.boolSentToWebSite = false;
                        context_M.Estimate.Update(estentity);
                    }
                    context_M.SaveChanges();

                    List<EstentityEstimateEntityDB> darrestentity = context_M.Estimate.Where(est =>
                        est.intJobId == intJobId_I && est.intPkWorkflow == intPkWorkflow_I &&
                        est.intId == JobJob.intIdConfirmedEstimate).ToList();

                    intStatus_IO = 402;
                    strUserMessage_IO = "There are no estimations confirmed.";
                    strDevMessage_IO = "There are no estimations confirmed.";
                    if (
                        darrestentity.Count > 0
                        )
                    {
                        sndestjsonPricesEstimate = new SndestjsoninSendEstimateJsonInternal();
                        sndestjsonPricesEstimate.intJobId = intJobId_I;
                        sndestjsonPricesEstimate.intContactId = intContactId_I;
                        sndestjsonPricesEstimate.Notes = "";
                        sndestjsonPricesEstimate.Terms = "";
                        String strPrices = "";

                        /*REPEATE-WHILE*/
                        int intI = 0;
                        while (
                            intI < darrestentity.Count
                            )
                        {
                            double numPrice = 0.0;
                            int intQty = 0;
                            if (
                            //                              //This estimate has a prices calculated.
                            darrestentity[intI].numnLastPrice != null
                            )
                            {
                                numPrice = (double)darrestentity[intI].numnLastPrice;
                                intQty = darrestentity[intI].intnQuantity == null ? (int)jobjson.intnQuantity :
                                    (int)darrestentity[intI].intnQuantity;

                                intStatus_IO = 200;
                                strUserMessage_IO = "";
                                strDevMessage_IO = "";
                            }
                            else
                            {
                                BdgestjsonBudgetEstimationJson bdgestjson;
                                JobJob.subGetBudgetEstimation(intJobId_I, intPkWorkflow_I, darrestentity[intI].intId,
                                    darrestentity[intI].intnCopyNumber, null, null, ps_I, configuration_I, out bdgestjson,
                                    context_M, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                                numPrice = (double)bdgestjson.numnJobEstimatePrice;
                                intQty = (int)bdgestjson.intnQuantity;
                            }

                            /*CASE*/
                            if (
                                //                          //Set first price.
                                intI == 0
                                )
                            {
                                sndestjsonPricesEstimate.numnPrice1 = numPrice;
                                sndestjsonPricesEstimate.intnQty1 = intQty;
                                strPrices = numPrice + "";
                            }
                            else if (
                                //                          //set second price.
                                intI == 1
                                )
                            {
                                sndestjsonPricesEstimate.numnPrice2 = numPrice;
                                sndestjsonPricesEstimate.intnQty2 = intQty;
                                strPrices = strPrices + "|" + numPrice;
                            }
                            else if (
                                //                          //set second price.
                                intI == 2
                                )
                            {
                                sndestjsonPricesEstimate.numnPrice3 = numPrice;
                                sndestjsonPricesEstimate.intnQty3 = intQty;
                                strPrices = strPrices + "|" + numPrice;
                            }
                            /*END-CASE*/

                            darrestentity[intI].boolSentToWebSite = true;
                            context_M.Estimate.Update(darrestentity[intI]);
                            context_M.SaveChanges();
                            intI = intI + 1;
                        }

                        //                                  //SEND JSON ESTIMATE PRICE TO WISNET.
                        //                                  //    sndestjsonPricesEstimate

                        String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                            GetSection("Odyssey2Settings")["urlWisnetApi"];

                        Task<String> Task_PostEstimateFromWisnet =
                        HttpTools<TjsonTJson>.PostEstimatePricesAsyncToEndPoint(strUrlWisnet +
                            "/Estimates/" + intJobId_I, sndestjsonPricesEstimate, configuration_I);
                        Task_PostEstimateFromWisnet.Wait();

                        intStatus_IO = 401;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Wisnet connection lost or there are not completed orders.";
                        if (
                            //                              //There is access to the service of Wisnet.
                            Task_PostEstimateFromWisnet.Result != null
                            )
                        {
                            //                              //Delete notifications.
                            JobJob.subDeleteNotificationFromJob(intJobId_I, ps_I, iHubContext_I, context_M);

                            //                              //Find the jobjson.
                            JobjsonentityJobJsonEntityDB jobjsonUpdatePrice = context_M.JobJson.FirstOrDefault(
                                jobjson => jobjson.intJobID == intJobId_I);

                            jobjsonUpdatePrice.strPrice = strPrices;
                            context_M.Update(jobjsonUpdatePrice);
                            context_M.SaveChanges();

                            //                              //Send email to printbuyer.
                            if (
                                boolSendEmail_I
                                )
                            {
                                JobJob.subSendEmailToPrintbuyer("notify_present_estimate", intContactId_I, intJobId_I,
                                    true, ps_I, ref strUserMessage_IO, ref strDevMessage_IO);
                            }

                            intStatus_IO = 200;
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subSendEmailToPrintbuyer(

            String strAction_I,
            int intEmployeeContactId_I,
            int intOrderOrJobId_I,
            bool boolAddJob_I,
            PsPrintShop ps_I,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            String strMessageApi = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["MessageApi"];
            String strWisnetToken = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["WisnetToken"];
            String strUrlMessageApi = "https://" + ps_I.strUrl + "/" + strMessageApi;

            if (
                boolAddJob_I
                )
            {
                JobemailtopbjsoninJobEmailToPrintbuyerJsonInternal jobemailtopbjsonin = 
                    new JobemailtopbjsoninJobEmailToPrintbuyerJsonInternal(strAction_I, intEmployeeContactId_I,
                    intOrderOrJobId_I);

                Task<String> Task_PostEmailToPrintbuyer =
                    HttpTools<TjsonTJson>.Task_PostJobEmailToPrintbuyer(strUrlMessageApi, jobemailtopbjsonin,
                    strWisnetToken);
                Task_PostEmailToPrintbuyer.Wait();

                strUserMessage_IO = "Unable to send message.";
                strDevMessage_IO = "Something fail with the Message API.";
                if (
                    //                                      //There is access to the service of Wisnet.
                    Task_PostEmailToPrintbuyer.Result != null
                    )
                {
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
            else
            {
                OrderemailtopbjsoninOrderEmailToPrintbuyerJsonInternal orderemailtopbjsonin = 
                    new OrderemailtopbjsoninOrderEmailToPrintbuyerJsonInternal(strAction_I, intEmployeeContactId_I,
                    intOrderOrJobId_I);

                Task<String> Task_PostEmailToPrintbuyer =
                    HttpTools<TjsonTJson>.Task_PostOrderEmailToPrintbuyer(strUrlMessageApi, orderemailtopbjsonin,
                    strWisnetToken);
                Task_PostEmailToPrintbuyer.Wait();

                strUserMessage_IO = "Unable to send message.";
                strDevMessage_IO = "Something fail with the Message API.";
                if (
                    //                                      //There is access to the service of Wisnet.
                    Task_PostEmailToPrintbuyer.Result != null
                    )
                {
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subDeleteNotificationFromJob(
            //                                              //Delete Notifications from the JobId.

            int intJobId_I,
            PsPrintShop ps_I,
            IHubContext<ConnectionHub> iHubContext_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Init the list for reduce notification.
            List<int> darrintRepsOrSuperviserToReduce = new List<int>();

            //                          //Add alerts type New Estimate.
            AlerttypeentityAlertTypeEntityDB alerttypeentityNewEstimate = context_M.AlertType.FirstOrDefault(
                at => at.strType == AlerttypeentityAlertTypeEntityDB.strNewEstimate);

            //                                              //Get Notifications from Job.
            List<AlertentityAlertEntityDB> darralertentity = context_M.Alert.Where(alert =>
                alert.intPkPrintshop == ps_I.intPk && alert.intnJobId == intJobId_I &&
                alert.intPkAlertType == alerttypeentityNewEstimate.intPk).ToList();

            foreach (AlertentityAlertEntityDB alertentity in darralertentity)
            {
                if (
                    //                                      //Only reduce if notification not read by contact.
                    alertentity.intnContactId != null &&
                    !PsPrintShop.boolNotificationReadByUser(alertentity,
                    (int)alertentity.intnContactId)
                    )
                {
                    darrintRepsOrSuperviserToReduce.Add((int)alertentity.intnContactId);
                }

                //                                          //Remove Notifications.
                context_M.Alert.Remove(alertentity);
                context_M.SaveChanges();
            }

            //                                              //Send reduce notifications to the Reps.
            AlnotAlertNotification.subReduceToAFew(ps_I.strPrintshopId,
                darrintRepsOrSuperviserToReduce.ToArray(), iHubContext_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subEstimateToOrder(
            //                                              //Turn an Estimate into an Order.

            int intJobId_I,
            int intEstimationId_I,
            int intPkWorkflow_I,
            int? intnCopyNumber_I,
            int intContactId_I,
            PsPrintShop ps_I,
            Odyssey2Context context_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            JobjsonJobJson jobjson;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong";
                strDevMessage_IO = "Estimate invalid, This is not a estimate.";
                if (
                    //                                      //Validate that it is a estimation.
                    (jobjson.intnOrderId != null && jobjson.intnOrderId == 0) || jobjson.intnOrderId == null &&
                    //                                      //Should be a estimate confirmed.
                    intEstimationId_I == 0
                    )
                {
                    //                                      //Get Estimate.
                    EstentityEstimateEntityDB estentity = context_I.Estimate.FirstOrDefault(est =>
                        est.intId == intEstimationId_I && est.intnCopyNumber == intnCopyNumber_I &&
                        est.intJobId == jobjson.intJobId && est.intPkWorkflow == intPkWorkflow_I);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong";
                    strDevMessage_IO = "Estimate not found.";
                    if (
                        estentity != null
                        )
                    {
                        double numPrice1;
                        int intQty1;
                        if (
                            //                              //This estimate has a prices calculated.
                            estentity.numnLastPrice != null
                            )
                        {
                            numPrice1 = (double)estentity.numnLastPrice;
                            intQty1 = estentity.intnQuantity == null ? (int)jobjson.intnQuantity :
                                (int)estentity.intnQuantity;

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                        else
                        {
                            BdgestjsonBudgetEstimationJson bdgestjson;
                            JobJob.subGetBudgetEstimation(intJobId_I, intPkWorkflow_I, estentity.intId,
                                estentity.intnCopyNumber, null, null, ps_I, configuration_I, out bdgestjson,
                                context_I, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                            numPrice1 = (double)bdgestjson.numnJobEstimatePrice;
                            intQty1 = (int)bdgestjson.intnQuantity;
                        }

                        if (
                            //                              //Status 200.
                            intStatus_IO == 200 &&
                            //                              //The price and quantity should be grather 
                            //                              //    than zero.
                            numPrice1 > 0 &&
                            intQty1 > 0
                            )
                        {
                            //                              //Send json to turn estimate into order.
                            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                                GetSection("Odyssey2Settings")["urlWisnetApi"];

                            ToorderjsoninToOrderJsonInternal toorderjson = new ToorderjsoninToOrderJsonInternal(
                                intJobId_I, intContactId_I, (ps_I.strPrintshopId).ParseToInt(), intQty1,
                                numPrice1);

                            Task<String> Task_PostEstimateFromWisnet =
                            HttpTools<TjsonTJson>.PostEstimateToOrderAsyncToEndPoint(strUrlWisnet +
                                "/Estimates/ToOrder/" + intJobId_I, toorderjson);
                            Task_PostEstimateFromWisnet.Wait();

                            intStatus_IO = 401;
                            strUserMessage_IO = "Unable to connect the Website.";
                            strDevMessage_IO = "Wisnet connection lost or there are not completed orders.";
                            if (
                                //                          //There is access to the service of Wisnet.
                                Task_PostEstimateFromWisnet.Result != null
                                )
                            {
                                //                          //Delete other confirmed estimation with other workflows.
                                JobJob.subDeleteOtherEstimateConfirmed(estentity, context_I);

                                //                          //Delete the jobjson, beacuse other price and quantity
                                //                          //    can be setted for this job.
                                //                          //    
                                //JobjsonentityJobJsonEntityDB jobjsonentity = context_I.JobJson.FirstOrDefault(
                                //    jobjson => jobjson.intJobID == estentity.intJobId);

                                //context_I.JobJson.Remove(jobjsonentity);
                                //context_I.SaveChanges();

                                //                          //Deleted notification for this estimation.
                                JobJob.subDeleteEstimateAlert(intJobId_I, ps_I, context_I);

                                //                          //Add accordated price with the client. 
                                PriceentityPriceEntityDB priceentity = new PriceentityPriceEntityDB
                                {
                                    numnPrice = numPrice1,
                                    intJobId = intJobId_I,
                                    strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                    strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                    strDescription = "Final price estimate accordated for the Job.",
                                    intContactId = intContactId_I,
                                    boolIsReset = false,
                                    intnPkWorkflow = intPkWorkflow_I,
                                    intnPkEstimate = null
                                };

                                context_I.Price.Add(priceentity);
                                context_I.SaveChanges();

                                //                          //Find the jobjson for update the price.
                                //                              //Find the jobjson.
                                JobjsonentityJobJsonEntityDB jobjsonUpdatePrice = context_I.JobJson.FirstOrDefault(
                                    jobjson => jobjson.intJobID == intJobId_I);

                                jobjsonUpdatePrice.strPrice = numPrice1 + "";
                                context_I.Update(jobjsonUpdatePrice);
                                context_I.SaveChanges();

                                intStatus_IO = 200;
                                strUserMessage_IO = "";
                                strDevMessage_IO = "";
                            }
                        }
                        else
                        {
                            intStatus_IO = 404;
                            strUserMessage_IO = "Price can not be 0.";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteEstimateAlert(
            //                                              //Delete the alert, if exists, for an estimate when an 
            //                                              //      estimate is accepted from odyssey2.0

            int intJobId_I,
            PsPrintShop ps_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Find new estimate alert type.
            AlerttypeentityAlertTypeEntityDB alerttypeentity = context_I.AlertType.FirstOrDefault(alert =>
                alert.strType == AlerttypeentityAlertTypeEntityDB.strNewEstimate);

            if (alerttypeentity != null)
            {
                //                                          //Try to get a notification for the estimate.
                AlertentityAlertEntityDB alertentity = context_I.Alert.FirstOrDefault(alert =>
                    alert.intPkPrintshop == ps_I.intPk && alert.intPkAlertType == alerttypeentity.intPk &&
                    alert.intnJobId == intJobId_I);

                //                                          //If an alert exists, deleted.
                if (
                    alertentity != null
                    )
                {
                    context_I.Alert.Remove(alertentity);
                    context_I.SaveChanges();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subEstimateToRejected(
            //                                              //Turn an Estimate into an Rejected.

            int intJobId_I,
            PsPrintShop ps_I,
            int intContactId_I,
            Odyssey2Context context_M,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            JobjsonJobJson jobjson;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Send json to turn estimate into rejected.
                String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                    GetSection("Odyssey2Settings")["urlWisnetApi"];

                SendtowisjsoninSendToWisnetJsonInternal sendtowisn = new SendtowisjsoninSendToWisnetJsonInternal(
                    intJobId_I, ps_I.strPrintshopId, intContactId_I);

                Task<String> Task_PostToRejectedFromWisnet =
                HttpTools<TjsonTJson>.PostEstimateToRejectedAsyncToEndPoint(strUrlWisnet +
                    "/Estimates/EstimateToRejected/", sendtowisn, configuration_I);
                Task_PostToRejectedFromWisnet.Wait();

                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Wisnet connection lost or there are not completed orders.";
                if (
                    //                          //There is access to the service of Wisnet.
                    Task_PostToRejectedFromWisnet.Result != null
                    )
                {
                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteOtherEstimateConfirmed(
            //                                              //Delete other estimate confirmed of the
            //                                              //    of the other workflow for this Job.

            EstentityEstimateEntityDB estentity_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Get other estimate confirmed.
            List<EstentityEstimateEntityDB> darrestimateAllEstimateConfirmedWithOtherWorkflow =
                                    context_M.Estimate.Where(est => est.intJobId == estentity_I.intJobId &&
                                    est.intPkWorkflow != estentity_I.intPkWorkflow).ToList();

            foreach (EstentityEstimateEntityDB estEstimateConfirmedOtherWorkflow in
                darrestimateAllEstimateConfirmedWithOtherWorkflow)
            {
                context_M.Estimate.Remove(estEstimateConfirmedOtherWorkflow);
            }
            context_M.SaveChanges();

            //                                              //Get estimation data of the other workflow.
            List<EstdataentityEstimationDataEntityDB> darrestdataOtherWorkflow =
                (from estdataentity in context_M.EstimationData
                 join piwentity in context_M.ProcessInWorkflow
                 on estdataentity.intPkProcessInWorkflow equals piwentity.intPk
                 where piwentity.intPkWorkflow != estentity_I.intPkWorkflow &&
                 estdataentity.intJobId == estentity_I.intJobId
                 select estdataentity).ToList();

            foreach (EstdataentityEstimationDataEntityDB estData in darrestdataOtherWorkflow)
            {
                context_M.EstimationData.Remove(estData);
            }

            //                                              //Get job confirmed in IOJ of the other workflow.
            List<IojentityInputsAndOutputsForAJobEntityDB> darriojentityOtherWorkflow =
                (from iojentity in context_M.InputsAndOutputsForAJob
                 join piwentity in context_M.ProcessInWorkflow
                 on iojentity.intPkProcessInWorkflow equals piwentity.intPk
                 where piwentity.intPkWorkflow != estentity_I.intPkWorkflow &&
                 iojentity.intJobId == estentity_I.intJobId
                 select iojentity).ToList();

            foreach (IojentityInputsAndOutputsForAJobEntityDB iojentity in darriojentityOtherWorkflow)
            {
                context_M.InputsAndOutputsForAJob.Remove(iojentity);
            }

            context_M.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetJobAsPending(
            //                                              //Job to pending stage.

            int intJobId_I,
            int intEstimationId_I,
            int intPkWorkflow_I,
            int intContactId_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            JobjsonJobJson jobjson;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong";
                strDevMessage_IO = "Estimate invalid, This is not a estimate.";
                if (
                    //                                      //Validate that it is a estimation.
                    jobjson.intnOrderId != null &&
                    jobjson.intnOrderId > 0 &&
                    //                                      //Should be a estimate confirmed.
                    intEstimationId_I == 0
                    )
                {
                    //                                      //Get Estimate.
                    EstentityEstimateEntityDB estentity = context_M.Estimate.FirstOrDefault(est =>
                                est.intId == intEstimationId_I && est.intJobId == jobjson.intJobId &&
                                est.intPkWorkflow == intPkWorkflow_I);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong";
                    strDevMessage_IO = "Estimate not found.";
                    if (
                        estentity != null
                        )
                    {
                        double numPrice;
                        if (
                            //                              //This estimate has price.
                            estentity.numnLastPrice != null
                            )
                        {
                            numPrice = (double)estentity.numnLastPrice;

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                        else
                        {
                            BdgestjsonBudgetEstimationJson bdgestjson;
                            JobJob.subGetBudgetEstimation(intJobId_I, intPkWorkflow_I, estentity.intId,
                                estentity.intnCopyNumber, null, null, ps_I, configuration_I, out bdgestjson,
                                context_M, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                            numPrice = (double)bdgestjson.numnJobEstimatePrice;
                        }

                        if (
                            intStatus_IO == 200 &&
                            numPrice >= 0
                            )
                        {
                            //                              //Send json to turn job as pending.
                            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                                GetSection("Odyssey2Settings")["urlWisnetApi"];

                            JobpendjsoninJobPendingJsonInternal jobpendjsonin = new JobpendjsoninJobPendingJsonInternal(
                                intJobId_I, ps_I.strPrintshopId, numPrice, intContactId_I);

                            Task Task_PostEstimateFromWisnet = HttpTools<TjsonTJson>.PostAsyncToEndPoint(JsonSerializer.
                                Serialize(jobpendjsonin).ToString(), strUrlWisnet + "/PrintShopData/SetJobInProgress");
                            Task_PostEstimateFromWisnet.Wait();

                            intStatus_IO = 401;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "Wisnet connection lost.";
                            if (
                                Task_PostEstimateFromWisnet.IsCompleted
                                )
                            {
                                //                          //Add accordated price with the client. 
                                PriceentityPriceEntityDB priceentity = new PriceentityPriceEntityDB
                                {
                                    numnPrice = numPrice,
                                    intJobId = intJobId_I,
                                    strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                    strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                    strDescription = "Price approved by printbuyer.",
                                    intContactId = intContactId_I,
                                    boolIsReset = false,
                                    intnPkWorkflow = intPkWorkflow_I,
                                    intnPkEstimate = null
                                };

                                context_M.Price.Add(priceentity);
                                context_M.SaveChanges();

                                //                          //Delete notifications of the this JobId.
                                JobJob.subDeleteNotificationFromJob(intJobId_I, ps_I, iHubContext_I, context_M);

                                JobjsonentityJobJsonEntityDB jobjsonUpdatePrice = context_M.JobJson.FirstOrDefault(
                                jobjson => jobjson.intJobID == intJobId_I);

                                jobjsonUpdatePrice.strPrice = numPrice + "";

                                context_M.Update(jobjsonUpdatePrice);
                                context_M.SaveChanges();

                                intStatus_IO = 200;
                                strUserMessage_IO = "";
                                strDevMessage_IO = "";
                            }
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSendJobPrice(
            //                                              //Estimate price to send.

            int intJobId_I,
            int intContactId_I,
            int intPkWorkflow_I,
            bool boolSendEmail_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            JobjsonJobJson jobjson;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Establish the connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Validate Workflow.
                WfentityWorkflowEntityDB wfentityRecieved = context.Workflow.FirstOrDefault(wf =>
                    wf.intPk == intPkWorkflow_I &&
                    wf.boolDeleted == false);

                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong";
                strDevMessage_IO = "Workflow not found.";
                if (
                    wfentityRecieved != null
                    )
                {
                    //                                      //Get job's price. from estimate table.
                    EstentityEstimateEntityDB estentity = context.Estimate.FirstOrDefault(est =>
                        est.intJobId == intJobId_I && est.intPkWorkflow == intPkWorkflow_I);

                    double numJobPrice = 0.0;

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong";
                    strDevMessage_IO = "Estimate not found.";
                    if (
                        estentity != null
                        )
                    {
                        if (
                            //                              //This estimate has a prices calculated.
                            estentity.numnLastPrice != null
                            )
                        {
                            numJobPrice = (double)estentity.numnLastPrice;
                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                        else
                        {
                            BdgestjsonBudgetEstimationJson bdgestjson;
                            JobJob.subGetBudgetEstimation(intJobId_I, intPkWorkflow_I, estentity.intId,
                                estentity.intnCopyNumber, null, null, ps_I, configuration_I, out bdgestjson,
                                context, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                            numJobPrice = bdgestjson != null ? (double)bdgestjson.numnJobEstimatePrice : numJobPrice;
                        }
                    }

                    if (
                        intStatus_IO == 200
                        )
                    {
                        //                                      //Get order id.
                        JobjsonentityJobJsonEntityDB jobjsonentity = context.JobJson.FirstOrDefault(job =>
                        job.strPrintshopId == ps_I.strPrintshopId && job.intJobID == intJobId_I);
                        int intOrderId = jobjsonentity.intOrderId;

                        //                                      //SEND JOB'S PRICE TO WISNET.
                        String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                            GetSection("Odyssey2Settings")["urlWisnetApi"];

                        Task<String> Task_PostEstimateFromWisnet =
                        HttpTools<TjsonTJson>.PostJobPriceAsyncToEndPoint(strUrlWisnet +
                            "/PrintshopData/SetJobPrice", intJobId_I, ps_I, intContactId_I, intOrderId, numJobPrice);
                        Task_PostEstimateFromWisnet.Wait();

                        intStatus_IO = 402;
                        strUserMessage_IO = "Unable to sent, probably already sent.";
                        strDevMessage_IO = "Wisnet connection lost.";
                        if (
                                //                              //There is access to the service of Wisnet and job's price
                                //                              //      was stored successfully.
                                Task_PostEstimateFromWisnet.Result != null &&
                                Task_PostEstimateFromWisnet.Result.Contains("200")
                            )
                        {
                            //                              //Find the jobjson.
                            JobjsonentityJobJsonEntityDB jobjsonUpdatePrice = context.JobJson.FirstOrDefault(
                                jobjson => jobjson.intJobID == intJobId_I);

                            jobjsonUpdatePrice.strPrice = numJobPrice + "";

                            context.Update(jobjsonUpdatePrice);
                            context.SaveChanges();

                            //                              //Send email to printbuyer.
                            if (
                            boolSendEmail_I
                            )
                            {
                                JobJob.subSendEmailToPrintbuyer("notify_order_price_added", intContactId_I,
                                    (int)jobjson.intnOrderId, false, ps_I, ref strUserMessage_IO,
                                    ref strDevMessage_IO);
                            }

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subUpdateWorkInProgressStatus(
            //                                              //Change the stage of the InProgress job to another 
            //                                              //      printer's substage.

            int intJobId_I,
            String strStatus_I,
            bool boolSendEmail_I,
            PsPrintShop ps_I,
            int intContactId_I,
            IConfiguration configuration_I,
            Odyssey2Context context_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 400;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Job must be inProgress stage";
            if (
                context_I.Job.FirstOrDefault(job => job.intJobID == intJobId_I &&
                job.intStage == JobJob.intInProgressStage) != null
                )
            {
                //                                          //Update job's subStage at wisnet.
                String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection(
                    "Odyssey2Settings")["urlWisnetApi"];
                Task<String> Task_strResult = HttpTools<TjsonTJson>.PostUpdateWorkInProgressStatus(strUrlWisnet + 
                    "/Job/UpdateWorkInProgressStatus/", ps_I.strPrintshopId.ParseToInt(), intJobId_I, strStatus_I,
                    intContactId_I);

                Task_strResult.Wait();

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Wisnet connection lost.";
                if (
                    //                                      //There was access to the service of Wisnet.
                    Task_strResult.Result != null
                    )
                {
                    //                                      //subStage was updated succesfully.
                    intStatus_IO = Task_strResult.Result.Substring(0, Task_strResult.Result.IndexOf('|')).ParseToInt();
                    strUserMessage_IO = "";
                    strDevMessage_IO = Task_strResult.Result.Substring(Task_strResult.Result.IndexOf('|') + 1);

                    if (
                        boolSendEmail_I
                        )
                    {
                        JobJob.subSendEmailToPrintbuyer("notify_job_status_update", intContactId_I, intJobId_I,
                                    true, ps_I, ref strUserMessage_IO, ref strDevMessage_IO);
                    }

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeleteProcessNote(
            //                                              //Delete a process note

            int intPkNote_I,
            int intContactId_I,
            String strPrintshopId_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Invalid employee.";
            if (
                ResResource.boolEmployeeOrOwnerIsFromPrintshop(strPrintshopId_I, intContactId_I)
                )
            {
                Odyssey2Context context = new Odyssey2Context();

                PronotesentityProcessNotesEntityDB pronotentity = context.ProcessNotes.FirstOrDefault(note =>
                    note.intPk == intPkNote_I);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Process note not found.";
                if (
                    pronotentity != null
                    )
                {
                    context.ProcessNotes.Remove(pronotentity);

                    context.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSendEmailToCustomer(
            //                                              //Send an email when an order is completed.

            int intJobId_I,
            PsPrintShop ps_I,
            int intContactId_I,
            List<int> darrintOrdersIdPaid_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            /*CASE*/
            if (
                //                                          //Notify orders already paid.
                darrintOrdersIdPaid_I != null &&
                darrintOrdersIdPaid_I.Count > 0 &&
                intJobId_I == 0
                )
            {
                //                                          //Create the connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Get all printshop's paid orders.
                List<InvoInvoiceEntityDB> darrinventity = context.Invoice.Where(inv =>
                    inv.intPkPrintshop == ps_I.intPk && inv.numOpenBalance == 0.0).ToList();

                //                                          //Only if all invoices are already paid, we send the email.
                bool boolAllInvoicesArePaid = true;
                int intI = 0;
                /*WHILE-DO*/
                while (
                    //                                      //Invoice already paid.
                    boolAllInvoicesArePaid &&
                    //                                      //Still elements in the list.
                    intI < darrintOrdersIdPaid_I.Count()
                    )
                {
                    if (
                        //                                  //Order is already paid.
                        !darrinventity.Exists(inv => inv.intOrderNumber == darrinventity[intI].intOrderNumber)
                        )
                    {
                        boolAllInvoicesArePaid = false;
                    }

                    intI++;
                }

                if (
                    //                                      //All received invoices are paid.
                    boolAllInvoicesArePaid
                    )
                {
                    foreach (int intOrderId in darrintOrdersIdPaid_I)
                    {
                        JobJob.subSendEmailToPrintbuyer("notify_order_marked_as_paid", intContactId_I, intOrderId,
                            false, ps_I, ref strUserMessage_IO, ref strDevMessage_IO);
                    }

                    if (
                        strUserMessage_IO == ""
                        )
                    {
                        intStatus_IO = 200;
                    }
                }
            }
            else if (
                //                                          //Notify order completed.
                intJobId_I > 0 &&
                darrintOrdersIdPaid_I != null &&
                darrintOrdersIdPaid_I.Count == 0
                )
            {
                JobjsonJobJson jobjson;
                if (
                    boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                    )
                {
                    strUserMessage_IO = "Something wrong.";
                    strDevMessage_IO = "Order is not completed.";
                    if (
                        JobJob.boolAskForSendAnEmail(intJobId_I, ps_I) ||
                        darrintOrdersIdPaid_I != null
                        )
                    {
                        JobJob.subSendEmailToPrintbuyer("notify_order_complete", intContactId_I,
                            (int)jobjson.intnOrderId, false, ps_I, ref strUserMessage_IO, ref strDevMessage_IO);

                        if (
                            strUserMessage_IO == ""
                            )
                        {
                            intStatus_IO = 200;
                        }
                    }
                }
            }
            /*END-CASE*/
        }


        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static JobJob jobFromDB(
            int intId_I
            )
        {
            JobJob job = null;
            if (
                intId_I > 0
                )
            {
                //                                          //Create the connection.
                Odyssey2Context context = new Odyssey2Context();

                JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(jobentity =>
                    jobentity.intJobID == intId_I);

                if (
                    jobentity != null
                    )
                {
                    job = new JobJob(jobentity.intPk, jobentity.intJobID, jobentity.intStage, jobentity.intPkPrintshop);
                }
            }
            return job;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static JobticjsonJobTicketJson jobticjsonGetJobInfo(
            //                                              //Get the information to create job ticket for an specific 
            //                                              //      job.

            PsPrintShop ps_I,
            int intJobId_I,
            int intPkWorkflow_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //To fill after and return it.
            JobticjsonJobTicketJson jobticJson = null;

            //                                              //Get job info from Wisnet.

            //                                              //This object will contain the job's information necessary
            //                                              //      to send back.
            JobjsonJobJson jobjson;

            intStatus_IO = 401;
            strUserMessage_IO = "";
            strDevMessage_IO = "";
            if (
                //                                          //Valid Job and Get data from Order form.
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Create the connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Validate Workflow.
                WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                    wf.intPk == intPkWorkflow_I);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Workflow not valid.";
                if (
                    wfentity != null
                    )
                {
                    //                                      //Validate workflow belongs to Job's product.
                    EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                        et.intPk == wfentity.intnPkProduct);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Workflow do not belongs to job's product.";
                    if (
                        etentity.intWebsiteProductKey == jobjson.intnProductKey &&
                        etentity.strCustomTypeId == jobjson.strProductName &&
                        etentity.strCategory == jobjson.strProductCategory
                        )
                    {
                        //                                  //Get prodtyp.
                        ProdtypProductType prodtyp = (ProdtypProductType)EtElementTypeAbstract.etFromDB(
                            wfentity.intnPkProduct);

                        //                                  //Get job status.
                        JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job =>
                            job.intJobID == intJobId_I && job.intPkPrintshop == ps_I.intPk);

                        String strJobStatus;
                        if (
                            jobentity != null
                            )
                        {
                            strJobStatus = (jobentity.intStage == 2) ? JobJob.strInProgressStage :
                                JobJob.strCompletedStage;
                        }
                        else
                        {
                            strJobStatus = JobJob.strPendingStage;
                        }

                        double numJobExtraCost = 0.0;

                        //                                  //Get delivery date.
                        //                                  //List to Add piws.
                        List<Piwjson1ProcessInWorkflowJson1> darrpiwjson1 =
                            JobJob.darrPiwjson1GetListOfProcessInWorkflow(intJobId_I, intPkWorkflow_I, jobentity,
                            jobjson, ps_I, prodtyp, configuration_I, ref numJobExtraCost);

                        intStatus_IO = 404;
                        strUserMessage_IO = "The product workflow is incomplete. Complete the product workflow" +
                            " to see the job ticket.";
                        strDevMessage_IO = "";

                        if (
                            darrpiwjson1.Count > 0
                            )
                        {
                            String strDeliveryDate;
                            String strDeliveryTime;
                            JobJob.subGetDeliveryDateForAnSpecificJob(intJobId_I, darrpiwjson1, jobjson,
                                out strDeliveryDate, out strDeliveryTime);

                            if (
                                //                          //Delivery date exists
                                strDeliveryDate.Length > 0 && strDeliveryTime.Length > 0
                                )
                            {
                                //                          //Convert date
                                ZonedTime ztimeDeliveryDate = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                    strDeliveryDate.ParseToDate(), strDeliveryTime.ParseToTime(), ps_I.strTimeZone);

                                //                          //Assign date converted
                                strDeliveryDate = ztimeDeliveryDate.Date.ToString();
                                strDeliveryTime = ztimeDeliveryDate.Time.ToString();
                            }

                            //                              //Get due date.
                            List<DuedateentityDueDateEntityDB> darrduedateentity = context.DueDate.Where(due =>
                                due.intJobId == intJobId_I).ToList();
                            darrduedateentity.Sort();

                            String strDueDate;
                            String strDueTime;
                            if (
                                //                          //DueDate exists.
                                darrduedateentity.Count() > 0
                                )
                            {
                                //                          //Easy code
                                strDueDate = darrduedateentity[0].strDate;
                                strDueTime = darrduedateentity[0].strHour + ":" + darrduedateentity[0].strMinute + ":" +
                                    darrduedateentity[0].strSecond;

                                //                          //Convert date
                                ZonedTime ztimeDueDate = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                    strDueDate.ParseToDate(), strDueTime.ParseToTime(), ps_I.strTimeZone);

                                //                          //Assign date converted
                                strDueDate = ztimeDueDate.Date.ToString();
                                strDueTime = ztimeDueDate.Time.ToString();
                            }
                            else
                            {
                                strDueDate = "-";
                                strDueTime = "";
                            }

                            //                                  //Get Start Date/Time and End Date/Time.
                            String strStartDate;
                            String strStartTime;
                            String strEndDate;
                            String strEndTime;
                            if (
                                jobentity != null
                                )
                            {
                                //                          //Convert start date
                                ZonedTime ztimeStartDate = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                    jobentity.strStartDate.ParseToDate(), jobentity.strStartTime.ParseToTime(), 
                                    ps_I.strTimeZone);

                                strStartDate = ztimeStartDate.Date.ToString();
                                strStartTime = ztimeStartDate.Time.ToString();

                                if (
                                    jobentity.intStage == JobJob.intInProgressStage
                                    )
                                {
                                    strEndDate = "-";
                                    strEndTime = "-";
                                }
                                else
                                {
                                    //                      //Convert end date
                                    ZonedTime ztimeEndDate = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                        jobentity.strEndDate.ParseToDate(), jobentity.strEndTime.ParseToTime(),
                                        ps_I.strTimeZone);

                                    strEndDate = ztimeEndDate.Date.ToString();
                                    strEndTime = ztimeEndDate.Time.ToString();
                                }
                            }
                            else
                            {
                                strStartDate = "-";
                                strStartTime = "";
                                strEndDate = "-";
                                strEndTime = "";
                            }

                            //                                  //Get Workflow name.
                            String strWorkflowName = wfentity.strName;

                            //                                  //Get calculations arragement.
                            //                                  //Get Calculation by Product.
                            double numJobFinalCost = 0.0;
                            List<CostbycaljsonCostByCalculationJson> darrcostbycaljsonByProduct;

                            bool boolWorkflowJobIsReadyNotUsed = true;
                            double numCostByProduct = prodtyp.numGetCostByProduct(jobjson, ps_I,
                                out darrcostbycaljsonByProduct, ref boolWorkflowJobIsReadyNotUsed);
                            numJobFinalCost = numJobFinalCost + numCostByProduct;

                            //                                  //Get processes arragement.
                            /*List<ProcjobjsonProcessJobJson> darrprocjobjson =
                                JobJob.darrProcjobjsonsubGetJobProcess(intJobId_I, intPkWorkflow_I, prodtyp, jobjson, ps_I,
                                configuration_I);*/

                            //                                  //Get processes array
                            List<ProcjobjsonProcessJobJson> darrprocjobjson = JobJob.darrprocjobjsonsubGetJobProcess(
                                intPkWorkflow_I, prodtyp, jobjson, ps_I, darrpiwjson1);

                            //                                  //Get job cost. and job price.
                            double numJobPrice = 0;
                            double numJobCost = 0;
                            double numJobProfit = 0;
                            double numJobFinalProfit = 0;
                            ProdtypProductType.subGetJobPriceCostAndProfit(prodtyp, jobjson, numCostByProduct,
                                darrpiwjson1, intPkWorkflow_I, ref numJobPrice, ref numJobCost, numJobExtraCost,
                                ref numJobProfit, ref numJobFinalCost, ref numJobFinalProfit);

                            //                              //Get Odyssey2 note.
                            JobnotesJobNotesEntityDB jobnoteentity = context.JobNotes.FirstOrDefault(jobnote =>
                                jobnote.intJobID == intJobId_I);

                            String strOdyssey2Note = "";
                            if (
                                jobnoteentity != null
                                )
                            {
                                strOdyssey2Note = jobnoteentity.strOdyssey2Note != null ?
                                    jobnoteentity.strOdyssey2Note : "";
                            }

                            //                              //Get Wisnet notes.
                            String strWisnetNote = JobJob.strWisnetNotes(jobjson);

                            //                          //Get strJobNumber
                            String strJobNumber = JobJob.strGetJobNumber((int)jobjson.intnOrderId, jobjson.intJobId,
                                ps_I.strPrintshopId, context);

                            //                                  //Create json to send back.
                            jobticJson = new JobticjsonJobTicketJson(jobjson.intJobId, strJobNumber,
                                jobjson.strJobTicket, (int)jobjson.intnProductKey, jobjson.strProductName,
                                jobjson.strProductCategory, strJobStatus, strDeliveryDate, strDeliveryTime, strDueDate,
                                strDueTime, strStartDate, strStartTime, strEndDate, strEndTime, jobjson.intnQuantity,
                                jobjson.darrattrjson, jobjson.strCustomerName, jobjson.strCompany, jobjson.strBranch,
                                jobjson.strAddressLine1, jobjson.strAddressLine2, jobjson.strCity, jobjson.strState,
                                jobjson.strPostalCode, jobjson.strCountry, jobjson.strEmail, jobjson.strPhone,
                                jobjson.strCellPhone, jobjson.strCustomerRep, jobjson.strSalesRep, jobjson.strDelivery,
                                strWorkflowName, darrcostbycaljsonByProduct, darrprocjobjson, numJobCost, numJobPrice,
                                strWisnetNote, strOdyssey2Note);

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }

            return jobticJson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<Piwjson1ProcessInWorkflowJson1> darrPiwjson1GetListOfProcessInWorkflow(
            //                                              //Get the complete list of each process in a worflow.

            int intJobId_I,
            int intPkWorkflow_I,
            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjson_I,
            PsPrintShop ps_I,
            ProdtypProductType prodtyp_I,
            IConfiguration configuration_I,
            ref double numJobExtraCost_IO
            )
        {
            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get all the processes.

            //                                              //To be fill inside next method.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity;
            List<DynLkjsonDynamicLinkJson> darrdynlkjson;

            //                                              //Get processes of a workflow depends the conditions.
            ProdtypProductType.subGetWorkflowValidWay(intPkWorkflow_I, jobjson_I, out darrpiwentity, out darrdynlkjson);

            //                                  //List of normal piw.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityNormalProcess =
                darrpiwentity.Where(piw => piw.boolIsPostProcess == false).ToList();

            //                                  //List of post piw.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityPostProcess =
                darrpiwentity.Where(piw => piw.boolIsPostProcess == true).ToList();

            //                                              //List to Add piws.
            List<Piwjson1ProcessInWorkflowJson1> darrpiwjson1 = new List<Piwjson1ProcessInWorkflowJson1>();

            double numJobFinalCost = 0;
            //                                              //Get the inputs and outputs for every process.

            bool boolWorkflowJobIsReadyNotUsed = true;
            ////                                              //Add normal processes to List of piw json.
            //ProdtypProductType.AddNormalProcess(darrpiwentity, darrpiwentityNormalProcess, darrdynlkjson, prodtyp_I, ps_I,
            //    intJobId_I, jobjson_I, configuration_I, ref darrpiwjson1, ref numJobFinalCost, ref numJobExtraCost_IO,
            //    ref boolWorkflowJobIsReadyNotUsed);

            ////                                              //Add post processes to List of piw json.
            //ProdtypProductType.AddPostProcess(darrpiwentity, darrpiwentityPostProcess,
            //    darrdynlkjson, prodtyp_I, ps_I, intJobId_I, jobjson_I, configuration_I, ref darrpiwjson1, ref numJobFinalCost,
            //    ref numJobExtraCost_IO, ref boolWorkflowJobIsReadyNotUsed);

            //                                              //Dictionary to store inputs and outputs of a process.
            prodtyp_I.dicProcessIOs = new Dictionary<int, List<Iofrmpiwjson2IOFromPIWJson2>>();
            //                                              //List to store resource thickness.
            prodtyp_I.darrresthkjsonResThickness = new List<ResthkjsonResourceThicknessJson>();
            //                                              //Add normal processes to List of piw json.
            ProdtypProductType.AddNormalProcess(jobentity_I, jobjson_I, prodtyp_I, ps_I, darrdynlkjson,
                darrpiwentity, darrpiwentityNormalProcess, configuration_I,
                darrpiwjson1, ref numJobExtraCost_IO, ref numJobFinalCost, ref boolWorkflowJobIsReadyNotUsed) ;
            //                                              //Add post processes to List of piw json.
            ProdtypProductType.AddPostProcess(jobentity_I, jobjson_I, prodtyp_I, ps_I, darrdynlkjson,
                darrpiwentity, darrpiwentityPostProcess, configuration_I, darrpiwjson1,
                ref numJobExtraCost_IO, ref numJobFinalCost, ref boolWorkflowJobIsReadyNotUsed);

            return darrpiwjson1;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetDeliveryDateForAnSpecificJob(
            //                                              //Calculate a delivery date for a job.

            int intJobId_I,
            List<Piwjson1ProcessInWorkflowJson1> darrpiwjson1_I,
            JobjsonJobJson jobjson_I,
            out String strDeliveryDate_O,
            out String strDeliveryTime_O
            )
        {
            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            strDeliveryDate_O = "Delivery date not available.";
            strDeliveryTime_O = "";
            ZonedTime ztimeDeliveryDate = ZonedTime.MinValue;

            //                                              //Process in workflow.
            Piwjson1ProcessInWorkflowJson1[] arrpiw = darrpiwjson1_I.ToArray();

            int intI = 0;
            /*WHILE-DO*/
            while (
                intI < arrpiw.Length
                )
            {
                //                                          //Get PIW in order to get the necessary data to filter
                //                                          //      the process's calculations.
                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(
                    piw => piw.intPk == arrpiw[intI].intPkProcessInWorkflow);

                //                                          //Get all process calculations.
                List<CalentityCalculationEntityDB> darrcalentityProcess = context.Calculation.Where(
                    cal => cal.intnPkProcess == piwentity.intPkProcess &&
                    cal.intnPkWorkflow == piwentity.intPkWorkflow &&
                    cal.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId).ToList();

                int intK = 0;
                /*WHILE-DO*/
                while (
                    intK < darrcalentityProcess.Count
                    )
                {
                    //                                      //Get calculation's period.
                    PerentityPeriodEntityDB perentityCalculation = context.Period.FirstOrDefault(
                        per => per.intnPkCalculation == darrcalentityProcess[intK].intPk);

                    //                                      //If there is a period, get delivery date, then delivery
                    //                                      //      date is not available.
                    if (
                        perentityCalculation != null
                        )
                    {
                        ZonedTime ztime = ZonedTimeTools.NewZonedTime((perentityCalculation.strEndDate).ParseToDate(),
                                         perentityCalculation.strEndTime.ParseToTime());
                        if (
                            ztime > ztimeDeliveryDate
                            )
                        {
                            ztimeDeliveryDate = ztime;
                            strDeliveryDate_O = ztimeDeliveryDate.Date.ToString();
                            strDeliveryTime_O = ztimeDeliveryDate.Time.ToString().Substring(0, 5);
                        }
                    }
                    else
                    {
                        strDeliveryDate_O = "Delivery date not available.";
                        strDeliveryTime_O = "";
                    }

                    intK++;
                }

                //                                          //Inputs and outputs.
                List<Iojson1InputOrOutputJson1> darriojson1 = new List<Iojson1InputOrOutputJson1>();
                darriojson1.AddRange(arrpiw[intI].arrresortypInput.ToList());
                darriojson1.AddRange(arrpiw[intI].arrresortypOutput.ToList());

                //                                          //Filter by physical resources.
                darriojson1 = darriojson1.Where(iojson1 => iojson1.boolIsPhysical == true).ToList();
                Iojson1InputOrOutputJson1[] arriojson1 = darriojson1.ToArray();

                int intJ = 0;
                /*WHILE-DO*/
                while (
                    intJ < arriojson1.Length
                    )
                {
                    ResResource res = ResResource.resFromDB(arriojson1[intJ].intnPkResource, false);
                    if (
                        res == null
                        )
                    {
                        //                                  //Nothing to do
                    }
                    else
                    {
                        if (
                            (bool)arriojson1[intJ].boolnIsAvailable
                            )
                        {
                            if (
                                (bool)res.boolnIsCalendar
                                )
                            {
                                //                          //To easy code.
                                int? intnPkElementElementType = null;
                                int? intnPkElementElement = null;
                                if (
                                    arriojson1[intJ].boolIsEleet
                                    )
                                {
                                    intnPkElementElementType = arriojson1[intJ].intPkEleetOrEleele;
                                }
                                else
                                {
                                    intnPkElementElement = arriojson1[intJ].intPkEleetOrEleele;
                                }

                                //                          //Get periods resource.
                                List<PerentityPeriodEntityDB> darrperentityRes = context.Period.Where(perentity =>
                                    perentity.intJobId == jobjson_I.intJobId &&
                                    perentity.intPkElement == res.intPk &&
                                    perentity.intPkWorkflow == piwentity.intPkWorkflow &&
                                    perentity.intProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
                                    perentity.intnPkElementElementType == intnPkElementElementType &&
                                    perentity.intnPkElementElement == intnPkElementElement &&
                                    perentity.intnEstimateId == null).ToList();

                                foreach (PerentityPeriodEntityDB perentity in darrperentityRes)
                                {
                                    ZonedTime ztime = ZonedTimeTools.NewZonedTime((perentity.strEndDate).ParseToDate(),
                                         perentity.strEndTime.ParseToTime());
                                    if (
                                        ztime > ztimeDeliveryDate
                                        )
                                    {
                                        ztimeDeliveryDate = ztime;
                                        strDeliveryDate_O = ztimeDeliveryDate.Date.ToString();
                                        strDeliveryTime_O = ztimeDeliveryDate.Time.ToString().Substring(0, 5);
                                    }
                                }
                            }
                        }
                        else
                        {
                            strDeliveryDate_O = "Delivery date not available.";
                            strDeliveryTime_O = "";
                        }
                    }
                    intJ = intJ + 1;
                }
                intI = intI + 1;
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<ProcjobjsonProcessJobJson> darrprocjobjsonsubGetJobProcess(
            //                                              //Get information for each process in a workflow.

            int intPkWorkflow_I,
            ProdtypProductType prodtyp_I,
            JobjsonJobJson jobjson_I,
            PsPrintShop ps_I,
            List<Piwjson1ProcessInWorkflowJson1> darrpiwjson1_I
            )
        {
            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //List process. 
            List<ProcjobjsonProcessJobJson> darrprocjobjson = new List<ProcjobjsonProcessJobJson>();

            //                                              //Get list of all employees to improve perfomance in
            //                                              //      following methods.
            List<ContactjsonContactJson> darrcontactjsonGetAllEmployee =
                ResResource.darrcontactjsonGetAllEmployee(ps_I.strPrintshopId);

            double numNotUse = 0;
            foreach (Piwjson1ProcessInWorkflowJson1 piwjson1 in darrpiwjson1_I)
            {
                //                                          //List to Add IO Inputs.
                List<RecjobjsonResourceJobJson> darrrecjobjson = new List<RecjobjsonResourceJobJson>();

                foreach (Iojson1InputOrOutputJson1 iojson1 in piwjson1.arrresortypInput)
                {
                    if (
                        //                                  //It is an IO without link
                        iojson1.strLink == null
                        )
                    {
                        //                                  //GetEmployee
                        String strEmployee = "";
                        if (
                            //                              //This resource is a Device.
                            iojson1.intnPkResource != null && iojson1.boolnIsCalendar == true
                            )
                        {
                            int intProcessInWorkflowId = context.ProcessInWorkflow.FirstOrDefault(piw =>
                                piw.intPk == piwjson1.intPkProcessInWorkflow).intProcessInWorkflowId;

                            PerentityPeriodEntityDB perentity = context.Period.FirstOrDefault(per =>
                                per.intPkWorkflow == intPkWorkflow_I &&
                                per.intProcessInWorkflowId == intProcessInWorkflowId &&
                                (per.intnPkElementElementType == iojson1.intPkEleetOrEleele ||
                                per.intnPkElementElement == iojson1.intPkEleetOrEleele) &&
                                per.intPkElement == iojson1.intnPkResource);

                            if (
                                perentity != null
                                )
                            {
                                if (
                                    //                      //Verify if the resource has an employee associated.
                                    perentity.intnContactId != null
                                    )
                                {
                                    //                      //Get contact json.
                                    ContactjsonContactJson contjson = darrcontactjsonGetAllEmployee.FirstOrDefault(
                                        employee => employee.intContactId == perentity.intnContactId);

                                    if (
                                        contjson != null
                                        )
                                    {
                                        //                  //Get employee's name.
                                        strEmployee = contjson.strFirstName + " " + contjson.strLastName;
                                    }
                                }
                            }
                        }

                        String strResourceName = iojson1.strResource;
                        if (
                            strResourceName == null
                            )
                        {
                            strResourceName = "Unassigned";
                        }

                        RecjobjsonResourceJobJson resjobjsonResource = new RecjobjsonResourceJobJson(
                            strResourceName, iojson1.numQuantity, iojson1.strUnit, iojson1.numCostByResource,
                            strEmployee, iojson1.boolUnitAllowDecimal, iojson1.intPkEleetOrEleele,
                            iojson1.boolIsEleet, iojson1.intnPkResource);

                        darrrecjobjson.Add(resjobjsonResource);
                    }
                }

                //                                      //Get Calculation per process.
                List<CostbycaljsonCostByCalculationJson> darrcostbycaljsonPerProcess;

                bool boolWorkflowJobIsReadyNotUsed = false;
                double numCostByProcess = prodtyp_I.numGetCostByProcess(jobjson_I, piwjson1.intPkProcess,
                    piwjson1.intPkProcessInWorkflow, ps_I, out darrcostbycaljsonPerProcess, ref numNotUse,
                    ref boolWorkflowJobIsReadyNotUsed);

                //                                      //Get the process.
                EleentityElementEntityDB eleentityProcess = context.Element.FirstOrDefault(ele =>
                    ele.intPk == piwjson1.intPkProcess);

                //                                      //Json process.
                ProcjobjsonProcessJobJson procjobjjson = new ProcjobjsonProcessJobJson(eleentityProcess.strElementName,
                        darrcostbycaljsonPerProcess, darrrecjobjson);
                darrprocjobjson.Add(procjobjjson);
            }

            return darrprocjobjson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static WfjsonWorkflowJson subGetEstimateWorkflow(
            //                                              //Get the workflow for an estimate.

            int intJobId_I,
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            WfjsonWorkflowJson wfjsonworkflow = null;
            //                                              //Validate job.
            JobjsonJobJson jobjson = new JobjsonJobJson();
            intStatus_IO = 401;
            if (
                  JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                //true
                )
            {
                //                                          //Verify if the Estimate has already a workflow.
                //                                          //Get workflow if exists.
                WfentityWorkflowEntityDB wfentity = context_M.Workflow.FirstOrDefault(wf =>
                    wf.intPkPrintshop == ps_I.intPk &&
                    wf.intnJobId == intJobId_I);

                if (
                    //                                      //Estimate has not workflow yet.
                    wfentity == null
                    )
                {
                    //                                      //Get pkProduct of dummy product.
                    int intProductKeyDummy = ("9999" + ps_I.strPrintshopId).ParseToInt();
                    EtentityElementTypeEntityDB etentity = context_M.ElementType.FirstOrDefault(et =>
                        et.intWebsiteProductKey == intProductKeyDummy);

                    //                                      //Create a new workflow.
                    WfentityWorkflowEntityDB wfentityEstimate = new WfentityWorkflowEntityDB
                    {
                        intnPkProduct = etentity.intPk,
                        strName = "Workflow for Estimate " + intJobId_I,
                        intWorkflowId = 1,
                        strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                        strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                        intPkPrintshop = ps_I.intPk,
                        boolDeleted = false,
                        boolDefault = false,
                        intnJobId = intJobId_I
                    };
                    context_M.Workflow.Add(wfentityEstimate);
                    context_M.SaveChanges();

                    List<PiwjsonProcessInWorkflowJson> darrarrpro = new List<PiwjsonProcessInWorkflowJson>();
                    WfjsonWorkflowJson wfjsonworkflowEstimate = new WfjsonWorkflowJson(wfentityEstimate.intPk,
                        darrarrpro.ToArray(), false, wfentityEstimate.strName, false, etentity.intPk);
                    wfjsonworkflow = wfjsonworkflowEstimate;

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
                else
                {
                    //                                      //There is already a workflow, get the information.

                    //                                      //Get process's array
                    PiwjsonProcessInWorkflowJson[] arrpiwjson =
                        ProdtypProductType.arrpiwjsonGetWorkflow(wfentity.intPk);

                    //                                      //Get boolIsReady
                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityWithFinalProduct;
                    bool boolWorkflowIsReady;
                    bool? boolnNotUsed;
                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityProcessesNotReady;
                    ProdtypProductType.subfunWorkflowIsReady(wfentity.intPk, out darrpiwentityWithFinalProduct,
                        out boolWorkflowIsReady, out boolnNotUsed, out darrpiwentityProcessesNotReady);

                    //                                      //Get workflow's name
                    String strWorkflowName = wfentity.strName;

                    //                                      //Get boolHasFinalProduct.
                    List<PiwjsonProcessInWorkflowJson> darrpiwjson = arrpiwjson.ToList();
                    PiwjsonProcessInWorkflowJson piwjsonestimate = darrpiwjson.Find(wf => wf.boolContainsFinalProduct == true);
                    bool boolHasFinalProduct = piwjsonestimate != null ? true : false;

                    WfjsonWorkflowJson wfjsonworkflowEstimate = new WfjsonWorkflowJson(wfentity.intPk, arrpiwjson,
                        boolWorkflowIsReady, strWorkflowName, boolHasFinalProduct, (int)wfentity.intnPkProduct);
                    wfjsonworkflow = wfjsonworkflowEstimate;

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }

            return wfjsonworkflow;

        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPrintshopJobs(
            //                                              //Get the jobs for an specific stage, if there are not jobs
            //                                              //      saved for the printer this method get all the jobs 
            //                                              //      to set the order number, then get the jobs for the 
            //                                              //      required stage.

            PsPrintShop ps_I,
            bool? boolnUnsubmitted_I,
            bool? boolnInEstimating_I,
            bool? boolnWaitingForPriceApproval_I,
            bool? boolnPendingStage_I,
            bool? boolnInProgressStage_I,
            bool? boolnCompletedStage_I,
            bool? boolnNotPaid_I,
            bool? boolnAll_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            Odyssey2Context context_M,
            out List<JobjsonJobJson> darrjobjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrjobjson_O = new List<JobjsonJobJson>();

            //                                              //Json that will be sent to wisnet to get the jobs for an 
            //                                              //      specific stage.
            StagesjsonStagesJsonInternal stagesjson = new StagesjsonStagesJsonInternal(ps_I.strPrintshopId.ParseToInt(),
                boolnUnsubmitted_I, boolnInEstimating_I, boolnWaitingForPriceApproval_I, boolnPendingStage_I,
                boolnInProgressStage_I, boolnCompletedStage_I, boolnNotPaid_I, boolnAll_I);

            if (
                //                                          
                context_M.JobJson.FirstOrDefault(job =>
                    //                                      //There are no orders-jobs in the jobjson table.
                    job.strPrintshopId == ps_I.strPrintshopId &&
                    //                                      //To be a order-job, OrderId must be greater than 0.
                    job.intOrderId > 0
                    ) == null
                )
            {
                //                                          //Get all the jobs to save the correct order number.
                JobJob.subGetAllJobsToSetOrderNumber(ps_I.strPrintshopId, configuration_I, 1, context_M,
                    ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
            }

            darrjobjson_O = JobJob.subGetStageJobs(ps_I, configuration_I, iHubContext_I, stagesjson, context_M,
                ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "Success.";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subGetAllJobsToSetOrderNumber(
            //                                              //Get all printshop's jobs' basic info.

            String strPrintshopId_I,
            IConfiguration configuration_I,
            //                                              //OffsetNumber.
            //                                              //When this method is called from login service we will 
            //                                              //      receive a number greather or equal to 0. This means
            //                                              //      that is the number which printer wants to start
            //                                              //      order's enumeration.
            //                                              //When is called internally this number needs to be 1.
            int intOffsetNumber_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Json that specifies all the jobs are required.
            StagesjsonStagesJsonInternal stagesjsonAll = new StagesjsonStagesJsonInternal(strPrintshopId_I.ParseToInt(),
                null, null, null, null, null, null, null, true);

            //                                              //Get jobs from Wisnet. 
            Task<List<JobbasicinfojsonJobBasicInfoJson>> Task_darrjobbasicinfojson = HttpTools<
                JobbasicinfojsonJobBasicInfoJson>.GetListAsyncWithBodyToEndPoint(stagesjsonAll,
                configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                "/PrintshopData/printshopJobsToFillList/");
            Task_darrjobbasicinfojson.Wait();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet database connection lost.";
            if (
                Task_darrjobbasicinfojson.Result != null
                )
            {
                List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfojson = Task_darrjobbasicinfojson.Result;

                JobJob.subVerifyJobIsInDB(strPrintshopId_I, intOffsetNumber_I, darrjobbasicinfojson, context_M);

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<JobjsonJobJson> subGetStageJobs(
            //                                              //Get the jobs from an specific stage.

            PsPrintShop ps_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            StagesjsonStagesJsonInternal stagesjson_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            List<JobjsonJobJson> darrjobjson = new List<JobjsonJobJson>();

            //                                              //To easy code.
            StagesjsonStagesJsonInternal stagesjsonCorrected = stagesjson_I;

            if (
                stagesjsonCorrected.boolnPending == true
                )
            {
                //                                          //This bool is needed as true to get the in progress jobs 
                //                                          //      from Wisnet to filter them.
                stagesjsonCorrected.boolnInProgress = true;
            }

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet database connection lost.";
            /*CASE*/
            if (
                //                                          //Unsubmitted, In Estimating or Waiting For Price Approval.
                stagesjson_I.boolnUnsubmitted == true ||
                stagesjson_I.boolnInEstimating == true ||
                stagesjson_I.boolnWaitingForPriceApproval == true
                )
            {
                //                                          //Get jobs from Wisnet. 
                Task<List<JobbasicinfojsonJobBasicInfoJson>> Task_darrjobbasicinfojson = HttpTools<
                    JobbasicinfojsonJobBasicInfoJson>.GetListAsyncWithBodyToEndPoint(stagesjsonCorrected,
                    configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                    "/PrintshopData/printshopJobsToFillList/");
                Task_darrjobbasicinfojson.Wait();

                if (
                    Task_darrjobbasicinfojson.Result != null
                    )
                {
                    List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfojson = Task_darrjobbasicinfojson.Result;

                    JobJob.subVerifyJobIsInDB(ps_I.strPrintshopId, 1, darrjobbasicinfojson, context_M);

                    //                                      //The jobs returned from Wisnet are converted to the json 
                    //                                      //      that will be returned.
                    darrjobjson = JobJob.darrjobjsonGetUnsubmittedInEstimatingOrWaiting(ps_I, darrjobbasicinfojson,
                        context_M);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
            else if (
                //                                          //Pending.
                stagesjson_I.boolnPending == true
                )
            {
                //                                          //Get jobs from Wisnet. 
                Task<List<JobbasicinfojsonJobBasicInfoJson>> Task_darrjobbasicinfojson = HttpTools<
                    JobbasicinfojsonJobBasicInfoJson>.GetListAsyncWithBodyToEndPoint(stagesjsonCorrected,
                    configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                    "/PrintshopData/printshopJobsToFillList/");
                Task_darrjobbasicinfojson.Wait();

                if (
                    Task_darrjobbasicinfojson.Result != null
                    )
                {
                    List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfojson = Task_darrjobbasicinfojson.Result;

                    JobJob.subVerifyJobIsInDB(ps_I.strPrintshopId, 1, darrjobbasicinfojson, context_M);

                    darrjobjson = JobJob.darrjobjsonGetPending(ps_I, darrjobbasicinfojson, iHubContext_I, context_M);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
            else if (
                //                                          //In Progress.
                stagesjson_I.boolnInProgress == true
                )
            {
                //                                          //Delete those jobs not exists in wisnet.
                JobJob.subDeleteJobsFromDBNotExistInWisnetAnymore(ps_I, true, context_M);

                darrjobjson = JobJob.darrjobjsonGetInProgress(ps_I, context_M, configuration_I);

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }
            else if (
                //                                          //Completed.
                stagesjson_I.boolnCompleted == true
                )
            {
                //                                          //Get jobs from Wisnet. 
                Task<List<JobbasicinfojsonJobBasicInfoJson>> Task_darrjobbasicinfojson = HttpTools<
                    JobbasicinfojsonJobBasicInfoJson>.GetListAsyncWithBodyToEndPoint(stagesjsonCorrected,
                    configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                    "/PrintshopData/printshopJobsToFillList/");
                Task_darrjobbasicinfojson.Wait();

                if (
                    Task_darrjobbasicinfojson.Result != null
                    )
                {
                    List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfojson = Task_darrjobbasicinfojson.Result;

                    JobJob.subVerifyJobIsInDB(ps_I.strPrintshopId, 1, darrjobbasicinfojson, context_M);

                    //                                      //Delete those jobs not exists in wisnet.
                    JobJob.subDeleteJobsFromDBNotExistInWisnetAnymore(ps_I, false, context_M);

                    darrjobjson = JobJob.darrjobjsonGetCompleted(ps_I, darrjobbasicinfojson, context_M, configuration_I);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
            else if (
                //                                          //Paid.
                stagesjson_I.boolnWaitingForPayment == true
                )
            {
                //                                          //Get jobs from Wisnet. 
                Task<List<JobbasicinfojsonJobBasicInfoJson>> Task_darrjobbasicinfojson = HttpTools<
                    JobbasicinfojsonJobBasicInfoJson>.GetListAsyncWithBodyToEndPoint(stagesjsonCorrected,
                    configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                    "/PrintshopData/printshopJobsToFillList/");
                Task_darrjobbasicinfojson.Wait();

                if (
                    Task_darrjobbasicinfojson.Result != null
                    )
                {
                    List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfojson = Task_darrjobbasicinfojson.Result;

                    JobJob.subVerifyJobIsInDB(ps_I.strPrintshopId, 1, darrjobbasicinfojson, context_M);

                    darrjobjson = JobJob.darrjobjsonGetNotPaid(ps_I, darrjobbasicinfojson, context_M);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
            else if (
                //                                          //All.
                stagesjson_I.boolnAll == true
                )
            {
                darrjobjson = JobJob.darrjobjsonGetAll(ps_I, configuration_I, iHubContext_I, context_M);
            }
            /*END-CASE*/

            //                                              //Sort by job number
            darrjobjson.Sort();

            return darrjobjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subVerifyJobIsInDB(
            //                                              //Verify that every job is in db and add the ones that are 
            //                                              //     not there.

            String strPrintshopId_I,
            int intOffsetNumber_I,
            List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfojson_I,
            Odyssey2Context context_M
            )
        {
            //                                              //To add jobnumber and order number.
            List<JobjsonentityJobJsonEntityDB> darrjobjsonentity = context_M.JobJson.Where(job =>
                job.strPrintshopId == strPrintshopId_I).ToList();

            List<JobjsonentityJobJsonEntityDB> darrjobjsonentityOrdersAdded = new List<JobjsonentityJobJsonEntityDB>();

            int intLast = darrjobbasicinfojson_I.Count - 1;
            for (int intI = intLast; intI >= 0; intI = intI - 1)
            {
                JobbasicinfojsonJobBasicInfoJson jobbasicinfojson = darrjobbasicinfojson_I[intI];

                if (
                    darrjobjsonentity.FirstOrDefault(job =>
                        job.intJobID == jobbasicinfojson.intJobId &&
                        job.intOrderId == jobbasicinfojson.intOrderId) == null
                    )
                {
                    String strPrice = null;
                    //                                          //Find the estimate.
                    JobjsonentityJobJsonEntityDB jobjsonJobEstimate = darrjobjsonentity.FirstOrDefault(
                        jobjson => jobjson.intJobID == jobbasicinfojson.intJobId &&
                        jobjson.intOrderId == 0);

                    if (
                        //                                      //The job exist in database with 
                        //                                      //    order zero(how estimate)
                        jobjsonJobEstimate != null
                        )
                    {
                        strPrice = jobjsonJobEstimate.strPrice;
                        context_M.JobJson.Remove(jobjsonJobEstimate);
                        context_M.SaveChanges();
                    }

                    JobJob.subCreateJobAndOrderNumberIntoJobJsonTable(jobbasicinfojson.intOrderId,
                        jobbasicinfojson.intJobId, strPrice, strPrintshopId_I, intOffsetNumber_I,
                        darrjobjsonentity, ref darrjobjsonentityOrdersAdded);
                }
            }

            if (
                darrjobjsonentityOrdersAdded.Count > 0
                )
            {
                //                                          //Add jobs to the DB
                //context_M.BulkInsert(darrjobjsonentityOrdersAdded);
                foreach (JobjsonentityJobJsonEntityDB jobjson in darrjobjsonentityOrdersAdded)
                {
                    context_M.Add(jobjson);
                }
                context_M.SaveChanges();
            }
        }


        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<JobjsonJobJson> darrjobjsonGetUnsubmittedInEstimatingOrWaiting(
            //                                              //Get the jobjson from jobbasicinfojson for this stages, it 
            //                                              //      is not necessary to process them.

            PsPrintShop ps_I,
            List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfojson_I,
            Odyssey2Context context_I
            )
        {
            List<JobjsonJobJson> darrjobjson = new List<JobjsonJobJson>();

            //                                              //Find product related to current printshop.
            List<EtentityElementTypeEntityDB> darretentityProduct = context_I.ElementType.Where(et =>
                et.intPrintshopPk == ps_I.intPk && et.strResOrPro == EletemElementType.strProduct).ToList();

            List<JobjsonentityJobJsonEntityDB> darrjobjsonentity = context_I.JobJson.Where(jobjson =>
                jobjson.strPrintshopId == ps_I.strPrintshopId && jobjson.intOrderId > 0).ToList();

            foreach (JobbasicinfojsonJobBasicInfoJson jobbasicinfojson in darrjobbasicinfojson_I)
            {
                JobjsonJobJson jobjson = new JobjsonJobJson();

                EtentityElementTypeEntityDB etentityProduct;
                //                                          //Get PkProduct based on a job.
                jobjson.intPkProduct = (etentityProduct = darretentityProduct.FirstOrDefault(et =>
                    et.intWebsiteProductKey == jobbasicinfojson.intProductKey)) != null ?
                    etentityProduct.intPk : 0;

                //                                          //Fill other data.
                jobjson.intJobId = jobbasicinfojson.intJobId;
                jobjson.strProductName = jobbasicinfojson.strProductName;
                jobjson.strJobTicket = jobbasicinfojson.strJobTicket;
                jobjson.dateLastUpdate = jobbasicinfojson.dateLastUpdate;
                jobjson.intnOrderId = jobbasicinfojson.intOrderId;
                jobjson.strCustomerName = jobbasicinfojson.strCustomerName;

                //                                          //Get strJobNumber
                jobjson.strJobNumber = JobJob.strGetJobNumber(jobbasicinfojson.intOrderId,
                    jobbasicinfojson.intJobId, darrjobjsonentity);

                //                                          //Add JobPending.
                darrjobjson.Add(jobjson);
            }

            return darrjobjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<JobjsonJobJson> darrjobjsonGetPending(
            //                                              //Filter the In Progress jobs in Wisnet, remove the 
            //                                              //InProgress (in Odyssey) jobs.

            PsPrintShop ps_I,
            List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfojson_I,
            IHubContext<ConnectionHub> iHubContext_I,
            Odyssey2Context context_M
            )
        {
            List<JobjsonJobJson> darrjobjson = new List<JobjsonJobJson>();

            //                                              //Jobs InProgress in Odyssey2.
            List<JobentityJobEntityDB> darrjobentity = context_M.Job.Where(job =>
                job.intPkPrintshop == ps_I.intPk && job.intStage == JobJob.intInProgressStage
                 && job.boolDeleted == false).ToList();

            //                                              //Find product related to current printshop.
            List<EtentityElementTypeEntityDB> darretentityProduct = context_M.ElementType.Where(et =>
                et.intPrintshopPk == ps_I.intPk && et.strResOrPro == EletemElementType.strProduct).ToList();


            //                                              //List of jobjson for the printshop.
            List<JobjsonentityJobJsonEntityDB> darrjobjsonentityForThisPrintshop = context_M.JobJson.Where(jobjson =>
                jobjson.strPrintshopId == ps_I.strPrintshopId && jobjson.intOrderId > 0).ToList();

            //                                              //Add alerts type New Order.
            AlerttypeentityAlertTypeEntityDB alerttypeentityNewOrder = context_M.AlertType.FirstOrDefault(
                at => at.strType == AlerttypeentityAlertTypeEntityDB.strNewOrder);

            //                                          //Add alerts type New Estimate.
            AlerttypeentityAlertTypeEntityDB alerttypeentityNewEstimate = context_M.AlertType.FirstOrDefault(
                at => at.strType == AlerttypeentityAlertTypeEntityDB.strNewEstimate);

            //                                              //Get all Notifications of type newEstimate
            //                                              //    from this printshop.
            List<AlertentityAlertEntityDB> darralerentityAlert = context_M.Alert.Where(aler =>
                aler.intPkPrintshop == ps_I.intPk &&
                (aler.intPkAlertType == alerttypeentityNewOrder.intPk ||
                aler.intPkAlertType == alerttypeentityNewEstimate.intPk)).ToList();

            //                                              //Init the list for reduce notification.
            List<int> darrintRepsOrSuperviserToReduce = new List<int>();

            foreach (JobbasicinfojsonJobBasicInfoJson jobbasicinfojson in darrjobbasicinfojson_I)
            {
                if (
                    //                                      //It is not InProgress or Completed in Odyssey2.
                    darrjobentity.FirstOrDefault(job => job.intJobID == jobbasicinfojson.intJobId) == null
                    )
                {
                    JobjsonJobJson jobjson = new JobjsonJobJson();

                    EtentityElementTypeEntityDB etentityProduct;

                    //                                      //To easy code.
                    String strProductName = jobbasicinfojson.strProductName.Length > 0 ?
                        jobbasicinfojson.strProductName : "Dummy";
                    //                                      //Get PkProduct based on a job.
                    jobjson.intPkProduct = (etentityProduct = darretentityProduct.FirstOrDefault(et =>
                        et.intWebsiteProductKey == jobbasicinfojson.intProductKey &&
                        et.strCustomTypeId == strProductName)) != null ?
                        etentityProduct.intPk : 0;

                    //                                      //Fill other data.
                    jobjson.intJobId = jobbasicinfojson.intJobId;
                    jobjson.strProductName = jobbasicinfojson.strProductName;
                    jobjson.strJobTicket = jobbasicinfojson.strJobTicket;
                    jobjson.dateLastUpdate = jobbasicinfojson.dateLastUpdate;
                    jobjson.intnOrderId = jobbasicinfojson.intOrderId;
                    jobjson.strCustomerName = jobbasicinfojson.strCustomerName;

                    //                                      //Get strJobNumber
                    jobjson.strJobNumber = JobJob.strGetJobNumber(jobbasicinfojson.intOrderId,
                        jobbasicinfojson.intJobId, darrjobjsonentityForThisPrintshop);

                    //                                      //Add wisnet price.
                    jobjson.numnWisnetPrice = jobbasicinfojson.numnWisnetPrice != null ? 
                        (double)jobbasicinfojson.numnWisnetPrice : (double?)null;

                    //                                      //Add JobPending.
                    darrjobjson.Add(jobjson);
                }
            }

            //                                              //Send reduce notifications to the contacts specified.
            AlnotAlertNotification.subReduceToAFew(ps_I.strPrintshopId,
                darrintRepsOrSuperviserToReduce.ToArray(), iHubContext_I);

            return darrjobjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteJobsFromDBNotExistInWisnetAnymore(
            //                                              //Will delete those jobs that were deleted in Wisnet.

            PsPrintShop ps_I,
            bool boolInProgress_I,
            Odyssey2Context context_M
            )
        {
            List<int> darrintInProgressOrCompletedJobsIds = JobJob.darrintGetInProgressOrCompletedJobsIds(ps_I,
                boolInProgress_I, context_M);

            //                                              //Validar el job en Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<DeljobsjsonDeletedJobsJson> Task_darrcatjsonFromWisnet =
                HttpTools<DeljobsjsonDeletedJobsJson>.GetListAsyncJobsDeleted(strUrlWisnet +
                "/PrintShopData/GetJobsDeletedOnWisnet", darrintInProgressOrCompletedJobsIds.ToArray());
            Task_darrcatjsonFromWisnet.Wait();

            if (
                 //                                          //If the list has elements means those elements were
                 //                                          //      deleted in Wisnet.
                 Task_darrcatjsonFromWisnet.Result.darrintJobsDeleted.Count() > 0
                )
            {
                DeljobsjsonDeletedJobsJson deljobsjson = Task_darrcatjsonFromWisnet.Result;

                //                                          //Set as deleted those jobs deleted in Wisnet.
                foreach (int intJobId in deljobsjson.darrintJobsDeleted)
                {
                    JobentityJobEntityDB jobentity = context_M.Job.FirstOrDefault(job => job.intJobID == intJobId &&
                        job.intPkPrintshop == ps_I.intPk);
                    jobentity.boolDeleted = true;
                    context_M.Job.Update(jobentity);
                }
                context_M.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<int> darrintGetInProgressOrCompletedJobsIds(
            //                                              //Will delete those jobs that were deleted in wisnet.

            PsPrintShop ps_I,
            bool boolInProgress_I,
            Odyssey2Context context_M
            )
        {
            List<int> darrintInProgressOrCompletedJobsIds = null;

            if (
                //                                          //Jobs InProgress were requested.
                boolInProgress_I
                )
            {
                darrintInProgressOrCompletedJobsIds = context_M.Job.Where(job => job.intPkPrintshop == ps_I.intPk &&
                    job.intStage == JobJob.intInProgressStage &&
                    job.boolDeleted == false).Select(job => job.intJobID).ToList();
            }
            else
            {
                //                                          //Jobs Completed were requested.
                darrintInProgressOrCompletedJobsIds = context_M.Job.Where(job => job.intPkPrintshop == ps_I.intPk &&
                    job.intStage == JobJob.intCompletedStage &&
                    job.boolDeleted == false).Select(job => job.intJobID).ToList();

            }

            return darrintInProgressOrCompletedJobsIds;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<JobjsonJobJson> darrjobjsonGetInProgress(
            //                                              //Get the jobs that are in progress in Odyssey.

            PsPrintShop ps_I,
            Odyssey2Context context_I,
            IConfiguration configuration_I
            )
        {
            List<JobjsonJobJson> darrjobjson = new List<JobjsonJobJson>();

            //                                              //Jobs InProgress in Odyssey2.
            List<JobentityJobEntityDB> darrjobentityInProgressInOdyssey = context_I.Job.Where(job =>
                job.intPkPrintshop == ps_I.intPk && job.intStage == JobJob.intInProgressStage &&
                job.boolDeleted == false).ToList();

            //                                              //Find product related to current printshop.
            List<EtentityElementTypeEntityDB> darretentityProduct = context_I.ElementType.Where(et =>
                et.intPrintshopPk == ps_I.intPk && et.strResOrPro == EletemElementType.strProduct).ToList();

            //                                              //List of jobjson for the printshop.
            List<JobjsonentityJobJsonEntityDB> darrjobjsonentityForThisPrintshop = context_I.JobJson.Where(jobjson =>
                jobjson.strPrintshopId == ps_I.strPrintshopId && jobjson.intOrderId > 0).ToList();

            foreach (JobentityJobEntityDB jobentity in darrjobentityInProgressInOdyssey)
            {
                JobjsonJobJson jobjsonInProgress = new JobjsonJobJson();

                //                                      //To easy code.
                JobjsonentityJobJsonEntityDB jobjsonentity = darrjobjsonentityForThisPrintshop.FirstOrDefault(job =>
                    job.intJobID == jobentity.intJobID);

                JobjsonJobJson jobjsonSaved;

                if (
                    jobjsonentity.jobjson != null
                    )
                {
                    jobjsonSaved = JsonSerializer.Deserialize<JobjsonJobJson>(jobjsonentity.jobjson);
                }
                else
                {
                    //                                  //Can be null if was just load to the jsontable and was
                    //                                  //      already added to the job table.
                    String strUserMessage = ""; String strDevMessage = ""; JobjsonJobJson jobjson;
                    JobJob.boolIsValidJobId(jobjsonentity.intJobID, ps_I.strPrintshopId, configuration_I, out jobjson,
                        ref strUserMessage, ref strDevMessage);
                    jobjsonSaved = jobjson;
                }

                EtentityElementTypeEntityDB etentityProduct;
                //                                      //Get PkProduct based on a job.
                jobjsonInProgress.intPkProduct = (etentityProduct = darretentityProduct.FirstOrDefault(et =>
                    et.intWebsiteProductKey == (int)jobjsonSaved.intnProductKey)) != null ?
                    etentityProduct.intPk : 0;

                //                                          //Fill other data.
                jobjsonInProgress.intJobId = jobentity.intJobID;
                jobjsonInProgress.strProductName = jobjsonSaved.strProductName;
                jobjsonInProgress.strJobTicket = jobjsonSaved.strJobTicket;
                jobjsonInProgress.dateLastUpdate = jobjsonSaved.dateLastUpdate;
                jobjsonInProgress.intnOrderId = jobjsonentity.intOrderId;
                jobjsonInProgress.intPkWorkflow = jobentity.intPkWorkflow;
                jobjsonInProgress.strCustomerName = jobjsonSaved.strCustomerName;

                //                                          //Get the progress of the job.
                jobjsonInProgress.numProgress = JobJob.numGetJobProgress(jobjsonInProgress, ps_I.intPk);

                ZonedTime ztimeStartJob = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                    jobentity.strStartDate.ParseToDate(), jobentity.strStartTime.ParseToTime(), ps_I.strTimeZone);

                //                                          //Dates.
                jobjsonInProgress.strStartDateTime = ztimeStartJob.Date + " " + ztimeStartJob.Time;
                jobjsonInProgress.strEndDateTime = null;

                //                                          //Get strJobNumber
                jobjsonInProgress.strJobNumber = JobJob.strGetJobNumber(jobjsonentity.intOrderId, jobentity.intJobID,
                    darrjobjsonentityForThisPrintshop);

                //                                          //Add wisnet price.
                jobjsonInProgress.numnWisnetPrice = jobjsonSaved.numnWisnetPrice;

                //                                          //Add JobPending.
                darrjobjson.Add(jobjsonInProgress);
            }
            return darrjobjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<JobjsonJobJson> darrjobjsonGetCompleted(
            //                                              //Get the jobjson for the jobs completed in Wisnet and add 
            //                                              //      info to the ones completed in Odyssey2.

            PsPrintShop ps_I,
            List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfojson_I,
            Odyssey2Context context_I,
            IConfiguration configuration_I
            )
        {
            List<JobjsonJobJson> darrjobjson = new List<JobjsonJobJson>();

            //                                              //Jobs Completed in Odyssey2.
            List<JobentityJobEntityDB> darrjobentityCompletedInOdyssey = context_I.Job.Where(job =>
                job.intPkPrintshop == ps_I.intPk && job.intStage == JobJob.intCompletedStage &&
                job.boolDeleted == false).ToList();

            //                                              //Find product related to current printshop.
            List<EtentityElementTypeEntityDB> darretentityProduct = context_I.ElementType.Where(et =>
                et.intPrintshopPk == ps_I.intPk && et.strResOrPro == EletemElementType.strProduct).ToList();

            //                                              //List of jobjson for the printshop.
            List<JobjsonentityJobJsonEntityDB> darrjobjsonentityForThisPrintshop = context_I.JobJson.Where(jobjson =>
                jobjson.strPrintshopId == ps_I.strPrintshopId && jobjson.intOrderId > 0).ToList();

            foreach (JobbasicinfojsonJobBasicInfoJson jobbasicinfojson in darrjobbasicinfojson_I)
            {
                //                                          //The jobs completed at Wisnet do not have dates of start 
                //                                          //      start and end and do not have workflow asociated.
                JobjsonJobJson jobjson = new JobjsonJobJson();

                EtentityElementTypeEntityDB etentityProduct;
                //                                          //Get PkProduct based on a job.
                jobjson.intPkProduct = (etentityProduct = darretentityProduct.FirstOrDefault(et =>
                    et.intWebsiteProductKey == jobbasicinfojson.intProductKey)) != null ?
                    etentityProduct.intPk : 0;

                //                                          //Fill other data.
                jobjson.intJobId = jobbasicinfojson.intJobId;
                jobjson.strProductName = jobbasicinfojson.strProductName;
                jobjson.strJobTicket = jobbasicinfojson.strJobTicket;
                jobjson.dateLastUpdate = jobbasicinfojson.dateLastUpdate;
                jobjson.intnOrderId = jobbasicinfojson.intOrderId;
                jobjson.strCustomerName = jobbasicinfojson.strCustomerName;

                JobentityJobEntityDB jobentity = darrjobentityCompletedInOdyssey.FirstOrDefault(jobentity =>
                    jobentity.intJobID == jobjson.intJobId);

                if (
                    //                                      //Completed on Odyssey, add extra information.
                    jobentity != null
                    )
                {
                    jobjson.intPkWorkflow = jobentity.intPkWorkflow;

                    ZonedTime ztimeStartJob = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        jobentity.strStartDate.ParseToDate(), jobentity.strStartTime.ParseToTime(), ps_I.strTimeZone);
                    ZonedTime ztimeEndJob = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        jobentity.strEndDate.ParseToDate(), jobentity.strEndTime.ParseToTime(), ps_I.strTimeZone);

                    //                                      //Dates.
                    jobjson.strStartDateTime = ztimeStartJob.Date + " " + ztimeStartJob.Time;
                    jobjson.strEndDateTime = ztimeEndJob.Date + " " + ztimeEndJob.Time;
                }

                //                                          //Get strJobNumber
                jobjson.strJobNumber = JobJob.strGetJobNumber(jobbasicinfojson.intOrderId, jobbasicinfojson.intJobId,
                   darrjobjsonentityForThisPrintshop);

                //                                          //Add wisnet price.
                jobjson.numnWisnetPrice = jobbasicinfojson.numnWisnetPrice != null ? 
                    (double)jobbasicinfojson.numnWisnetPrice :(double?)null;

                //                                          //Add JobPending.
                darrjobjson.Add(jobjson);
            }

            //                                              //List with completed jobs that has no been updated
            //                                              //      on Wisnet.
            List<JobentityJobEntityDB> darrjobentityNotOnWisnet = darrjobentityCompletedInOdyssey.Where(jobentity =>
                    jobentity.boolnOnWisnet == false).ToList();

            foreach (JobentityJobEntityDB jobentity in darrjobentityNotOnWisnet)
            {
                JobjsonJobJson jobjsonNotUpdatedInWisnet = new JobjsonJobJson();

                //                                      //To easy code.
                JobjsonentityJobJsonEntityDB jobjsonentity = darrjobjsonentityForThisPrintshop.FirstOrDefault(job =>
                    job.intJobID == jobentity.intJobID);

                JobjsonJobJson jobjsonSaved;

                if (
                    jobjsonentity.jobjson != null
                    )
                {
                    jobjsonSaved = JsonSerializer.Deserialize<JobjsonJobJson>(jobjsonentity.jobjson);
                }
                else
                {
                    //                                  //Can be null if was just load to the jsontable and was
                    //                                  //      already added to the job table.
                    String strUserMessage = ""; String strDevMessage = ""; JobjsonJobJson jobjson;
                    JobJob.boolIsValidJobId(jobjsonentity.intJobID, ps_I.strPrintshopId, configuration_I, out jobjson,
                        ref strUserMessage, ref strDevMessage);
                    jobjsonSaved = jobjson;
                }

                EtentityElementTypeEntityDB etentityProduct;
                //                                      //Get PkProduct based on a job.
                jobjsonNotUpdatedInWisnet.intPkProduct = (etentityProduct = darretentityProduct.FirstOrDefault(et =>
                    et.intWebsiteProductKey == jobjsonSaved.intnProductKey)) != null ?
                    etentityProduct.intPk : 0;

                //                                      //Fill other data.
                jobjsonNotUpdatedInWisnet.intJobId = jobentity.intJobID;
                jobjsonNotUpdatedInWisnet.strProductName = jobjsonSaved.strProductName;
                jobjsonNotUpdatedInWisnet.strJobTicket = jobjsonSaved.strJobTicket;
                jobjsonNotUpdatedInWisnet.dateLastUpdate = jobjsonSaved.dateLastUpdate;
                jobjsonNotUpdatedInWisnet.intnOrderId = jobjsonentity.intOrderId;
                jobjsonNotUpdatedInWisnet.strCustomerName = jobjsonSaved.strCustomerName;
                jobjsonNotUpdatedInWisnet.intPkWorkflow = jobentity.intPkWorkflow;

                //                                      //Dates.
                jobjsonNotUpdatedInWisnet.strStartDateTime = jobentity.strStartDate + " " + jobentity.strStartTime;
                jobjsonNotUpdatedInWisnet.strEndDateTime = jobentity.strEndDate + " " + jobentity.strEndTime;

                //                                      //Get strJobNumber
                jobjsonNotUpdatedInWisnet.strJobNumber = JobJob.strGetJobNumber(jobjsonentity.intOrderId,
                    jobjsonSaved.intJobId, darrjobjsonentityForThisPrintshop);

                //                                      //Add JobPending.
                darrjobjson.Add(jobjsonNotUpdatedInWisnet);
            }
            return darrjobjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<JobjsonJobJson> darrjobjsonGetNotPaid(
            //                                              //Get the jobjson for the jobs completed in Wisnet and add 
            //                                              //      them the ones that are completed in Odyssey.

            PsPrintShop ps_I,
            List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfojson_I,
            Odyssey2Context context_I
            )
        {
            //                                              //List add jobjson to return.
            List<JobjsonJobJson> darrjobjson = new List<JobjsonJobJson>();

            //                                              //List with jobs with open balance.
            List<JobjsonentityJobJsonEntityDB> darrjobjsonentity = (
                    from jobjson in context_I.JobJson
                    join invoice in context_I.Invoice
                    on jobjson.intOrderId equals invoice.intOrderNumber
                    where invoice.numOpenBalance > 0.0 &&
                    invoice.intPkPrintshop == ps_I.intPk
                    select jobjson).ToList();

            //                                              //List of Odyssey2 Completed jobs.
            List<JobentityJobEntityDB> darrjobentity = context_I.Job.Where(job =>
                job.intPkPrintshop == ps_I.intPk && job.intStage == JobJob.intCompletedStage &&
                job.boolDeleted == false).ToList();

            //                                              //Find products related to current printshop.
            List<EtentityElementTypeEntityDB> darretentityProduct = context_I.ElementType.Where(et =>
                et.intPrintshopPk == ps_I.intPk && et.strResOrPro == EletemElementType.strProduct).ToList();

            //                                              //List of jobjson for the printshop.
            List<JobjsonentityJobJsonEntityDB> darrjobjsonentityForThisPrintshop = context_I.JobJson.Where(jobjson =>
                jobjson.strPrintshopId == ps_I.strPrintshopId && jobjson.intOrderId > 0).ToList();

            //                                              //List of Invoices paid not updates on Wisnet.
            List<InvoInvoiceEntityDB> darrinvoentityNotUpdatedOnWisnet = context_I.Invoice.Where(invoice =>
                invoice.intPkPrintshop == ps_I.intPk && invoice.boolnOnWisnet == false).ToList();

            //                                              //List of job for Invoices paid not updates on Wisnet.
            List<int> darrintJobIdsForNotUpdatedOnWisnet = new List<int>();
            foreach (InvoInvoiceEntityDB invoentity in darrinvoentityNotUpdatedOnWisnet)
            {
                List<int> darrintJobIdsForNotUpdatedOnWisnetThisInvoice =
                    (from jobjsonentity in context_I.JobJson
                     where jobjsonentity.intOrderId == invoentity.intOrderNumber
                     select jobjsonentity.intJobID).ToList();
                darrintJobIdsForNotUpdatedOnWisnet.AddRange(darrintJobIdsForNotUpdatedOnWisnetThisInvoice);
            }

            foreach (JobbasicinfojsonJobBasicInfoJson jobbasicinfojson in darrjobbasicinfojson_I)
            {
                JobjsonJobJson jobjsonNotPaid = new JobjsonJobJson();

                EtentityElementTypeEntityDB etentityProduct;
                //                                          //Get PkProduct based on a job.
                jobjsonNotPaid.intPkProduct = (etentityProduct = darretentityProduct.FirstOrDefault(et =>
                    et.intWebsiteProductKey == jobbasicinfojson.intProductKey)) != null ?
                    etentityProduct.intPk : 0;

                //                                          //Fill other data.
                jobjsonNotPaid.intJobId = jobbasicinfojson.intJobId;
                jobjsonNotPaid.strProductName = jobbasicinfojson.strProductName;
                jobjsonNotPaid.strJobTicket = jobbasicinfojson.strJobTicket;
                jobjsonNotPaid.dateLastUpdate = jobbasicinfojson.dateLastUpdate;
                jobjsonNotPaid.intnOrderId = jobbasicinfojson.intOrderId;
                jobjsonNotPaid.strCustomerName = jobbasicinfojson.strCustomerName;

                JobentityJobEntityDB jobentity = darrjobentity.FirstOrDefault(job =>
                    job.intJobID == jobjsonNotPaid.intJobId);

                if (
                    //                                      //Completed on Odyssey, add extra information.
                    jobentity != null
                    )
                {
                    jobjsonNotPaid.intPkWorkflow = jobentity.intPkWorkflow;

                    //                                      //Dates.
                    jobjsonNotPaid.strStartDateTime = jobentity.strStartDate + " " + jobentity.strStartTime;
                    jobjsonNotPaid.strEndDateTime = jobentity.strEndDate + " " + jobentity.strEndTime;
                }

                //                                          //Get strJobNumber
                jobjsonNotPaid.strJobNumber = JobJob.strGetJobNumber(jobbasicinfojson.intOrderId,
                    jobbasicinfojson.intJobId, darrjobjsonentityForThisPrintshop);

                if (
                    //                                      //If job is in this List, is already paid but is not
                    //                                      //      still in Wisnet, do not add to the not paid.
                    !darrintJobIdsForNotUpdatedOnWisnet.Contains(jobjsonNotPaid.intJobId)
                    )
                {
                    //                                      //Add JobWaitingForPayment.
                    darrjobjson.Add(jobjsonNotPaid);
                }
            }
            return darrjobjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<JobjsonJobJson> darrjobjsonGetAll(
            //                                              //Get the jobjson for the jobs all jobs.

            PsPrintShop ps_I,
            IConfiguration configuration_I,
            IHubContext<ConnectionHub> iHubContext_I,
            Odyssey2Context context_M
            )
        {
            //                                              //To easy code.
            StagesjsonStagesJsonInternal stagesjsonEasy =
                new StagesjsonStagesJsonInternal(ps_I.strPrintshopId.ParseToInt(), null, null, null, null, null, null,
                null, null);

            //                                              //List add jobjson to return.
            List<JobjsonJobJson> darrjobjson = new List<JobjsonJobJson>();

            //                                              //Unsubmitted
            stagesjsonEasy.boolnUnsubmitted = true;
            List<JobbasicinfojsonJobBasicInfoJson> darrjobBasicInfoUnsubmitted =
                JobJob.darrjobBasicInfoByRequestedStage(stagesjsonEasy, configuration_I);

            if (
                darrjobBasicInfoUnsubmitted.Count > 0
                )
            {
                JobJob.subVerifyJobIsInDB(ps_I.strPrintshopId, 1, darrjobBasicInfoUnsubmitted, context_M);

                //                                      //The jobs returned from Wisnet are converted to the json 
                //                                      //      that will be returned.
                darrjobjson.AddRange(JobJob.darrjobjsonGetUnsubmittedInEstimatingOrWaiting(ps_I, darrjobBasicInfoUnsubmitted,
                    context_M));
            }

            //                                              //In Estimating
            stagesjsonEasy.boolnUnsubmitted = null;
            stagesjsonEasy.boolnInEstimating = true;
            List<JobbasicinfojsonJobBasicInfoJson> darrjobBasicInfoEstimating =
                JobJob.darrjobBasicInfoByRequestedStage(stagesjsonEasy, configuration_I);

            if (
                darrjobBasicInfoEstimating.Count > 0
                )
            {
                JobJob.subVerifyJobIsInDB(ps_I.strPrintshopId, 1, darrjobBasicInfoEstimating, context_M);

                //                                      //The jobs returned from Wisnet are converted to the json 
                //                                      //      that will be returned.
                darrjobjson.AddRange(JobJob.darrjobjsonGetUnsubmittedInEstimatingOrWaiting(ps_I, darrjobBasicInfoEstimating,
                    context_M));
            }

            //                                              //Waiting For Price Approval.
            stagesjsonEasy.boolnUnsubmitted = null;
            stagesjsonEasy.boolnInEstimating = null;
            stagesjsonEasy.boolnWaitingForPriceApproval = true;
            List<JobbasicinfojsonJobBasicInfoJson> darrjobBasicInfoWaitingPriceApproval =
                JobJob.darrjobBasicInfoByRequestedStage(stagesjsonEasy, configuration_I);

            if (
                darrjobBasicInfoWaitingPriceApproval.Count > 0
                )
            {
                JobJob.subVerifyJobIsInDB(ps_I.strPrintshopId, 1, darrjobBasicInfoWaitingPriceApproval, context_M);

                //                                          //The jobs returned from Wisnet are converted to the json 
                //                                          //      that will be returned.
                darrjobjson.AddRange(JobJob.darrjobjsonGetUnsubmittedInEstimatingOrWaiting(ps_I,
                    darrjobBasicInfoWaitingPriceApproval, context_M));
            }

            //                                              //Pending.
            stagesjsonEasy.boolnUnsubmitted = null;
            stagesjsonEasy.boolnInEstimating = null;
            stagesjsonEasy.boolnWaitingForPriceApproval = null;
            stagesjsonEasy.boolnPending = true;
            stagesjsonEasy.boolnInProgress = true;
            List<JobbasicinfojsonJobBasicInfoJson> darrjobBasicInfoPending =
                JobJob.darrjobBasicInfoByRequestedStage(stagesjsonEasy, configuration_I);

            if (
                darrjobBasicInfoPending.Count > 0
                )
            {
                JobJob.subVerifyJobIsInDB(ps_I.strPrintshopId, 1, darrjobBasicInfoPending, context_M);

                //                                          //The jobs returned from Wisnet are converted to the json 
                //                                          //      that will be returned.
                darrjobjson.AddRange(JobJob.darrjobjsonGetPending(ps_I, darrjobBasicInfoPending, iHubContext_I,
                    context_M));
            }

            //                                              //In Progress.
            stagesjsonEasy.boolnUnsubmitted = null;
            stagesjsonEasy.boolnInEstimating = null;
            stagesjsonEasy.boolnWaitingForPriceApproval = null;
            stagesjsonEasy.boolnPending = null;
            stagesjsonEasy.boolnInProgress = null;
            stagesjsonEasy.boolnInProgress = null;
            //                                              //Delete those jobs not exists in wisnet.
            JobJob.subDeleteJobsFromDBNotExistInWisnetAnymore(ps_I, true, context_M);

            darrjobjson.AddRange(JobJob.darrjobjsonGetInProgress(ps_I, context_M, configuration_I));

            //                                              //Completed.
            stagesjsonEasy.boolnUnsubmitted = null;
            stagesjsonEasy.boolnInEstimating = null;
            stagesjsonEasy.boolnWaitingForPriceApproval = null;
            stagesjsonEasy.boolnPending = null;
            stagesjsonEasy.boolnInProgress = null;
            stagesjsonEasy.boolnInProgress = null;
            stagesjsonEasy.boolnCompleted = true;
            List<JobbasicinfojsonJobBasicInfoJson> darrjobBasicInfoCompleted =
                JobJob.darrjobBasicInfoByRequestedStage(stagesjsonEasy, configuration_I);

            if (
                darrjobBasicInfoCompleted.Count > 0
                )
            {
                JobJob.subVerifyJobIsInDB(ps_I.strPrintshopId, 1, darrjobBasicInfoCompleted, context_M);

                //                                          //Delete those jobs not exists in wisnet.
                JobJob.subDeleteJobsFromDBNotExistInWisnetAnymore(ps_I, false, context_M);

                darrjobjson.AddRange(JobJob.darrjobjsonGetCompleted(ps_I, darrjobBasicInfoCompleted, context_M,
                    configuration_I));
            }

            //                                              //Paid. 
            //                                              //S e comentó porque ya van incluidos en los completados
            //stagesjsonEasy.boolnUnsubmitted = null;
            //stagesjsonEasy.boolnInEstimating = null;
            //stagesjsonEasy.boolnWaitingForPriceApproval = null;
            //stagesjsonEasy.boolnPending = null;
            //stagesjsonEasy.boolnInProgress = null;
            //stagesjsonEasy.boolnInProgress = null;
            //stagesjsonEasy.boolnCompleted = null;
            //stagesjsonEasy.boolnWaitingForPayment = true;
            //List<JobbasicinfojsonJobBasicInfoJson> darrjobBasicInfoPaid =
            //    JobJob.darrjobBasicInfoByRequestedStage(stagesjsonEasy, configuration_I);

            //if (
            //    darrjobBasicInfoPaid.Count > 0
            //    )
            //{
            //    JobJob.subVerifyJobIsInDB(ps_I.strPrintshopId, 1, darrjobBasicInfoPaid, context_M);

            //    darrjobjson.AddRange(JobJob.darrjobjsonGetNotPaid(ps_I, darrjobBasicInfoPaid, context_M));
            //}

            return darrjobjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<JobbasicinfojsonJobBasicInfoJson> darrjobBasicInfoByRequestedStage(
            //                                              //Get the job basic info for a requested stage.

            StagesjsonStagesJsonInternal stagesjsonEasy_I,
            IConfiguration configuration_I
            )
        {
            List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicinfojson = new List<JobbasicinfojsonJobBasicInfoJson>();
            //                                          //Get jobs from Wisnet. 
            Task<List<JobbasicinfojsonJobBasicInfoJson>> Task_darrjobbasicinfojson = HttpTools<
                JobbasicinfojsonJobBasicInfoJson>.GetListAsyncWithBodyToEndPoint(stagesjsonEasy_I,
                configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                "/PrintshopData/printshopJobsToFillList/");
            Task_darrjobbasicinfojson.Wait();

            if (
                Task_darrjobbasicinfojson.Result != null
                )
            {
                darrjobbasicinfojson = Task_darrjobbasicinfojson.Result;
            }

            return darrjobbasicinfojson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static double numGetJobProgress(
            //                                              //Get the progress of a job.
            JobjsonJobJson jobFromWisnet_I,
            int intPkPrintshop_I
            )
        {
            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get job's process completed.
            IQueryable<PiwjentityProcessInWorkflowForAJobEntityDB> setpiwjentity =
                context.ProcessInWorkflowForAJob.Where(piwj => piwj.intJobId == jobFromWisnet_I.intJobId &&
                piwj.intPkPrintshop == intPkPrintshop_I &&
                piwj.intStage == JobJob.intProcessInWorkflowCompleted);
            List<PiwjentityProcessInWorkflowForAJobEntityDB> darrpiwjentity = setpiwjentity.ToList();
            int intJobProcessCompleted = darrpiwjentity.Count();

            double numJobProgress = 0;
            if (
                intJobProcessCompleted > 0
                )
            {
                //                                          //Get pkproduct of the job.
                int intPkWorkflow = context.ProcessInWorkflow.FirstOrDefault(piw =>
                    piw.intPk == darrpiwjentity[0].intPkProcessInWorkflow).intPkWorkflow;

                //                                          //Get all the processes.
                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
                List<DynLkjsonDynamicLinkJson> darrdynlkjsonNotUsed;
                ProdtypProductType.subGetWorkflowValidWay(intPkWorkflow, jobFromWisnet_I, out darrpiwentityAllProcesses,
                    out darrdynlkjsonNotUsed);

                //                                          //Get job's total process.
                int intJobTotalProcess = darrpiwentityAllProcesses.Count();

                if (
                    intJobTotalProcess == 0
                    )
                {
                    numJobProgress = 0;
                }
                else
                {
                    //                                          //Calculate job's progress.
                    numJobProgress = (((double)intJobProcessCompleted / (double)intJobTotalProcess) * 100).Round(2);
                }
            }
            return numJobProgress;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPrintshopEstimates(
            //                                              //Get estimate from a printshop.

            PsPrintShop psPrintshop_I,
            bool boolRequested_I,
            bool boolWaitingForCustResponse_I,
            bool boolRejected_I,
            IConfiguration configuration_I,
            out List<EstimjsonEstimateJson> darrestimjson_O,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrestimjson_O = new List<EstimjsonEstimateJson>();

            //                                          //Verify an update/add(if needed) estimates to db.
            bool boolEstimatesWereUpdated;
            JobJob.subUpdateOrAddEstimNumberForAPrintshop(psPrintshop_I.strPrintshopId, out boolEstimatesWereUpdated,
                configuration_I, context_M);

            JobjsonentityJobJsonEntityDB jobjsonentity = context_M.JobJson.FirstOrDefault(jjson =>
                jjson.strPrintshopId == psPrintshop_I.strPrintshopId && (jjson.intOrderId == 0 ||
                jjson.intOrderId == -1) && jjson.intnOrderNumber == null && jjson.intnJobNumber == null);

            //                                              //Get all estimate' basic info.
            List<JobbasicinfojsonJobBasicInfoJson> darrjobstofilllstjsonJobsFromWisnet =
                JobJob.darrjobstofilllstjsonEstimateFromPrintshop(psPrintshop_I.strPrintshopId, boolRequested_I,
                boolWaitingForCustResponse_I, boolRejected_I, configuration_I, ref intStatus_IO, ref strUserMessage_IO,
                ref strDevMessage_IO);

            //                                              //Add new Estimates to jobjson table.
            if (
                //                                          //When estimates were not all updated, update only new
                //                                          //      ones.
                !boolEstimatesWereUpdated
                )
            {
                JobJob.subAddNewEstimatesToJobJsonTable(darrjobstofilllstjsonJobsFromWisnet,
                psPrintshop_I.strPrintshopId, context_M);
            }

            List<WfentityWorkflowEntityDB> darrwfentity = context_M.Workflow.Where(wf =>
                    wf.intPkPrintshop == psPrintshop_I.intPk &&
                    wf.intnJobId != null).ToList();

            //                                              //Get jobjson's list in order to get the estimateNumber.
            List<JobjsonentityJobJsonEntityDB> darrjobjson = context_M.JobJson.Where(jjson =>
                jjson.strPrintshopId == psPrintshop_I.strPrintshopId && jjson.intOrderId <= 0).ToList();

            foreach (JobbasicinfojsonJobBasicInfoJson jobFromWisnet in darrjobstofilllstjsonJobsFromWisnet)
            {
                //                                          //Get PkProduct based on a job.
                int intPkProduct = JobJob.intGetPkProductFromJob(jobFromWisnet.intProductKey,
                    psPrintshop_I.intPk, context_M);

                //                                          //Get Estimate's workflow if exists.
                int? intnPkWorkflow = darrwfentity.Exists(wf => wf.intnJobId == jobFromWisnet.intJobId) ?
                    darrwfentity.Find(wf => wf.intnJobId == jobFromWisnet.intJobId).intPk : (int?)null;

                //                                          //Get Estimate's number.
                String strEstimateNumber = JobJob.strGetEstimateNumber(jobFromWisnet.intJobId,
                    psPrintshop_I.strPrintshopId, darrjobjson);

                //                                          //Object to fill and return.
                EstimjsonEstimateJson estimjsonJob = new EstimjsonEstimateJson(
                    jobFromWisnet.intJobId, jobFromWisnet.strJobTicket, jobFromWisnet.strProductName,
                    jobFromWisnet.dateLastUpdate, jobFromWisnet.strCustomerName, intPkProduct, intnPkWorkflow,
                    strEstimateNumber);

                //                                          //Add JobPending.
                darrestimjson_O.Add(estimjsonJob);
            }

            darrestimjson_O.Sort();
            intStatus_IO = 200;
            strUserMessage_IO = "";
            strDevMessage_IO = "Success.";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subUpdateOrAddEstimNumberForAPrintshop(
            //                                              //Add printshop estimates.

            String strPrintshopId_I,
            out bool boolEstimatesWereUpdated_O,
            IConfiguration configuration_I,
            Odyssey2Context context_M
            )
        {
            boolEstimatesWereUpdated_O = false;

            //                                              //Try to get register from jobjson table in order to know
            //                                              //      what step must be taken.
            //                                              //1.- Add strEstimateNumber to all estimates.
            //                                              //2.-Delete exisiting estimates and add strEstimateNumber
            //                                              //      to all of estimates.

            JobjsonentityJobJsonEntityDB jobjsonentity = context_M.JobJson.FirstOrDefault(jjson =>
                jjson.strPrintshopId == strPrintshopId_I && (jjson.intOrderId == 0 ||
                jjson.intOrderId == -1) && jjson.intnOrderNumber == null && jjson.intnJobNumber == null);

            /*CASE*/
            if (
                //                                          //Printshop does not have estimation, add them all to DB.
                jobjsonentity == null
                )
            {
                JobJob.subGetAllPrintshopEstimatesAndAddStrEstimateNumber(strPrintshopId_I, configuration_I, context_M);
                boolEstimatesWereUpdated_O = true;
            }
            else if (
                //                                          //2
                jobjsonentity != null && jobjsonentity.intnEstimateNumber == null
                )
            {
                //                                          //Delete estimates.
                JobJob.subDeleteEstimatesFromJobJsonTable(strPrintshopId_I, context_M);
                JobJob.subGetAllPrintshopEstimatesAndAddStrEstimateNumber(strPrintshopId_I, configuration_I, context_M);
                boolEstimatesWereUpdated_O = true;
            }
            /*END-CASE*/
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<JobbasicinfojsonJobBasicInfoJson> darrjobstofilllstjsonEstimateFromPrintshop(
            //                                              //Get all printshop's estimate' basic info.

            //                                              //PrintshopId
            String strPrintshopId_I,
            bool boolRequested_I,
            bool boolWaitingForCustResponse_I,
            bool boolRejected_I,
            //                                              //Configuration.
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            List<JobbasicinfojsonJobBasicInfoJson> darrjobstofilllstjsonFromWisnet = new
                List<JobbasicinfojsonJobBasicInfoJson>();

            //                                              //Get Jobs ids from Wisnet. 
            Task<List<JobbasicinfojsonJobBasicInfoJson>> Task_darrjobsidsjsonFromWisnet =
                HttpTools<JobbasicinfojsonJobBasicInfoJson>.GetListAsyncToEndPoint(
                    configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                    "/PrintshopData/printshopEstimatesToFillList/" + strPrintshopId_I + "/" + boolRequested_I.ToString() + "/" +
                    boolWaitingForCustResponse_I.ToString() + "/" + boolRejected_I.ToString());

            Task_darrjobsidsjsonFromWisnet.Wait();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet database connection lost.";
            if (
                Task_darrjobsidsjsonFromWisnet.Result != null
                )
            {
                darrjobstofilllstjsonFromWisnet = Task_darrjobsidsjsonFromWisnet.Result;

                /*//                                          //To add jobnumber and order number.
                //                                          //Establish the connection.
                Odyssey2Context context = new Odyssey2Context();

                List<JobjsonentityJobJsonEntityDB> darrjobjsonentity = context.JobJson.Where(job =>
                    job.intJobID > 0 && job.strPrintshopId == strPrintshopId_I).ToList();

                int intLast = darrjobstofilllstjsonFromWisnet.Count - 1;
                for (int intI = intLast; intI >= 0; intI = intI - 1)
                {
                    JobbasicinfojsonJobBasicInfoJson jobstofilllstjsonFromWisnet =
                        darrjobstofilllstjsonFromWisnet[intI];
                    if (
                        !(darrjobjsonentity.Exists(job => job.intJobID == jobstofilllstjsonFromWisnet.intJobId &&
                            job.intOrderId == jobstofilllstjsonFromWisnet.intOrderId))
                        )
                    {
                        JobJob.subCreateJobAndOrderNumberIntoJobJsonTable(jobstofilllstjsonFromWisnet.intOrderId,
                        jobstofilllstjsonFromWisnet.intJobId, strPrintshopId_I);
                    }
                }*/

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            return darrjobstofilllstjsonFromWisnet;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subCreateJobAndOrderNumberIntoJobJsonTable(
            //                                              //Generates the numbers for the order and job into jobjson
            //                                              //      table.

            int intOrderId_I,
            int intJobId_I,
            String strPrintshopId_I
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            if (
                intOrderId_I <= 0
                )
            {
                /*//                                          //Create new register.
                 JobjsonentityJobJsonEntityDB jobjsonentityToAdd = new JobjsonentityJobJsonEntityDB
                 {
                     intJobID = intJobId_I,
                     strPrintshopId = strPrintshopId_I,
                     intOrderId = intOrderId_I,
                     intnOrderNumber = null,
                     intnJobNumber = null
                 };
                 context.JobJson.Add(jobjsonentityToAdd);*/
            }
            else
            {
                //                                              //Verify if the order already exists.
                List<JobjsonentityJobJsonEntityDB> darrjobjsonentity = context.JobJson.Where(job =>
                    job.intOrderId == intOrderId_I &&
                    job.strPrintshopId == strPrintshopId_I &&
                    job.intnOrderNumber != null &&
                    job.intnJobNumber != null).ToList();

                if (
                    //                                          //Order already exists in DB
                    darrjobjsonentity.Count > 0
                    )
                {
                    darrjobjsonentity.OrderBy(job => job.intnJobNumber);
                    //                                          //Takes last job.
                    JobjsonentityJobJsonEntityDB jobjsonentity = darrjobjsonentity.Last();

                    //                                          //Create new register.
                    JobjsonentityJobJsonEntityDB jobjsonentityToAdd = new JobjsonentityJobJsonEntityDB
                    {
                        intJobID = intJobId_I,
                        strPrintshopId = strPrintshopId_I,
                        intOrderId = intOrderId_I,
                        intnOrderNumber = jobjsonentity.intnOrderNumber,
                        intnJobNumber = jobjsonentity.intnJobNumber + 1
                    };
                    context.JobJson.Add(jobjsonentityToAdd);
                }
                else
                {
                    //                                          //Order does not exists.
                    //                                          //Takes all orders.
                    List<JobjsonentityJobJsonEntityDB> darrjobjsonentityOrders = context.JobJson.Where(job =>
                    job.strPrintshopId == strPrintshopId_I &&
                    job.intnOrderNumber != null &&
                    job.intnJobNumber != null).ToList();

                    if (
                        darrjobjsonentityOrders.Count == 0
                        )
                    {
                        //                                          //Create new register.
                        JobjsonentityJobJsonEntityDB jobjsonentityToAdd = new JobjsonentityJobJsonEntityDB
                        {
                            intJobID = intJobId_I,
                            strPrintshopId = strPrintshopId_I,
                            intOrderId = intOrderId_I,
                            intnOrderNumber = 1,
                            intnJobNumber = 1
                        };
                        context.JobJson.Add(jobjsonentityToAdd);
                    }
                    else
                    {
                        darrjobjsonentityOrders.OrderBy(job => job.intnOrderNumber);
                        //                                          //Takes last order.
                        JobjsonentityJobJsonEntityDB jobjsonentityOrder = darrjobjsonentityOrders.Last();

                        //                                          //Create new register.
                        JobjsonentityJobJsonEntityDB jobjsonentityToAdd = new JobjsonentityJobJsonEntityDB
                        {
                            intJobID = intJobId_I,
                            strPrintshopId = strPrintshopId_I,
                            intOrderId = intOrderId_I,
                            intnOrderNumber = jobjsonentityOrder.intnOrderNumber + 1,
                            intnJobNumber = 1
                        };
                        context.JobJson.Add(jobjsonentityToAdd);
                    }
                }
            }
            context.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subGetAllPrintshopEstimatesAndAddStrEstimateNumber(
            //                                              //Get all printshop's estimate' basic info.

            //                                              //PrintshopId
            String strPrintshopId_I,
            //                                              //Configuration.
            IConfiguration configuration_I,
            Odyssey2Context context_M
            )
        {
            List<JobbasicinfojsonJobBasicInfoJson> darrestimatesFromWisnet = new
                List<JobbasicinfojsonJobBasicInfoJson>();

            //                                              //Get Jobs ids from Wisnet. 
            Task<List<JobbasicinfojsonJobBasicInfoJson>> Task_darrjobsidsjsonFromWisnet =
                HttpTools<JobbasicinfojsonJobBasicInfoJson>.GetListAsyncToEndPoint(
                    configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                    "/PrintshopData/getAllPrintshopEstimates/" + strPrintshopId_I);
            Task_darrjobsidsjsonFromWisnet.Wait();

            if (
                Task_darrjobsidsjsonFromWisnet.Result != null
                )
            {
                darrestimatesFromWisnet = Task_darrjobsidsjsonFromWisnet.Result;

                //                                          //To add jobnumber and order number.
                int intLast = darrestimatesFromWisnet.Count - 1;
                for (int intI = intLast; intI >= 0; intI--)
                {
                    JobbasicinfojsonJobBasicInfoJson jobstofilllstjsonFromWisnet =
                        darrestimatesFromWisnet[intI];

                    JobJob.subAddIntEstimateNumber(jobstofilllstjsonFromWisnet.intJobId,
                        jobstofilllstjsonFromWisnet.intOrderId, intI + 1, strPrintshopId_I, context_M);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subAddIntEstimateNumber(
            //                                              //Add register to job json table, adding intEstimateNumber.

            int intJobId_I,
            int intOrderId_I,
            int intEstimateNumber,
            String strPrintshopId_I,
            Odyssey2Context context_M
            )
        {
            JobjsonentityJobJsonEntityDB jobjsonentity = new JobjsonentityJobJsonEntityDB
            {
                intJobID = intJobId_I,
                strPrintshopId = strPrintshopId_I,
                jobjson = null,
                intOrderId = intOrderId_I,
                intnEstimateNumber = intEstimateNumber
            };
            context_M.JobJson.Add(jobjsonentity);
            context_M.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subDeleteEstimatesFromJobJsonTable(
            //                                              //Delete all estimates for a printshop from jobjson table.

            String strPrintshopId_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Get all printshop's estimates.
            List<JobjsonentityJobJsonEntityDB> darrjobjsonentity = context_M.JobJson.Where(jjson =>
                jjson.strPrintshopId == strPrintshopId_I && (jjson.intOrderId == 0 || jjson.intOrderId == -1) &&
                jjson.intnOrderNumber == null && jjson.intnJobNumber == null).ToList();

            foreach (JobjsonentityJobJsonEntityDB jobjsonentity in darrjobjsonentity)
            {
                context_M.JobJson.Remove(jobjsonentity);
            }
            context_M.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddNewEstimatesToJobJsonTable(
            //                                              //Delete all estimates for a printshop from jobjson table.

            List<JobbasicinfojsonJobBasicInfoJson> darrjobstofilllstjsonJobsFromWisnet_I,
            String strPrintshopId_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Get all printshop's estimates.
            List<JobjsonentityJobJsonEntityDB> darrjobjsonentity = context_M.JobJson.Where(jjson =>
                jjson.strPrintshopId == strPrintshopId_I && (jjson.intOrderId == 0 || jjson.intOrderId == -1) &&
                jjson.intnOrderNumber == null && jjson.intnJobNumber == null).ToList();

            String strEstimateNumber;
            foreach (JobbasicinfojsonJobBasicInfoJson jobbasicinfo in darrjobstofilllstjsonJobsFromWisnet_I)
            {
                //                                          //If is a new job, we have to add it to jobjson table.
                if (
                    !darrjobjsonentity.Exists(job => job.intJobID == jobbasicinfo.intJobId)
                    )
                {
                    JobJob.subAddNewEstimateNumberToJobJsonTable(jobbasicinfo, strPrintshopId_I,
                        out strEstimateNumber, context_M);
                }
            }
            context_M.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subAddNewEstimateNumberToJobJsonTable(
            //                                              //Add new estimate to jobjson table.

            JobbasicinfojsonJobBasicInfoJson jobstofilllstjson_I,
            String strPrintshopId_I,
            out String strEstimateNumer_O,
            Odyssey2Context context_M
            )
        {
            //                                              //Get all printshop's estimates.
            List<JobjsonentityJobJsonEntityDB> darrjobjsonentity = context_M.JobJson.Where(jjson =>
                jjson.strPrintshopId == strPrintshopId_I && (jjson.intOrderId == 0 || jjson.intOrderId == -1) &&
                jjson.intnOrderNumber == null && jjson.intnJobNumber == null).ToList();

            int intNextEstimateNumber = (darrjobjsonentity.Count > 0) ?
                                ((int)darrjobjsonentity.Max(job => job.intnEstimateNumber)) + 1 : 1;

            strEstimateNumer_O = intNextEstimateNumber.ToString();

            JobjsonentityJobJsonEntityDB jobjsonentity = new JobjsonentityJobJsonEntityDB
            {
                intJobID = jobstofilllstjson_I.intJobId,
                strPrintshopId = strPrintshopId_I,
                jobjson = null,
                intOrderId = jobstofilllstjson_I.intOrderId,
                intnEstimateNumber = intNextEstimateNumber
            };
            context_M.JobJson.Add(jobjsonentity);
            context_M.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static String strGetEstimateNumber(
            //                                              //Add printshop estimates.

            int intJobId_I,
            String strPrintshopId_I,
            List<JobjsonentityJobJsonEntityDB> darrjobjsonentity_I
            )
        {
            String strEstimateNumber = (darrjobjsonentity_I.Exists(jjson => jjson.intJobID == intJobId_I &&
                jjson.strPrintshopId == strPrintshopId_I)) ? darrjobjsonentity_I.FirstOrDefault(jjson =>
                jjson.intJobID == intJobId_I &&
                jjson.strPrintshopId == strPrintshopId_I).intnEstimateNumber.ToString() : "";

            return strEstimateNumber;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetFilesUrl(
            PsPrintShop ps_I,
            int intJobId_I,
            IConfiguration configuration_I,
            out List<FileurljsonFileUrlJson> darrfileurljson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //To return.
            darrfileurljson_O = new List<FileurljsonFileUrlJson>();

            intStatus_IO = 401;
            JobjsonJobJson jobjson;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                              //Get Jobs ids from Wisnet. 
                Task<List<FileurljsonFileUrlJson>> Task_darrjobsidsjsonFromWisnet =
                    HttpTools<FileurljsonFileUrlJson>.GetListAsyncToEndPoint(
                        configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                      "/PrintshopData/GetJobFilesUrl/" + ps_I.strPrintshopId + "/" + intJobId_I);
                Task_darrjobsidsjsonFromWisnet.Wait();

                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Wisnet database connection lost.";
                if (
                    Task_darrjobsidsjsonFromWisnet.Result != null
                    )
                {
                    List<FileurljsonFileUrlJson> darrfileurljson = Task_darrjobsidsjsonFromWisnet.Result;

                    foreach (FileurljsonFileUrlJson fileurljson in darrfileurljson)
                    {
                        if (
                            //                              //Verify file exists.
                            fileurljson.strFileUrl.Length > 0
                            )
                        {
                            darrfileurljson_O.Add(fileurljson);
                        }
                    }

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static JobnotesjsonJobNotesJson subGetNotes(
            //                                              //Get all notes related to a job.

            int intJobId_I,
            int intPkWorkflow_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            JobnotesjsonJobNotesJson jobnotesjson = null;

            JobjsonJobJson jobjson = new JobjsonJobJson();
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Get Workflow.
                WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf => wf.intPk == intPkWorkflow_I);

                //                                          //Get job's register in job table.
                JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job => job.intJobID == intJobId_I &&
                    job.intPkPrintshop == ps_I.intPk);

                if (
                    //                                      //If job is inprogress or completed (there is a register
                    //                                      //      in job's table) does not matter if workflow is
                    //                                      //      deleted.
                    (jobentity != null && wfentity != null && jobentity.intPkWorkflow == intPkWorkflow_I) ||
                    //                                      //If the job is pending, the workflow can not be deleted.
                    (jobentity == null && wfentity != null && wfentity.boolDeleted == false)
                    )
                {
                    //                                      //Wisnet notes extracted from the attributes.
                    String strWisnetNote = JobJob.strWisnetNotes(jobjson);

                    //                                      //Look for job's notes.
                    JobnotesJobNotesEntityDB jobnoteentity = context.JobNotes.FirstOrDefault(jobnote =>
                        jobnote.intJobID == intJobId_I);

                    String strOdyssey2Note = "";
                    int intJobNoteEntity = -1;
                    if (
                        //                                  //There is a note for the job.
                        jobnoteentity != null
                        )
                    {
                        strOdyssey2Note = jobnoteentity.strOdyssey2Note != null ? jobnoteentity.strOdyssey2Note : "";
                        intJobNoteEntity = jobnoteentity.intPk;
                    }

                    //                                          //Get info of the job previous.
                    int? intnPreviousJobId = null;
                    int? intnPkWorkflow = null;
                    String strJobName = null;
                    JobJob.subGetInfoPreviusJob(strOdyssey2Note, ps_I, context, out intnPreviousJobId, out intnPkWorkflow,
                        out strJobName);

                    //                                          //Get process's notes for the job.
                    List<PronotesjsonProcessNotesJson> darrpronotesjson =
                        JobJob.darrpronotesjsonGetProcessNotes(intPkWorkflow_I, jobjson, ps_I, ref intnPreviousJobId, 
                        ref intnPkWorkflow, ref strJobName);

                    //                                      //Owerwrite json to return.
                    jobnotesjson = new JobnotesjsonJobNotesJson(intJobNoteEntity, strWisnetNote, strOdyssey2Note,
                        intnPreviousJobId, intnPkWorkflow, strJobName, darrpronotesjson.ToArray());

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "Success.";
                }
            }

            return jobnotesjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subGetInfoPreviusJob(
            //                                              //Get the order from the note of the reorder.
            
            String strNote_I,
            PsPrintShop ps_I,
            Odyssey2Context context_I,
            out int? intnPreviousJobId_O, 
            out int? intnPkWorkflow_O,
            out String strJobName_O
            )
        {
            intnPreviousJobId_O = null;
            intnPkWorkflow_O = null;
            strJobName_O = null;

            int? intnOrderNumber;
            int? intnJobNumber;

            //                                              //Get info of the job from the note of the reorder.
            JobJob.strBuildTheNewNoteOrGetFatherOrderJobNumber(
                strNote_I, out intnOrderNumber, out intnJobNumber);

            if (
                //                                          //Info exist.
                intnOrderNumber != null &&
                intnJobNumber != null
                )
            {
                strJobName_O = intnOrderNumber + " - " + intnJobNumber;

                //                                          //The jobjson should be in the DB.
                JobjsonentityJobJsonEntityDB jobjsonentity = context_I.JobJson.FirstOrDefault(jobjson => 
                    jobjson.strPrintshopId == ps_I.strPrintshopId && jobjson.intnOrderNumber == intnOrderNumber &&
                    jobjson.intnJobNumber == intnJobNumber);

                if (
                    jobjsonentity != null
                    )
                {
                    JobjsonJobJson jobjsonJob = JsonSerializer.Deserialize<JobjsonJobJson>(jobjsonentity.jobjson);
                    intnPreviousJobId_O = jobjsonentity.intJobID;

                    //                                      //Get the workflow ot the previus job.
                    JobentityJobEntityDB jobentity = context_I.Job.FirstOrDefault(job => job.intPkPrintshop == ps_I.intPk &&
                        job.intJobID == jobjsonentity.intJobID);

                    //                                      //Record will be found if the JobPrevius was completed in
                    //                                      //    Odyssey2.
                    if (
                        //                                  //JobPrevius was completed in odyssey2.
                        jobentity != null
                        )
                    {
                        intnPkWorkflow_O = jobentity.intPkWorkflow;
                    }
                }
                else
                {
                    //                                      //Review because the jobjson previus not exist.
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strWisnetNotes(
            //                                              //Create a note for a job into jobNotes table.

            JobjsonJobJson jobjsonJob_I
            )
        {
            String strWisnetNote = "";
            Regex regex = new Regex(@"^.*Other\s--.*$");
            foreach (AttrjsonAttributeJson attrjson in jobjsonJob_I.darrattrjson)
            {
                if (
                    regex.IsMatch(attrjson.strValue)
                    )
                {
                    String strToAddToWisnetNote = JobJob.strGetOtherAttributeValue(attrjson.strValue);

                    if (
                        strWisnetNote.Length == 0
                        )
                    {
                        strWisnetNote = strWisnetNote + attrjson.strAttributeName + " " + "-" + " " +
                            strToAddToWisnetNote;
                    }
                    else
                    {
                        strWisnetNote = strWisnetNote + "." + " " + attrjson.strAttributeName + " " + "-" + " " +
                            strToAddToWisnetNote;
                    }
                }
            }

            return strWisnetNote;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strGetOtherAttributeValue(
            //                                              //Create a note for a job into jobNotes table.

            String strOtherAttributeValue_I
            )
        {
            int intStartOfAttributeOther = strOtherAttributeValue_I.IndexOfOrdinal("Other --");
            String strOtherValue = strOtherAttributeValue_I.Substring(intStartOfAttributeOther + 9,
                strOtherAttributeValue_I.Length - (intStartOfAttributeOther + 9));

            return strOtherValue;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static List<PronotesjsonProcessNotesJson> darrpronotesjsonGetProcessNotes(
             //                                              //Look and return notes of all process for an especific
             //                                              //      workflow and job.

            int intPkWorkflow_I,
            JobjsonJobJson jobjson_I,
            PsPrintShop ps_I,
            ref int? intnPreviousJobId_M,
            ref int? intnPkWorkflow_M,
            ref String strJobName_M
             )
        {
            List<PronotesjsonProcessNotesJson> darrpronotesjson = new List<PronotesjsonProcessNotesJson>();
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the process for this job.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity;
            List<DynLkjsonDynamicLinkJson> darrdynlkjson;
            ProdtypProductType.subGetWorkflowValidWay(intPkWorkflow_I, jobjson_I, out darrpiwentity, out darrdynlkjson);

            //                                          //Get all processes's name.
            //                                          //To store process's name.
            List<PronamejsonProcessNameJson> darrpronamejson = (from piwentity in context.ProcessInWorkflow
                                                                join eleentity in context.Element on
                                                                piwentity.intPkProcess equals eleentity.intPk
                                                                where
                                                                piwentity.intPkWorkflow == intPkWorkflow_I
                                                                select new PronamejsonProcessNameJson(
                                                                piwentity.intProcessInWorkflowId, piwentity.intnId,
                                                                eleentity.strElementName)).ToList();

            foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentity)
            {
                //                                          //Get process's name.
                String strProcessName = JobJob.strGetProcessName(piwentity.intProcessInWorkflowId, darrpronamejson);

                List<PronotesentityProcessNotesEntityDB> darrpronotesentity = context.ProcessNotes.Where(pronote =>
                    pronote.intPkProcessInworkflow == piwentity.intPk && pronote.intJobID == jobjson_I.intJobId).ToList();

                List<NoteprojsonNoteProcessJson> darrnoteprojson = new List<NoteprojsonNoteProcessJson>();
                foreach (PronotesentityProcessNotesEntityDB pronotesentity in darrpronotesentity)
                {
                    if (
                        //                                          //Data null therefor continue find the previusJob.
                        intnPreviousJobId_M == null && intnPkWorkflow_M == null && strJobName_M == null
                        )
                    {
                        //                                          //Get info of the job previous.
                        int? intnPreviousJobId = null;
                        int? intnPkWorkflow = null;
                        String strJobName = null;
                        JobJob.subGetInfoPreviusJob(pronotesentity.strText, ps_I, context, out intnPreviousJobId, out intnPkWorkflow,
                            out strJobName);

                        //                                          //Assign the new data.
                        intnPreviousJobId_M = intnPreviousJobId;
                        intnPkWorkflow_M = intnPkWorkflow;
                        strJobName_M = strJobName == null ? null : strJobName.ToString();
                    }

                    //                                      //Build the json.
                    NoteprojsonNoteProcessJson noteprojson = new NoteprojsonNoteProcessJson(pronotesentity.intPk, 
                        pronotesentity.strText);
                    darrnoteprojson.Add(noteprojson);
                }

                //                                          //Create json to add to list to return.
                PronotesjsonProcessNotesJson pronotesjson = new PronotesjsonProcessNotesJson(strProcessName, null, null, null,
                    darrnoteprojson.ToArray());
                darrpronotesjson.Add(pronotesjson);
            }

            return darrpronotesjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strGetProcessName(
            //                                              //Return the name of an especific process.

            int intProcessInWorkflowId_I,
            List<PronamejsonProcessNameJson> darrpronamejson_I
            )
        {
            PronamejsonProcessNameJson proname = darrpronamejson_I.FirstOrDefault(pro =>
                pro.intProcessInWorkflowId == intProcessInWorkflowId_I);

            String strProcessName = proname.intnId == null ? proname.strProcessName :
                proname.strProcessName + " " + "(" + proname.intnId + ")";

            return strProcessName;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strGetCalDescription(
            //                                              //Return the description of a given calculation.

            int intPkCalculation_I
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            String strGetCalcDescription = context.Calculation.FirstOrDefault(cal => cal.intPk == intPkCalculation_I).strDescription;

            return strGetCalcDescription;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static PronotesjsonProcessNotesJson pronotesjsonGetProcessNotes(
            //                                              //Search for all notes for an especific process.

            int intJobId_I,
            int? intnPkPeriod_I,
            int? intnPkProcessInWorkflow_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //To send back with processes's notes.
            PronotesjsonProcessNotesJson pronotesjson = null;

            JobjsonJobJson jobjson;
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjson,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                //                                          //Establish the connection.
                Odyssey2Context context = new Odyssey2Context();

                /*CASE*/
                if (
                    //                                      //Service was called from myCalendar.
                    intnPkPeriod_I != null && intnPkProcessInWorkflow_I == null
                    )
                {
                    //                                      //Validate Period.
                    PerentityPeriodEntityDB perentity = context.Period.FirstOrDefault(per => 
                        per.intPk == intnPkPeriod_I);

                    intStatus_IO = 402;
                    strUserMessage_IO = "Something wrong.";
                    strDevMessage_IO = "Invalid PkPeriod.";
                    if (
                        perentity != null
                        )
                    {
                        //                                  //Create process's notes json to send back.
                        pronotesjson = JobJob.pronotejsonGetProcessNotes(intJobId_I, perentity.intPkWorkflow,
                            perentity.intProcessInWorkflowId, ps);

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "Success.";
                    }
                }
                else if (
                    //                                      //Service was called from WFJ.
                    intnPkPeriod_I == null && intnPkProcessInWorkflow_I != null
                    )
                {
                    //                                      //Validate process in workflow.
                    PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                        piw.intPk == intnPkProcessInWorkflow_I);

                    intStatus_IO = 402;
                    strUserMessage_IO = "Something wrong.";
                    strDevMessage_IO = "Invalid PkProcessInWorkflow.";
                    if (
                        piwentity != null
                        )
                    {
                        //                                  //Create process's notes json to send back.
                        pronotesjson = JobJob.pronotejsonGetProcessNotes(intJobId_I, piwentity.intPkWorkflow,
                            piwentity.intProcessInWorkflowId, ps);

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "Success.";
                    }
                }
                /*END-CASE*/
            }

            return pronotesjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static PronotesjsonProcessNotesJson pronotejsonGetProcessNotes(
            //                                              //Get notes for an especific process.

            int intJobId_I,
            int intPkWorkflow_I,
            int intProcessInWorkflowId_I,
            PsPrintShop ps_I
            )
        {
            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            String strProcessName = (from piwentity in context.ProcessInWorkflow
                                     join eleentity in context.Element on
                                     piwentity.intPkProcess equals eleentity.intPk
                                     where piwentity.intPkWorkflow == intPkWorkflow_I &&
                                     piwentity.intProcessInWorkflowId == intProcessInWorkflowId_I
                                     select eleentity).FirstOrDefault().strElementName;

            //                                              //Get list of process's notes.
            /*List<PronotesentityProcessNotesEntityDB> darrpronotesentity = (from piwentity in context.ProcessInWorkflow
                                                                           join pronotesentity in context.ProcessNotes on
                                                                           piwentity.intPk equals
                                                                           pronotesentity.intPkProcessInworkflow
                                                                           where piwentity.intPkWorkflow == 
                                                                           intPkWorkflow_I &&
                                                                           piwentity.intProcessInWorkflowId ==
                                                                           intProcessInWorkflowId_I
                                                                           select pronotesentity).ToList();

            //                                              //Creae json notes to send back.
            List<NoteprojsonNoteProcessJson> darrnoteprojson = new List<NoteprojsonNoteProcessJson>();
            foreach(PronotesentityProcessNotesEntityDB pronotesentity in darrpronotesentity)
            {
                NoteprojsonNoteProcessJson noteprojson = new NoteprojsonNoteProcessJson(pronotesentity.strText);
                darrnoteprojson.Add(noteprojson);

            }*/

            //                                          //Get info of the job previous.
            int? intnPreviousJobId = null;
            int? intnPkWorkflow = null;
            String strJobName = null;

            List<NoteprojsonNoteProcessJson> darrnoteprojson = (from piwentity in context.ProcessInWorkflow
                                            join pronotesentity in context.ProcessNotes on
                                            piwentity.intPk equals
                                            pronotesentity.intPkProcessInworkflow
                                            where piwentity.intPkWorkflow ==
                                            intPkWorkflow_I &&
                                            piwentity.intProcessInWorkflowId ==
                                            intProcessInWorkflowId_I &&
                                            pronotesentity.intJobID == intJobId_I
                                            select pronotesentity
                                                ).ToList().Select(rpro =>
                                                {
                                                    if (
                                                        //  //Data null therefor continue find the previusJob.
                                                        intnPreviousJobId == null &&
                                                        intnPkWorkflow == null && strJobName == null
                                                        )
                                                    {
                                                        JobJob.subGetInfoPreviusJob(
                                                            rpro.strText, ps_I, context, out intnPreviousJobId,
                                                            out intnPkWorkflow,
                                                            out strJobName);
                                                    }

                                                    return new NoteprojsonNoteProcessJson(rpro.intPk,
                                                        rpro.strText);
                                                }
                                                ).ToList(); 

            //                                              //Create process's notes json to send back.
            PronotesjsonProcessNotesJson pronotesjson = new PronotesjsonProcessNotesJson(strProcessName, intnPreviousJobId,
                intnPkWorkflow, strJobName, darrnoteprojson.ToArray());

            return pronotesjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static List<JobjsonJobJson> darrjobjsonOfAPrintshopFromWisnet(
            //                                              //Get all printshop's jobs.

            //                                              //PrintshopId
            String strPrintshopId_I,
            //                                              //Configuration.
            IConfiguration configuration_I
            )
        {
            List<JobjsonJobJson> darrjobjsonFromWisnet = null;

            //                                              //Get Jobs from Wisnet. 
            Task<List<JobjsonJobJson>> Task_darrjobjsonFromWisnet = HttpTools<JobjsonJobJson>.GetListAsyncToEndPoint(
                  configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                  "/PrintshopData/printshopJobs/" + strPrintshopId_I);
            Task_darrjobjsonFromWisnet.Wait();
            if (
                Task_darrjobjsonFromWisnet.Result != null
                )
            {
                darrjobjsonFromWisnet = Task_darrjobjsonFromWisnet.Result;
            }

            return darrjobjsonFromWisnet;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subCreateJobAndOrderNumberIntoJobJsonTable(
            //                                              //Generates the numbers for the order and job into jobjson
            //                                              //      table.

            //                                              //OrderId_I is always > 0.
            int intOrderId_I,
            int intJobId_I,
            String strPrice_I,
            String strPrintshopId_I,
            int intOffsetNumber_I,
            //                                              //Jobjson from the database
            List<JobjsonentityJobJsonEntityDB> darrjobjsonentityAll_I,
            //                                              //Jobjson to be added to the database
            ref List<JobjsonentityJobJsonEntityDB> darrjobjsonentityOrdersAdded_M
            )
        {
            JobjsonentityJobJsonEntityDB jobjsonentityJobExists = darrjobjsonentityOrdersAdded_M.FirstOrDefault(
                job => job.strPrintshopId == strPrintshopId_I &&
                job.intOrderId == intOrderId_I &&
                job.intJobID == intJobId_I);

            if (
                jobjsonentityJobExists != null
                )
            {
                //                                          //Do nothing
            }
            else
            {
                List<JobjsonentityJobJsonEntityDB> darrjobjsonentityJobsSameOrder = darrjobjsonentityOrdersAdded_M.Where(
                    job => job.strPrintshopId == strPrintshopId_I &&
                    job.intOrderId == intOrderId_I).ToList();

                if (
                    //                                      //Order already exists in DB
                    darrjobjsonentityJobsSameOrder.Count > 0
                    )
                {
                    darrjobjsonentityJobsSameOrder.OrderBy(job => job.intnJobNumber);
                    //                                      //Takes last job.
                    JobjsonentityJobJsonEntityDB jobjsonentity = darrjobjsonentityJobsSameOrder.Last();

                    //                                      //Add to the same order.
                    //                                      //Create new register.
                    JobjsonentityJobJsonEntityDB jobjsonentityToAdd = new JobjsonentityJobJsonEntityDB
                    {
                        intJobID = intJobId_I,
                        strPrintshopId = strPrintshopId_I,
                        intOrderId = intOrderId_I,
                        intnOrderNumber = darrjobjsonentityJobsSameOrder[0].intnOrderNumber,
                        intnJobNumber = jobjsonentity.intnJobNumber + 1,
                        strPrice = strPrice_I
                    };
                    darrjobjsonentityOrdersAdded_M.Add(jobjsonentityToAdd);
                }
                else
                {
                    List<JobjsonentityJobJsonEntityDB> darrjobjsonentityOrders;
                    int intJobNumber = 1;
                    int intMaxOrder = 0;
                    if (
                        //                                  //There are already orders in the database
                        darrjobjsonentityAll_I.Count > 0
                        )
                    {
                        //                                  //Takes order all orders.
                        darrjobjsonentityOrders = darrjobjsonentityAll_I.Where(job =>
                            job.strPrintshopId == strPrintshopId_I &&
                            job.intOrderId == intOrderId_I &&
                            job.intnOrderNumber != null &&
                            job.intnJobNumber != null).ToList();

                        /*CASE*/
                        if (
                            //                              //The order already exists in the database
                            darrjobjsonentityOrders.Count > 0
                            )
                        {
                            darrjobjsonentityOrders.OrderBy(job => job.intnJobNumber);
                            //                              //Takes last job.
                            JobjsonentityJobJsonEntityDB jobjsonentity = darrjobjsonentityOrders.Last();
                            intMaxOrder = (int)jobjsonentity.intnOrderNumber;
                            intJobNumber = (int)jobjsonentity.intnJobNumber + 1;
                        }
                        else if (
                            //                              //The order does not exist yet in the database and
                            //                              //      there are no orders in the list to add
                            darrjobjsonentityOrders.Count == 0 && darrjobjsonentityOrdersAdded_M.Count == 0
                            )
                        {
                            //                              //Get order number from orders in the database
                            darrjobjsonentityOrders = darrjobjsonentityAll_I.Where(job =>
                                job.strPrintshopId == strPrintshopId_I &&
                                job.intnOrderNumber != null &&
                                job.intnJobNumber != null).ToList();

                            intMaxOrder = (darrjobjsonentityOrders.Count > 0) ?
                                ((int)darrjobjsonentityOrders.Max(job => job.intnOrderNumber)) + 1 : intOffsetNumber_I;
                        }
                        else if (
                            //                              //The order does not exist yet in the database but
                            //                              //      there are already orders in the list to add
                            darrjobjsonentityOrders.Count == 0 && darrjobjsonentityOrdersAdded_M.Count > 0
                            )
                        {
                            //                              //Get order number from orders in the list to add
                            darrjobjsonentityOrders = darrjobjsonentityOrdersAdded_M.Where(job =>
                                job.strPrintshopId == strPrintshopId_I &&
                                job.intnOrderNumber != null &&
                                job.intnJobNumber != null).ToList();

                            intMaxOrder = (darrjobjsonentityOrders.Count > 0) ?
                                ((int)darrjobjsonentityOrders.Max(job => job.intnOrderNumber)) + 1 : intOffsetNumber_I;
                        }
                        /*END-CASE*/
                    }
                    else
                    {
                        //                                  //Get order number from orders in the list to add
                        darrjobjsonentityOrders = darrjobjsonentityOrdersAdded_M.Where(job =>
                            job.strPrintshopId == strPrintshopId_I &&
                            job.intnOrderNumber != null &&
                            job.intnJobNumber != null).ToList();

                        intMaxOrder = (darrjobjsonentityOrders.Count > 0) ?
                            ((int)darrjobjsonentityOrders.Max(job => job.intnOrderNumber)) + 1 : intOffsetNumber_I;
                    }

                    //                                      //Add to new order.
                    //                                      //Create new register.
                    JobjsonentityJobJsonEntityDB jobjsonentityToAdd = new JobjsonentityJobJsonEntityDB
                    {
                        intJobID = intJobId_I,
                        strPrintshopId = strPrintshopId_I,
                        intOrderId = intOrderId_I,
                        intnOrderNumber = intMaxOrder,
                        intnJobNumber = intJobNumber,
                        strPrice = strPrice_I
                    };
                    darrjobjsonentityOrdersAdded_M.Add(jobjsonentityToAdd);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String strGetJobNumber(
            //                                              //Get string of the jobnumber.

            int? intnOrderId_I,
            int intJobId_I,
            String strPrintshopId_I,
            Odyssey2Context context_I
            )
        {
            String strJobNumber = "Estimate";
            if (intnOrderId_I != null)
            {
                if (
                intnOrderId_I > 0
                )
                {
                    JobjsonentityJobJsonEntityDB jobjsonentity = context_I.JobJson.FirstOrDefault(
                        job => job.intJobID == intJobId_I && job.intOrderId == intnOrderId_I);

                    strJobNumber = jobjsonentity.intnOrderNumber + " - " + jobjsonentity.intnJobNumber;
                }
            }
            else
            {
                //                                          //Get strJobNumber
                JobjsonentityJobJsonEntityDB jobjsonentity = context_I.JobJson.FirstOrDefault(job =>
                    job.intJobID == intJobId_I && job.strPrintshopId == strPrintshopId_I);

                strJobNumber = jobjsonentity != null ? (jobjsonentity.intnOrderNumber.ToString() + " " + "-" + " " +
                    jobjsonentity.intnJobNumber.ToString()) : "";
            }
            return strJobNumber;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String strGetJobNumber(
            //                                              //Get string of the jobnumber

            int intOrderId_I,
            int intJobId_I,
            List<JobjsonentityJobJsonEntityDB> darrjobjsonentity_I
            )
        {

            String strJobNumber = "Estimate";
            if (
                intOrderId_I > 0
                )
            {
                JobjsonentityJobJsonEntityDB jobjsonentity = darrjobjsonentity_I.FirstOrDefault(
                    job => job.intJobID == intJobId_I && job.intOrderId == intOrderId_I);

                strJobNumber = jobjsonentity.intnOrderNumber + " - " + jobjsonentity.intnJobNumber;
            }


            return strJobNumber;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subDeleteJobFromDB(
            //                                              //Get the progress of a job.

            int intJobId_I,
            int intPkPrintshop_I
            )
        {
            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get job to delete.
            JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job => job.intJobID == intJobId_I &&
                job.intPkPrintshop == intPkPrintshop_I);

            //                                              //Before to delete the job from job's table, we have to 
            //                                              //      delete the reference to FinalCost table.

            FnlcostentityFinalCostEntityDB fnlcostentity = context.FinalCost.FirstOrDefault(fc =>
                fc.intPkJob == jobentity.intPk);

            if (
                fnlcostentity != null
                )
            {
                context.FinalCost.Remove(fnlcostentity);
            }

            //                                              //Delete register from jobJsonTable
            JobjsonentityJobJsonEntityDB jobjsonentity = context.JobJson.FirstOrDefault(job =>
            job.intJobID == jobentity.intJobID);

            if (
                jobjsonentity != null
                )
            {
                context.JobJson.Remove(jobjsonentity);

            }
            context.Job.Remove(jobentity);
            context.SaveChanges();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static int intGetPkProductFromJob(

            int? intnProductKey_I,
            int intPkPrintshop_I,
            Odyssey2Context context_M
            )
        {
            int intPkProduct = 0;

            if (
                intnProductKey_I != null
                )
            {
                if (
                    intnProductKey_I == 0 
                    )
                {
                    //                                          //Find product related to a job and current printshop.
                    EtentityElementTypeEntityDB etentityProduct = context_M.ElementType.FirstOrDefault(et =>
                        et.strCustomTypeId == "Dummy" && et.intPrintshopPk == intPkPrintshop_I);
                    if (
                        etentityProduct != null
                        )
                    {
                        //                                      //Get PkProduct.
                        intPkProduct = etentityProduct.intPk;
                    }
                }
                else
                {
                    //                                          //Find product related to a job and current printshop.
                    EtentityElementTypeEntityDB etentityProduct = context_M.ElementType.FirstOrDefault(et =>
                        et.intWebsiteProductKey == intnProductKey_I && et.intPrintshopPk == intPkPrintshop_I);
                    if (
                        etentityProduct != null
                        )
                    {
                        //                                      //Get PkProduct.
                        intPkProduct = etentityProduct.intPk;
                    }
                }
            }
            return intPkProduct;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subCountJobs(
            //                                              //Calculate number of pending, in progress and completed
            //                                              //      jobs.

            PsPrintShop ps_I,
            int intContactId_I,
            IConfiguration configuration_I,
            out JobandqtysonJobTypeAndQuantityJson[] arrjobquantityjson_O,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Send to Wisnet those invoces not updated when were paid.
            List<InvoInvoiceEntityDB> darrinvoentityNotUpdatedOnWisnetYet = context_M.Invoice.Where(inv =>
                inv.intPkPrintshop == ps_I.intPk && (
                inv.boolnOnWisnet == false || (inv.boolnOnWisnet == null && inv.numOpenBalance == 0)))
                .ToList();

            AccAccounting.subSendToWisnetInvoicesAlreadyPaid(ps_I, darrinvoentityNotUpdatedOnWisnetYet, context_M);

            //                                              //Send to Wisnet Jobs Completed in Odyssey2 not updated yet.
            List<JobentityJobEntityDB> darrjobentityCompletedNotOnWisnetYet = context_M.Job.Where(job =>
                job.intPkPrintshop == ps_I.intPk && job.intStage == JobJob.intCompletedStage &&
                job.boolDeleted == false && (job.boolnOnWisnet == false || job.boolnOnWisnet == null)).ToList();

            foreach (JobentityJobEntityDB jobentity in darrjobentityCompletedNotOnWisnetYet)
            {
                JobJob.subUpdateJobInProgressToCompleteOnWisnet(jobentity, ps_I, intContactId_I, context_M, configuration_I);
            }

            //                                              //Array containing unsubmitted[0], in estimating[1], 
            //                                              //      waiting for approval[2], pending[3], 
            //                                              //      in progress[4], paid[5] and completed[6] jobs.
            arrjobquantityjson_O = new JobandqtysonJobTypeAndQuantityJson[7];

            //                                              //Delete jobs InProgress and Completed that does not exists
            //                                              //      at wisnet anymore.
            //                                              //Delete InProgress.
            JobJob.subDeleteJobsFromDBNotExistInWisnetAnymore(ps_I, true, context_M);

            //                                              //Delete Completed.
            JobJob.subDeleteJobsFromDBNotExistInWisnetAnymore(ps_I, false, context_M);

            //                                              //Update job completed.
            JobJob.subUpdateJobCompleted(ps_I, configuration_I, context_M);

            //                                              //Get all printshop's jobs' ids.
            int[] arrintStagesQuantities = JobJob.arrintGetStagesQuantities(ps_I, configuration_I,
                context_M, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

            //                                              //Unsubmitted.
            JobandqtysonJobTypeAndQuantityJson jobandqtyjsonUnsubmitted = new JobandqtysonJobTypeAndQuantityJson(
                JobJob.strUnsubmittedStage, arrintStagesQuantities[0]);
            arrjobquantityjson_O[0] = jobandqtyjsonUnsubmitted;

            //                                              //In Estimating.
            JobandqtysonJobTypeAndQuantityJson jobandqtyjsonInEstimating = new JobandqtysonJobTypeAndQuantityJson(
                JobJob.strInEstimatingStage, arrintStagesQuantities[1]);
            arrjobquantityjson_O[1] = jobandqtyjsonInEstimating;

            //                                              //Waiting For Price Approval.
            JobandqtysonJobTypeAndQuantityJson jobandqtyjsonWaiting = new JobandqtysonJobTypeAndQuantityJson(
                JobJob.strWaitingForPriceApprovalStage, arrintStagesQuantities[2]);
            arrjobquantityjson_O[2] = jobandqtyjsonWaiting;

            //                                              //Pending.
            JobandqtysonJobTypeAndQuantityJson jobandqtyjsonPending = new JobandqtysonJobTypeAndQuantityJson(
                JobJob.strPendingStage, arrintStagesQuantities[3]);
            arrjobquantityjson_O[3] = jobandqtyjsonPending;

            //                                              //In progress.
            JobandqtysonJobTypeAndQuantityJson jobandqtyjsonInProgress = new JobandqtysonJobTypeAndQuantityJson(
                JobJob.strInProgressStage, arrintStagesQuantities[4]);
            arrjobquantityjson_O[4] = jobandqtyjsonInProgress;

            //                                              //Completed.
            JobandqtysonJobTypeAndQuantityJson jobandqtyjsonCompleted = new JobandqtysonJobTypeAndQuantityJson(
                JobJob.strCompletedStage, arrintStagesQuantities[5]);
            arrjobquantityjson_O[5] = jobandqtyjsonCompleted;

            //                                              //Not Paid.
            JobandqtysonJobTypeAndQuantityJson jobandqtyjsonNotPaid = new JobandqtysonJobTypeAndQuantityJson(
                JobJob.strNotPaidStage, arrintStagesQuantities[6]);
            arrjobquantityjson_O[6] = jobandqtyjsonNotPaid;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subUpdateJobCompleted(
            //                                              //Verified and update if there are a JobInProgress that
            //                                              //    will be update to completed.

            PsPrintShop ps_I,
            IConfiguration configuration_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Json that will be sent to wisnet to get the jobs completed.
            StagesjsonStagesJsonInternal stagesjson = new StagesjsonStagesJsonInternal(ps_I.strPrintshopId.ParseToInt(),
                null, null, null, null, null, true, null, null);

            //                                              //Get jobs from Wisnet. 
            Task<List<JobbasicinfojsonJobBasicInfoJson>> Task_darrjobbasicinfojson = HttpTools<
                JobbasicinfojsonJobBasicInfoJson>.GetListAsyncWithBodyToEndPoint(stagesjson,
                configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                "/PrintshopData/printshopJobsToFillList/");
            Task_darrjobbasicinfojson.Wait();

            if (
                Task_darrjobbasicinfojson.Result != null
                )
            {
                List<JobbasicinfojsonJobBasicInfoJson> darrjobbasicCompleted = Task_darrjobbasicinfojson.Result;

                //                                          //Jobs InProgress in Odyssey2.
                List<JobentityJobEntityDB> darrjobentityInProgress = context_M.Job.Where(job =>
                    job.intPkPrintshop == ps_I.intPk && job.intStage == JobJob.intInProgressStage
                     && job.boolDeleted == false).ToList();

                foreach (JobentityJobEntityDB jobentityInProgress in darrjobentityInProgress)
                {
                    if (
                        //                                  //Job was completed in wisnet.
                        darrjobbasicCompleted.Exists(jobbasic => jobbasic.intJobId == jobentityInProgress.intJobID)
                        )
                    {
                        //                                  //Update the job in progress to job completed.
                        jobentityInProgress.intStage = JobJob.intCompletedStage;
                        jobentityInProgress.strEndDate = Date.Now(ZonedTimeTools.timezone).ToString();
                        jobentityInProgress.strEndTime = Time.Now(ZonedTimeTools.timezone).ToString();
                        context_M.Job.Update(jobentityInProgress);
                        context_M.SaveChanges();
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static int[] arrintGetStagesQuantities(
            //                                              //Return an array with the quantities of the different 
            //                                              //      stages.
            //                                              //Example:
            //                                              //  [
            //                                              //      56(unsubmitted),
            //                                              //      12(in estimating),
            //                                              //      18(waiting for price approval),
            //                                              //      25(pending),
            //                                              //      67(in progress),
            //                                              //      33(completed),
            //                                              //      09(paid)
            //                                              //  ]
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            Odyssey2Context context_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            int[] arrintStagesQuantities = new int[7];

            //                                              //Get Jobs quantities from Wisnet. 
            Task<JobsqtyjsonJobsQuantitiesJson> Task_jobsqtyjsonFromWisnet = HttpTools<JobsqtyjsonJobsQuantitiesJson>.
                GetOneAsyncToEndPoint(configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                  "/PrintshopData/getjobsquantities/" + ps_I.strPrintshopId);
            Task_jobsqtyjsonFromWisnet.Wait();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet database connection lost.";
            if (
                Task_jobsqtyjsonFromWisnet.Result != null
                )
            {
                int[] arrintStagesQuantitiesFromWisnet = Task_jobsqtyjsonFromWisnet.Result.arrintStages;

                int intJobsInProgress = context_I.Job.Where(job => job.intStage == JobJob.intInProgressStage &&
                    job.intPkPrintshop == ps_I.intPk &&
                    job.boolDeleted == false).ToList().Count;
                int intJobsCompleted = context_I.Job.Where(job => job.intStage == JobJob.intCompletedStage &&
                    job.intPkPrintshop == ps_I.intPk &&
                    job.boolDeleted == false).ToList().Count;

                //                                          //Get completed that have not been sent OnWisnet.
                int intCountNotUpdatedOnWisnet = context_I.Job.Where(job => job.intPkPrintshop == ps_I.intPk &&
                    job.intStage == intCompletedStage &&
                    job.boolnOnWisnet == false).Count();

                //                                          //Get completed-paid that have not been sent OnWisnet.
                int intCountPaidNotUpdatedOnWisnet = 0;
                //                                          //List of Invoices paid not updates on Wisnet
                List<InvoInvoiceEntityDB> darrinvoentityNotUpdatedOnWisnet = context_I.Invoice.Where(invoice =>
                    invoice.intPkPrintshop == ps_I.intPk && invoice.boolnOnWisnet == false).ToList();

                foreach (InvoInvoiceEntityDB invoentity in darrinvoentityNotUpdatedOnWisnet)
                {
                    int intCountPaidNotUpdatedOnWisnetThisInvoice =
                        context_I.JobJson.Where(jobjson => jobjson.intOrderId == invoentity.intOrderNumber).ToList().Count();

                    intCountPaidNotUpdatedOnWisnet = intCountPaidNotUpdatedOnWisnet +
                        intCountPaidNotUpdatedOnWisnetThisInvoice;
                }

                arrintStagesQuantities[0] = arrintStagesQuantitiesFromWisnet[0];
                arrintStagesQuantities[1] = arrintStagesQuantitiesFromWisnet[1];
                arrintStagesQuantities[2] = arrintStagesQuantitiesFromWisnet[2];
                arrintStagesQuantities[3] = arrintStagesQuantitiesFromWisnet[3] - intJobsInProgress;
                arrintStagesQuantities[4] = intJobsInProgress;
                arrintStagesQuantities[5] = arrintStagesQuantitiesFromWisnet[4] + intCountNotUpdatedOnWisnet;
                arrintStagesQuantities[6] = arrintStagesQuantitiesFromWisnet[5] - intCountPaidNotUpdatedOnWisnet;

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            return arrintStagesQuantities;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subCountEstimates(
            //                                              //Calculate number of pending, in progress and completed
            //                                              //      jobs.

            PsPrintShop ps_I,
            IConfiguration configuration_I,
            out JobandqtysonJobTypeAndQuantityJson[] arrestquantityjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Create the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Send to wisnet those invoces not updated when were paid.
            List<InvoInvoiceEntityDB> darrinvoentity = context.Invoice.Where(inv => inv.intPkPrintshop == ps_I.intPk &&
                inv.boolnOnWisnet == false).ToList();
            AccAccounting.subSendToWisnetInvoicesAlreadyPaid(ps_I, darrinvoentity, context);

            //                                              //Array containing requested[0], waiting for response[1], 
            //                                              //      rejected[2].
            arrestquantityjson_O = new JobandqtysonJobTypeAndQuantityJson[3];

            //                                              //Get all printshop's jobs' ids.
            int[] arrintGetEstStagesQuantities = JobJob.arrintGetEstStagesQuantities(ps_I, configuration_I,
                ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

            //                                              //Requested.
            JobandqtysonJobTypeAndQuantityJson jobandqtyjsonEstRequested = new JobandqtysonJobTypeAndQuantityJson(
                JobJob.strRequestedEstimates, arrintGetEstStagesQuantities[0]);
            arrestquantityjson_O[0] = jobandqtyjsonEstRequested;

            //                                              //Waiting for response.
            JobandqtysonJobTypeAndQuantityJson jobandqtyjsonEstWaiting = new JobandqtysonJobTypeAndQuantityJson(
                JobJob.strWaitingForResponseEstimates, arrintGetEstStagesQuantities[1]);
            arrestquantityjson_O[1] = jobandqtyjsonEstWaiting;

            //                                              //Rejected.
            JobandqtysonJobTypeAndQuantityJson jobandqtyjsonEstRejected = new JobandqtysonJobTypeAndQuantityJson(
                JobJob.strRejectedEstimates, arrintGetEstStagesQuantities[2]);
            arrestquantityjson_O[2] = jobandqtyjsonEstRejected;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static int[] arrintGetEstStagesQuantities(
            //                                              //Return an array with the quantities of the different 
            //                                              //      estimates's stages.
            //                                              //Example:
            //                                              //  [
            //                                              //      56(requested),
            //                                              //      12(waiting for response),
            //                                              //      18(rejected)
            //                                              //  ]
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            int[] arrintGetEstStagesQuantities = new int[3];

            //                                              //Get Jobs quantities from Wisnet. 
            Task<JobsqtyjsonJobsQuantitiesJson> Task_jobsqtyjsonFromWisnet = HttpTools<JobsqtyjsonJobsQuantitiesJson>.
                GetOneAsyncToEndPoint(configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                  "/Estimates/getestimatesquantities/" + ps_I.strPrintshopId);
            Task_jobsqtyjsonFromWisnet.Wait();

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Wisnet database connection lost.";
            if (
                Task_jobsqtyjsonFromWisnet.Result != null
                )
            {
                int[] arrintEstStagesQuantitiesFromWisnet = Task_jobsqtyjsonFromWisnet.Result.arrintStages;

                arrintGetEstStagesQuantities[0] = arrintEstStagesQuantitiesFromWisnet[0];
                arrintGetEstStagesQuantities[1] = arrintEstStagesQuantitiesFromWisnet[1];
                arrintGetEstStagesQuantities[2] = arrintEstStagesQuantitiesFromWisnet[2];

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            return arrintGetEstStagesQuantities;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetOptionsForEstimate(
            String strPrintshopId_I,
            int intJobId_I,
            int intId_I,
            int intPkWorkflow_I,
            //                                              //Size is equals the number of resource groups in 
            //                                              //      the workflow. Each resestimjson represents one 
            //                                              //      selected resource in the view options module.
            List<ResestimjsonResourceEstimatedJson> darrresestimjsonResourceSelected_I,
            String strBaseDate_I,
            String strBaseTime_I,
            IConfiguration configuration_I,
            out EstimopjsonEstimationOptionsJson estimopjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            estimopjson_O = null;

            intStatus_IO = 401;
            JobjsonJobJson jobjson;
            if (
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                ProdtypProductType prodtyp = ProdtypProductType.prodtypGetFromDB((int)jobjson.intnProductKey,
                    strPrintshopId_I);

                Odyssey2Context context = new Odyssey2Context();

                WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                    wf.intPk == intPkWorkflow_I && wf.intnPkProduct == prodtyp.intPk);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No workflow found for the job.";
                if (
                    wfentity != null
                    )
                {
                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Date is not in the correct format.";
                    if (
                        //                                  //Validate date.
                        (strBaseDate_I != null && strBaseTime_I != null) &&
                        (strBaseDate_I.IsParsableToDate()) &&
                        (strBaseTime_I.IsParsableToTime())
                        )
                    {
                        //                                  //Create Ztime Object we receive.
                        ZonedTime ztimeBaseDate = ZonedTimeTools.NewZonedTime(strBaseDate_I.ParseToDate(),
                            strBaseTime_I.ParseToTime());

                        intStatus_IO = 404;
                        strUserMessage_IO = "Select valid date or time.";
                        strDevMessage_IO = "Date is in the past.";
                        if (
                            ztimeBaseDate >= ZonedTimeTools.ztimeNow
                            )
                        {
                            //                              //Get Piw's list.

                            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity;
                            List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                            ProdtypProductType.subGetWorkflowValidWay(intPkWorkflow_I, jobjson, out darrpiwentity,
                                out darrdynlkjson);

                            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                            //                              //Get the current cost and the not set ios.
                            double numJobCost = 0.0;
                            List<ResestimjsonResourceEstimatedJson> darrresestimjsonAllIOs;
                            JobJob.subGetIOsWithoutAnyResourceSetted(jobjson, darrpiwentity, out
                                darrresestimjsonAllIOs);

                            List<ResestimjsonResourceEstimatedJson> darrestimjsonFinalSelected = new
                                List<ResestimjsonResourceEstimatedJson>();

                            String strUnit = null;
                            bool boolAllowDecimal = true;
                            if (
                                darrresestimjsonResourceSelected_I.Count == 0
                                )
                            {
                                List<EstdataentityEstimationDataEntityDB> darrestdataentity =
                                    (from estdataentity in context.EstimationData
                                     join piwentity in context.ProcessInWorkflow
                                     on estdataentity.intPkProcessInWorkflow equals piwentity.intPk
                                     where piwentity.intPkWorkflow == intPkWorkflow_I &&
                                     estdataentity.intJobId == intJobId_I && estdataentity.intId == intId_I
                                     select estdataentity).ToList();

                                foreach (EstdataentityEstimationDataEntityDB estdataentity in darrestdataentity)
                                {
                                    int intPkEleetOrEleele = (estdataentity.intnPkElementElement == null) ?
                                        (int)estdataentity.intnPkElementElementType : (int)estdataentity.
                                        intnPkElementElement;
                                    bool boolEleet = (estdataentity.intnPkElementElement == null) ? true : false;

                                    EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(
                                        ele => ele.intPk == estdataentity.intPkResource);

                                    if (
                                        EtElementTypeAbstract.etFromDB(eleentity.intPkElementType).strClassification !=
                                        RestypResourceType.strResourceTypeParameter
                                        )
                                    {
                                        //                  //Get the current unit of measurement.
                                        ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(
                                            eleentity.intPk);

                                        strUnit = valentity != null ? valentity.strValue : null;
                                        boolAllowDecimal = (valentity.boolnIsDecimal == null) ? true :
                                            (bool)valentity.boolnIsDecimal;
                                    }

                                    bool boolIsAvailable = (eleentity.boolnIsCalendar == true) ||
                                        (eleentity.boolnIsAvailable != null && eleentity.boolnIsAvailable == true);

                                    ResestimjsonResourceEstimatedJson resestimjson =
                                        new ResestimjsonResourceEstimatedJson(estdataentity.intPkProcessInWorkflow,
                                        intPkEleetOrEleele, boolEleet, estdataentity.intPkResource, null, null, null,
                                        strUnit, boolIsAvailable);
                                    darrestimjsonFinalSelected.Add(resestimjson);
                                }
                            }
                            else
                            {
                                darrestimjsonFinalSelected.AddRange(darrresestimjsonResourceSelected_I);
                            }

                            List<ResestimjsonResourceEstimatedJson> darrresestimjsonToOptions = new
                                List<ResestimjsonResourceEstimatedJson>();
                            darrresestimjsonToOptions.AddRange(darrresestimjsonAllIOs);

                            //                              //Verify the selected resources.
                            foreach (ResestimjsonResourceEstimatedJson resestimjsonFinalSelected in
                            darrestimjsonFinalSelected)
                            {
                                if (
                                    resestimjsonFinalSelected.intnPk != null
                                    )
                                {
                                    ResestimjsonResourceEstimatedJson resestimjsonSelectedFound =
                                        darrresestimjsonAllIOs.FirstOrDefault(resestim =>
                                        resestim.intPkProcessInWorkflow ==
                                        resestimjsonFinalSelected.intPkProcessInWorkflow &&
                                        resestim.intPkEleetOrEleele == resestimjsonFinalSelected.intPkEleetOrEleele &&
                                        resestim.boolIsEleet == resestimjsonFinalSelected.boolIsEleet);

                                    if (
                                        resestimjsonSelectedFound != null
                                        )
                                    {
                                        darrresestimjsonToOptions.Remove(resestimjsonSelectedFound);

                                        PiwentityProcessInWorkflowEntityDB piwentity =
                                            context.ProcessInWorkflow.FirstOrDefault(piw =>
                                            piw.intPk == resestimjsonFinalSelected.
                                            intPkProcessInWorkflow);

                                        int? intnPkEleet = null;
                                        int? intnPkEleele = resestimjsonFinalSelected.intPkEleetOrEleele;
                                        if (
                                            resestimjsonFinalSelected.boolIsEleet
                                            )
                                        {
                                            intnPkEleet = resestimjsonFinalSelected.intPkEleetOrEleele;
                                            intnPkEleele = null;
                                        }

                                        ResResource res = ResResource.resFromDB(resestimjsonFinalSelected.intnPk,
                                        false);
                                        EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(
                                            ele => ele.intPk == res.intPk);

                                        if (
                                        EtElementTypeAbstract.etFromDB(eleentity.intPkElementType).strClassification !=
                                        RestypResourceType.strResourceTypeParameter
                                        )
                                        {
                                            //              //Get the current unit of measurement.
                                            ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(
                                                eleentity.intPk);

                                            strUnit = valentity != null ? valentity.strValue : null;
                                        }

                                        resestimjsonSelectedFound.intnPk = res.intPk;
                                        resestimjsonSelectedFound.strName = res.strName;
                                        resestimjsonSelectedFound.strUnit = strUnit;
                                    }
                                }
                            }

                            //                              //Get the resources for the ios.
                            object[,] arr2objIOwithResources = new object[darrresestimjsonToOptions.Count, 2];
                            for (int intI = 0; intI < darrresestimjsonToOptions.Count; intI = intI + 1)
                            {
                                ResestimjsonResourceEstimatedJson resestimjson = darrresestimjsonToOptions[intI];

                                List<ResestimjsonResourceEstimatedJson> darrresestimjsonPosibilities;
                                JobJob.subGetPossibleResourcesForIO(resestimjson, jobjson, prodtyp, intPkWorkflow_I,
                                    darrpiwentity, darrdynlkjson, configuration_I, out darrresestimjsonPosibilities);

                                //                          //Set the info the current IO.
                                arr2objIOwithResources[intI, 0] = resestimjson;

                                //                          //Set the resources posible of the grpResources for 
                                //                          //     the current IO.
                                arr2objIOwithResources[intI, 1] = darrresestimjsonPosibilities;
                            }

                            //                              //Get combinations.
                            List<CombestimjsonCombinationEstimatedJson> darrcombestimjson = new
                                List<CombestimjsonCombinationEstimatedJson>();
                            List<ResestimjsonResourceEstimatedJson> darrresestimjson = new
                                List<ResestimjsonResourceEstimatedJson>();
                            JobJob.subGetCombinations(arr2objIOwithResources, 0, 0, darrestimjsonFinalSelected,
                                numJobCost, ztimeBaseDate, ref darrresestimjson, ref darrcombestimjson);
                            darrcombestimjson.Sort();

                            //                              //Take each combination.
                            foreach (CombestimjsonCombinationEstimatedJson comb in darrcombestimjson)
                            {
                                //                          //Take each process of the current combination.
                                foreach (ProestimjsonProcessEstimatedJson proestim in comb.arrpro)
                                {

                                    //                      //Get Resestim of the current process.
                                    List<ResestimjsonResourceEstimatedJson> darrresestiFromProEstim =
                                        proestim.arrres.ToList();

                                    //                      //Create list resestim unike.
                                    List<ResestimjsonResourceEstimatedJson> arrresestimUnike =
                                        new List<ResestimjsonResourceEstimatedJson>();

                                    //                      //Take resestim of the current process.
                                    foreach (ResestimjsonResourceEstimatedJson resesstimFromProEstim in
                                    darrresestiFromProEstim)
                                    {
                                        //                  //Per each resestim of the current process, 
                                        //                  //      it is necesary create the resestim reference Unike
                                        //                  //      for after we can work with references a manipulate 
                                        //                  //      references.
                                        ResestimjsonResourceEstimatedJson resestimUnikeRef =
                                        new ResestimjsonResourceEstimatedJson
                                        (resesstimFromProEstim.intPkProcessInWorkflow,
                                        resesstimFromProEstim.intPkEleetOrEleele, resesstimFromProEstim.boolIsEleet,
                                        resesstimFromProEstim.intnPk,
                                        resesstimFromProEstim.strName, resesstimFromProEstim.numnCost,
                                        resesstimFromProEstim.numnQuantity, resesstimFromProEstim.strUnit,
                                        resesstimFromProEstim.boolIsAvailable);

                                        arrresestimUnike.Add(resestimUnikeRef);
                                    }

                                    //                      //Update the list resestim of the current process.
                                    proestim.arrres = arrresestimUnike.ToList();
                                }
                            }

                            intStatus_IO = 405;
                            strUserMessage_IO = "Many workflow possibilities found.";
                            strDevMessage_IO = "";
                            if (
                                darrpiwentity != null
                                )
                            {
                                //                          //Dictionary to store inputs and outputs of a process.
                                prodtyp.dicProcessIOs = new Dictionary<int, List<Iofrmpiwjson2IOFromPIWJson2>>();
                                //                                              //Get job stage.
                                JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job =>
                                    job.intJobID == jobjson.intJobId);

                                //                          //List of normal piw.
                                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityNormalProcess =
                                    darrpiwentity.Where(piw =>
                                    piw.boolIsPostProcess == false).ToList();

                                //                          //List of post piw.
                                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityPostProcess =
                                    darrpiwentity.Where(piw =>
                                    piw.boolIsPostProcess == true).ToList();

                                //                          //For each combination, find the delivery date.
                                for (int intA = 0; intA < darrcombestimjson.Count; intA++)
                                {
                                    CombestimjsonCombinationEstimatedJson combestimjson = darrcombestimjson[intA];

                                    List<Piwjson1ProcessInWorkflowJson1> darrpiwjson1 =
                                        new List<Piwjson1ProcessInWorkflowJson1>();

                                    //                      //Get the cost and quantity for the combination.
                                    JobJob.subGetCostAndQuantity(intPkWorkflow_I, jobentity, jobjson, prodtyp, ps,
                                        darrdynlkjson, darrpiwentity, darrpiwentityNormalProcess,
                                        darrpiwentityPostProcess, darrresestimjsonAllIOs,
                                        darrresestimjsonResourceSelected_I, configuration_I,
                                        combestimjson, darrpiwjson1);

                                    //                      //Stores each json per combination that will be used
                                    //                      //      to calculate the delivery date.
                                    List<Piwjson2ProcessInWorkflowJson2> darrpiwjson2 =
                                        new List<Piwjson2ProcessInWorkflowJson2>();
                                    foreach (Piwjson1ProcessInWorkflowJson1 piwjson in darrpiwjson1)
                                    {
                                        List<RecbdgjsonResourceBudgetJson> darrrecbdgjson =
                                            new List<RecbdgjsonResourceBudgetJson>();

                                        foreach (Iojson1InputOrOutputJson1 iojson1 in piwjson.arrresortypInput)
                                        {
                                            RecbdgjsonResourceBudgetJson recbdgjson = new RecbdgjsonResourceBudgetJson(
                                                iojson1.intnPkResource, iojson1.strResource, iojson1.intPkEleetOrEleele,
                                                iojson1.boolIsEleet, null, false, iojson1.numQuantity, iojson1.strUnit,
                                                iojson1.numCostByResource, iojson1.boolnIsAvailable,
                                                iojson1.boolnIsCalendar, null, false, iojson1.boolUnitAllowDecimal,
                                                iojson1.boolIsPaper, iojson1.strLink,
                                                iojson1.boolnIsDeviceOrMiscConsumable);

                                            recbdgjson.intHours = iojson1.intHours;
                                            recbdgjson.intMinutes = iojson1.intMinutes;
                                            recbdgjson.intSeconds = iojson1.intSeconds;

                                            darrrecbdgjson.Add(recbdgjson);
                                        }

                                        Piwjson2ProcessInWorkflowJson2 piwjson2 = new Piwjson2ProcessInWorkflowJson2(
                                            piwjson.intPkProcessInWorkflow, darrrecbdgjson.ToArray());
                                        darrpiwjson2.Add(piwjson2);
                                    }

                                    String strDeliveryDate;
                                    String strDeliveryTime;
                                    bool boolAllResourceAreAvailable;
                                    //                      //Method to get DeliveryDate.
                                    JobJob.subDeliveryDateBudgetEstimate(intId_I, jobjson, ps, ztimeBaseDate,
                                        darrpiwentity, darrpiwjson2, configuration_I, out boolAllResourceAreAvailable,
                                        out strDeliveryDate, out strDeliveryTime);

                                    ZonedTime ztimeDeliveryConverted = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                        strDeliveryDate.ParseToDate(), strDeliveryTime.ParseToTime(), ps.strTimeZone);

                                    //                      //Delivery date per resource.
                                    combestimjson.strDeliveryDate = ztimeDeliveryConverted.Date.ToString();
                                    combestimjson.strDeliveryTime = ztimeDeliveryConverted.Time.ToString();

                                }
                            }

                            ZonedTime ztimeBaseDateConverted = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                strBaseDate_I.ParseToDate(), strBaseTime_I.ParseToTime(), ps.strTimeZone);

                            estimopjson_O = new EstimopjsonEstimationOptionsJson(
                                ztimeBaseDateConverted.Date.ToString(), ztimeBaseDateConverted.Time.ToString(),
                                darrresestimjsonAllIOs.ToArray(), darrcombestimjson.ToArray());

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subGetCostAndQuantity(
            //                                              //Get all piw with all ios, get the resources already set.
            //                                              //Set the resources selected for the combination at their
            //                                              //      respective io.
            //                                              //Once all resources are set, get the quantities and cost 
            //                                              //      for every io starting at the final product and get
            //                                              //      the final cost for the combination.

            int intPkWorkflow_I,
            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjson_I,
            ProdtypProductType prodtyp_I,
            PsPrintShop ps_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityInJob_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityNormalProcess_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityPostProcess_I,
            List<ResestimjsonResourceEstimatedJson> darrresestimjsonAllIOs_I,
            List<ResestimjsonResourceEstimatedJson> darrresestimjsonResourceSelected_I,
            IConfiguration configuration_I,
            CombestimjsonCombinationEstimatedJson combestimjson_M,
            //                                              //List to add piws.
            List<Piwjson1ProcessInWorkflowJson1> darrpiwjson1_M
            )
        {
            //                                              //Reset the List IOs for each combinations.
            prodtyp_I.darriojsoninInputsCombinationsAndInputsSelected = new List<IojsoninInputOrOutputJsonInternal>();

            List<PiwjsoninProcessInWorkflowJsonInternal> darrpiwjsonin =
                new List<PiwjsoninProcessInWorkflowJsonInternal>();

            foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentityInJob_I)
            {
                //                                          //Get proestim for the current piw .
                ProestimjsonProcessEstimatedJson proestimjson = combestimjson_M.arrpro.FirstOrDefault(pro =>
                    pro.arrres[0].intPkProcessInWorkflow == piwentity.intPk);

                if (
                    proestimjson != null
                    )
                {
                    //                                      //Get all Ios from this piw that has combination.
                    PiwjsoninProcessInWorkflowJsonInternal piwjsonin = JobJob.piwjsoninGet(piwentity, jobjson_I.intJobId,
                        proestimjson, darrresestimjsonResourceSelected_I);
                    darrpiwjsonin.Add(piwjsonin);

                    //                                      //Prepare the product with the options for each IO with
                    //                                      //      grpResources.
                    prodtyp_I.darriojsoninInputsCombinationsAndInputsSelected.AddRange(piwjsonin.darriojsoninInputs);
                }
            }

            if (
                darrresestimjsonResourceSelected_I != null
                )
            {
                //                                          //Prepare the product with the options selected for each 
                //                                          //    IO with grpResources.
                foreach (ResestimjsonResourceEstimatedJson resestimSelected in darrresestimjsonResourceSelected_I)
                {
                    if (
                        //                                  //Valid that resources exist.
                        resestimSelected.intnPk != null
                        )
                    {
                        //                                  //Create the jsonInternal.
                        IojsoninInputOrOutputJsonInternal iojsoninselected = new IojsoninInputOrOutputJsonInternal(
                            resestimSelected.intPkProcessInWorkflow,
                            (resestimSelected.boolIsEleet ? (int?)resestimSelected.intPkEleetOrEleele : null),
                            (!resestimSelected.boolIsEleet ? (int?)resestimSelected.intPkEleetOrEleele : null));

                        //                                  //Assign the resources.
                        iojsoninselected.intPkResource = (int)resestimSelected.intnPk;

                        //                                  //Prepare the product with the options for each IO with
                        //                                  //      grpResources.
                        prodtyp_I.darriojsoninInputsCombinationsAndInputsSelected.Add(iojsoninselected);
                    }
                }
            }

            //                                              //List to store resource thickness.
            prodtyp_I.darrresthkjsonResThickness = new List<ResthkjsonResourceThicknessJson>();

            //                                              //To acumulate job final cost.
            double numJobFinalCost = 0;
            double numJobExtraCost = 0;

            JobJob.AddNormalProcessAtOptions(jobentity_I, jobjson_I, prodtyp_I, ps_I, darrdynlkjson_I,
                darrpiwentityInJob_I, darrpiwentityNormalProcess_I, configuration_I, darrpiwjson1_M,
                ref numJobExtraCost, ref numJobFinalCost);

            JobJob.AddPostProcessAtOptions(jobentity_I, jobjson_I, prodtyp_I, ps_I, darrdynlkjson_I,
                darrpiwentityInJob_I, darrpiwentityPostProcess_I, configuration_I, darrpiwjson1_M,
                ref numJobExtraCost, ref numJobFinalCost);

            //                                              //By product/workflow info.
            List<CostbycaljsonCostByCalculationJson> darrcostbycaljson;
            bool boolWorkflowJobIsReadyNotUsed = true;
            double numCostByProduct = prodtyp_I.numGetCostByProduct(jobjson_I, ps_I,
                out darrcostbycaljson, ref boolWorkflowJobIsReadyNotUsed);
            numJobFinalCost = numJobFinalCost + numCostByProduct;

            //                                              //Set price, cost and profit for a job.
            //                                              //Get the job's stage.
            double numJobPrice = 0;
            double numJobProfit = 0;
            double numJobFinalProfit = 0;
            double numJobCost = 0;

            ProdtypProductType.subGetJobPriceCostAndProfit(prodtyp_I, jobjson_I, numCostByProduct,
                darrpiwjson1_M, intPkWorkflow_I, ref numJobPrice, ref numJobCost, numJobExtraCost,
                ref numJobProfit, ref numJobFinalCost, ref numJobFinalProfit);

            //                                              //Update cost of the Job.
            combestimjson_M.numJobCost = (numJobCost).Round(2);

            //                                              //Get the cost for the resources of the combination..
            for (int intC = 0; intC < combestimjson_M.arrpro.Length; intC++)
            {
                for (int intD = 0; intD < combestimjson_M.arrpro[intC].arrres.Count; intD++)
                {
                    //                                      //To easy code.
                    int? intnPkEleet = null;
                    int? intnPkEleele = combestimjson_M.arrpro[intC].arrres[intD].intPkEleetOrEleele;
                    if (
                        combestimjson_M.arrpro[intC].arrres[intD].boolIsEleet
                        )
                    {
                        intnPkEleet = combestimjson_M.arrpro[intC].arrres[intD].intPkEleetOrEleele;
                        intnPkEleele = null;
                    }

                    int intPkPIW = combestimjson_M.arrpro[intC].intPkProcessInWorkflow;

                    //                                      //Get info of quantity of the IOs from PIW.
                    Piwjson1ProcessInWorkflowJson1 piwjson = darrpiwjson1_M.FirstOrDefault(io => io.intPkProcessInWorkflow == intPkPIW);

                    //                                      //Join IOInput and IOOutput.
                    List<Iojson1InputOrOutputJson1> darriojson1AllIosFromProcessInWoerkflow = new List<Iojson1InputOrOutputJson1>();

                    darriojson1AllIosFromProcessInWoerkflow.AddRange(piwjson.arrresortypInput.ToList());
                    darriojson1AllIosFromProcessInWoerkflow.AddRange(piwjson.arrresortypOutput.ToList());

                    //                                      //Get the eleetoreleele from the combination.
                    int intPkEleetOrEleele = combestimjson_M.arrpro[intC].arrres[intD].intPkEleetOrEleele;
                    bool boolIsEleet = combestimjson_M.arrpro[intC].arrres[intD].boolIsEleet;

                    //                                      //Update the quantity in resestimjson from iojsonin.
                    combestimjson_M.arrpro[intC].arrres[intD].numnQuantity = darriojson1AllIosFromProcessInWoerkflow.
                        FirstOrDefault(io => io.intPkEleetOrEleele == intPkEleetOrEleele &&
                        io.boolIsEleet == boolIsEleet).numQuantity;

                    //                                      //Update the cost in resestimjson from iojsonin.
                    combestimjson_M.arrpro[intC].arrres[intD].numnCost = darriojson1AllIosFromProcessInWoerkflow.
                        FirstOrDefault(io => io.intPkEleetOrEleele == intPkEleetOrEleele &&
                        io.boolIsEleet == boolIsEleet).numCostByResource;
                }
            }

            //                                              //Update cost and Quantity for resources selected.
            foreach (ResestimjsonResourceEstimatedJson resestimIO in darrresestimjsonAllIOs_I)
            {
                if (
                    resestimIO.intnPk != null
                    )
                {
                    //                                      //Get info of quantity of the IOs from PIW.
                    Piwjson1ProcessInWorkflowJson1 piwjson = darrpiwjson1_M.FirstOrDefault(io =>
                        io.intPkProcessInWorkflow == resestimIO.intPkProcessInWorkflow);

                    //                                      //Join IOInput and IOOutput.
                    List<Iojson1InputOrOutputJson1> darriojson1AllIosFromProcessInWoerkflow = new List<Iojson1InputOrOutputJson1>();

                    darriojson1AllIosFromProcessInWoerkflow.AddRange(piwjson.arrresortypInput.ToList());
                    darriojson1AllIosFromProcessInWoerkflow.AddRange(piwjson.arrresortypOutput.ToList());

                    //                                      //Update the quantity of the resources selected.
                    resestimIO.numnQuantity = darriojson1AllIosFromProcessInWoerkflow.
                        FirstOrDefault(io => io.intPkEleetOrEleele == resestimIO.intPkEleetOrEleele &&
                        io.boolIsEleet == resestimIO.boolIsEleet).numQuantity;

                    //                                      //Update the quantity of the resources selected.
                    resestimIO.numnCost = darriojson1AllIosFromProcessInWoerkflow.
                        FirstOrDefault(io => io.intPkEleetOrEleele == resestimIO.intPkEleetOrEleele &&
                        io.boolIsEleet == resestimIO.boolIsEleet).numCostByResource;
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void AddNormalProcessAtOptions(
            //                                              //Get cost and quantity of the procesess normal 
            //                                              //    considering the options generated.

            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjson_I,
            ProdtypProductType prodtyp_I,
            PsPrintShop ps_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcess_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityNormalProcess_I,
            IConfiguration configuration_I,
            List<Piwjson1ProcessInWorkflowJson1> darrpiwjson1_M,
            //                                              //Cost due to Hourly rate.
            ref double numJobExtraCost_IO,
            ref double numJobFinalCost_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            prodtyp_I.subAddCalculationsBasedOnJobStatus(jobentity_I, darrpiwentityAllProcess_I, context);

            //                                              //List of quantityInputs and quantityOutputs
            //                                              //    for optimization.
            List<IoqytjsonIOQuantityJson> darrioqytjsonIOQuantity = new List<IoqytjsonIOQuantityJson>();

            //                                              //List of waste to propagate.                          
            List<WstpropjsonWasteToPropagateJson> darrwstpropjson = new List<WstpropjsonWasteToPropagateJson>();

            //                                              //Get the inputs and outputs for every process.
            foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentityNormalProcess_I)
            {
                //                                          //The lists are for optimization

                //                                          //Get eleet-s.
                List<EleetentityElementElementTypeEntityDB> darreleetentityAllEleEt =
                    context.ElementElementType.Where(eleet =>
                    eleet.intPkElementDad == piwentity.intPkProcess).ToList();

                //                                          //Get eleele-s.
                List<EleeleentityElementElementEntityDB> darreleeleentityAllEleEle =
                    context.ElementElement.Where(
                    eleele => eleele.intPkElementDad == piwentity.intPkProcess).ToList();

                //                                          //Get io-s.
                List<IoentityInputsAndOutputsEntityDB> darrioentityAllIO =
                    context.InputsAndOutputs.Where(io =>
                    io.intPkWorkflow == piwentity.intPkWorkflow &&
                    io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId).ToList();

                //                                          //Get ioj-s.
                List<IojentityInputsAndOutputsForAJobEntityDB> darriojentityAllIOJ =
                    context.InputsAndOutputsForAJob.Where(ioj =>
                    ioj.intPkProcessInWorkflow == piwentity.intPk &&
                    ioj.intJobId == jobjson_I.intJobId).ToList();

                if (
                    !prodtyp_I.dicProcessIOs.ContainsKey(piwentity.intPk)
                    )
                {
                    List<Iofrmpiwjson2IOFromPIWJson2> darrioinfrmpiwjson2IosFromPIW;
                    ProdtypProductType.subGetProcessInputsAndOutputs(jobjson_I, piwentity, prodtyp_I,
                        darreleeleentityAllEleEle, darreleetentityAllEleEt, out darrioinfrmpiwjson2IosFromPIW);

                    prodtyp_I.dicProcessIOs.Add(piwentity.intPk, darrioinfrmpiwjson2IosFromPIW);
                }

                //                                          //Get the process.
                EleentityElementEntityDB eleentityProcess = context.Element.
                    FirstOrDefault(ele => ele.intPk == piwentity.intPkProcess);

                //                                          //List to Add IO Inputs.
                List<Iojson1InputOrOutputJson1> darriojson1Input = new List<Iojson1InputOrOutputJson1>();

                //                                          //Get the input types.
                darriojson1Input.AddRange(prodtyp_I.arriojson1GetTypesAtOptions(jobentity_I, jobjson_I, piwentity,
                    darrdynlkjson_I, darreleetentityAllEleEt, darrioentityAllIO, darriojentityAllIOJ,
                    darrpiwentityAllProcess_I, darrwstpropjson, darrioqytjsonIOQuantity));

                //                                          //Get the input templates.
                darriojson1Input.AddRange(prodtyp_I.arriojson1GetTemplatesAtOptions(jobentity_I, jobjson_I, piwentity,
                    darrdynlkjson_I, darreleeleentityAllEleEle, darrioentityAllIO, darriojentityAllIOJ,
                    darrpiwentityAllProcess_I, darrwstpropjson, darrioqytjsonIOQuantity));

                IoqytjsonIOQuantityJson ioqytjsonWasPropagate = darrioqytjsonIOQuantity.FirstOrDefault(
                    ioqyt => ioqyt.intPkProcessInWorkflow == piwentity.intPk);

                //                                          //Get index of the current PIW
                int index = Array.IndexOf(darrpiwentityNormalProcess_I.ToArray(), piwentity);

                if (
                    //                                      //This PIW was not analized or is the first PIW.
                    ioqytjsonWasPropagate == null || index == 0
                    )
                {
                    ProdtypProductType.subPropagateWaste(jobjson_I, piwentity, prodtyp_I, darrwstpropjson,
                        configuration_I, ps_I.strPrintshopId, prodtyp_I.darriojsoninInputsCombinationsAndInputsSelected,
                        ref darriojson1Input);
                }

                ProdtypProductType.CalculateTime(jobjson_I, piwentity, configuration_I, ps_I.strPrintshopId,
                     ref darriojson1Input, prodtyp_I.darriojsoninInputsCombinationsAndInputsSelected);

                //                                          //Total extra cost per process.
                //                                          //This variable must be use for inputs and outputs.
                double numProcessExtraCost = 0.0;

                //                                          //Increase cost to input resources that contain an hourly 
                //                                          //      rate.
                ProdtypProductType.subCalculateResourcesHourlyRates(context, ref darriojson1Input,
                    ref numProcessExtraCost);

                //                                          //List to Add IO Outputs.
                List<Iojson1InputOrOutputJson1> darriojson1Output = new List<Iojson1InputOrOutputJson1>();

                //                                          //Each extra cost generated by process will be added to 
                //                                          //      this variable.
                numJobExtraCost_IO = numJobExtraCost_IO + numProcessExtraCost;

                //                                          //By Process data.    

                //                                          //Get the cost By Process.
                List<CostbycaljsonCostByCalculationJson> darrcostbycaljsonPerProcess;
                bool boolWorkflowJobIsReadyNotUsed = false;
                double numCostByProcess = prodtyp_I.numGetCostByProcess(jobjson_I,
                    piwentity.intPkProcess, piwentity.intPk, ps_I, out darrcostbycaljsonPerProcess,
                    ref numJobFinalCost_IO, ref boolWorkflowJobIsReadyNotUsed);

                //                                          //Get Process Name and Id.
                String strProcessNameAndId = piwentity.intnId != null ? strProcessNameAndId =
                    eleentityProcess.strElementName + " (" + piwentity.intnId + ")" :
                    eleentityProcess.strElementName;

                //                                          //Json with all of the information about the process.
                Piwjson1ProcessInWorkflowJson1 piwjson1 = new Piwjson1ProcessInWorkflowJson1(
                    piwentity.intPk, eleentityProcess.intPk, strProcessNameAndId, numCostByProcess,
                    darriojson1Input.ToArray(), darriojson1Output.ToArray(), 0, false,
                    false, piwentity.boolIsPostProcess);

                //                                          //Array with information of all the processes in a 
                //                                          //      workflow.
                darrpiwjson1_M.Add(piwjson1);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void AddPostProcessAtOptions(
            //                                              //Get cost and quantity of the postprocesess  
            //                                              //    considering the options generated.

            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjson_I,
            ProdtypProductType prodtyp_I,
            PsPrintShop ps_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcess_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityPostProcess_I,
            IConfiguration configuration_I,
            List<Piwjson1ProcessInWorkflowJson1> darrpiwjson1_M,
            //                                              //Cost due to Hourly rate.
            ref double numJobExtraCost_IO,
            ref double numJobFinalCost_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //List of quantityInputs and quantityOutputs
            //                                              //    for optimization.
            List<IoqytjsonIOQuantityJson> darrioqytjsonIOQuantity = new List<IoqytjsonIOQuantityJson>();

            //                                              //Get the inputs and outputs for every process.
            foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentityPostProcess_I)
            {
                //                                          //The lists are for optimization

                //                                          //Get eleet-s.
                List<EleetentityElementElementTypeEntityDB> darreleetentityAllEleEt =
                    context.ElementElementType.Where(eleet =>
                    eleet.intPkElementDad == piwentity.intPkProcess).ToList();

                //                                          //Get eleele-s.
                List<EleeleentityElementElementEntityDB> darreleeleentityAllEleEle =
                    context.ElementElement.Where(
                    eleele => eleele.intPkElementDad == piwentity.intPkProcess).ToList();

                //                                          //Get io-s.
                List<IoentityInputsAndOutputsEntityDB> darrioentityAllIO =
                    context.InputsAndOutputs.Where(io =>
                    io.intPkWorkflow == piwentity.intPkWorkflow &&
                    io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId).ToList();

                //                                          //Get ioj-s.
                List<IojentityInputsAndOutputsForAJobEntityDB> darriojentityAllIOJ =
                    context.InputsAndOutputsForAJob.Where(ioj =>
                    ioj.intPkProcessInWorkflow == piwentity.intPk &&
                    ioj.intJobId == jobjson_I.intJobId).ToList();

                if (
                    !prodtyp_I.dicProcessIOs.ContainsKey(piwentity.intPk)
                    )
                {
                    List<Iofrmpiwjson2IOFromPIWJson2> darrioinfrmpiwjson2IosFromPIW;
                    ProdtypProductType.subGetProcessInputsAndOutputs(jobjson_I, piwentity, prodtyp_I,
                        darreleeleentityAllEleEle, darreleetentityAllEleEt, out darrioinfrmpiwjson2IosFromPIW);

                    prodtyp_I.dicProcessIOs.Add(piwentity.intPk, darrioinfrmpiwjson2IosFromPIW);
                }

                //                                          //Get the process.
                EleentityElementEntityDB eleentityProcess = context.Element.
                    FirstOrDefault(ele => ele.intPk == piwentity.intPkProcess);

                //                                          //List to Add IO Inputs.
                List<Iojson1InputOrOutputJson1> darriojson1Input = new List<Iojson1InputOrOutputJson1>();

                //                                          //Get the input types.
                darriojson1Input.AddRange(prodtyp_I.arriojson1GetTypesPostProcessAtOptions(true, jobentity_I, jobjson_I,
                    piwentity, darrdynlkjson_I, darreleetentityAllEleEt, darrioentityAllIO, darriojentityAllIOJ,
                    darrpiwentityAllProcess_I, darrioqytjsonIOQuantity));

                //                                          //Get the input templates.
                darriojson1Input.AddRange(prodtyp_I.arriojson1GetTemplatesPostProcessAtOptions(true, jobentity_I,
                    jobjson_I, piwentity, darrdynlkjson_I, darreleeleentityAllEleEle, darrioentityAllIO,
                    darriojentityAllIOJ, darrpiwentityAllProcess_I, darrioqytjsonIOQuantity));

                ProdtypProductType.CalculateTime(jobjson_I, piwentity, configuration_I, ps_I.strPrintshopId,
                     ref darriojson1Input, prodtyp_I.darriojsoninInputsCombinationsAndInputsSelected);

                //                                          //Total extra cost per process.
                //                                          //This variable must be use for inputs and outputs.
                double numProcessExtraCost = 0.0;

                //                                          //Increase cost to input resources that contain an hourly 
                //                                          //      rate.
                ProdtypProductType.subCalculateResourcesHourlyRates(context, ref darriojson1Input,
                    ref numProcessExtraCost);

                List<Iojson1InputOrOutputJson1> darriojson1Output = new List<Iojson1InputOrOutputJson1>();

                //                                          //Get the input types.
                darriojson1Output.AddRange(prodtyp_I.arriojson1GetTypesPostProcessAtOptions(false, jobentity_I,
                    jobjson_I, piwentity, darrdynlkjson_I, darreleetentityAllEleEt, darrioentityAllIO,
                    darriojentityAllIOJ, darrpiwentityAllProcess_I, darrioqytjsonIOQuantity));

                //                                          //Get the input templates.
                darriojson1Output.AddRange(prodtyp_I.arriojson1GetTemplatesPostProcessAtOptions(false, jobentity_I,
                    jobjson_I, piwentity, darrdynlkjson_I, darreleeleentityAllEleEle, darrioentityAllIO,
                    darriojentityAllIOJ, darrpiwentityAllProcess_I, darrioqytjsonIOQuantity));

                //                                          //Each extra cost generated by process will be added to 
                //                                          //      this variable.
                numJobExtraCost_IO = numJobExtraCost_IO + numProcessExtraCost;

                //                                          //By Process data.    

                //                                          //Get the cost By Process.
                List<CostbycaljsonCostByCalculationJson> darrcostbycaljsonPerProcess;
                bool boolWorkflowJobIsReadyNotUsed = false;
                double numCostByProcess = prodtyp_I.numGetCostByProcess(jobjson_I,
                    piwentity.intPkProcess, piwentity.intPk, ps_I, out darrcostbycaljsonPerProcess,
                    ref numJobFinalCost_IO, ref boolWorkflowJobIsReadyNotUsed);

                //                                          //Get Process Name and Id.
                String strProcessNameAndId = piwentity.intnId != null ? strProcessNameAndId =
                    eleentityProcess.strElementName + " (" + piwentity.intnId + ")" :
                    eleentityProcess.strElementName;

                //                                          //Json with all of the information about the process.
                Piwjson1ProcessInWorkflowJson1 piwjson1 = new Piwjson1ProcessInWorkflowJson1(
                    piwentity.intPk, eleentityProcess.intPk, strProcessNameAndId, numCostByProcess,
                    darriojson1Input.ToArray(), darriojson1Output.ToArray(), 0, false,
                    false, piwentity.boolIsPostProcess);

                //                                          //Array with information of all the processes in a 
                //                                          //      workflow.
                darrpiwjson1_M.Add(piwjson1);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subGetIOsWithoutAnyResourceSetted(
            //                                              //Add it to the array of resestimjson2 if the IO has 
            //                                              //    a grpResources.

            JobjsonJobJson jobjson_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllOfTheJob_I,
            out List<ResestimjsonResourceEstimatedJson> darrresestimjson2_O
            )
        {
            darrresestimjson2_O = new List<ResestimjsonResourceEstimatedJson>();

            Odyssey2Context context = new Odyssey2Context();

            foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentityAllOfTheJob_I)
            {
                //                                          //Get the Eleet inputs.
                List<EleetentityElementElementTypeEntityDB> darreleetentityWithoutFilter =
                    context.ElementElementType.Where(eleet => eleet.intPkElementDad == piwentity.intPkProcess && eleet.boolDeleted == false && eleet.boolUsage == true).ToList();

                //                                          //List of element element types without not physical
                //                                          //      resources.

                //                                          //Clone previous list.
                List<EleetentityElementElementTypeEntityDB> darreleetentity =
                    new List<EleetentityElementElementTypeEntityDB>(darreleetentityWithoutFilter);

                foreach (EleetentityElementElementTypeEntityDB eleet in darreleetentityWithoutFilter)
                {
                    //                                      //Apply a filter in order to avoid not physical
                    //                                      //      resources.

                    //                                      //Find element type.
                    EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                        et.intPk == eleet.intPkElementTypeSon);
                    if (
                        !RestypResourceType.boolIsPhysical(etentity.strClassification)
                        )
                    {
                        darreleetentity.Remove(eleet);
                    }
                }

                foreach (EleetentityElementElementTypeEntityDB eleetentity in darreleetentity)
                {
                    //                                      //Check if there is a resource define in InputsAndOutputs.
                    IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                    io.intPkWorkflow == piwentity.intPkWorkflow &&
                    io.intnPkElementElementType == eleetentity.intPk &&
                    io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId);

                    //                                      //Check if there is a resource define in 
                    //                                      //      InputsAndOutputsForAJob.
                    IojentityInputsAndOutputsForAJobEntityDB iojentity = context.InputsAndOutputsForAJob.
                        FirstOrDefault(ioj => ioj.intPkProcessInWorkflow == piwentity.intPk &&
                        ioj.intnPkElementElementType == eleetentity.intPk && ioj.intJobId ==
                        jobjson_I.intJobId);

                    if (
                        //                                  //Io eleet has a grpResource and the wfJob there is not setted a resource
                        //                                  //    for this IO.
                        iojentity == null && ioentity != null && ioentity.strLink == null &&
                        ioentity.intnPkResource == null && ioentity.intnGroupResourceId != null
                        )
                    {
                        ResestimjsonResourceEstimatedJson resestimjson2 = new ResestimjsonResourceEstimatedJson(
                            piwentity.intPk, eleetentity.intPk, true, null, null, null, null, null, false);
                        darrresestimjson2_O.Add(resestimjson2);
                    }
                }

                //                                          //Get the Eleele inputs.
                List<EleeleentityElementElementEntityDB> darreleeleentityWithoutFilter =
                    context.ElementElement.Where(eleele => eleele.intPkElementDad == piwentity.intPkProcess &&
                    eleele.boolDeleted == false && eleele.boolUsage == true).ToList();

                //                                          //List of element element without not physical
                //                                          //      resources.

                //                                          //Clone previous list.
                List<EleeleentityElementElementEntityDB> darreleeleentity =
                    new List<EleeleentityElementElementEntityDB>(darreleeleentityWithoutFilter);

                foreach (EleeleentityElementElementEntityDB eleele in darreleeleentityWithoutFilter)
                {
                    //                                      //Find resource.
                    EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                        ele.intPk == eleele.intPkElementSon);

                    //                                      //Apply a filter in order to avoid not physical resources.

                    //                                      //Find element type.
                    EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                        et.intPk == eleentity.intPkElementType);
                    if (
                        !RestypResourceType.boolIsPhysical(etentity.strClassification)
                        )
                    {
                        darreleeleentity.Remove(eleele);
                    }
                }

                foreach (EleeleentityElementElementEntityDB eleeleentity in darreleeleentity)
                {
                    //                                      //Check if there is a resource define in InputsAndOutputs.
                    IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                    io.intPkWorkflow == piwentity.intPkWorkflow &&
                    io.intnPkElementElement == eleeleentity.intPk &&
                    io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId);

                    //                                      //Check if there is a resource define in 
                    //                                      //      InputsAndOutputsForAJob.
                    IojentityInputsAndOutputsForAJobEntityDB iojentity = context.InputsAndOutputsForAJob.
                        FirstOrDefault(ioj => ioj.intPkProcessInWorkflow == piwentity.intPk &&
                        ioj.intnPkElementElement == eleeleentity.intPk);


                    if (
                        //                                  //Io eleele has a grpResource and the wfJob there is not setted a resource
                        //                                  //    for this IO.
                        iojentity == null && ioentity != null && ioentity.strLink == null &&
                        ioentity.intnPkResource == null && ioentity.intnGroupResourceId != null
                        )
                    {
                        ResestimjsonResourceEstimatedJson resestimjson2 = new ResestimjsonResourceEstimatedJson(
                            piwentity.intPk, eleeleentity.intPk, false, null, null, null, null, null, false);
                        darrresestimjson2_O.Add(resestimjson2);
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subGetPossibleResourcesForIO(
            ResestimjsonResourceEstimatedJson resestimjson2_I,
            JobjsonJobJson jobjson_I,
            ProdtypProductType prodtyp_I,
            int intPkWorkflow_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            IConfiguration configuration_I,
            out List<ResestimjsonResourceEstimatedJson> darrresestimjson2Posibilities_O
            )
        {
            darrresestimjson2Posibilities_O = new List<ResestimjsonResourceEstimatedJson>();

            Odyssey2Context context = new Odyssey2Context();
            int? intnPkEleet = null;
            int? intnPkEleele = resestimjson2_I.intPkEleetOrEleele;
            if (
                resestimjson2_I.boolIsEleet
                )
            {
                intnPkEleet = resestimjson2_I.intPkEleetOrEleele;
                intnPkEleele = null;
            }

            PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
            piw.intPk == resestimjson2_I.intPkProcessInWorkflow);

            IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
            io.intPkWorkflow == intPkWorkflow_I && io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
            io.intnPkElementElementType == intnPkEleet && io.intnPkElementElement == intnPkEleele);

            List<GpresentityGroupResourceEntityDB> darrgpresentity = context.GroupResource.Where(gp =>
            gp.intId == ioentity.intnGroupResourceId && gp.intPkWorkflow == intPkWorkflow_I).ToList();

            bool boolAllowDecimal = true;
            foreach (GpresentityGroupResourceEntityDB gpresentity in darrgpresentity)
            {
                EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(
                    ele => ele.intPk == gpresentity.intPkResource);

                if (
                    EtElementTypeAbstract.etFromDB(eleentity.intPkElementType).strClassification !=
                    RestypResourceType.strResourceTypeParameter
                    )
                {
                    //                              //Get the current unit of measurement.
                    ValentityValueEntityDB valentity = ResResource.GetResourceUnitOfMeasurement(
                        eleentity.intPk);

                    boolAllowDecimal = (valentity.boolnIsDecimal == null) ? true :
                                (bool)valentity.boolnIsDecimal;
                }

                String strUnit = ProdtypProductType.strUnitFromEleentityResource(eleentity);

                bool boolIsAvailable = (eleentity.boolnIsCalendar == true) ||
                    (eleentity.boolnIsAvailable != null && eleentity.boolnIsAvailable == true);

                ResestimjsonResourceEstimatedJson resestimjson2 = new ResestimjsonResourceEstimatedJson(
                    resestimjson2_I.intPkProcessInWorkflow, resestimjson2_I.intPkEleetOrEleele,
                    resestimjson2_I.boolIsEleet, eleentity.intPk, eleentity.strElementName, 0, 0,
                    strUnit, boolIsAvailable);

                darrresestimjson2Posibilities_O.Add(resestimjson2);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subGetCombinations(
            //                                              //Get combinations for each IO with grp of the resources.
            //                                              //This method it is called for each IO with grpResources.

            //                                              //Obj contains the IO(eleetoreleele) and resources from 
            //                                              //      the group for this IO.
            //                                              //[i, j] i contains the IO(eleetoreleele)
            //                                              //[i, j] j contains the resources from resourceGroup 
            //                                              //      for this IO(i)
            object[,] arr2objIOwithResources_I,
            int intRow_I,
            int intResestimjson2Index_I,
            List<ResestimjsonResourceEstimatedJson> darrresestimjson2Selected_I,
            double numJobCost_I,
            ZonedTime ztimeBaseDate_I,
            //                                              //List that contain resources from the grpResources of each IO.
            ref List<ResestimjsonResourceEstimatedJson> darrresrestimjson2ForCombination_M,
            ref List<CombestimjsonCombinationEstimatedJson> darrcombestimjson_M
            )
        {
            if (
                //                                          //It is the last IO.
                (intRow_I + 1) == arr2objIOwithResources_I.GetLength(0)
                )
            {
                List<ResestimjsonResourceEstimatedJson> darrresestimjson2 =
                    (List<ResestimjsonResourceEstimatedJson>)arr2objIOwithResources_I[intRow_I, 1];

                //                                          //Generate combination with all grpResources from all IOs.
                for (int intJ = 0; intJ < darrresestimjson2.Count; intJ = intJ + 1)
                {
                    //                                      //Join all resources(grpResources) per each IO.
                    List<ResestimjsonResourceEstimatedJson> darrresestimjsonAll =
                        new List<ResestimjsonResourceEstimatedJson>();
                    darrresestimjsonAll.AddRange(darrresrestimjson2ForCombination_M);

                    //                                      //Join the resources(grpResources) of the current IO
                    darrresestimjsonAll.Add(darrresestimjson2[intJ]);

                    //                                      //proestim contain the info process with your resources.
                    List<ProestimjsonProcessEstimatedJson> darrproestimjson = new
                        List<ProestimjsonProcessEstimatedJson>();
                    double numCostResourcesOfTheCombination = 0.0;

                    //                                      //take each resource from the list resources.
                    foreach (ResestimjsonResourceEstimatedJson resestimjson2 in darrresestimjsonAll)
                    {
                        ProestimjsonProcessEstimatedJson proestimjsonForThisResource = darrproestimjson.
                            FirstOrDefault(pro =>
                            pro.intPkProcessInWorkflow == resestimjson2.intPkProcessInWorkflow);

                        /*CASE*/
                        if (
                            //                              //the list proestim not it is void.
                            (darrproestimjson.Count() == 0) || (proestimjsonForThisResource == null)
                            )
                        {
                            List<ResestimjsonResourceEstimatedJson> darrrestimjson2ForProcess = new
                                List<ResestimjsonResourceEstimatedJson>();
                            darrrestimjson2ForProcess.Add(resestimjson2);

                            Odyssey2Context context = new Odyssey2Context();

                            PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(
                                piw => piw.intPk == resestimjson2.intPkProcessInWorkflow);

                            //                              //Get ProProcess.
                            ProProcess pro = ProProcess.proFromDB(piwentity.intPkProcess);

                            //                              //Get the name of the process.
                            String strName = pro.strName;
                            strName = (piwentity.intnId != null) ? (strName + piwentity.intnId) : strName;

                            //                              //Create proEstim.
                            ProestimjsonProcessEstimatedJson proestimjson = new ProestimjsonProcessEstimatedJson(
                                strName, piwentity.intPk, darrrestimjson2ForProcess);
                            darrproestimjson.Add(proestimjson);
                        }
                        else if (
                            proestimjsonForThisResource != null
                            )
                        {
                            proestimjsonForThisResource.arrres.Add(resestimjson2);
                        }
                        /*END-CASE*/

                        numCostResourcesOfTheCombination = numCostResourcesOfTheCombination +
                            (double)resestimjson2.numnCost;
                    }

                    CombestimjsonCombinationEstimatedJson combestimjson = new CombestimjsonCombinationEstimatedJson(
                        numJobCost_I + numCostResourcesOfTheCombination, darrproestimjson.ToArray(), "", "");
                    darrcombestimjson_M.Add(combestimjson);
                }
            }
            else
            {
                if (
                    //                                      //take each IO from objIOwithResource recursively.
                    intRow_I + 1 < arr2objIOwithResources_I.GetLength(0)
                    )
                {
                    //                                      //For this current IO, take all resource from this grpResource of this 
                    //                                      //    current IO.
                    List<ResestimjsonResourceEstimatedJson> darrresestimjson2 =
                        (List<ResestimjsonResourceEstimatedJson>)arr2objIOwithResources_I[intRow_I, 1];

                    //                                      //Important. Create references unike for each resestimjson
                    //                                      //    beacuase after it work with references.

                    int intIndex = intResestimjson2Index_I;
                    /*UNTIL-DO*/
                    while (!(
                        intIndex >= darrresestimjson2.Count
                        ))
                    {
                        if (
                            //                              //It is the first IO.
                            intRow_I == 0
                            )
                        {
                            darrresrestimjson2ForCombination_M = new List<ResestimjsonResourceEstimatedJson>();
                        }
                        else if (
                            intIndex > 0
                            )
                        {
                            darrresrestimjson2ForCombination_M.Remove(darrresestimjson2[intIndex - 1]);
                        }
                        darrresrestimjson2ForCombination_M.Add(darrresestimjson2[intIndex]);

                        JobJob.subGetCombinations(arr2objIOwithResources_I, intRow_I + 1, 0, darrresestimjson2Selected_I,
                            numJobCost_I, ztimeBaseDate_I, ref darrresrestimjson2ForCombination_M,
                            ref darrcombestimjson_M);

                        intIndex = intIndex + 1;
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static PiwjsoninProcessInWorkflowJsonInternal piwjsoninGet(
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            int intJobId_I,
            ProestimjsonProcessEstimatedJson proestimjson_I,
            List<ResestimjsonResourceEstimatedJson> darrresestimjsonResourceSelected_I
            )
        {
            List<IojsoninInputOrOutputJsonInternal> darriojsoninInputs = new
                    List<IojsoninInputOrOutputJsonInternal>();
            List<IojsoninInputOrOutputJsonInternal> darriojsoninOutputs = new
                List<IojsoninInputOrOutputJsonInternal>();

            //                                              //Add ios that are eleet.
            JobJob.subAddElementElementTypeIos(piwentity_I.intPk, piwentity_I.intPkProcess, ref darriojsoninInputs,
                ref darriojsoninOutputs);

            //                                              //Add ios that are eleele.
            JobJob.subAddElementElementIos(piwentity_I.intPk, piwentity_I.intPkProcess, ref darriojsoninInputs,
                ref darriojsoninOutputs);

            //                                              //Get the arres from combination.
            List<ResestimjsonResourceEstimatedJson> darresestim = new List<ResestimjsonResourceEstimatedJson>();
            if (
                proestimjson_I != null
                )
            {
                darresestim = proestimjson_I.arrres;
            }

            //                                              //Add resources set to the inputs.
            JobJob.subAddResources(intJobId_I, piwentity_I.intPkWorkflow, piwentity_I.intPk,
                piwentity_I.intProcessInWorkflowId, true, darresestim, darrresestimjsonResourceSelected_I,
                ref darriojsoninInputs);

            //                                              //Add resources set to the outputs.
            JobJob.subAddResources(intJobId_I, piwentity_I.intPkWorkflow, piwentity_I.intPk,
                piwentity_I.intProcessInWorkflowId, false, darresestim, darrresestimjsonResourceSelected_I,
                ref darriojsoninOutputs);

            PiwjsoninProcessInWorkflowJsonInternal piwjsonin = new PiwjsoninProcessInWorkflowJsonInternal(
                piwentity_I.intPk, piwentity_I.intPkWorkflow, piwentity_I.boolIsPostProcess);
            piwjsonin.darriojsoninInputs = darriojsoninInputs;
            piwjsonin.darriojsoninOutputs = darriojsoninOutputs;

            return piwjsonin;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddElementElementTypeIos(
            int intPkPIW_I,
            int intPkProcess_I,
            ref List<IojsoninInputOrOutputJsonInternal> darriojsoninInputs_M,
            ref List<IojsoninInputOrOutputJsonInternal> darriojsoninOutputs_M
            )
        {
            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the ios that are eleet.
            List<EleetentityElementElementTypeEntityDB> darreleetentity = context.ElementElementType.Where(io =>
            io.intPkElementDad == intPkProcess_I).ToList();

            //                                              //Divide between inputs and outputs.
            foreach (EleetentityElementElementTypeEntityDB eleetentity in darreleetentity)
            {
                IojsoninInputOrOutputJsonInternal iojsonin = new IojsoninInputOrOutputJsonInternal(
                    intPkPIW_I, eleetentity.intPk, null);
                if (
                    //                                      //Input.
                    eleetentity.boolUsage
                    )
                {
                    darriojsoninInputs_M.Add(iojsonin);
                }
                else
                {
                    darriojsoninOutputs_M.Add(iojsonin);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddElementElementIos(
            int intPkPIW_I,
            int intPkProcess_I,
            ref List<IojsoninInputOrOutputJsonInternal> darriojsoninInputs_M,
            ref List<IojsoninInputOrOutputJsonInternal> darriojsoninOutputs_M
            )
        {
            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                          //Get the ios that are eleet.
            List<EleeleentityElementElementEntityDB> darreleeleentity = context.ElementElement.Where(io =>
            io.intPkElementDad == intPkProcess_I).ToList();

            //                                          //Divide between inputs and outputs.
            foreach (EleeleentityElementElementEntityDB eleeleentity in darreleeleentity)
            {
                IojsoninInputOrOutputJsonInternal iojsonin = new IojsoninInputOrOutputJsonInternal(
                    intPkPIW_I, null, eleeleentity.intPk);
                if (
                    //                                  //Input.
                    eleeleentity.boolUsage
                    )
                {
                    darriojsoninInputs_M.Add(iojsonin);
                }
                else
                {
                    darriojsoninOutputs_M.Add(iojsonin);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAddResources(
            //                                              //Get resurce from InputsAndOutputs table or 
            //                                              //      InputsAndOutputsForAJob table.

            int intJobId_I,
            int intPkWorkflow_I,
            int intPkProcessInWorkflow_I,
            int intProcessInWorkflowId_I,
            bool boolAreInputs_I,
            List<ResestimjsonResourceEstimatedJson> darresestim_I,
            List<ResestimjsonResourceEstimatedJson> darrresestimjsonResourceSelected_I,
            ref List<IojsoninInputOrOutputJsonInternal> darriojsonin_M
            )
        {
            //                                              //Connection.
            Odyssey2Context context = new Odyssey2Context();

            for (int intA = 0; intA < darriojsonin_M.Count; intA++)
            {
                //                                          //To easy code.
                int? intnPkElementElementType = darriojsonin_M[intA].intnPkElementElementType;
                int? intnPkElementElement = darriojsonin_M[intA].intnPkElementElement;

                //                                          //Search in IO table.
                IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                io.intPkWorkflow == intPkWorkflow_I && io.intnProcessInWorkflowId == intProcessInWorkflowId_I &&
                io.intnPkElementElementType == intnPkElementElementType &&
                io.intnPkElementElement == intnPkElementElement);

                //                                          //Search in ForAJob table.
                IojentityInputsAndOutputsForAJobEntityDB iojentity = context.InputsAndOutputsForAJob.FirstOrDefault(
                    ioj => ioj.intPkProcessInWorkflow == intPkProcessInWorkflow_I &&
                    ioj.intnPkElementElementType == intnPkElementElementType &&
                    ioj.intnPkElementElement == intnPkElementElement &&
                    ioj.intJobId == intJobId_I);

                if (
                    //                                      //Found in ForAJob table.
                    iojentity != null
                    )
                {
                    darriojsonin_M[intA].intPkResource = iojentity.intPkResource;
                }
                else
                {
                    if (
                        //                                  //Resource found in IO table.
                        ioentity != null &&
                        ioentity.intnPkResource != null
                        )
                    {
                        darriojsonin_M[intA].intPkResource = (int)ioentity.intnPkResource;
                    }
                }

                if (
                    //                                      //Final product.
                    (ioentity != null) && (ioentity.boolnIsFinalProduct == true)
                    )
                {
                    darriojsonin_M[intA].boolIsFinalProduct = true;
                }

                if (
                    //                                      //Has link.
                    (ioentity != null) && (ioentity.strLink != null)
                    )
                {
                    darriojsonin_M[intA].boolHasLink = true;
                    darriojsonin_M[intA].strLink = ioentity.strLink;
                }

                if (
                    //                                      //The IO has not resource setted or grpResource.
                    ioentity == null && iojentity == null
                    )
                {
                    darriojsonin_M[intA].boolnHasAnyResourceOrGrpSetted = false;
                }
                else
                {
                    darriojsonin_M[intA].boolnHasAnyResourceOrGrpSetted = true;
                }
            }

            if (
                boolAreInputs_I
                )
            {
                //                                          //Adding the resources of the combination if they exist.
                foreach (ResestimjsonResourceEstimatedJson resestimjson in darresestim_I)
                {
                    int? intnPkElementElementType = null;
                    int? intnPkElementElement = resestimjson.intPkEleetOrEleele;
                    if (
                        resestimjson.boolIsEleet
                        )
                    {
                        intnPkElementElementType = resestimjson.intPkEleetOrEleele;
                        intnPkElementElement = null;
                    }
                    darriojsonin_M.First(io => io.intnPkElementElement == intnPkElementElement &&
                    io.intnPkElementElementType == intnPkElementElementType).intPkResource = (int)resestimjson.intnPk;
                }
            }

            for (int intA = 0; intA < darriojsonin_M.Count; intA++)
            {
                if (
                    darriojsonin_M[intA].intPkResource == 0
                    )
                {
                    int? intnPkElementElement = darriojsonin_M[intA].intnPkElementElement;
                    int? intnPkElementElementType = darriojsonin_M[intA].intnPkElementElementType;

                    EstdataentityEstimationDataEntityDB estdataentity = context.EstimationData.FirstOrDefault(est =>
                        est.intnPkElementElement == intnPkElementElement &&
                        est.intnPkElementElementType == intnPkElementElementType);
                    if (
                        estdataentity != null
                        )
                    {
                        darriojsonin_M[intA].intPkResource = estdataentity.intPkResource;
                    }
                    if (
                        darriojsonin_M[intA].intPkResource == 0 &&
                        darriojsonin_M[intA].boolnHasAnyResourceOrGrpSetted != false
                        )
                    {
                        intnPkElementElement = darriojsonin_M[intA].intnPkElementElement;
                        intnPkElementElementType = darriojsonin_M[intA].intnPkElementElementType;
                        int intPkEleetOrEleele = intnPkElementElementType != null ? (int)intnPkElementElementType :
                            (int)intnPkElementElement;
                        bool boolIsEleet = intnPkElementElementType != null ? true : false;

                        int? intnPkResource = darrresestimjsonResourceSelected_I.FirstOrDefault(sel =>
                        sel.intPkEleetOrEleele == intPkEleetOrEleele && sel.boolIsEleet == boolIsEleet).intnPk;

                        darriojsonin_M[intA].intPkResource = intnPkResource != null ? (int)intnPkResource :
                            darriojsonin_M[intA].intPkResource;
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetEstimationsIds(
            int intJobId_I,
            int intPkWorkflow_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            out EstjsonEstimationJson estjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            List<Estjson2EstimationDataJson2> darrestjson2 = new List<Estjson2EstimationDataJson2>();
            bool boolIsDownloadable = false;
            bool boolIsFromJob = false;

            estjson_O = new EstjsonEstimationJson(darrestjson2, boolIsDownloadable, boolIsFromJob);

            JobjsonJobJson jobjson;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Establish the connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Validate Workflow.
                WfentityWorkflowEntityDB wfentityRecieved = context.Workflow.FirstOrDefault(wf =>
                    wf.intPk == intPkWorkflow_I);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong";
                strDevMessage_IO = "Workflow not found.";
                if (
                    wfentityRecieved != null
                    )
                {
                    List<JobjsonentityJobJsonEntityDB> jobjsonentity = context.JobJson.Where(jobjson =>
                        jobjson.intJobID == intJobId_I && jobjson.strPrintshopId == ps_I.strPrintshopId).ToList();

                    int intI = 0;
                    /*WHILE-DO*/
                    while (
                        intI < jobjsonentity.Count && !boolIsFromJob
                        )
                    {
                        if (
                            //                              //It is not an estimate
                            jobjsonentity[intI].intOrderId > 0
                            )
                        {
                            boolIsFromJob = true;
                        }

                        intI = intI + 1;
                    }

                    //                                      //Get the not deleted wf.
                    WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                        wf.intnPkProduct == wfentityRecieved.intnPkProduct &&
                        wf.intWorkflowId == wfentityRecieved.intWorkflowId &&
                        wf.boolDeleted == false &&
                        wf.intnJobId == wfentityRecieved.intnJobId
                        );

                    List<EstentityEstimateEntityDB> darrestentity = context.Estimate.Where(est =>
                        est.intJobId == intJobId_I &&
                        est.intPkWorkflow == wfentity.intPk
                        ).ToList();

                    if (
                        darrestentity.Count() == 0
                        )
                    {
                        bool boolCanJobBeEstimate;
                        JobJob.boolAllResourceSettedForBudgetEstimation(jobjson, wfentity.intPk, null, ps_I,
                            configuration_I, out boolCanJobBeEstimate, ref strUserMessage_IO, ref strDevMessage_IO);

                        int intOneHour = 60 * 60 * 1000;
                        ZonedTime ztimeBase = ZonedTimeTools.ztimeNow + intOneHour;

                        //                                  //Convert base date to printshop date
                        ZonedTime ztimeBaseConverted = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                            ztimeBase.Date, ztimeBase.Time, ps_I.strTimeZone);
                        String strBaseDate = ztimeBaseConverted.Date.ToString();
                        String strBaseTime = ztimeBaseConverted.Time.ToString();

                        JobJob.boolAllResourceSettedForBudgetEstimation(jobjson, wfentity.intPk, null, ps_I,
                            configuration_I, out boolCanJobBeEstimate, ref strUserMessage_IO, ref strDevMessage_IO);

                        if (
                            boolCanJobBeEstimate
                            )
                        {
                            //                              //Add estimation 0.
                            EstentityEstimateEntityDB estentity = new EstentityEstimateEntityDB
                            {
                                intId = 0,
                                intJobId = intJobId_I,
                                strBaseDate = ztimeBase.Date.ToString(),
                                strBaseTime = ztimeBase.Time.ToString(),
                                intPkWorkflow = wfentity.intPk,
                                strName = "Confirmed Estimate"
                            };
                            context.Estimate.Add(estentity);
                            context.SaveChanges();

                            //                          //Get quantity and price.
                            BdgestjsonBudgetEstimationJson bdgestjson;
                            JobJob.subGetBudgetEstimation(intJobId_I, intPkWorkflow_I, estentity.intId,
                                estentity.intnCopyNumber, null, null, ps_I,
                                configuration_I, out bdgestjson, context, ref intStatus_IO, ref strUserMessage_IO,
                                ref strDevMessage_IO);
                            double numPrice = bdgestjson.numnJobEstimatePrice != null ?
                                (double)bdgestjson.numnJobEstimatePrice : 0.0;

                            Estjson2EstimationDataJson2 estjson2Estimation0 = new Estjson2EstimationDataJson2(
                                0, null, strBaseDate, strBaseTime, "Confirmed Estimate",
                                (int)bdgestjson.intnQuantity, numPrice);
                            darrestjson2.Add(estjson2Estimation0);
                            estjson_O = new EstjsonEstimationJson(darrestjson2, boolCanJobBeEstimate,
                                boolIsFromJob);

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "Confirmed Estimate, first time.";
                        }
                        else
                        {
                            Estjson2EstimationDataJson2 estjson2Estimation0 = new Estjson2EstimationDataJson2(
                                -1, null, strBaseDate, strBaseTime, "Partial Estimate", (int)jobjson.intnQuantity, 0);
                            darrestjson2.Add(estjson2Estimation0);
                            estjson_O = new EstjsonEstimationJson(darrestjson2, boolCanJobBeEstimate,
                                boolIsFromJob);

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "Estimate -1, there are options.";
                        }
                    }
                    else
                    {
                        if (
                            //                              //Confirmed Estimate.
                            darrestentity.Count == 1 &&
                            darrestentity[0].intId == 0
                            )
                        {
                            EstentityEstimateEntityDB est0 = darrestentity[0];
                            //bool boolCanJobBeEstimate;
                            bool boolCanJobBeEstimate = true; //Cesar.

                            intStatus_IO = 403;
                            if (
                                true
                                //JobJob.boolAllResourceSettedForBudgetEstimation(jobjson, wfentity.intPk, null,
                                //    ps_I, configuration_I, out boolCanJobBeEstimate, ref strUserMessage_IO, ref strDevMessage_IO)
                                )
                            {
                                int intQuantity;
                                double numPrice;
                                JobJob.subGetEstimationQuantityAndPrice(est0, ps_I, configuration_I,
                                    out intQuantity, out numPrice, ref intStatus_IO, ref strUserMessage_IO,
                                    ref strDevMessage_IO);

                                //                          //Convert base date to printshop date
                                ZonedTime ztimeBaseConverted = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                    est0.strBaseDate.ParseToDate(), est0.strBaseTime.ParseToTime(), ps_I.strTimeZone);

                                Estjson2EstimationDataJson2 estjson2Estimation0 = new Estjson2EstimationDataJson2(
                                    est0.intId, est0.intnCopyNumber, ztimeBaseConverted.Date.ToString(), 
                                    ztimeBaseConverted.Time.ToString(), est0.strName, intQuantity, numPrice);
                                darrestjson2.Add(estjson2Estimation0);
                                estjson_O = new EstjsonEstimationJson(darrestjson2, boolCanJobBeEstimate,
                                    boolIsFromJob);

                                intStatus_IO = 200;
                                strUserMessage_IO = "";
                                strDevMessage_IO = "Confirmed Estimate.";
                            }
                        }
                        else
                        {
                            foreach (EstentityEstimateEntityDB estentity in darrestentity)
                            {
                                int intQuantity;
                                double numPrice;
                                JobJob.subGetEstimationQuantityAndPrice(estentity, ps_I, configuration_I,
                                    out intQuantity, out numPrice, ref intStatus_IO, ref strUserMessage_IO,
                                    ref strDevMessage_IO);

                                //                          //Convert base date to printshop date
                                ZonedTime ztimeBaseConverted = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                    estentity.strBaseDate.ParseToDate(), estentity.strBaseTime.ParseToTime(),
                                    ps_I.strTimeZone);

                                Estjson2EstimationDataJson2 estjson2 = new Estjson2EstimationDataJson2(estentity.intId,
                                    estentity.intnCopyNumber, ztimeBaseConverted.Date.ToString(),
                                    ztimeBaseConverted.Time.ToString(), estentity.strName + 
                                    (estentity.intnCopyNumber == null ? "" : " (" + estentity.intnCopyNumber + ")"),
                                    intQuantity, numPrice);
                                darrestjson2.Add(estjson2);
                            }

                            boolIsDownloadable = true;
                            estjson_O = new EstjsonEstimationJson(darrestjson2, boolIsDownloadable, boolIsFromJob);

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "There are estimates.";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subGetEstimationQuantityAndPrice(
            //                                              //Calculate quantity and price for a given estimate.

            //int intJobId_I,
            //int intPkWorkflow_I,
            EstentityEstimateEntityDB estentity_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            out int intQuantity_O,
            out double numPrice_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            intQuantity_O = 0;
            numPrice_O = 0.0;

            //                          //Get quantity and price.
            BdgestjsonBudgetEstimationJson bdgestjson;
            JobJob.subGetBudgetEstimation(estentity_I.intJobId, estentity_I.intPkWorkflow, estentity_I.intId,
                estentity_I.intnCopyNumber, null, null, ps_I, configuration_I, out bdgestjson,
                context, ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

            intQuantity_O = (int)bdgestjson.intnQuantity;
            numPrice_O = bdgestjson.numnJobEstimatePrice != null ? (double)bdgestjson.numnJobEstimatePrice : 0.0;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetEstimationsDetails(
            int intJobId_I,
            int intPkWorkflow_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            out List<EstdetjsonEstimationDetailsJson> darrestdetjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            darrestdetjson_O = new List<EstdetjsonEstimationDetailsJson>();
            JobjsonJobJson jobjson;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Establish the connection.
                Odyssey2Context context = new Odyssey2Context();

                //                                          //Validate Workflow.
                WfentityWorkflowEntityDB wfentityRecieved = context.Workflow.FirstOrDefault(wf =>
                    wf.intPk == intPkWorkflow_I &&
                    wf.boolDeleted == false);

                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong";
                strDevMessage_IO = "Workflow not found.";
                if (
                    wfentityRecieved != null
                    )
                {
                    List<EstentityEstimateEntityDB> darrestentity = context.Estimate.Where(est => est.intJobId == intJobId_I &&
                        est.intPkWorkflow == intPkWorkflow_I).ToList();

                    intStatus_IO = 402;
                    strUserMessage_IO = "There are no estimations confirmed.";
                    strDevMessage_IO = "There are no estimations confirmed.";
                    if (
                        darrestentity.Count > 0
                        )
                    {
                        foreach (EstentityEstimateEntityDB estentity in darrestentity)
                        {
                            BdgestjsonBudgetEstimationJson bdgestjson;
                            JobJob.subGetBudgetEstimation(intJobId_I, intPkWorkflow_I, estentity.intId,
                                estentity.intnCopyNumber, null, null, ps_I,
                                configuration_I, out bdgestjson, context, ref intStatus_IO, ref strUserMessage_IO,
                                ref strDevMessage_IO);

                            String strCopyNumber = estentity.intnCopyNumber != null ?
                                "(" + estentity.intnCopyNumber.ToString() + ")" : "";

                            EstdetjsonEstimationDetailsJson estdetjson = new EstdetjsonEstimationDetailsJson(
                                estentity.strName + strCopyNumber, (int)bdgestjson.intnQuantity,
                                (double)bdgestjson.numnJobEstimatePrice);
                            darrestdetjson_O.Add(estdetjson);
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolAllResourceSettedForBudgetEstimation(
            JobjsonJobJson jobjson_I,
            int intPkWorkflow_I,
            int? intnEstimationId_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            out bool boolCanJobBeEstimate_O,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolHasAllResourceSetted = false;
            boolCanJobBeEstimate_O = false;

            ProdtypProductType prodtyp = null;

            Odyssey2Context context = new Odyssey2Context();

            //                                              //Update products in DB App from Wisnet.
            Dictionary<int, ProdtypProductType> dicprodtem = ps_I.dicprodtyp;

            EtentityElementTypeEntityDB etentityProduct = context.ElementType.FirstOrDefault(et =>
                et.intPrintshopPk == ps_I.intPk &&
                et.strCustomTypeId == jobjson_I.strProductName &&
                et.intWebsiteProductKey == jobjson_I.intnProductKey);

            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No product found.";
            if (
                etentityProduct != null
                )
            {
                prodtyp = (ProdtypProductType)EtElementTypeAbstract.etFromDB(etentityProduct.intPk);
                strUserMessage_IO = "The product's workflow is incomplete. Complete the product's workflow" +
                    " to see the budget's estimation.";
                strDevMessage_IO = "";

                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityWithFinalProduct;
                bool boolWorkflowIsReady;
                bool? boolnNotUsed;
                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityProcessesNotReady;
                ProdtypProductType.subfunWorkflowIsReady(intPkWorkflow_I, out darrpiwentityWithFinalProduct,
                    out boolWorkflowIsReady, out boolnNotUsed, out darrpiwentityProcessesNotReady);
                if (
                    boolWorkflowIsReady
                    )
                {
                    boolHasAllResourceSetted = true;
                    boolCanJobBeEstimate_O = true;

                    //                                      //Get valid PIWs depending on job's attributes.
                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentity;
                    List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                    ProdtypProductType.subGetWorkflowValidWay(intPkWorkflow_I, jobjson_I, out darrpiwentity,
                        out darrdynlkjson);                    

                    //                                      //List of quantityInputs and quantityOutputs.
                    //                                      //    for optimization.
                    List<IoqytjsonIOQuantityJson> darrioqytjsonIOQuantityNotUsed = new List<IoqytjsonIOQuantityJson>();

                    //                                      //List waste to propagate.                          
                    List<WstpropjsonWasteToPropagateJson> darrwstpropjsonNotUsed =
                        new List<WstpropjsonWasteToPropagateJson>();

                    strUserMessage_IO = "Many workflow possibilities found.";
                    strDevMessage_IO = "";
                    if (
                        darrpiwentity != null
                        )
                    {
                        ProdtypProductType.subUpdateResourceForAJob(prodtyp, null, darrpiwentity, jobjson_I);

                        int intI = 0;
                        /*REPEAT-WHILE*/
                        while (
                            //                              //Get the inputs every process.
                            intI < darrpiwentity.Count &&
                            boolHasAllResourceSetted
                            )
                        {
                            //                              //List to Add IO Inputs.
                            List<RecbdgjsonResourceBudgetJson> darrrecbdgjson =
                                new List<RecbdgjsonResourceBudgetJson>();

                            bool boolNotUsed = false;
                            //                              //Get cost resources by each Eleet IO.
                            darrrecbdgjson.AddRange(prodtyp.arrbdgresjsonFromType(darrpiwentity[intI], jobjson_I,
                                ps_I.strPrintshopId, intnEstimationId_I, darrpiwentity, darrdynlkjson, configuration_I,
                                ref boolHasAllResourceSetted, ref boolCanJobBeEstimate_O, ref boolNotUsed,
                                ref darrwstpropjsonNotUsed, ref darrioqytjsonIOQuantityNotUsed));

                            //                              //Get cost resources by each Eleele IO.
                            darrrecbdgjson.AddRange(prodtyp.arrbdgresjsonFromTemplate(darrpiwentity[intI], jobjson_I,
                                ps_I.strPrintshopId, intnEstimationId_I, darrpiwentity, darrdynlkjson, configuration_I,
                                ref boolHasAllResourceSetted, ref boolCanJobBeEstimate_O, ref boolNotUsed,
                                ref darrwstpropjsonNotUsed, ref darrioqytjsonIOQuantityNotUsed));

                            intI = intI + 1;
                        }

                        if (
                            !boolHasAllResourceSetted
                            )
                        {
                            strUserMessage_IO = "There must be at least one resource on every input in the processes " +
                                "for the workflow.";
                            strDevMessage_IO = "";
                        }
                        else
                        {
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }

            return boolHasAllResourceSetted;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetCalendar(
            String strPrintshopId_I,
            int intJobId_I,
            int intPkWorkflow_I,
            String strSunday_I,
            IConfiguration configuration_I,
            out LvlpiwjsonLevelAndPIWJson lvlpiwjson_O,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO)
        {
            lvlpiwjson_O = null;

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "There is not a connection to Wisnet.";
            JobjsonJobJson jobjson = new JobjsonJobJson();
            if (
                //                                          //Valid the jobID.
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                intStatus_IO = 404;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No printshop found.";
                if (
                    //                                      //Valid the printshop.                      
                    ps != null
                    )
                {
                    Odyssey2Context context = new Odyssey2Context();

                    WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                        wf.intPk == intPkWorkflow_I);

                    intStatus_IO = 405;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "No workflow found.";
                    if (
                        //                                  //Valid the wf.
                        wfentity != null
                        )
                    {
                        lvlpiwjson_O = JobJob.arrleveljsonGetLevelsOfTheJob(ps.intPk, intJobId_I,
                            intPkWorkflow_I, strSunday_I, ps.strTimeZone);

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static LvlpiwjsonLevelAndPIWJson arrleveljsonGetLevelsOfTheJob(
            int intPkPrintshop_I,
            int intJobId_I,
            int intPkWorkflow_I,
            String strSunday_I,
            String strTimeZoneId_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //All periods of the job and wf, 
            //                                              //      not considering temporary ones.
            List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per =>
                per.intPkWorkflow == intPkWorkflow_I && per.intJobId == intJobId_I &&
                per.intnEstimateId == null).ToList();
            darrperentity.Sort();

            Date dateSunday;
            /*CASE*/
            if (
                //                                          //Get a specific week of the job calendar.
                (strSunday_I != null) &&
                (strSunday_I.IsParsableToDate())
                )
            {
                dateSunday = strSunday_I.ParseToDate();
            }
            else if (
                //                                          //The first week of the job.
                darrperentity.Count > 0
                )
            {
                dateSunday = darrperentity[0].strStartDate.ParseToDate() -
                    ((int)darrperentity[0].strStartDate.ParseToDate().DayOfWeek); ;
            }
            else
            {
                //                                          //Sunday of the current week.
                dateSunday = Date.Now(ZonedTimeTools.timezone) -
                    ((int)Date.Now(ZonedTimeTools.timezone).DayOfWeek);
            }
            /*END-CASE*/

            Rulejson1RuleJson1[] arrrulejson1 = JobJob.arrrulejson1GetRulesForLevels(intPkPrintshop_I, strTimeZoneId_I,
                dateSunday);

            //                                              //Get all piw.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity = context.ProcessInWorkflow.Where(piw =>
                piw.intPkWorkflow == intPkWorkflow_I).ToList();

            //                                              //Arr of PIW Ids.
            List<Piwjson3ProcessInWorkflowJson3> darrpiwjson3 = new List<Piwjson3ProcessInWorkflowJson3>();

            //                                              //Get all process with their periods.
            List<ProjsonProcessJson> darrprojson = JobJob.darrprojsonGetAllProcessForTheJob(strTimeZoneId_I, 
                darrpiwentity, darrperentity, dateSunday, ref darrpiwjson3);
            darrprojson.Sort();

            //                                              //Create the levels.
            List<LeveljsonLevelJson> darrleveljson = new List<LeveljsonLevelJson>();
            for (int intI = 0; intI < darrprojson.Count; intI = intI + 1)
            {
                if (
                    intI == 0
                    )
                {
                    LeveljsonLevelJson leveljson = new LeveljsonLevelJson(arrrulejson1);
                    leveljson.arrpro.Add(darrprojson[intI]);
                    leveljson.arrper.AddRange(darrprojson[intI].arrper);
                    darrprojson[intI].arrper = new Perjson1PeriodJson1[0];
                    darrleveljson.Add(leveljson);
                }
                else
                {
                    bool boolProjsonAdded = false;
                    int intJ = 0;
                    /*UNTIL-DO*/
                    while (!(
                        (intJ >= darrleveljson.Count) ||
                        boolProjsonAdded
                        ))
                    {
                        //                                  //To easy code.
                        ZonedTime ztimeEndLastProjsonOfTheLevel = ZonedTimeTools.NewZonedTime(
                            darrleveljson[intJ].arrpro.Last().strEndDate.ParseToDate(),
                            darrleveljson[intJ].arrpro.Last().strEndTime.ParseToTime());

                        ZonedTime ztimeStartProjson = ZonedTimeTools.NewZonedTime(
                            darrprojson[intI].strStartDate.ParseToDate(), darrprojson[intI].strStartTime.ParseToTime());

                        if (
                            ztimeStartProjson >= ztimeEndLastProjsonOfTheLevel
                            )
                        {
                            darrleveljson[intJ].arrpro.Add(darrprojson[intI]);
                            darrleveljson[intJ].arrper.AddRange(darrprojson[intI].arrper);
                            darrprojson[intI].arrper = new Perjson1PeriodJson1[0];
                            boolProjsonAdded = true;
                        }

                        intJ = intJ + 1;
                    }

                    if (
                        !boolProjsonAdded
                        )
                    {
                        LeveljsonLevelJson leveljson = new LeveljsonLevelJson(arrrulejson1);
                        leveljson.arrpro.Add(darrprojson[intI]);
                        leveljson.arrper.AddRange(darrprojson[intI].arrper);
                        darrprojson[intI].arrper = new Perjson1PeriodJson1[0];
                        darrleveljson.Add(leveljson);
                    }
                }
            }

            if (
                darrleveljson.Count == 0
                )
            {
                LeveljsonLevelJson leveljson = new LeveljsonLevelJson(arrrulejson1);
                darrleveljson.Add(leveljson);
            }

            foreach (LeveljsonLevelJson leveljsonToOrder in darrleveljson)
            {
                leveljsonToOrder.arrper.Sort();
            }

            LvlpiwjsonLevelAndPIWJson lvlpiw = new LvlpiwjsonLevelAndPIWJson(darrleveljson, darrpiwjson3);

            return lvlpiw;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static Rulejson1RuleJson1[] arrrulejson1GetRulesForLevels(
            int intPkPrintshop_I,
            String strTimeZoneId_I,
            Date dateSunday_I
            )
        {
            List<Rulejson1RuleJson1> darrrulejson1 = new List<Rulejson1RuleJson1>();

            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get once rules.
            List<RuleentityRuleEntityDB> darrruleentityOnce = context.Rule.Where(rule =>
                rule.intnPkPrintshop == intPkPrintshop_I && rule.intnPkResource == null &&
                rule.strFrecuency == ResResource.strOnce).ToList();

            foreach (RuleentityRuleEntityDB ruleentity in darrruleentityOnce)
            {

                ZonedTime ztimeStartRule = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                    ruleentity.strFrecuencyValue.Substring(0, 10).ParseToDate(), 
                    ruleentity.strStartTime.ParseToTime(), strTimeZoneId_I);

                ZonedTime ztimeEndRule = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                    ruleentity.strFrecuencyValue.Substring(11).ParseToDate(), 
                    ruleentity.strEndTime.ParseToTime(), strTimeZoneId_I);

                Date dateStartRule = ztimeStartRule.Date;
                Date dateEndRule = ztimeEndRule.Date;

                if (
                    ((dateStartRule >= dateSunday_I) && (dateStartRule < (dateSunday_I + 7))) ||
                    ((dateEndRule > dateSunday_I) && (dateEndRule <= (dateSunday_I + 7)))
                    )
                {
                    Rulejson1RuleJson1 rulejson1 = new Rulejson1RuleJson1(dateStartRule.ToText(),
                        ztimeStartRule.Time.ToString(), dateEndRule.ToText(), ztimeEndRule.Time.ToString());
                    darrrulejson1.Add(rulejson1);
                }
            }

            //                                              //Get all rules.
            List<RuleentityRuleEntityDB> darrruleentity = context.Rule.Where(rule =>
                rule.strFrecuency != ResResource.strOnce && rule.intnPkResource == null &&
                rule.intnPkPrintshop == intPkPrintshop_I).ToList();

            foreach (RuleentityRuleEntityDB ruleentity in darrruleentity)
            {
                ZonedTime ztimeStartRule = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                    ruleentity.strRangeStartDate.ParseToDate(), ruleentity.strRangeStartTime.ParseToTime(),
                    strTimeZoneId_I);

                Date dateStartRange = ztimeStartRule.Date;

                Date dateEndRange = Date.MaxValue;
                if (
                    ruleentity.strRangeEndDate != null
                    )
                {
                    ZonedTime ztimeEndRule = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        ruleentity.strRangeEndDate.ParseToDate(), ruleentity.strRangeEndTime.ParseToTime(), 
                        strTimeZoneId_I);

                    dateEndRange = ztimeEndRule.Date;
                }

                for (int intI = 0; intI < 7; intI = intI + 1)
                {
                    Date date = dateSunday_I + intI;

                    if (
                        //                                  //Date is in the range.
                        ((date >= dateStartRange) && (date <= dateEndRange)) &&
                        //                                  //Is daily.
                        ((ruleentity.strFrecuency == ResResource.strDaily) ||
                        //                                  //Is weekly and the date is a day with rule.
                        ((ruleentity.strFrecuency == ResResource.strWeekly) &&
                        (ruleentity.strFrecuencyValue[(int)date.DayOfWeek] == '1')) ||
                        //                                  //Is monthly and the date is a day with rule.
                        ((ruleentity.strFrecuency == ResResource.strMonthly) &&
                        (ruleentity.strFrecuencyValue[(int)date.Day - 1] == '1')) ||
                        //                                  //Is annually and the date is a day with rule.
                        ((ruleentity.strFrecuency == ResResource.strAnnually) &&
                        (ruleentity.strFrecuencyValue == date.ToString("MMdd"))))
                        )
                    {
                        Rulejson1RuleJson1 rulejson1 = new Rulejson1RuleJson1(date.ToText(), ruleentity.strStartTime,
                            date.ToText(), ruleentity.strEndTime);
                        darrrulejson1.Add(rulejson1);
                    }
                }
            }

            return darrrulejson1.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<ProjsonProcessJson> darrprojsonGetAllProcessForTheJob(

            String strTimeZoneId_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity_I,
            List<PerentityPeriodEntityDB> darrperentity_I,
            Date dateSunday_I,
            ref List<Piwjson3ProcessInWorkflowJson3> darrpiwjson3_M
            )
        {
            List<ProjsonProcessJson> darrprojson = new List<ProjsonProcessJson>();
            for (int intX = 0; intX < darrpiwentity_I.Count; intX = intX + 1)
            {
                PiwentityProcessInWorkflowEntityDB piwentity = darrpiwentity_I[intX];

                //                                          //Get name the process.
                ProProcess pro = ProProcess.proFromDB(piwentity.intPkProcess);
                String strName = pro.strName;
                if (
                    piwentity.intnId != null
                    )
                {
                    strName = strName + " (" + piwentity.intnId + ")";
                }

                //                                          //Add Info of the PIW.
                darrpiwjson3_M.Add(new Piwjson3ProcessInWorkflowJson3(darrpiwentity_I[intX].intPk,
                strName));

                ZonedTime? ztimeStartProcess = null;
                ZonedTime? ztimeEndProcess = null;

                List<PerentityPeriodEntityDB> darrperentityToProcess = new List<PerentityPeriodEntityDB>();
                foreach (PerentityPeriodEntityDB perentity in darrperentity_I)
                {
                    ZonedTime ztimeStartPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        perentity.strStartDate.ParseToDate(), perentity.strStartTime.ParseToTime(), strTimeZoneId_I);
                    ZonedTime ztimeEndPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        perentity.strEndDate.ParseToDate(), perentity.strEndTime.ParseToTime(), strTimeZoneId_I);

                    Date dateStartPeriod = ztimeStartPeriod.Date;
                    Date dateEndPeriod = ztimeEndPeriod.Date;

                    if (
                        //                                  //Period starts in the week.
                        (((dateStartPeriod >= dateSunday_I) &&
                        (dateStartPeriod < (dateSunday_I + 7))) ||
                        ((dateEndPeriod >= dateSunday_I) &&
                        (dateEndPeriod < (dateSunday_I + 7)))) &&
                        //                                  //Period is for the current piw.
                        (perentity.intProcessInWorkflowId == piwentity.intProcessInWorkflowId)
                        )
                    {
                        /*CASE*/
                        if (
                            ztimeStartProcess == null
                            )
                        {
                            ztimeStartProcess = ztimeStartPeriod;
                            ztimeEndProcess = ztimeEndPeriod;
                        }
                        else if (
                            ztimeEndProcess < ztimeEndPeriod
                            )
                        {
                            ztimeEndProcess = ztimeEndPeriod;
                        }
                        else
                        {

                        }
                        /*END-CASE*/

                        darrperentityToProcess.Add(perentity);
                    }
                }
                darrperentityToProcess.Sort();

                if (
                    //                                      //The piw has periods.
                    darrperentityToProcess.Count > 0
                    )
                {
                    List<Perjson1PeriodJson1> darrperjson1 = new List<Perjson1PeriodJson1>();
                    for (int intY = 0; intY < darrperentityToProcess.Count; intY = intY + 1)
                    {
                        PerentityPeriodEntityDB perentity = darrperentityToProcess[intY];
                        bool boolByProcess = (perentity.intnPkElementElement == null) &&
                            (perentity.intnPkElementElementType == null);

                        Odyssey2Context context = new Odyssey2Context();

                        EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                            ele.intPk == perentity.intPkElement);
                        String strPeriodName = eleentity.strElementName;
                        if (
                            perentity.intnPkCalculation != null
                            )
                        {
                            CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(cal =>
                                cal.intPk == perentity.intnPkCalculation);
                            strPeriodName = calentity.strDescription;
                        }

                        ZonedTime ztimeStartPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                            perentity.strStartDate.ParseToDate(), perentity.strStartTime.ParseToTime(), 
                            strTimeZoneId_I);
                        ZonedTime ztimeEndPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                            perentity.strEndDate.ParseToDate(), perentity.strEndTime.ParseToTime(), strTimeZoneId_I);

                        Perjson1PeriodJson1 perjson1 = new Perjson1PeriodJson1(strPeriodName, 
                            ztimeStartPeriod.Date.ToString(), ztimeStartPeriod.Time.ToString(),
                            ztimeEndPeriod.Date.ToString(), ztimeEndPeriod.Time.ToString(),
                            boolByProcess, false, false);

                        if (
                            intX == 0 && intY == 0
                            )
                        {
                            perjson1.boolIsTheFirstPeriod = true;
                        }

                        if (
                            intX == (darrpiwentity_I.Count - 1) && intY == (darrperentityToProcess.Count - 1)
                            )
                        {
                            perjson1.boolIsTheLastPeriod = true;
                        }
                        darrperjson1.Add(perjson1);
                    }

                    if (
                        piwentity.intnId != null
                        )
                    {
                        strName = strName + " (" + piwentity.intnId + ")";
                    }

                    String strStartDate = ((ZonedTime)ztimeStartProcess).Date.ToText();
                    String strStartTime = ((ZonedTime)ztimeStartProcess).Time.ToString();
                    String strEndDate = ((ZonedTime)ztimeEndProcess).Date.ToText();
                    String strEndTime = ((ZonedTime)ztimeEndProcess).Time.ToString();

                    ProjsonProcessJson projson = new ProjsonProcessJson(strName, strStartDate, strStartTime, strEndDate,
                        strEndTime, darrperjson1.ToArray());

                    darrprojson.Add(projson);
                }
            }

            return darrprojson;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPeriodsFromOneProcess(
            PsPrintShop ps_I,
            int intJobId_I,
            int intPkProcessInWorkflow_I,
            IConfiguration configuration_I,
            out PerfrmpiwjsonPeriodFromPIWJson perfrmpiwjson_O,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO)
        {
            perfrmpiwjson_O = null;

            JobjsonJobJson jobjson = new JobjsonJobJson();
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                Odyssey2Context context = new Odyssey2Context();

                PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(
                    piw => piw.intPk == intPkProcessInWorkflow_I);

                intStatus_IO = 405;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "PIW not found";
                if (
                    piwentity != null
                    )
                {
                    //                                      //Get PKPIW and ProcessInWorkflowId.
                    int intPkWorkflow = piwentity.intPkWorkflow;
                    int intPIWId = piwentity.intProcessInWorkflowId;

                    //                                      //Get all periods from PIW except temporal periods.
                    List<PerentityPeriodEntityDB> darrperiod = context.Period.Where(
                        per => per.intPkWorkflow == intPkWorkflow &&
                        per.intProcessInWorkflowId == intPIWId &&
                        per.intJobId == jobjson.intJobId &&
                        per.intnEstimateId == null).ToList();

                    //                                      //List period from Process.
                    List<ProperjsonProcessPeriodJson> arrproper = new List<ProperjsonProcessPeriodJson>();

                    //                                      //List period from Resource.
                    List<ResperjsonResourcePeriodJson> arrresper = new List<ResperjsonResourcePeriodJson>();

                    //                                      //Get Employees from printshop.
                    List<ContactjsonContactJson> darrcontactjson = ResResource.darrcontactjsonGetAllEmployee(
                        ps_I.strPrintshopId);

                    foreach (PerentityPeriodEntityDB perentity in darrperiod)
                    {
                        if (
                            //                              //It is a period from process.
                            perentity.intnPkElementElement == null &&
                            perentity.intnPkElementElementType == null
                            )
                        {
                            JobJob.subAddPeriodToListProcessPeriod(ps_I, perentity, darrcontactjson,
                                ref arrproper);
                        }
                        else
                        {
                            //                              //It is a period from resource.
                            JobJob.subAddPeriodToListResourcePeriod(jobjson.intnQuantity, piwentity, ps_I,
                                perentity, darrcontactjson, ref arrresper);
                        }
                    }

                    //                                      //Get the process.
                    EleentityElementEntityDB eleentityProcess = context.Element.
                        FirstOrDefault(ele => ele.intPk == piwentity.intPkProcess);

                    //                                      //Get the name of this PIW.
                    String strProcessNameAndId = eleentityProcess.strElementName;
                    if (
                        piwentity.intnId != null
                        )
                    {
                        strProcessNameAndId = eleentityProcess.strElementName + " (" +
                            piwentity.intnId + ")";
                    }

                    perfrmpiwjson_O = new PerfrmpiwjsonPeriodFromPIWJson(piwentity.intPk, strProcessNameAndId,
                        arrproper, arrresper);

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subAddPeriodToListProcessPeriod(
            //                                              //Add period to the list Process'periods.

            PsPrintShop ps_I,
            PerentityPeriodEntityDB perentity_I,
            List<ContactjsonContactJson> darrcontactjson_I,
            ref List<ProperjsonProcessPeriodJson> arrproper_M
            )
        {
            //                                              //To easy code.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get calculation of this period.
            CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(cal =>
            cal.intPk == perentity_I.intnPkCalculation);

            String strFirstName = "-";
            String strLastName = "-";
            if (
                perentity_I.intnContactId != null
                )
            {
                //                                          //Data from employee.
                ResResource.subGetFirstAndLastNameOfEmployee(ps_I.strPrintshopId, (int)perentity_I.intnContactId,
                    darrcontactjson_I, ref strFirstName, ref strLastName);
            }

            //                                              //Determine the status for this period.
            String strStatus = null;
            if (
                //                                          //Process periods Pending.
                perentity_I.strFinalStartTime == null &&
                perentity_I.strFinalEndTime == null
                )
            {
                strStatus = "Pending.";
            }
            else if (
               //                                          //Process periods InProgress.
               perentity_I.strFinalStartTime != null &&
               perentity_I.strFinalEndTime == null
               )
            {
                strStatus = "InProgress.";
            }
            else if (
                //                                          //Process periods Complete.
                perentity_I.strFinalEndTime != null
                )
            {
                strStatus = "Complete.";
            }

            ZonedTime ztimeStartPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                perentity_I.strStartDate.ParseToDate(), perentity_I.strStartTime.ParseToTime(), ps_I.strTimeZone);

            ZonedTime ztimeEndPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                perentity_I.strEndDate.ParseToDate(), perentity_I.strEndTime.ParseToTime(), ps_I.strTimeZone);

            ProperjsonProcessPeriodJson properjson = new ProperjsonProcessPeriodJson(calentity.intPk,
                calentity.strDescription, ztimeStartPeriod.Date.ToString(), ztimeStartPeriod.Time.ToString(),
                ztimeEndPeriod.Date.ToString(), ztimeEndPeriod.Time.ToString(), strFirstName, strLastName,
                perentity_I.intnContactId, strStatus, perentity_I.intMinsBeforeDelete);

            arrproper_M.Add(properjson);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subAddPeriodToListResourcePeriod(
            //                                              //Add period to the list Resource'periods.
            int? intnJobQuantity,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            PsPrintShop ps_I,
            PerentityPeriodEntityDB perentity_I,
            List<ContactjsonContactJson> darrcontactjson_I,
            ref List<ResperjsonResourcePeriodJson> arrresper_M
            )
        {
            //                                              //To easy code.
            Odyssey2Context context = new Odyssey2Context();

            String strResourceFromTypeOrTemp = null;

            //                                              //Get Info from resource of the period.
            if (
                //                                          //It is a Eleet.
                perentity_I.intnPkElementElementType != null
                )
            {
                ResResource resResource = ResResource.resFromDB(perentity_I.intPkElement, false);
                strResourceFromTypeOrTemp = "(" + resResource.restypBelongsTo.strXJDFTypeId + ") " +
                resResource.strName;

                //                                          //Get eleet entity.
                EleetentityElementElementTypeEntityDB eleetentity = context.ElementElementType.FirstOrDefault(
                    eleet => eleet.intPk == perentity_I.intnPkElementElementType);

                //                                          //Get eleentity.
                EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                ele.intPk == perentity_I.intPkElement);
            }
            else
            {
                //                                          //It is a Eleele
                ResResource resResource = ResResource.resFromDB(perentity_I.intPkElement, true);

                strResourceFromTypeOrTemp = "(" + resResource.restypBelongsTo.strXJDFTypeId + " : " +
                    resResource.resinherited.strName + ") " + resResource.strName;

                //                                          //Get eleele entity.
                EleeleentityElementElementEntityDB eleeleentity = context.ElementElement.FirstOrDefault(eleele =>
                                eleele.intPk == perentity_I.intnPkElementElement);

                //                                          //Get eleentity.
                EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                ele.intPk == perentity_I.intPkElement);
            }

            //                                              //Get Data from contactId.
            String strFirstName = "-";
            String strLastName = "-";
            if (
                perentity_I.intnContactId != null
                )
            {
                //                                          //Data from employee.
                ResResource.subGetFirstAndLastNameOfEmployee(ps_I.strPrintshopId, (int)perentity_I.intnContactId,
                    darrcontactjson_I, ref strFirstName, ref strLastName);
            }

            //                                              //Determine the status for this period.
            String strStatus = null;
            if (
                //                                          //Process periods Pending.
                perentity_I.strFinalStartTime == null &&
                perentity_I.strFinalEndTime == null
                )
            {
                strStatus = "Pending.";
            }
            else if (
               //                                          //Process periods InProgress.
               perentity_I.strFinalStartTime != null &&
               perentity_I.strFinalEndTime == null
               )
            {
                strStatus = "InProgress.";
            }
            else if (
                //                                          //Process periods Complete.
                perentity_I.strFinalEndTime != null
                )
            {
                strStatus = "Complete.";
            }

            ZonedTime ztimeStartPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                perentity_I.strStartDate.ParseToDate(), perentity_I.strStartTime.ParseToTime(), ps_I.strTimeZone);

            ZonedTime ztimeEndPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                perentity_I.strEndDate.ParseToDate(), perentity_I.strEndTime.ParseToTime(), ps_I.strTimeZone);

            ResperjsonResourcePeriodJson resperjson = new ResperjsonResourcePeriodJson(
                strResourceFromTypeOrTemp, ztimeStartPeriod.Date.ToString(), ztimeStartPeriod.Time.ToString(),
                ztimeEndPeriod.Date.ToString(), ztimeEndPeriod.Time.ToString(), strFirstName,
                strLastName, perentity_I.intnContactId, strStatus, perentity_I.intMinsBeforeDelete);

            arrresper_M.Add(resperjson);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPeriodsForAJobAndWorkflow(
            //                                              //Get resources or process periods or resources/process 
            //                                              //      to add a period for one job with an especific
            //                                              //      workflow.

            PsPrintShop ps_I,
            int intJobId_I,
            int intPkWorkflow_I,
            IConfiguration configuration_I,
            out PerjobjsonPeriodsJobJson perjobjson_O,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO
            )
        {
            List<Projson3ProcessJson3> darrprojson3 = new List<Projson3ProcessJson3>();

            perjobjson_O = null;
            //                                              //Validate job.
            JobjsonJobJson jobjson = new JobjsonJobJson();
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Establish the connection.
                Odyssey2Context context = new Odyssey2Context();

                WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                    wf.intPk == intPkWorkflow_I);

                intStatus_IO = 402;
                strUserMessage_IO = "Something wrong";
                strDevMessage_IO = "Workflow not valid";
                if (
                    wfentity != null
                    )
                {
                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityWithFinalProduct;
                    bool boolWorkflowIsReady;
                    bool? boolnNotUsed;
                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityProcessesNotReady;
                    ProdtypProductType.subfunWorkflowIsReady(intPkWorkflow_I, out darrpiwentityWithFinalProduct,
                        out boolWorkflowIsReady, out boolnNotUsed, out darrpiwentityProcessesNotReady);

                    intStatus_IO = 403;
                    strUserMessage_IO = "The product workflow is incomplete.";
                    strDevMessage_IO = "";
                    if (
                        boolWorkflowIsReady
                        )
                    {
                        //                                  //Get all the correct processes.
                        List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
                        List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                        ProdtypProductType.subGetWorkflowValidWay(intPkWorkflow_I, jobjson,
                            out darrpiwentityAllProcesses, out darrdynlkjson);

                        intStatus_IO = 404;
                        strUserMessage_IO = "Workflow has not a valid path. Check your workflow.";
                        strDevMessage_IO = "";
                        if (
                            darrpiwentityAllProcesses.Count > 0
                            )
                        {
                            //                              //Get all periods asociated to a job with an
                            //                              //      especific workflow.      
                            perjobjson_O = JobJob.darrprojson3GetPeriodsForAllProcessInAWorkflow(jobjson,
                                darrpiwentityAllProcesses, ps_I, configuration_I);

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static PerjobjsonPeriodsJobJson darrprojson3GetPeriodsForAllProcessInAWorkflow(
            //                                              //Evaluate a valid workflow way and get their process and
            //                                              //      resources periods.

            JobjsonJobJson jobjson_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job => job.intJobID == jobjson_I.intJobId);

            //                                              //Init values necesary for calculate EstimateDate.
            String strEstimateDate;
            ZonedTime ztimeTemp = ZonedTimeTools.NewZonedTime("1000-01-01".ParseToDate(),
                            "01:00:00".ParseToTime());
            ZonedTime ztimeLastEstimateDate = ZonedTimeTools.NewZonedTime("1000-01-01".ParseToDate(),
                            "01:00:00".ParseToTime());

            List<Projson3ProcessJson3> darrprojson3 = new List<Projson3ProcessJson3>();
            foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentity_I)
            {
                List<CalentityCalculationEntityDB> darrcalentity;
                if (
                    //                                      //Job in progress o completed.
                    jobentity != null
                    )
                {
                    List<CalentityCalculationEntityDB> darrcalentityAll = context.Calculation.Where(cal =>
                    cal.intnPkWorkflow == piwentity.intPkWorkflow && cal.boolIsEnable == true &&
                    cal.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId && cal.intnHours != null &&
                    cal.intnPkElementElement == null && cal.intnPkElementElementType == null &&
                    Tools.boolCalculationOrLinkApplies(cal.intPk, null, null, null, jobjson_I)).ToList();

                    //                                      //To easy code.
                    ZonedTime ztimeStartJob = ZonedTimeTools.NewZonedTime(jobentity.strStartDate.ParseToDate(),
                        jobentity.strStartTime.ParseToTime());

                    darrcalentity = new List<CalentityCalculationEntityDB>();
                    foreach (CalentityCalculationEntityDB calentity in darrcalentityAll)
                    {
                        //                                  //To easy code.
                        ZonedTime ztimeStartCal = ZonedTimeTools.NewZonedTime(calentity.strStartDate.ParseToDate(),
                            calentity.strStartTime.ParseToTime());

                        ZonedTime ztimeEndCal = ZonedTimeTools.NewZonedTime(Date.MaxValue, Time.MinValue);
                        if (
                            calentity.strEndDate != null
                            )
                        {
                            ztimeEndCal = ZonedTimeTools.NewZonedTime(calentity.strEndDate.ParseToDate(),
                                calentity.strEndTime.ParseToTime());
                        }

                        if (
                            ztimeStartJob >= ztimeStartCal &&
                            ztimeStartJob < ztimeEndCal
                            )
                        {
                            darrcalentity.Add(calentity);
                        }
                    }
                }
                else
                {
                    darrcalentity = context.Calculation.Where(cal => 
                    cal.intnPkWorkflow == piwentity.intPkWorkflow &&
                    cal.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId && cal.boolIsEnable == true &&
                    cal.intnPkElementElement == null && cal.intnPkElementElementType == null &&
                    cal.intnHours != null && cal.strEndDate == null &&
                    Tools.boolCalculationOrLinkApplies(cal.intPk, null, null, null, jobjson_I)).ToList();
                }

                List<CalperjsonCalculationPeriodJson> darrcalperjson = new List<CalperjsonCalculationPeriodJson>();

                //                                          //Get all employees from printshop.
                List<ContactjsonContactJson> darrcontactjson =
                    ResResource.darrcontactjsonGetAllEmployee(ps_I.strPrintshopId);

                //                                          //Get the resources periods for this process.
                Resperjson2ResourcePeriodJson2[] arrresperjsonResourcesPeriod =
                    JobJob.arrresperjsonResourcesPeriodForOneProcess(piwentity, jobjson_I.intJobId, ps_I,
                    configuration_I, ref ztimeLastEstimateDate);

                foreach (CalentityCalculationEntityDB calentity in darrcalentity)
                {

                    PerentityPeriodEntityDB perentity = context.Period.FirstOrDefault(per =>
                        per.intnPkCalculation == calentity.intPk && per.intJobId == jobjson_I.intJobId);

                    String strStartDate = null;
                    String strStartTime = null;
                    String strEndDate = null;
                    String strEndTime = null;
                    int? intnPkPeriod = null;
                    String strFirstName = null;
                    String strLastName = null;
                    int? intnContactId = null;
                    int intMinsBeforeDelete = 0;
                    bool boolPeriodStarted = false;
                    bool boolPeriodCompleted = false;
                    if (
                        perentity != null
                        )
                    {
                        ZonedTime ztimeStartPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                            perentity.strStartDate.ParseToDate(), perentity.strStartTime.ParseToTime(),
                            ps_I.strTimeZone);

                        ZonedTime ztimeEndPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                           perentity.strEndDate.ParseToDate(), perentity.strEndTime.ParseToTime(), ps_I.strTimeZone);

                        strStartDate = ztimeStartPeriod.Date.ToString();
                        strStartTime = ztimeStartPeriod.Time.ToString();
                        strEndDate = ztimeEndPeriod.Date.ToString();
                        strEndTime = ztimeEndPeriod.Time.ToString();
                        intnPkPeriod = perentity.intPk;
                        intMinsBeforeDelete = perentity.intMinsBeforeDelete;

                        //                                      //Check the EstimateDate.
                        ztimeLastEstimateDate = ztimeLastEstimateDate != null ?
                            //                                  //ZtimeLastEstimate contain a value.
                            (ztimeEndPeriod > ztimeLastEstimateDate ?
                            ztimeEndPeriod : ztimeLastEstimateDate) :
                            //                                  //ZtimeLastEstimate is Null
                            (ztimeLastEstimateDate);

                        //                                  //Update boolen value for period started and period
                        //                                  //      completed.
                        boolPeriodStarted = (perentity.strFinalStartDate != null &&
                            perentity.strFinalEndDate == null) ? true : false;
                        boolPeriodCompleted = (perentity.strFinalEndDate != null) ? true : false;

                        if (
                            perentity.intnContactId != null
                            )
                        {
                            intnContactId = perentity.intnContactId;
                            ResResource.subGetFirstAndLastNameOfEmployee(ps_I.strPrintshopId, (int)intnContactId,
                                darrcontactjson, ref strFirstName, ref strLastName);
                        }
                    }

                    int intAllSeconds = ((int)calentity.intnHours * 3600) + ((int)calentity.intnMinutes * 60) +
                        (int)calentity.intnSeconds;
                    if (
                        calentity.strCalculationType == CalCalculation.strPerQuantity
                        )
                    {
                        intAllSeconds = (int)((((calentity.numnNeeded / calentity.numnPerUnits) *
                            (double)jobjson_I.intnQuantity)) * ((double)(((int)calentity.intnHours * 3600) +
                            ((int)calentity.intnMinutes * 60) + ((int)calentity.intnSeconds)) /
                            (calentity.numnQuantity)));
                    }

                    int intHours = (int)(intAllSeconds / 3600);
                    int intMinutes = (int)((intAllSeconds % 3600) / 60);
                    int intSeconds = (intAllSeconds % 3600) % 60;

                    //                                      //Get PkProcessInWorkflow.
                    int intPkPiw = context.ProcessInWorkflow.FirstOrDefault(
                        piw => piw.intPkWorkflow == piwentity.intPkWorkflow &&
                        piw.intProcessInWorkflowId == calentity.intnProcessInWorkflowId).intPk;

                    //                                      //Get process status.
                    PiwjentityProcessInWorkflowForAJobEntityDB piwjentity =
                        context.ProcessInWorkflowForAJob.FirstOrDefault(piwj => piwj.intJobId ==
                        jobjson_I.intJobId && piwj.intPkProcessInWorkflow == intPkPiw);

                    bool boolProcessIsCompleted = false;
                    if (
                        piwjentity != null
                        )
                    {
                        boolProcessIsCompleted = (
                        piwjentity.intStage == JobJob.intProcessInWorkflowCompleted
                        );
                    }

                    CalperjsonCalculationPeriodJson calperjson = new CalperjsonCalculationPeriodJson(calentity.intPk,
                        calentity.strDescription, intHours, intMinutes, intSeconds, intnPkPeriod, strStartDate,
                        strStartTime, strEndDate, strEndTime, strFirstName, strLastName, intnContactId,
                        boolProcessIsCompleted, intMinsBeforeDelete, boolPeriodStarted, boolPeriodCompleted, null);
                    darrcalperjson.Add(calperjson);
                }

                EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                        ele.intPk == piwentity.intPkProcess);

                String strName = eleentity.strElementName;
                if (
                    piwentity.intnId != null
                    )
                {
                    strName = strName + " (" + piwentity.intnId + ")";
                }

                //                                          //Create json.
                Projson3ProcessJson3 projson3 = new Projson3ProcessJson3(piwentity.intPk, strName,
                    darrcalperjson.ToArray(), arrresperjsonResourcesPeriod);

                if (
                    darrprojson3.Exists(pro =>
                    pro.intPkProcessInWorkflow == projson3.intPkProcessInWorkflow)
                    )
                {
                    darrprojson3.RemoveAll(pro =>
                    pro.intPkProcessInWorkflow == projson3.intPkProcessInWorkflow);
                }
                darrprojson3.Add(projson3);
            }

            strEstimateDate = ztimeTemp == ztimeLastEstimateDate ? null : ztimeLastEstimateDate.Date.ToString();

            PerjobjsonPeriodsJobJson perjobjson = new PerjobjsonPeriodsJobJson(strEstimateDate, darrprojson3.ToArray());

            return perjobjson;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static Resperjson2ResourcePeriodJson2[] arrresperjsonResourcesPeriodForOneProcess(
            //                                              //Evaluate a process and find their inputs calendarized
            //                                              //      type.

            PiwentityProcessInWorkflowEntityDB piwentity_I,
            int intJobId_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            ref ZonedTime ztimeLastEstimateDate_M
            )
        {
            List<Resperjson2ResourcePeriodJson2> darrresperjsonResourcesPeriod = 
                new List<Resperjson2ResourcePeriodJson2>();

            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get the list of IOs for this process.
            List<IoentityInputsAndOutputsEntityDB> darrioentityProcess = context.InputsAndOutputs.Where(io =>
                io.intPkWorkflow == piwentity_I.intPkWorkflow &&
                io.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId).ToList();

            //                                              //Filter list of IOs in order to keep only inputs.
            List<IoentityInputsAndOutputsEntityDB> darrioentityFiltered = new List<IoentityInputsAndOutputsEntityDB>();
            foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentityProcess)
            {
                if (
                    //                                      //It´s a type
                    ioentity.intnPkElementElementType != null
                    )
                {
                    EleetentityElementElementTypeEntityDB eleetentity = context.ElementElementType.FirstOrDefault(et =>
                        et.intPk == (int)ioentity.intnPkElementElementType);
                    if (
                        eleetentity.boolUsage == true
                        )
                    {
                        darrioentityFiltered.Add(ioentity);
                    }
                }
                else
                {
                    //                                      //It´s a template.
                    EleeleentityElementElementEntityDB eleeleentity = context.ElementElement.FirstOrDefault(ele =>
                        ele.intPk == (int)ioentity.intnPkElementElement);
                    if (
                        eleeleentity.boolUsage == true
                        )
                    {
                        darrioentityFiltered.Add(ioentity);
                    }
                }
            }

            //                                              //Get the periods of each resource set in an IO.
            JobJob.subGetResourcesPeriodsSetInAnIO(intJobId_I, ps_I, darrioentityFiltered, piwentity_I, configuration_I,
                ref ztimeLastEstimateDate_M, ref darrresperjsonResourcesPeriod);

            return darrresperjsonResourcesPeriod.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subGetResourcesPeriodsSetInAnIO(
            //                                              //Receive an input and verify if has or
            //                                              //      not period.
            //                                              //1.-Has resource set and has period, return period.
            //                                              //2.-Has resource set and not has period, return the
            //                                              //      necessary info in order to be able to add a period.
            //                                              //3.-No resource set, nothing to do.

            int intJobId_I,
            PsPrintShop ps_I,
            List<IoentityInputsAndOutputsEntityDB> darrioentity_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            IConfiguration configuration_I,
            ref ZonedTime ztimeLastEstimateDate_M,
            ref List<Resperjson2ResourcePeriodJson2> darrresperjsonResourcesPeriod_M
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //To fill and return.
            List<Resperjson3ResourcePeriodJson3> darrresperjson3 = new List<Resperjson3ResourcePeriodJson3>();

            //                                              //Go througth each IO and check if has resource, and if the
            //                                              //      resource has periods.
            foreach (IoentityInputsAndOutputsEntityDB ioentity in darrioentity_I)
            {
                //                                              //To easy code.
                int intPkEleetOrEleele = ioentity.intnPkElementElementType != null ?
                    (int)ioentity.intnPkElementElementType : (int)ioentity.intnPkElementElement;
                bool boolIsEleet = ioentity.intnPkElementElementType != null ? true : false;

                /*CASE*/
                if (
                    //                                      //Has resource set as IO table.
                    ioentity.intnPkResource != null
                    )
                {
                    //                                      //Verify if the resource set is calendar type.
                    EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                        ele.intPk == (int)ioentity.intnPkResource);
                    if (
                        eleentity.boolnIsCalendar == true
                        )
                    {
                        int intPkResource = eleentity.intPk;
                        String strResourceName = eleentity.strElementName;

                        //                                  //Get periods for an especific resource.
                        darrresperjson3.AddRange(JobJob.darrresperjsonGetResourcesPeriods(intPkEleetOrEleele,
                            boolIsEleet, piwentity_I.intPkWorkflow, piwentity_I.intProcessInWorkflowId,
                            (int)ioentity.intnPkResource, intJobId_I, ps_I,
                            configuration_I, piwentity_I.intPk, ref ztimeLastEstimateDate_M));

                        //                                          //Add Information.
                        Resperjson2ResourcePeriodJson2 resperjson2 = new Resperjson2ResourcePeriodJson2(strResourceName,
                            intPkResource, intPkEleetOrEleele, boolIsEleet, darrresperjson3.ToArray());
                        darrresperjsonResourcesPeriod_M.Add(resperjson2);
                    }
                } else if (
                     //                                      //IO has a group, verify at IOJ if there is a resource set.
                     ioentity.intnGroupResourceId != null
                     )
                {
                    IojentityInputsAndOutputsForAJobEntityDB iojentity =
                        context.InputsAndOutputsForAJob.FirstOrDefault(ioj =>
                        ioj.intPkProcessInWorkflow == piwentity_I.intProcessInWorkflowId &&
                        ioj.intnPkElementElementType == ioentity.intnPkElementElementType &&
                        ioj.intnPkElementElement == ioentity.intnPkElementElement &&
                        ioj.intJobId == intJobId_I);

                    if (
                        //                                  //There is a resource set.
                        iojentity != null
                        )
                    {
                        //                                      //Verify if the resource set is calendar type.
                        EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                            ele.intPk == (int)iojentity.intPkResource);
                        if (
                            eleentity.boolnIsCalendar == true
                            )
                        {
                            int intPkResource = eleentity.intPk;
                            String strResourceName = eleentity.strElementName;

                            //                                  //Get periods for an especific resource.
                            darrresperjson3.AddRange(JobJob.darrresperjsonGetResourcesPeriods(intPkEleetOrEleele,
                                boolIsEleet, piwentity_I.intPkWorkflow,
                                piwentity_I.intProcessInWorkflowId, iojentity.intPkResource, intJobId_I, ps_I,
                                configuration_I, piwentity_I.intPk, ref ztimeLastEstimateDate_M));

                            //                                          //Add Information.
                            Resperjson2ResourcePeriodJson2 resperjson2 = new Resperjson2ResourcePeriodJson2(strResourceName,
                                intPkResource, intPkEleetOrEleele, boolIsEleet, darrresperjson3.ToArray());
                            darrresperjsonResourcesPeriod_M.Add(resperjson2);
                        }
                    }
                }
                /*END-CASE*/
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static List<Resperjson3ResourcePeriodJson3> darrresperjsonGetResourcesPeriods(
            //                                              //Receive a IO with resources and get his periods.
            //                                              //1.-Has periods, return periods.
            //                                              //2.-Not has period, return the necessary info in order
            //                                              //      to be able to add a periods.

            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            int intPkWorkflow_I,
            int intProcessInWorkflowId_I,
            int intPkResource_I,
            int intJobId_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            int intPkProcessInWorkflow_I,
            ref ZonedTime ztimeLastEstimateDate_M
            )
        {
            List<Resperjson3ResourcePeriodJson3> darrresperjson3 = new List<Resperjson3ResourcePeriodJson3>();
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                               //Get all the periods for the given resource.
            List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per => per.intJobId == intJobId_I &&
               per.intPkWorkflow == intPkWorkflow_I && per.intProcessInWorkflowId == intProcessInWorkflowId_I &&
               per.intPkElement == intPkResource_I).ToList();

            if (
                //                                          //Resource has period.
                darrperentity.Count() > 0
                )
            {
                //                                          //Get employess to avoid wisnet request lot times.
                Empljson2EmployeeJson2 empljson2;
                int intStatus = 200;
                String strUserMessage = "";
                String strDevMessage = "";
                PsPrintShop.subGetEmployees(false, ps_I, out empljson2, ref intStatus, ref strUserMessage, ref strDevMessage);
                foreach (PerentityPeriodEntityDB perentity in darrperentity)
                {
                    //                                      //Get employee's name.
                    String strFirstName = "";
                    String strLastNameName = "";
                    bool boolPeriodStarted = false;
                    bool boolPeriodCompleted = false;
                    if (
                        perentity.intnContactId != null
                        )
                    {
                        if (
                            empljson2.arrEmployee.Length > 0
                            )
                        {
                            strFirstName = (empljson2.arrEmployee.FirstOrDefault(empl =>
                                empl.intContactId == (int)perentity.intnContactId) != null) ?
                                empljson2.arrEmployee.FirstOrDefault(empl =>
                                empl.intContactId == (int)perentity.intnContactId).strFirstName : strFirstName;

                            strLastNameName = (empljson2.arrEmployee.FirstOrDefault(empl =>
                                empl.intContactId == (int)perentity.intnContactId) != null) ?
                                empljson2.arrEmployee.FirstOrDefault(empl =>
                                empl.intContactId == (int)perentity.intnContactId).strLastName : strLastNameName;
                        }
                    }

                    boolPeriodStarted = perentity.strFinalStartDate != null ? true : false;
                    boolPeriodCompleted = perentity.strFinalEndDate != null ? true : false;

                    ZonedTime ztimeStartPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        perentity.strStartDate.ParseToDate(), perentity.strStartTime.ParseToTime(),
                        ps_I.strTimeZone);

                    ZonedTime ztimeEndPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                       perentity.strEndDate.ParseToDate(), perentity.strEndTime.ParseToTime(), ps_I.strTimeZone);

                    ztimeLastEstimateDate_M = ztimeLastEstimateDate_M != null ?
                        //                                  //ZtimeLastEstimate contain a value.
                        (ztimeEndPeriod > ztimeLastEstimateDate_M ?
                        ztimeEndPeriod : ztimeLastEstimateDate_M) :
                        //                                  //ZtimeLastEstimate is Null
                        (ztimeEndPeriod);

                    Resperjson3ResourcePeriodJson3 resperjson = new Resperjson3ResourcePeriodJson3(perentity.intPk,
                        null, ztimeStartPeriod.Date.ToString(), ztimeStartPeriod.Time.ToString(), 
                        ztimeEndPeriod.Date.ToString(), ztimeEndPeriod.Time.ToString(), strFirstName, strLastNameName,
                        perentity.intnContactId, perentity.intMinsBeforeDelete, boolPeriodStarted, boolPeriodCompleted);
                    darrresperjson3.Add(resperjson);
                }
            }
            else
            {
                //                                          //Get Estimation duration.
                int intEstimateonDuration = JobJob.intGetEstimationDurationForAResource(intPkEleetOrEleele_I,
                    boolIsEleet_I, intPkProcessInWorkflow_I, intPkResource_I, intJobId_I, ps_I, configuration_I);

                //                                          //Resource has not not.
                Resperjson3ResourcePeriodJson3 resperjson = new Resperjson3ResourcePeriodJson3(null,
                    intEstimateonDuration, "", "", "", "", "", "", null, null, false, false);
                darrresperjson3.Add(resperjson);
            }

            return darrresperjson3;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static int intGetEstimationDurationForAResource(
            //                                              //Get the estimation duration for a resource.

            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            int intPkProcessInWorkflow_I,
            int intPkResource_I,
            int intJobId_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I
            )
        {
            List<PerentityPeriodEntityDB> darrperentityNotUsed = new List<PerentityPeriodEntityDB>();
            ZonedTime ztimeBaseNow = ZonedTimeTools.NewZonedTime(Date.Now(ZonedTimeTools.timezone),
                    Time.Now(ZonedTimeTools.timezone));
            int intOffsetTimeMinuteAfterDateNow = 2;

            int intStatus = 400;
            String strUserMessage = "";
            String strDevMessage = "";
            TimesjsonTimesJson timesjson;
            long longMilisecondNeeded = 0;
            ResResource.subGetAvailableTime(ps_I.strPrintshopId, intPkResource_I, intJobId_I, intPkProcessInWorkflow_I,
                intPkEleetOrEleele_I, boolIsEleet_I, ztimeBaseNow, intOffsetTimeMinuteAfterDateNow,
                darrperentityNotUsed, configuration_I, null, out timesjson, ref intStatus,
                ref strUserMessage, ref strDevMessage, ref longMilisecondNeeded);

            return (int)longMilisecondNeeded / 60000;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetProcessesAndCalculationsWithPeriods(
            String strPrintshopId_I,
            int intJobId_I,
            int intPkWorkflow_I,
            String strSunday_I,
            IConfiguration configuration_I,
            out Projson2ProcessJson2[] arrprojson2_O,
            ref int intStatus_IO,
            ref String strDevMessage_IO,
            ref String strUserMessage_IO)
        {
            arrprojson2_O = null;

            JobjsonJobJson jobjson = new JobjsonJobJson();
            if (
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                intStatus_IO = 404;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No printshop found.";
                if (
                    ps != null
                    )
                {
                    Odyssey2Context context = new Odyssey2Context();

                    WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                        wf.intPk == intPkWorkflow_I);

                    intStatus_IO = 405;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "No workflow found.";
                    if (
                        wfentity != null
                        )
                    {
                        arrprojson2_O = JobJob.arrprojson2GetProcessWithCalculationsForTheJob(jobjson,
                            intPkWorkflow_I, strPrintshopId_I, ps.strTimeZone);

                        intStatus_IO = 200;
                        strUserMessage_IO = "";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static Projson2ProcessJson2[] arrprojson2GetProcessWithCalculationsForTheJob(
            JobjsonJobJson jobjson_I,
            int intPkWorkflow_I,
            String strPrintshopId_I,
            String strTimeZoneId_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get all piw.
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentity = context.ProcessInWorkflow.Where(piw =>
                piw.intPkWorkflow == intPkWorkflow_I).ToList();

            List<Projson2ProcessJson2> darrprojson2 = new List<Projson2ProcessJson2>();
            foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentity)
            {
                List<CalentityCalculationEntityDB> darrcalentity;
                JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job => job.intJobID == jobjson_I.intJobId);
                if (
                    //                                      //Job in progress o completed.
                    jobentity != null
                    )
                {
                    List<CalentityCalculationEntityDB> darrcalentityAll = context.Calculation.Where(cal =>
                    cal.intnPkWorkflow == piwentity.intPkWorkflow && cal.boolIsEnable == true &&
                    cal.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId && cal.intnHours != null &&
                    cal.intnPkElementElement == null && cal.intnPkElementElementType == null).ToList().Where(cal =>
                    Tools.boolCalculationOrLinkApplies(cal.intPk, null, null, null, jobjson_I)).ToList();

                    //                                      //To easy code.
                    ZonedTime ztimeStartJob = ZonedTimeTools.NewZonedTime(jobentity.strStartDate.ParseToDate(),
                        jobentity.strStartTime.ParseToTime());

                    darrcalentity = new List<CalentityCalculationEntityDB>();
                    foreach (CalentityCalculationEntityDB calentity in darrcalentityAll)
                    {
                        //                                  //To easy code.
                        ZonedTime ztimeStartCal = ZonedTimeTools.NewZonedTime(calentity.strStartDate.ParseToDate(),
                            calentity.strStartTime.ParseToTime());

                        ZonedTime ztimeEndCal = ZonedTimeTools.NewZonedTime(Date.MaxValue, Time.MinValue);
                        if (
                            calentity.strEndDate != null
                            )
                        {
                            ztimeEndCal = ZonedTimeTools.NewZonedTime(calentity.strEndDate.ParseToDate(),
                                calentity.strEndTime.ParseToTime());
                        }

                        if (
                            ztimeStartJob >= ztimeStartCal &&
                            ztimeStartJob < ztimeEndCal
                            )
                        {
                            darrcalentity.Add(calentity);
                        }
                    }
                }
                else
                {
                    darrcalentity = context.Calculation.Where(cal => cal.intnPkWorkflow == piwentity.intPkWorkflow &&
                    cal.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId && cal.boolIsEnable == true &&
                    cal.intnPkElementElement == null && cal.intnPkElementElementType == null &&
                    cal.intnHours != null && cal.strEndDate == null).ToList().Where(cal =>
                    Tools.boolCalculationOrLinkApplies(cal.intPk, null, null, null, jobjson_I)).ToList();
                }

                List<CalperjsonCalculationPeriodJson> darrcalperjson = new List<CalperjsonCalculationPeriodJson>();

                //                                          //Get all employees from printshop.
                List<ContactjsonContactJson> darrcontactjson = ResResource.darrcontactjsonGetAllEmployee(strPrintshopId_I);

                foreach (CalentityCalculationEntityDB calentity in darrcalentity)
                {

                    PerentityPeriodEntityDB perentity = context.Period.FirstOrDefault(per =>
                        per.intnPkCalculation == calentity.intPk && per.intJobId == jobjson_I.intJobId);

                    String strStartDate = null;
                    String strStartTime = null;
                    String strEndDate = null;
                    String strEndTime = null;
                    int? intnPkPeriod = null;
                    String strFirstName = null;
                    String strLastName = null;
                    int? intnContactId = null;
                    int intMinsBeforeDelete = 0;
                    bool boolPeriodStarted = false;
                    bool boolPeriodCompleted = false;
                    if (
                        perentity != null
                        )
                    {
                        ZonedTime ztimeStartPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                            perentity.strStartDate.ParseToDate(), perentity.strStartTime.ParseToTime(),
                            strTimeZoneId_I);
                        ZonedTime ztimeEndPeriod = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                            perentity.strEndDate.ParseToDate(), perentity.strEndTime.ParseToTime(),
                            strTimeZoneId_I);

                        strStartDate = ztimeStartPeriod.Date.ToString();
                        strStartTime = ztimeStartPeriod.Time.ToString();
                        strEndDate = ztimeEndPeriod.Date.ToString();
                        strEndTime = ztimeEndPeriod.Time.ToString();
                        intnPkPeriod = perentity.intPk;
                        intMinsBeforeDelete = perentity.intMinsBeforeDelete;

                        //                                  //Update boolen value for period started and period
                        //                                  //      completed.
                        boolPeriodStarted = (perentity.strFinalStartDate != null &&
                            perentity.strFinalEndDate == null) ? true : false;
                        boolPeriodCompleted = (perentity.strFinalStartDate != null &&
                            perentity.strFinalEndDate != null) ? true : false;

                        if (
                            perentity.intnContactId != null
                            )
                        {
                            intnContactId = perentity.intnContactId;
                            ResResource.subGetFirstAndLastNameOfEmployee(strPrintshopId_I, (int)intnContactId,
                                darrcontactjson, ref strFirstName, ref strLastName);
                        }
                    }

                    int intAllSeconds = ((int)calentity.intnHours * 3600) + ((int)calentity.intnMinutes * 60) +
                        (int)calentity.intnSeconds;
                    if (
                        calentity.strCalculationType == CalCalculation.strPerQuantity
                        )
                    {
                        intAllSeconds = (int)((((calentity.numnNeeded / calentity.numnPerUnits) *
                            (double)jobjson_I.intnQuantity)) * ((double)(((int)calentity.intnHours * 3600) +
                            ((int)calentity.intnMinutes * 60) + ((int)calentity.intnSeconds)) /
                            (calentity.numnQuantity)));
                    }

                    int intHours = (int)(intAllSeconds / 3600);
                    int intMinutes = (int)((intAllSeconds % 3600) / 60);
                    int intSeconds = (intAllSeconds % 3600) % 60;

                    //                                      //Get PkProcessInWorkflow.
                    int intPkPiw = context.ProcessInWorkflow.FirstOrDefault(
                        piw => piw.intPkWorkflow == intPkWorkflow_I &&
                        piw.intProcessInWorkflowId == calentity.intnProcessInWorkflowId).intPk;

                    //                                      //Get process status.
                    PiwjentityProcessInWorkflowForAJobEntityDB piwjentity =
                        context.ProcessInWorkflowForAJob.FirstOrDefault(piwj => piwj.intJobId ==
                        jobjson_I.intJobId && piwj.intPkProcessInWorkflow == intPkPiw);

                    bool boolProcessIsCompleted = false;
                    if (
                        piwjentity != null
                        )
                    {
                        boolProcessIsCompleted = (
                        piwjentity.intStage == JobJob.intProcessInWorkflowCompleted
                        );
                    }

                    CalperjsonCalculationPeriodJson calperjson = new CalperjsonCalculationPeriodJson(calentity.intPk,
                        calentity.strDescription, intHours, intMinutes, intSeconds, intnPkPeriod, strStartDate,
                        strStartTime, strEndDate, strEndTime, strFirstName, strLastName, intnContactId,
                        boolProcessIsCompleted, intMinsBeforeDelete, boolPeriodStarted, boolPeriodCompleted, null);
                    darrcalperjson.Add(calperjson);

                    if (
                        darrcalperjson.Count > 0
                        )
                    {
                        EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                        ele.intPk == piwentity.intPkProcess);

                        String strName = eleentity.strElementName;
                        if (
                            piwentity.intnId != null
                            )
                        {
                            strName = strName + " (" + piwentity.intnId + ")";
                        }

                        Projson2ProcessJson2 projson2 = new Projson2ProcessJson2(piwentity.intPk, strName,
                            darrcalperjson.ToArray());

                        if (
                            darrprojson2.Exists(pro =>
                            pro.intPkProcessInWorkflow == projson2.intPkProcessInWorkflow)
                            )
                        {
                            darrprojson2.RemoveAll(pro =>
                            pro.intPkProcessInWorkflow == projson2.intPkProcessInWorkflow);
                        }
                        darrprojson2.Add(projson2);
                    }
                }
            }

            return darrprojson2.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolIsValidJobId(
            //                                              //Find job in wisnet. If this one exists, return true.
            //                                              //Create jobjson.

            //                                              //WHEN JBOJSON TABLE IS MODIFIED, VERIFY IF THIS METHOD 
            //                                              //      NEEDS TO BE MODIFY TOO.

            int intJobId_I,
            //bool boolGetLocal_I,
            String strPrintshopId_I,
            IConfiguration configuration_I,
            out JobjsonJobJson jobjsonJob_O,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            jobjsonJob_O = null;
            bool boolIsValidJobId = false;

            //                                              //Verify if job still existing in wisnet.
            if (
                JobJob.boolJobStillExistingInWisnet(intJobId_I, ref strUserMessage_IO, ref strDevMessage_IO) 
                ||
                //                                          //Temporary Job for price functionality.
                intJobId_I == -999

                )
            {
                //                                          //Transform string to integer.
                int intPrintshopId = strPrintshopId_I.ParseToInt();

                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                if (
                    true
                    //boolGetLocal_I
                    )
                {
                    //                                          //Find job if exists in DB.
                    JobjsonentityJobJsonEntityDB jobjsonentity = context.JobJson.FirstOrDefault(jobjsonentity =>
                    jobjsonentity.intJobID == intJobId_I);

                    if (
                        //                                      //Exists in DB.
                        jobjsonentity != null &&
                        jobjsonentity.jobjson != null
                        )
                    {
                        //                                      //Build Jobjson.
                        jobjsonJob_O = JsonSerializer.Deserialize<JobjsonJobJson>(jobjsonentity.jobjson);
                        boolIsValidJobId = true;
                    }
                }


                if (
                    //!boolGetLocal_I ||
                    //                                      //Not found.
                    jobjsonJob_O == null ||
                    //                                      //There is already a jobjson but we have to updated because
                    //                                      //      boolnOdyssey2Pricing propertie is null which means
                    //                                      //      we do not know if the price of the job was 
                    //                                      //      calculated using Odyssey2.0.
                    (jobjsonJob_O != null && jobjsonJob_O.boolnOdyssey2Pricing == null)
                    )
                {
                    //                                          //Go to Wisnet.

                    Task<List<JobjsonJobJson>> Task_jobjsonFromWisnet = HttpTools<JobjsonJobJson>.GetListAsyncToEndPoint(
                          configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") + "/PrintshopData/job/" +
                          intJobId_I + "/" + intPrintshopId);
                    Task_jobjsonFromWisnet.Wait();

                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "There is not a connection to Wisnet.";
                    if (
                        Task_jobjsonFromWisnet.Result != null
                        )
                    {
                        List<JobjsonJobJson> darrjobjsonFromWisnet = new List<JobjsonJobJson>();
                        darrjobjsonFromWisnet = Task_jobjsonFromWisnet.Result;

                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "There is more than one job with that id in Wisnet.";
                        if (
                            darrjobjsonFromWisnet.Count == 1
                            )
                        {
                            JobjsonJobJson jobjson = darrjobjsonFromWisnet[0];

                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "No job found on Wisnet.";
                            if (
                                jobjson.intJobId != -1
                                )
                            {
                                //                          //It´s a fromScratch Estimate.
                                if (
                                    jobjson.intnOrderId == null
                                    )
                                {
                                    //                      //Verify if the printshops has already estimates added and
                                    //                      //      add the new fromScratch.
                                    JobJob.subUpdteVerifyOrUpdateEstimateNumber(jobjson.intJobId, strPrintshopId_I,
                                        configuration_I);
                                }

                                jobjsonJob_O = jobjson;
                                boolIsValidJobId = true;

                                JobjsonentityJobJsonEntityDB jobjsonentity = context.JobJson.FirstOrDefault(job =>
                                    job.intJobID == intJobId_I &&
                                    job.strPrintshopId == strPrintshopId_I);

                                if (
                                    jobjsonentity == null
                                    )
                                {
                                    //                              //Add job to DB.
                                    JobjsonentityJobJsonEntityDB jobjsonentityNew = new JobjsonentityJobJsonEntityDB
                                    {
                                        intJobID = jobjsonJob_O.intJobId,
                                        strPrintshopId = strPrintshopId_I,
                                        intOrderId = (int)jobjsonJob_O.intnOrderId,
                                        jobjson = JsonSerializer.Serialize(jobjsonJob_O)
                                    };
                                    context.JobJson.Add(jobjsonentityNew);
                                }
                                else
                                {
                                    jobjsonentity.jobjson = JsonSerializer.Serialize(jobjsonJob_O);
                                    context.JobJson.Update(jobjsonentity);
                                }

                                bool boolHasNotes =
                                    (context.ProcessNotes.FirstOrDefault(job => job.intJobID == intJobId_I) != null) &&
                                    (context.JobNotes.FirstOrDefault(job => job.intJobID == intJobId_I) != null) ?
                                    true : false;

                                if (
                                    //                      //It was a reorder.
                                    jobjson.intnReorderedFromJobID != null &&
                                    !boolHasNotes
                                    )
                                {
                                    //                      //Find the workflow with the job father was completed.
                                    JobentityJobEntityDB jobCompleted = context.Job.FirstOrDefault(job => 
                                        job.intJobID == (int)jobjson.intnReorderedFromJobID 
                                        && job.intStage == JobJob.intCompletedStage);

                                    if (
                                        //                  //Only if the job father was completed in Odyssey2, 
                                        //                  //    the notes can be copied.
                                        jobCompleted != null
                                        )
                                    {
                                        //                  //Get job note from job father.
                                        JobnotesJobNotesEntityDB jobnotesFather = context.JobNotes.FirstOrDefault(jn =>
                                            jn.intJobID == jobjson.intnReorderedFromJobID);

                                        //                  //Get JobJson of the father.
                                        JobjsonentityJobJsonEntityDB jobjsonentityFather = context.JobJson.FirstOrDefault(
                                            job => job.intJobID == (int)jobjson.intnReorderedFromJobID &&
                                            job.strPrintshopId == strPrintshopId_I);

                                        if (
                                            jobnotesFather != null
                                            )
                                        {
                                            int? intnOrderNumberNotUsed;
                                            int? intnJobNumberNotUsed;

                                            //              //Build the new note.
                                            String strNewNote = JobJob.strBuildTheNewNoteOrGetFatherOrderJobNumber(
                                                jobnotesFather.strOdyssey2Note, out intnOrderNumberNotUsed, out intnJobNumberNotUsed);

                                            //              //Add note to son.
                                            JobnotesJobNotesEntityDB jobnotesSon = new JobnotesJobNotesEntityDB
                                            {
                                                intJobID = intJobId_I,
                                                intnContactId = jobnotesFather.intnContactId,
                                                strOdyssey2Note = $"Job {jobjsonentityFather.intnOrderNumber} - " +
                                                $"{jobjsonentityFather.intnJobNumber} : {strNewNote}",
                                                strDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                                strTime = Time.Now(ZonedTimeTools.timezone).ToString()
                                            };
                                            context.JobNotes.Add(jobnotesSon);
                                            context.SaveChanges();
                                        }

                                        //                  //Get Process Notes of the father job.
                                        List<PronotesentityProcessNotesEntityDB> darrpronotesentityFromFather =
                                            (from piw in context.ProcessInWorkflow
                                             join pronote in context.ProcessNotes
                                             on piw.intPk equals pronote.intPkProcessInworkflow
                                             where piw.intPkWorkflow == jobCompleted.intPkWorkflow &&
                                             pronote.intJobID == jobjsonentityFather.intJobID
                                             select pronote).ToList();

                                        foreach (PronotesentityProcessNotesEntityDB pronote in darrpronotesentityFromFather)
                                        {
                                            int? intnOrderNumberNotUsed;
                                            int? intnJobNumberNotUsed;
                                            //              //Build the new note.
                                            String strNewNote = JobJob.strBuildTheNewNoteOrGetFatherOrderJobNumber(
                                                pronote.strText, out intnOrderNumberNotUsed, out intnJobNumberNotUsed);

                                            //              //Add note to a process.
                                            PronotesentityProcessNotesEntityDB pronotesentitySave = new PronotesentityProcessNotesEntityDB
                                            {
                                                intJobID = intJobId_I,
                                                strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                                                strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                                                intContactId = pronote.intContactId,
                                                strText = $"Job {jobjsonentityFather.intnOrderNumber} - " +
                                                $"{jobjsonentityFather.intnJobNumber} : {strNewNote}",
                                                intPkProcessInworkflow = pronote.intPkProcessInworkflow
                                            };
                                            context.ProcessNotes.Add(pronotesentitySave);
                                            context.SaveChanges();
                                        }
                                    }
                                }
                                context.SaveChanges();
                            }
                        }
                    }
                }

                if (
                    jobjsonJob_O != null && jobjsonJob_O.intnProductKey != null
                    )
                {
                    JobJob.subCreateProductDummy(strPrintshopId_I);
                }                
            }
            return boolIsValidJobId;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static String strBuildTheNewNoteOrGetFatherOrderJobNumber(
            //                                              //Automaton that checks if the initial string has to be 
            //                                              //    removed.
            //                                              //The string should follow the formate "Job ## - ## :" for 
            //                                              //    this string part can be removed.
            //                                              //Also return the Order number and Job Number Father.

            String strNote_I,
            out int? intnOrderNumber_O,
            out int? intnJobNumber_O
            )
        {
            //                                              //Init Order number and Job Number.
            intnOrderNumber_O = null;
            intnJobNumber_O = null;
            String strOrderNumber = "";
            String strJobNumber = "";

            String strNoteTemp = strNote_I.ToString();
            String strNoteReturn;
            String strCurretStage = "A";

            //                                              //Verify if the note started with Job # - # :
            int intEndToRemove = 0;

            if (
                strNoteTemp.StartsWith("Job ")
                )
            {
                strCurretStage = "B";
                strNoteTemp = strNoteTemp.Substring(4);
                intEndToRemove = 4;

                bool boolNextCharIsNumber = true;
                /*REPEAT-WHILE*/
                while (
                    boolNextCharIsNumber
                    )
                {
                    char charNext = strNoteTemp.Length > 0 ? strNoteTemp[0] : 
                        //                                  //assigne other caracter for leave the cicle.
                        'x';
                    if (
                        //                                  //Next char is a number.
                        charNext >= '0' && charNext <= '9'
                        )
                    {
                        //                                  //Remove this caracter from the string.
                        boolNextCharIsNumber = true;
                        strNoteTemp = strNoteTemp.Substring(1);
                        intEndToRemove = intEndToRemove + 1;

                        //                                  //Concat number of the order number.
                        strOrderNumber = strOrderNumber + charNext.ToString();
                    }
                    else
                    {
                        if (
                            //                              //strOrderNumber Is not null or empty.
                            !String.IsNullOrEmpty(strOrderNumber)
                            )
                        {
                            intnOrderNumber_O = strOrderNumber.ParseToInt();
                        }

                        boolNextCharIsNumber = false;
                    }
                }

                if (
                    strNoteTemp.StartsWith(" - ")
                    )
                {
                    strCurretStage = "C";
                    strNoteTemp = strNoteTemp.Substring(3);
                    intEndToRemove = intEndToRemove + 3;

                    boolNextCharIsNumber = true;
                    /*REPEAT-WHILE*/
                    while (
                        boolNextCharIsNumber
                        )
                    {
                        char charNext = strNoteTemp.Length > 0 ? strNoteTemp[0] :
                        //                                  //Assigne other caracter for leave the cicle.
                        'x';

                        if (
                            //                              //Next char is a number.
                            charNext >= '0' && charNext <= '9'
                            )
                        {
                            //                              //Remove this caracter from the string.
                            boolNextCharIsNumber = true;
                            strNoteTemp = strNoteTemp.Substring(1);
                            intEndToRemove = intEndToRemove + 1;

                            //                              //Concat number of the job number.
                            strJobNumber = strJobNumber + charNext.ToString();
                        }
                        else
                        {
                            if (
                                //                          //strJobNumber Is not null or empty.
                                !String.IsNullOrEmpty(strJobNumber)
                            )
                            {
                                intnJobNumber_O = strJobNumber.ParseToInt();
                            }
                            boolNextCharIsNumber = false;
                        }
                    }

                    if (
                        //                                  //STATUS FINAL.
                        strNoteTemp.StartsWith(" :")
                        )
                    {
                        strCurretStage = "D";
                        strNoteTemp = strNoteTemp.Substring(2);
                        intEndToRemove = intEndToRemove + 2;

                        bool boolNextCharIsSpace = true;
                        //                                  //Increment counter for remove the next spaces.
                        /*REPEAT-WHILE*/
                        while (
                            boolNextCharIsSpace
                            )
                        {
                            char charNext = strNoteTemp.Length > 0 ? strNoteTemp[0] :
                            //                              //assigne other caracter for leave the cicle.
                            'x';

                            if (
                                //                          //Next char is a space.
                                charNext == ' '
                                )
                            {
                                //                          //Remove this caracter from the string.
                                boolNextCharIsSpace = true;
                                strNoteTemp = strNoteTemp.Substring(1);
                                intEndToRemove = intEndToRemove + 1;
                            }
                            else
                            {
                                boolNextCharIsSpace = false;
                            }
                        }
                    }
                }
            }

            if (
                //                                          //Automate arrive to status final.
                strCurretStage == "D"
                )
            {
                //                                          //Remove one part of the start string.
                strNoteReturn = strNote_I.Substring(intEndToRemove);
            }
            else
            {
                strNoteReturn = strNote_I;
            }

            return strNoteReturn;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static bool boolJobStillExistingInWisnet(
            //                                              //Verify if the printshop's productKey dummy not exists in
            //                                              //      DB and created if necessary.

            int intJobId_I,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolJobStillExistingInWisnet = false;

            //                                              //Create List to send to API.
            List<int> darrintJobIdToVerify = new List<int>();
            darrintJobIdToVerify.Add(intJobId_I);

            //                                              //Validar el job en wisnet.
            //                                              //Get data from Wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                GetSection("Odyssey2Settings")["urlWisnetApi"];
            Task<DeljobsjsonDeletedJobsJson> Task_darrcatjsonFromWisnet =
                HttpTools<DeljobsjsonDeletedJobsJson>.GetListAsyncJobsDeleted(strUrlWisnet +
                "/PrintShopData/GetJobsDeletedOnWisnet", darrintJobIdToVerify.ToArray());
            Task_darrcatjsonFromWisnet.Wait();

            strUserMessage_IO = "Job has been deleted from website.";
            strDevMessage_IO = "";
            if (
                //                                          //Job still existing in wisnet.
                Task_darrcatjsonFromWisnet.Result.darrintJobsDeleted.Count() == 0
                )
            {
                boolJobStillExistingInWisnet = true;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
            }

            return boolJobStillExistingInWisnet;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subUpdteVerifyOrUpdateEstimateNumber(
            //                                              //Update EstimateNumber for Estimates.

            int intJobId_I,
            String strPrintshopId_I,
            IConfiguration configuration_I
            )
        {
            //                                              //Establish the connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                          //Verify an update/add(if needed) estimates to db.
            bool boolEstimatesWereUpdated;
            JobJob.subUpdateOrAddEstimNumberForAPrintshop(strPrintshopId_I, out boolEstimatesWereUpdated,
                configuration_I, context);

            //                                              //Add new Estimates to jobjson table.
            if (
                //                                          //When estimates were not all updated, update only new
                //                                          //      one.
                !boolEstimatesWereUpdated
                )
            {
                //                                          //Create basic info for the job.
                JobbasicinfojsonJobBasicInfoJson jobbasicinfo = new JobbasicinfojsonJobBasicInfoJson();
                jobbasicinfo.intJobId = intJobId_I;
                jobbasicinfo.intOrderId = -1;

                //                                          //The method expect a list, so we add the only estimate 
                //                                          //      to add in order to works property. 
                List<JobbasicinfojsonJobBasicInfoJson> darrjobstofilllstjsonJobsFromWisnet = 
                    new List<JobbasicinfojsonJobBasicInfoJson>();
                darrjobstofilllstjsonJobsFromWisnet.Add(jobbasicinfo);
                JobJob.subAddNewEstimatesToJobJsonTable(darrjobstofilllstjsonJobsFromWisnet,
                strPrintshopId_I, context);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subCreateProductDummy(
            //                                              //Verify if the printshop's productKey dummy not exists in
            //                                              //      DB and created if necessary.

            String strPrintshopId_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get psPrintshop.
            PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

            //                                              //To easy code.
            int intProductKeyDummy = ("9999" + ps.strPrintshopId).ParseToInt();

            //                                              //Add productKey dummy, if not exists.
            EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                et.intWebsiteProductKey == intProductKeyDummy);

            if (
                //                                          //Product key dummy not exists.
                etentity == null
                )
            {
                //                                          //Create product key dummy.
                EtentityElementTypeEntityDB etentityDummy = new EtentityElementTypeEntityDB
                {
                    strCustomTypeId = "Dummy",
                    intPrintshopPk = ps.intPk,
                    strXJDFTypeId = "None",
                    strAddedBy = ps.strPrintshopId,
                    strResOrPro = "Product",
                    intWebsiteProductKey = intProductKeyDummy,
                    strCategory = "Dummy",
                    boolnIsPublic = false,
                    boolDeleted = false
                };
                context.ElementType.Add(etentityDummy);
                context.SaveChanges();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static int? intnJobProductKey(
            //                                              //Find Product key in wisnet. If this one exists, return true.
            //                                              //Create jobjson.

            int intJobId_I,
            IConfiguration configuration_I
            )
        {
            int? intnProductKey = null;
            //                                          //Go to wisnet.

            Task<PrdkeyjsonProductKeyJson> Task_jobjsonFromWisnet = HttpTools<PrdkeyjsonProductKeyJson>.
                GetOneAsyncToEndPoint(configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                "/PrintshopData/jobproductkey/" + intJobId_I);
            Task_jobjsonFromWisnet.Wait();

            if (
                Task_jobjsonFromWisnet.Result != null
                )
            {
                intnProductKey = Task_jobjsonFromWisnet.Result.intnProductKey;
            }

            return intnProductKey;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetProcessCostEstimateAndFinalFromJob(
            PsPrintShop ps_I,
            int intJobId_I,
            int intPkWorkflow_I,
            IConfiguration configuration_I,
            out FcsumjsonFinalCostsSummaryJson fcsumjson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            fcsumjson_O = null;

            JobjsonJobJson jobjsonJob;
            intStatus_IO = 401;
            //                                              //Validate the Job.
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjsonJob,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                //                                          //Update products in DB App from Wisnet.
                //Dictionary<int, ProdtypProductType> dicprodtem = ps_I.dicprodtyp;

                //EtentityElementTypeEntityDB etentityProduct = context.ElementType.FirstOrDefault(et =>
                //    et.intPrintshopPk == ps_I.intPk &&  et.strCustomTypeId == jobjsonJob.strProductName &&
                //    et.intWebsiteProductKey == jobjsonJob.intnProductKey);

                //                                          //To get the product updated with Wisnet new info.
                ProdtypProductType prodtyp = ProdtypProductType.GetProductTypeUpdated(ps_I,
                    jobjsonJob.strProductName, (int)jobjsonJob.intnProductKey, true);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No product found.";
                if (
                    prodtyp != null
                    )
                {
                    Odyssey2Context context = new Odyssey2Context();
                    //prodtyp = (ProdtypProductType)EtElementTypeAbstract.etFromDB(etentityProduct.intPk);

                    //                                      //Find workflow.
                    WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                        wf.intPk == intPkWorkflow_I);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Workflow not exist.";
                    if (
                        wfentity != null
                        )
                    {
                        List<PiwentityProcessInWorkflowEntityDB> darrpiwentityWithFinalProduct;
                        bool boolWorkflowIsReady;
                        bool? boolnNotUsed;
                        List<PiwentityProcessInWorkflowEntityDB> darrpiwentityProcessesNotReady;
                        ProdtypProductType.subfunWorkflowIsReady(intPkWorkflow_I, out darrpiwentityWithFinalProduct,
                            out boolWorkflowIsReady, out boolnNotUsed, out darrpiwentityProcessesNotReady);

                        intStatus_IO = 404;
                        strUserMessage_IO = "The product's workflow is incomplete. Complete the product's" +
                            " workflow to see the job's workflow.";
                        strDevMessage_IO = "";
                        if (
                            boolWorkflowIsReady
                            )
                        {
                            //ProdtypProductType.subUpdateResourceForAJob(prodtyp, intPkWorkflow_I, jobjsonJob);

                            JobentityJobEntityDB jobCompleted = context.Job.FirstOrDefault(jobentity => 
                                jobentity.intPkPrintshop == ps_I.intPk && jobentity.intPkWorkflow == intPkWorkflow_I && 
                                jobentity.intJobID == intJobId_I && jobentity.intStage == JobJob.intCompletedStage);

                            intStatus_IO = 405;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "The job should be completed.";
                            if (
                                jobCompleted != null
                                )
                            {
                                //                          //Get job's correct processes.
                                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
                                List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                                ProdtypProductType.subGetWorkflowValidWay(intPkWorkflow_I, jobjsonJob,
                                    out darrpiwentityAllProcesses, out darrdynlkjson);

                                List<ProfcsumjsonProcessFinalCostSummaryJson> darrprofcsumjson =
                                        new List<ProfcsumjsonProcessFinalCostSummaryJson>();

                                //                          //To acumulate job final cost.
                                double numJobEstimateCost = 0;
                                double numJobFinalCost = 0;

                                if (
                                    darrpiwentityAllProcesses.Count > 0
                                    )
                                {
                                    ProdtypProductType.subUpdateResourceForAJob(prodtyp, null,
                                        darrpiwentityAllProcesses, jobjsonJob);

                                    //JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job =>
                                    //job.intJobID == intJobId_I && job.intPkWorkflow == intPkWorkflow_I);

                                    //                          //List of normal piw.
                                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityNormalProcess =
                                        darrpiwentityAllProcesses.Where(piw => piw.boolIsPostProcess == false).ToList();

                                    //                          //List of post piw.
                                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityPostProcess =
                                        darrpiwentityAllProcesses.Where(piw => piw.boolIsPostProcess == true).ToList();

                                    //                          //List to add piws.
                                    List<Piwjson1ProcessInWorkflowJson1> darrpiwjson1 =
                                        new List<Piwjson1ProcessInWorkflowJson1>();

                                    bool boolWFJobReady = true;
                                    double numJobExtraCost = 0;
                                    double numJobCostA = 0;

                                    //                      //Dictionary to store inputs and outputs of a process.
                                    prodtyp.dicProcessIOs = new Dictionary<int, List<Iofrmpiwjson2IOFromPIWJson2>>();
                                    //                      //List to store resource thickness.
                                    prodtyp.darrresthkjsonResThickness = new List<ResthkjsonResourceThicknessJson>();

                                    ////                      //Add normal processes to List of piw json.
                                    //ProdtypProductType.AddNormalProcess(jobCompleted, jobjsonJob, prodtyp, ps_I, 
                                    //    darrdynlkjson, darrpiwentityAllProcesses, darrpiwentityNormalProcess, 
                                    //    configuration_I, darrpiwjson1, ref numJobExtraCost, ref numJobCostA, 
                                    //    ref boolWFJobReady);

                                    ////                      //Add post processes to List of piw json.
                                    //ProdtypProductType.AddPostProcess(jobCompleted, jobjsonJob, prodtyp, ps_I, 
                                    //    darrdynlkjson, darrpiwentityAllProcesses, darrpiwentityPostProcess, 
                                    //    configuration_I, darrpiwjson1, ref numJobExtraCost, ref numJobCostA, 
                                    //    ref boolWFJobReady);

                                    //                      //Add normal processes to List of piw json.
                                    JobJob.CostByNormalProcess(darrpiwentityAllProcesses, darrpiwentityNormalProcess,
                                        darrdynlkjson, prodtyp, ps_I, jobCompleted, jobjsonJob, configuration_I, 
                                        darrprofcsumjson, darrpiwjson1, ref numJobExtraCost, ref numJobCostA);

                                    //                      //Add post processes to List of piw json.
                                    JobJob.CostByPostProcess(darrpiwentityAllProcesses, darrpiwentityPostProcess,
                                        darrdynlkjson, prodtyp, ps_I, jobCompleted, jobjsonJob, configuration_I,
                                        darrprofcsumjson, darrpiwjson1, ref numJobExtraCost, ref numJobCostA);

                                    //                      //By product/workflow info.
                                    List<CostbycaljsonCostByCalculationJson> darrcostbycaljson;
                                    double numCostByProduct = prodtyp.numGetCostByProduct(jobjsonJob, ps_I,
                                        out darrcostbycaljson, ref boolWFJobReady);
                                    numJobCostA = numJobCostA + numCostByProduct;

                                    double numJobPrice = 0;
                                    double numJobCost = 0;
                                    double numJobProfit = 0;
                                    double numJobFinalProfit = 0;
                                    ProdtypProductType.subGetJobPriceCostAndProfit(prodtyp, jobjsonJob, 
                                        numCostByProduct, darrpiwjson1, intPkWorkflow_I, ref numJobPrice, 
                                        ref numJobCost, numJobExtraCost, ref numJobProfit, ref numJobCostA, 
                                        ref numJobFinalProfit);

                                    fcsumjson_O = new FcsumjsonFinalCostsSummaryJson(numJobCost.Round(2), 
                                        numJobCostA.Round(2), (numJobCostA - numJobCost).Round(2), 
                                        numJobProfit.Round(2), numJobFinalProfit.Round(2), 
                                        (numJobFinalProfit - numJobProfit).Round(2), numCostByProduct.Round(2),
                                        darrprofcsumjson);
                                }

                                intStatus_IO = 200;
                                strUserMessage_IO = "Success.";
                                strDevMessage_IO = "";
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void CostByNormalProcess(
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcess_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityNormalProcess_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            ProdtypProductType prodtyp_I,
            PsPrintShop ps_I,
            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjson_I,
            IConfiguration configuration_I,
            List<ProfcsumjsonProcessFinalCostSummaryJson> darrprofcsumjson_M, 
            List<Piwjson1ProcessInWorkflowJson1> darrpiwjson1_M,
            //                                              //Cost due to Hourly rate.
            ref double numJobExtraCost_IO,
            ref double numJobFinalCost_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            prodtyp_I.subAddCalculationsBasedOnJobStatus(jobentity_I, darrpiwentityAllProcess_I, context);

            //                                              //List of quantityInputs and quantityOutputs.
            //                                              //    for optimization.
            List<IoqytjsonIOQuantityJson> darrioqytjsonIOQuantity = new List<IoqytjsonIOQuantityJson>();

            //                                              //List of waste to propagate.                          
            List<WstpropjsonWasteToPropagateJson> darrwstpropjson = new List<WstpropjsonWasteToPropagateJson>();

            //                                              //Get the inputs and outputs for every process.
            foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentityNormalProcess_I)
            {
                //                                          //The lists are for optimization

                //                                          //Get eleet-s.
                List<EleetentityElementElementTypeEntityDB> darreleetentityAllEleEt =
                    context.ElementElementType.Where(eleet => eleet.intPkElementDad == piwentity.intPkProcess).ToList();

                //                                          //Get eleele-s.
                List<EleeleentityElementElementEntityDB> darreleeleentityAllEleEle = context.ElementElement.Where(
                    eleele => eleele.intPkElementDad == piwentity.intPkProcess).ToList();

                //                                          //Get io-s.
                List<IoentityInputsAndOutputsEntityDB> darrioentityAllIO = context.InputsAndOutputs.Where(io =>
                    io.intPkWorkflow == piwentity.intPkWorkflow &&
                    io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId).ToList();

                //                                          //Get ioj-s.
                List<IojentityInputsAndOutputsForAJobEntityDB> darriojentityAllIOJ =
                    context.InputsAndOutputsForAJob.Where(ioj => ioj.intPkProcessInWorkflow == piwentity.intPk &&
                    ioj.intJobId == jobjson_I.intJobId).ToList();

                if (
                    !prodtyp_I.dicProcessIOs.ContainsKey(piwentity.intPk)
                    )
                {
                    List<Iofrmpiwjson2IOFromPIWJson2> darrioinfrmpiwjson2IosFromPIW;
                    ProdtypProductType.subGetProcessInputsAndOutputs(jobjson_I, piwentity, prodtyp_I,
                        darreleeleentityAllEleEle, darreleetentityAllEleEt, out darrioinfrmpiwjson2IosFromPIW);

                    prodtyp_I.dicProcessIOs.Add(piwentity.intPk, darrioinfrmpiwjson2IosFromPIW);
                }

                //                                          //Reset final cost by Process.
                double numResourcesFinalCost = 0;

                //                                          //Get the process.
                EleentityElementEntityDB eleentityProcess = context.Element.
                    FirstOrDefault(ele => ele.intPk == piwentity.intPkProcess);

                //                                          //List to Add IO Inputs.
                List<Iojson1InputOrOutputJson1> darriojson1Input = new List<Iojson1InputOrOutputJson1>();

                //                                          //Get the input types.
                bool boolAreInputs = true;
                bool boolGetQuantityAndCost = true;
                bool boolWorkflowJobIsReadyNotUsed = true;
                //darriojson1Input.AddRange(prodtyp_I.arriojson1GetTypes(piwentity, jobjson_I, ps_I.strPrintshopId,
                //    boolAreInputs, boolGetQuantityAndCost, darrpiwentityAllProcess_I, darrdynlkjson_I, configuration_I,
                //    ref numJobFinalCost_IO, ref darrwstpropjson, ref darrioqytjsonIOQuantity,
                //    ref boolWorkflowJobIsReadyNotUsed));

                ////                                          //Get the input templates.
                //darriojson1Input.AddRange(prodtyp_I.arriojson1GetTemplates(piwentity, jobjson_I, ps_I.strPrintshopId,
                //    boolAreInputs, boolGetQuantityAndCost, darrpiwentityAllProcess_I, darrdynlkjson_I, configuration_I,
                //    ref numJobFinalCost_IO, ref darrwstpropjson, ref darrioqytjsonIOQuantity,
                //    ref boolWorkflowJobIsReadyNotUsed));

                darriojson1Input.AddRange(prodtyp_I.arriojson1GetTypes(boolAreInputs, jobentity_I, jobjson_I, piwentity,
                    darrdynlkjson_I, darreleetentityAllEleEt, darrioentityAllIO, darriojentityAllIOJ,
                    darrpiwentityAllProcess_I, darrioqytjsonIOQuantity, darrwstpropjson, ref numResourcesFinalCost,
                    ref boolWorkflowJobIsReadyNotUsed));

                //                                          //Get the input templates.
                darriojson1Input.AddRange(prodtyp_I.arriojson1GetTemplates(boolAreInputs, jobentity_I, jobjson_I,
                    piwentity, darrdynlkjson_I, darreleeleentityAllEleEle, darrioentityAllIO, darriojentityAllIOJ,
                    darrpiwentityAllProcess_I, darrioqytjsonIOQuantity, darrwstpropjson, ref numResourcesFinalCost,
                    ref boolWorkflowJobIsReadyNotUsed));

                IoqytjsonIOQuantityJson ioqytjsonWasPropagate = darrioqytjsonIOQuantity.FirstOrDefault(
                    ioqyt => ioqyt.intPkProcessInWorkflow == piwentity.intPk);

                //                                          //Get index of the current PIW
                int index = Array.IndexOf(darrpiwentityNormalProcess_I.ToArray(), piwentity);

                if (
                    //                                      //This PIW was not analized or is the first PIW.
                    ioqytjsonWasPropagate == null || index == 0
                    )
                {
                    ProdtypProductType.subPropagateWaste(jobjson_I, piwentity, prodtyp_I, darrwstpropjson,
                        configuration_I, ps_I.strPrintshopId, null, ref darriojson1Input);
                }

                ProdtypProductType.CalculateTime(jobjson_I, piwentity, configuration_I, ps_I.strPrintshopId,
                    ref darriojson1Input, null);

                darriojson1Input = darriojson1Input.Where(io => io.strLink == null).ToList();

                //                                          //Total extra cost per process.
                //                                          //This variable must be use for inputs and outputs.
                double numProcessExtraCost = 0.0;

                //                                          //Increase cost to input resources that contain an hourly 
                //                                          //      rate.
                ProdtypProductType.subCalculateResourcesHourlyRates(context, ref darriojson1Input,
                    ref numProcessExtraCost);

                //                                          //Each extra cost generated by process will be added to 
                //                                          //      this variable.
                numJobExtraCost_IO = numJobExtraCost_IO + numProcessExtraCost;

                //                                          //Cost Estimates Resources Input per each PIW.
                List<ResfcsumjsonResourceFinalCostSummaryJson> darrresfcsumjson = new
                    List<ResfcsumjsonResourceFinalCostSummaryJson>();
                double numCostEstimateResourcesByProcess = 0;
                foreach (Iojson1InputOrOutputJson1 iojson1Input in darriojson1Input)
                {
                    //                                      //Add cost estimate.
                    numCostEstimateResourcesByProcess = numCostEstimateResourcesByProcess +
                        iojson1Input.numCostByResource;

                    double numFinalCost = iojson1Input.numCostByResource;

                    //                                      //To easy code.
                    int? intnPkEleet = null;
                    int? intnPkEleele = iojson1Input.intPkEleetOrEleele;
                    if (
                        iojson1Input.boolIsEleet
                        )
                    {
                        intnPkEleet = iojson1Input.intPkEleetOrEleele;
                        intnPkEleele = null;
                    }

                    JobJob job = JobJob.jobFromDB(jobjson_I.intJobId);

                    //                                      //Get the final cost for this resource.
                    List<FnlcostentityFinalCostEntityDB> darrfnlcostentity = context.FinalCost.Where(fnl =>
                    fnl.intnPkElementElement == intnPkEleele && fnl.intnPkElementElementType == intnPkEleet &&
                    fnl.intnPkResource == iojson1Input.intnPkResource && fnl.intPkJob == job.intPk &&
                    fnl.intPkProcessInWorkflow == piwentity.intPk).ToList();
                    
                    if (
                        darrfnlcostentity.Count > 0
                        )
                    {
                        darrfnlcostentity.Sort();
                        FnlcostentityFinalCostEntityDB fnlcostentity = darrfnlcostentity.Last();
                        if (
                            fnlcostentity.numnCost != null
                            )
                        {
                            numFinalCost = (double)fnlcostentity.numnCost;
                        }
                        else
                        {
                            numFinalCost = (double)fnlcostentity.numnQuantity * (iojson1Input.numCostByResource / iojson1Input.numQuantity);
                        }
                    }

                    ResfcsumjsonResourceFinalCostSummaryJson resfcsumjson = new ResfcsumjsonResourceFinalCostSummaryJson(
                        iojson1Input.strResource, iojson1Input.numCostByResource, numFinalCost, numFinalCost -
                        iojson1Input.numCostByResource);
                    darrresfcsumjson.Add(resfcsumjson);
                }

                List<CostbycaljsonCostByCalculationJson> darrcostbycaljsonPerProcess;
                double numProcessFinalCost = 0;
                double numCostByProcess = prodtyp_I.numGetCostByProcess(jobjson_I,
                    piwentity.intPkProcess, piwentity.intPk, ps_I, out darrcostbycaljsonPerProcess,
                    ref numProcessFinalCost, ref boolWorkflowJobIsReadyNotUsed);

                List<CalfcsumjsonCalculationFinalCostSummaryJson> darrcalfcsumjson = new
                    List<CalfcsumjsonCalculationFinalCostSummaryJson>();
                foreach (CostbycaljsonCostByCalculationJson costbycaljson in darrcostbycaljsonPerProcess)
                {
                    CalfcsumjsonCalculationFinalCostSummaryJson calfcsumjson = new
                        CalfcsumjsonCalculationFinalCostSummaryJson(costbycaljson.strDescription, costbycaljson.numCost,
                        costbycaljson.numFinalCost, costbycaljson.numFinalCost - costbycaljson.numCost);
                    darrcalfcsumjson.Add(calfcsumjson);
                }

                //                                          //Add cost Estimates.
                double numCostEstimatetByProcess = numCostEstimateResourcesByProcess + numCostByProcess;

                //                                          //Add cost final.
                double numCosFinalByProcess = numResourcesFinalCost + numProcessFinalCost;

                numJobFinalCost_IO = numJobFinalCost_IO + numCosFinalByProcess;

                String strProcessNameAndId = eleentityProcess.strElementName;
                if (
                    piwentity.intnId != null
                    )
                {
                    strProcessNameAndId = eleentityProcess.strElementName + " (" + piwentity.intnId + ")";
                }

                //                                          //Add Costs to Json.
                ProfcsumjsonProcessFinalCostSummaryJson costestimfinaljsonCost =
                    new ProfcsumjsonProcessFinalCostSummaryJson(piwentity.intPk, strProcessNameAndId,
                    numCostEstimatetByProcess.Round(2), numCosFinalByProcess.Round(2),
                    (numCosFinalByProcess - numCostEstimatetByProcess).Round(2), darrcalfcsumjson, darrresfcsumjson);

                darrprofcsumjson_M.Add(costestimfinaljsonCost);

                //                                          //Json with all of the information about the process.
                Piwjson1ProcessInWorkflowJson1 piwjson1 = new Piwjson1ProcessInWorkflowJson1(
                    piwentity.intPk, eleentityProcess.intPk, strProcessNameAndId, numCostByProcess,
                    darriojson1Input.ToArray(), null, 0, false, false, piwentity.boolIsPostProcess);

                //                                          //Array with information of all the processes in a 
                //                                          //      workflow.
                darrpiwjson1_M.Add(piwjson1);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void CostByPostProcess(
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcess_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityPostProcess_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            ProdtypProductType prodtyp_I,
            PsPrintShop ps_I,
            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjson_I,
            IConfiguration configuration_I,
            List<ProfcsumjsonProcessFinalCostSummaryJson> darrprofcsumjson_M, 
            List<Piwjson1ProcessInWorkflowJson1> darrpiwjson1_M,
            ref double numJobExtraCost_IO,
            ref double numJobFinalCost_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //List of quantityInputs and quantityOutputs
            //                                              //    for optimization.
            List<IoqytjsonIOQuantityJson> darrioqytjsonIOQuantity = new List<IoqytjsonIOQuantityJson>();

            //                                              //Get the inputs and outputs for every process.
            foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentityPostProcess_I)
            {
                //                                          //Get eleet-s.
                List<EleetentityElementElementTypeEntityDB> darreleetentityAll = context.ElementElementType.Where(
                    eleet => eleet.intPkElementDad == piwentity.intPkProcess).ToList();

                //                                          //Get eleele-s.
                List<EleeleentityElementElementEntityDB> darreleeleentityAll = context.ElementElement.Where(
                    eleele => eleele.intPkElementDad == piwentity.intPkProcess).ToList();

                //                                          //Get io-s.
                List<IoentityInputsAndOutputsEntityDB> darrioentityAllIO = context.InputsAndOutputs.Where(io =>
                    io.intPkWorkflow == piwentity.intPkWorkflow &&
                    io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId).ToList();

                //                                          //Get ioj-s.
                List<IojentityInputsAndOutputsForAJobEntityDB> darriojentityAllIOJ =
                    context.InputsAndOutputsForAJob.Where(ioj => ioj.intPkProcessInWorkflow == piwentity.intPk &&
                    ioj.intJobId == jobjson_I.intJobId).ToList();

                if (
                    !prodtyp_I.dicProcessIOs.ContainsKey(piwentity.intPk)
                    )
                {
                    List<Iofrmpiwjson2IOFromPIWJson2> darrioinfrmpiwjson2IosFromPIW;
                    ProdtypProductType.subGetProcessInputsAndOutputs(jobjson_I, piwentity, prodtyp_I,
                        darreleeleentityAll, darreleetentityAll, out darrioinfrmpiwjson2IosFromPIW);

                    prodtyp_I.dicProcessIOs.Add(piwentity.intPk, darrioinfrmpiwjson2IosFromPIW);
                }

                //                                          //Reset final cost by Process.
                double numResourcesFinalCost = 0;

                //                                          //Get the process.
                EleentityElementEntityDB eleentityProcess = context.Element.
                    FirstOrDefault(ele => ele.intPk == piwentity.intPkProcess);

                ////                                          //List to Add IO Inputs.
                //List<Iojson1InputOrOutputJson1> darriojson1Input = new List<Iojson1InputOrOutputJson1>();

                ////                                          //Get the input types.
                //darriojson1Input.AddRange(prodtyp_I.arriojson1GetTypesPostProcess(piwentity, jobjson_I,
                //    ps_I.strPrintshopId, true, true, darrpiwentityAllProcess_I, darrdynlkjson_I, configuration_I,
                //    ref numJobFinalCost, ref darrioqytjsonIOQuantity));

                ////                                          //Get the input templates.
                //darriojson1Input.AddRange(prodtyp_I.arriojson1GetTemplatesPostProcess(piwentity, jobjson_I,
                //    ps_I.strPrintshopId, true, true, darrpiwentityAllProcess_I, darrdynlkjson_I, configuration_I,
                //    ref numJobFinalCost, ref darrioqytjsonIOQuantity));

                ////                                          //List to Add IO Inputs.
                //List<Iojson1InputOrOutputJson1> darriojson1Output = new List<Iojson1InputOrOutputJson1>();

                ////                                          //Get the output types.
                //darriojson1Output.AddRange(prodtyp_I.arriojson1GetTypesPostProcess(piwentity, jobjson_I,
                //    ps_I.strPrintshopId, false, true, darrpiwentityAllProcess_I, darrdynlkjson_I, configuration_I,
                //    ref numJobFinalCost, ref darrioqytjsonIOQuantity));

                ////                                          //Get the input templates.
                //darriojson1Output.AddRange(prodtyp_I.arriojson1GetTemplatesPostProcess(piwentity, jobjson_I,
                //    ps_I.strPrintshopId, false, true, darrpiwentityAllProcess_I, darrdynlkjson_I, configuration_I,
                //    ref numJobFinalCost, ref darrioqytjsonIOQuantity));

                //                                          //List to Add IO Inputs.
                List<Iojson1InputOrOutputJson1> darriojson1Input = new List<Iojson1InputOrOutputJson1>();

                //                                          //Get the input types.
                darriojson1Input.AddRange(prodtyp_I.arriojson1GetTypesPostProcess(true, ps_I.strPrintshopId,
                    jobentity_I, jobjson_I, piwentity, darrdynlkjson_I, darreleetentityAll, darrioentityAllIO,
                    darriojentityAllIOJ, darrpiwentityAllProcess_I, configuration_I, darrioqytjsonIOQuantity,
                    ref numResourcesFinalCost));

                //                                          //Get the input templates.
                darriojson1Input.AddRange(prodtyp_I.arriojson1GetTemplatesPostProcess(true, ps_I.strPrintshopId,
                    jobentity_I, jobjson_I, piwentity, darrdynlkjson_I, darreleeleentityAll, darrioentityAllIO,
                    darriojentityAllIOJ, darrpiwentityAllProcess_I, configuration_I, darrioqytjsonIOQuantity,
                    ref numResourcesFinalCost));

                //                                          //List to Add IO Outputs.
                List<Iojson1InputOrOutputJson1> darriojson1Output = new List<Iojson1InputOrOutputJson1>();

                //                                          //Get the output types.
                darriojson1Output.AddRange(prodtyp_I.arriojson1GetTypesPostProcess(false, ps_I.strPrintshopId,
                    jobentity_I, jobjson_I, piwentity, darrdynlkjson_I, darreleetentityAll, darrioentityAllIO,
                    darriojentityAllIOJ, darrpiwentityAllProcess_I, configuration_I, darrioqytjsonIOQuantity,
                    ref numResourcesFinalCost));

                //                                          //Get the input templates.
                darriojson1Output.AddRange(prodtyp_I.arriojson1GetTemplatesPostProcess(false, ps_I.strPrintshopId,
                    jobentity_I, jobjson_I, piwentity, darrdynlkjson_I, darreleeleentityAll, darrioentityAllIO,
                    darriojentityAllIOJ, darrpiwentityAllProcess_I, configuration_I, darrioqytjsonIOQuantity,
                    ref numResourcesFinalCost));

                ProdtypProductType.CalculateTime(jobjson_I, piwentity, configuration_I, ps_I.strPrintshopId,
                    ref darriojson1Input, null);

                //                                          //Total extra cost per process.
                //                                          //This variable must be use for inputs and outputs.
                double numProcessExtraCost = 0.0;

                //                                          //Increase cost to input resources that contain an hourly 
                //                                          //      rate.
                ProdtypProductType.subCalculateResourcesHourlyRates(context, ref darriojson1Input,
                    ref numProcessExtraCost);

                //                                          //Each extra cost generated by process will be added to 
                //                                          //      this variable.
                numJobExtraCost_IO = numJobExtraCost_IO + numProcessExtraCost;

                //                      //Cost Estimates Resources Input per each PIW.
                List<ResfcsumjsonResourceFinalCostSummaryJson> darrresfcsumjson = new
                    List<ResfcsumjsonResourceFinalCostSummaryJson>();
                double numCostEstimateResourcesByProcess = 0;

                darriojson1Input = darriojson1Input.Where(io => io.strLink == null).ToList();

                foreach (Iojson1InputOrOutputJson1 iojson1Input in darriojson1Input)
                {
                    //                  //Add cost estimate.
                    numCostEstimateResourcesByProcess = numCostEstimateResourcesByProcess +
                        iojson1Input.numCostByResource;

                    double numFinalCost = iojson1Input.numCostByResource;

                    //                                      //To easy code.
                    int? intnPkEleet = null;
                    int? intnPkEleele = iojson1Input.intPkEleetOrEleele;
                    if (
                        iojson1Input.boolIsEleet
                        )
                    {
                        intnPkEleet = iojson1Input.intPkEleetOrEleele;
                        intnPkEleele = null;
                    }

                    JobJob job = JobJob.jobFromDB(jobjson_I.intJobId);

                    //                                      //Get the final cost for this resource.
                    FnlcostentityFinalCostEntityDB fnlcostentity = context.FinalCost.FirstOrDefault(fnl =>
                    fnl.intnPkElementElement == intnPkEleele && fnl.intnPkElementElementType == intnPkEleet &&
                    fnl.intnPkResource == iojson1Input.intnPkResource && fnl.intPkJob == job.intPk &&
                    fnl.intPkProcessInWorkflow == piwentity.intPk);

                    numFinalCost = (fnlcostentity != null) ? (double)fnlcostentity.numnCost : numFinalCost;

                    ResfcsumjsonResourceFinalCostSummaryJson resfcsumjson = new ResfcsumjsonResourceFinalCostSummaryJson(
                        iojson1Input.strResource, iojson1Input.numCostByResource, numFinalCost, numFinalCost -
                        iojson1Input.numCostByResource);
                    darrresfcsumjson.Add(resfcsumjson);
                }

                List<CostbycaljsonCostByCalculationJson> darrcostbycaljsonPerProcess;
                double numProcessFinalCost = 0;
                bool boolWorkflowJobIsReadyNotUsed = false;
                double numCostByProcess = prodtyp_I.numGetCostByProcess(jobjson_I,
                    piwentity.intPkProcess, piwentity.intPk, ps_I, out darrcostbycaljsonPerProcess,
                    ref numProcessFinalCost, ref boolWorkflowJobIsReadyNotUsed);

                List<CalfcsumjsonCalculationFinalCostSummaryJson> darrcalfcsumjson = new
                    List<CalfcsumjsonCalculationFinalCostSummaryJson>();
                foreach (CostbycaljsonCostByCalculationJson costbycaljson in darrcostbycaljsonPerProcess)
                {
                    CalfcsumjsonCalculationFinalCostSummaryJson calfcsumjson = new
                        CalfcsumjsonCalculationFinalCostSummaryJson(costbycaljson.strDescription, 
                        costbycaljson.numCost, costbycaljson.numFinalCost, 
                        costbycaljson.numFinalCost - costbycaljson.numCost);
                    darrcalfcsumjson.Add(calfcsumjson);
                }

                //                                          //Add cost Estimates.
                double numCostEstimatetByProcess = numCostEstimateResourcesByProcess + numCostByProcess;

                //                                          //Add cost final.
                double numCostFinalByProcess = numResourcesFinalCost + numProcessFinalCost;

                numJobFinalCost_IO = numJobFinalCost_IO + numCostFinalByProcess;

                String strProcessNameAndId = eleentityProcess.strElementName;
                if (
                    piwentity.intnId != null
                    )
                {
                    strProcessNameAndId = eleentityProcess.strElementName + " (" +
                        piwentity.intnId + ")";
                }

                //                                          //Add Costs to Json.
                ProfcsumjsonProcessFinalCostSummaryJson costestimfinaljsonCost =
                    new ProfcsumjsonProcessFinalCostSummaryJson(piwentity.intPk, strProcessNameAndId,
                    numCostEstimatetByProcess.Round(2), numCostFinalByProcess.Round(2),
                    (numCostFinalByProcess - numCostEstimatetByProcess).Round(2), darrcalfcsumjson, darrresfcsumjson);

                darrprofcsumjson_M.Add(costestimfinaljsonCost);

                //                                          //Json with all of the information about the process.
                Piwjson1ProcessInWorkflowJson1 piwjson1 = new Piwjson1ProcessInWorkflowJson1(piwentity.intPk, 
                    eleentityProcess.intPk, strProcessNameAndId, numCostByProcess, darriojson1Input.ToArray(),
                    darriojson1Output.ToArray(), 0, false, false, piwentity.boolIsPostProcess);

                //                                          //Array with information of all the processes in a 
                //                                          //      workflow.
                darrpiwjson1_M.Add(piwjson1);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetProcessFinalCostLog(
            String strPrintshopId_I,
            int intPkFinal_I,
            out Fnlcostjson2FinalCostJson2[] fnlcostjson2_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            fnlcostjson2_O = null;

            List<Fnlcostjson2FinalCostJson2> darrfnlcostjson2Log = new List<Fnlcostjson2FinalCostJson2>();

            //                                              //Initialize connection.
            Odyssey2Context context = new Odyssey2Context();

            FnlcostentityFinalCostEntityDB fnlcostentityCurrent = context.FinalCost.FirstOrDefault(
                fnlcost => fnlcost.intPk == intPkFinal_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "Could no get the final entity from this pk.";
            if (
                fnlcostentityCurrent != null
                )
            {
                List<FnlcostentityFinalCostEntityDB> darrfnlcostentityLog = context.FinalCost.Where(fnlcost =>
                    fnlcost.intnPkResource == fnlcostentityCurrent.intnPkResource &&
                    fnlcost.intnPkCalculation == fnlcostentityCurrent.intnPkCalculation &&
                    fnlcost.intPkJob == fnlcostentityCurrent.intPkJob &&
                    fnlcost.intPkProcessInWorkflow == fnlcostentityCurrent.intPkProcessInWorkflow &&
                    fnlcost.intnPkElementElementType == fnlcostentityCurrent.intnPkElementElementType &&
                    fnlcost.intnPkElementElement == fnlcostentityCurrent.intnPkElementElement
                    ).ToList();
                darrfnlcostentityLog.Sort();

                //                                          //Get the cost of the res or cal.
                double numCost = 0;
                if (
                    fnlcostentityCurrent.intnPkResource != null
                    )
                {
                    ResResource res = ResResource.resFromDB(fnlcostentityCurrent.intnPkResource, false);
                    if (
                        res.costentityCurrent != null
                        )
                    {
                        //                                  //Get Job to get the cost.
                        JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(jobentity =>
                            jobentity.intPk == fnlcostentityCurrent.intPkJob);
                        ZonedTime ztimeJob = ZonedTimeTools.NewZonedTime(jobentity.strStartDate.ParseToDate(),
                            jobentity.strStartTime.ParseToTime());

                        //                                  //Get the cost to use according to the date of the job.
                        CostentityCostEntityDB costentity = res.GetCostDependingDate(ztimeJob);

                        if (
                            costentity != null && costentity.numnCost != null
                            )
                        {
                            double? numCalculationQuantity = 0.0;
                            double? numnCalculationMin = 0.0;
                            double? numnCalculationBlock = 0.0;
                            double? numnCost = 0.0;
                            ProdtypProductType.subGetCostEntityData(costentity, ref numnCost,
                                ref numCalculationQuantity, ref numnCalculationMin,
                                ref numnCalculationBlock);
                            numCost = (double)numnCost;
                        }
                    }
                }
                else
                {
                    //                                      //fnlcostentityCurrent.intnPkCalculation != null
                    CalentityCalculationEntityDB calentity = context.Calculation.FirstOrDefault(
                        cal => cal.intPk == fnlcostentityCurrent.intnPkCalculation);

                    /*CASE*/
                    if (
                        calentity.strByX == CalCalculation.strByProcess &&
                        calentity.strCalculationType == CalCalculation.strPerQuantity &&
                        calentity.numnCost != null
                        )
                    {
                        numCost = (double)calentity.numnCost / (double)calentity.numnQuantity;
                    }
                    else if (
                        calentity.strByX == CalCalculation.strByProcess &&
                        calentity.strCalculationType == CalCalculation.strBase &&
                        calentity.numnCost != null
                        )
                    {
                        numCost = (double)calentity.numnCost;
                    }
                    else
                    {
                        //                                  //Do nothing.
                    }
                    /*END-CASE*/
                }

                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);
                List<ContactjsonContactJson> darrcontactjson = 
                    ResResource.darrcontactjsonGetAllEmployee(strPrintshopId_I);

                foreach (FnlcostentityFinalCostEntityDB fnlcost in darrfnlcostentityLog)
                {
                    //                                      //Convert date to printshop date
                    ZonedTime ztimeDateConverted = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                        fnlcost.strStartDate.ParseToDate(), fnlcost.strStartTime.ParseToTime(), ps.strTimeZone);

                    String strDateTime = ztimeDateConverted.Date + " " + ztimeDateConverted.Time;

                    double? numnCostWithFinalQuantity = null;
                    double? numnCostFinal = fnlcost.numnCost;
                    if (
                        fnlcost.numnQuantity != null
                        )
                    {
                        //                                  //Calculate the cost with the final Quantity.
                        numnCostWithFinalQuantity = ((double)fnlcost.numnQuantity * numCost).Round(2);
                        numnCostFinal = numnCostWithFinalQuantity;
                    }

                    String strFirstName = "";
                    String strLastName = "";
                    ResResource.subGetFirstAndLastNameOfEmployee(strPrintshopId_I, fnlcost.intContactId,
                        darrcontactjson, ref strFirstName, ref strLastName);

                    Fnlcostjson2FinalCostJson2 fnlcostjson2 = new Fnlcostjson2FinalCostJson2(strDateTime,
                        fnlcost.numnQuantity, numnCostWithFinalQuantity, numnCostFinal, fnlcost.strDescription,
                        strFirstName, strLastName);

                    darrfnlcostjson2Log.Add(fnlcostjson2);
                }

                intStatus_IO = 200;
                strUserMessage_IO = "";
                strDevMessage_IO = "";
                fnlcostjson2_O = darrfnlcostjson2Log.ToArray();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetProcessFinalCostData(
            String strPrintshopId_I,
            int intJobId_I,
            int intPkProduct_I,
            int intPkProcessInWorkflow_I,
            IConfiguration configuration_I,
            out FnlcostjsonFinalCostJson fnlcostjsonFinalCostJson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            fnlcostjsonFinalCostJson_O = null;

            intStatus_IO = 401;
            JobjsonJobJson jobjsonJob;
            if (
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjsonJob,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Job has no quantity.";
                if (
                    jobjsonJob.intnQuantity > 0
                    )
                {
                    //                                      //Initialize connection.
                    Odyssey2Context context = new Odyssey2Context();

                    //                                      //Find process in workflow.
                    PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                        piw.intPk == intPkProcessInWorkflow_I);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Process in workflow not found.";
                    if (
                        piwentity != null
                        )
                    {
                        //                                  //Get job's correct processes.
                        List<PiwentityProcessInWorkflowEntityDB> darrpiwentity;
                        List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                        ProdtypProductType.subGetWorkflowValidWay(piwentity.intPkWorkflow, jobjsonJob,
                            out darrpiwentity, out darrdynlkjson);

                        intStatus_IO = 404;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Process is not from this job.";
                        if (
                            darrpiwentity != null &&
                            //                              //Verify if input PIW exists in job's processes.
                            darrpiwentity.Exists(piw => piw.intPk == piwentity.intPk)
                            )
                        {
                            //                              //Process is completed.
                            PiwjentityProcessInWorkflowForAJobEntityDB piwjentity =
                                context.ProcessInWorkflowForAJob.FirstOrDefault(piwj => piwj.intStage == 2 &&
                                piwj.intPkProcessInWorkflow == intPkProcessInWorkflow_I &&
                                piwj.intJobId == intJobId_I);

                            intStatus_IO = 405;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "Process in workflow is not completed.";
                            if (
                                piwjentity != null
                                )
                            {
                                int intPkWorkflow = piwentity.intPkWorkflow;
                                //                          //Find Job.
                                JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job =>
                                    job.intJobID == intJobId_I && job.intPkWorkflow == intPkWorkflow);

                                intStatus_IO = 406;
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "Job not found.";
                                if (
                                    jobentity != null
                                    )
                                {
                                    //                      //Get the product.
                                    ProdtypProductType prodtyp = (ProdtypProductType)EtElementTypeAbstract.
                                        etFromDB(intPkProduct_I);

                                    intStatus_IO = 407;
                                    strUserMessage_IO = "Something is wrong.";
                                    strDevMessage_IO = "Product not found.";
                                    if (
                                        prodtyp != null
                                        )
                                    {
                                        int intPkProcess = piwentity.intPkProcess;

                                        //                  //Find base costs by process.
                                        BscostjsonBaseCostJson[] arrbscostjson = JobJob.arrbscostjsonByProcess(
                                            intPkProcess, intPkWorkflow, intPkProcessInWorkflow_I, jobentity,
                                            jobjsonJob, prodtyp);

                                        //                  //Find per quantity costs by process.
                                        PqcostjsonPerQuantityCostJson[] arrpqcostjson = JobJob.arrpqcostjsonByProcess(
                                            intPkProcess, intPkWorkflow, intPkProcessInWorkflow_I, jobentity,
                                            jobjsonJob, prodtyp);

                                        //                  //Find per quantity costs by resource.
                                        PqcostjsonPerQuantityCostJson[] arrpqcostjsonByResource =
                                            JobJob.arrpqcostjsonByResource(piwentity, jobentity);

                                        //                  //To know if the job is already completed
                                        bool boolJobCompleted = jobentity.intStage == JobJob.intCompletedStage ? true :
                                            false;

                                        //                  //Get job's substage.
                                        String strSubStage = JobJob.strGetJobSubStage(intJobId_I, strPrintshopId_I);

                                        //                  //Final json to return.
                                        int intPkJob = jobentity.intPk;
                                        fnlcostjsonFinalCostJson_O = new FnlcostjsonFinalCostJson(intPkJob,
                                            intPkProcessInWorkflow_I, arrbscostjson, arrpqcostjson,
                                            arrpqcostjsonByResource, strSubStage, boolJobCompleted);

                                        intStatus_IO = 200;
                                        strUserMessage_IO = "";
                                        strDevMessage_IO = "";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static BscostjsonBaseCostJson[] arrbscostjsonByProcess(
            int intPkProcess_I,
            int intPkWorkflow_I,
            int intPkProcessInWorkflow_I,
            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjsonJob_I,
            ProdtypProductType prodtyp_I
            )
        {
            //                                              //Json to return.
            List<BscostjsonBaseCostJson> darrbscostjson = new List<BscostjsonBaseCostJson>();

            int intPkJob = jobentity_I.intPk;
            //                                              //Get calculations in this wf and job.
            List<CalCalculation> darrcalentity = prodtyp_I.darrcalGetCalculationsCurrentByJobsStageAndWFFromDB(intPkJob,
                intPkWorkflow_I);
            //                                              //Filter them by cost, process and base.
            darrcalentity = darrcalentity.Where(cal => cal.boolIsEnable &&
                cal.strEndDate == null && cal.strEndTime == null &&
                cal.intnPkProcessElementBelongsTo == intPkProcess_I && cal.numnCost != null &&
                cal.strCalculationType == CalCalculation.strBase &&
                cal.strByX == CalCalculation.strByProcess).ToList();

            if (
                //                                         //Calculations found.
                darrcalentity.Count > 0
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                foreach (CalCalculation cal in darrcalentity)
                {
                    if (
                        //                                  //Verify calculations conditions.
                        Tools.boolCalculationOrLinkApplies(cal.intPk, null, null, null, jobjsonJob_I)
                        )
                    {
                        //                                  //Variables to return.
                        double numBaseCost = (double)cal.numnCost;

                        //                                  //Find final base costs.
                        List<FnlcostentityFinalCostEntityDB> darrfnlcostentity = context.FinalCost.Where(
                            fnlcost => fnlcost.intPkProcessInWorkflow == intPkProcessInWorkflow_I &&
                            fnlcost.intPkJob == intPkJob &&
                            fnlcost.intnPkCalculation == cal.intPk && fnlcost.numnCost != null &&
                            fnlcost.numnQuantity == null).ToList();
                        darrfnlcostentity.Sort();

                        bool boolManyRowsInFinalTable = false;

                        FnlcostentityFinalCostEntityDB fnlcostentity = darrfnlcostentity.Last();
                        double numFinalCost = (double)fnlcostentity.numnCost;
                        int? intnPkFinalCost = fnlcostentity.intPk;
                        String strDescription = fnlcostentity.strDescription;
                        int intPkAccountMovement = fnlcostentity.intPkAccountMovement;

                        if (
                           //                           //There are two registers or more.
                           darrfnlcostentity.Count >= 2
                           )
                        {
                            boolManyRowsInFinalTable = true;
                        }

                        //                                  //Build ouput Json.
                        BscostjsonBaseCostJson bscostjson = new BscostjsonBaseCostJson(intnPkFinalCost, cal.intPk,
                            cal.strDescription, numBaseCost.Round(2), numFinalCost.Round(2), strDescription,
                            boolManyRowsInFinalTable, intPkAccountMovement);
                        darrbscostjson.Add(bscostjson);
                    }
                }
            }

            return darrbscostjson.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static PqcostjsonPerQuantityCostJson[] arrpqcostjsonByProcess(
            int intPkProcess_I,
            int intPkWorkflow_I,
            int intPkProcessInWorkflow_I,
            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjsonJob_I,
            ProdtypProductType prodtyp_I
            )
        {
            //                                              //Array of Jsons which will contain each calculation.
            List<PqcostjsonPerQuantityCostJson> darrpqcostjson = new List<PqcostjsonPerQuantityCostJson>();

            int intPkJob = jobentity_I.intPk;
            //                                              //Get calculations in this wf and job.
            List<CalCalculation> darrcalentity = prodtyp_I.darrcalGetCalculationsCurrentByJobsStageAndWFFromDB(intPkJob,
                intPkWorkflow_I);

            //                                              //Filter them by wf, by cost, process and perquantity.
            darrcalentity = darrcalentity.Where(cal => cal.boolIsEnable &&
                cal.strEndDate == null && cal.strEndTime == null &&
                cal.intnPkProcessElementBelongsTo == intPkProcess_I && cal.numnCost != null &&
                cal.strCalculationType == CalCalculation.strPerQuantity &&
                cal.strByX == CalCalculation.strByProcess).ToList();

            if (
                darrcalentity.Count > 0
                )
            {
                //                                          //Establish connection.
                Odyssey2Context context = new Odyssey2Context();

                foreach (CalCalculation cal in darrcalentity)
                {
                    if (
                        //                                  //Verify calculations conditions.
                        Tools.boolCalculationOrLinkApplies(cal.intPk, null, null, null, jobjsonJob_I)
                        )
                    {
                        //                                  //Find final cost.
                        List<FnlcostentityFinalCostEntityDB> darrfnlcostentity = context.FinalCost.Where
                            (fnlcost => fnlcost.intPkProcessInWorkflow == intPkProcessInWorkflow_I &&
                            fnlcost.intPkJob == intPkJob && fnlcost.intnPkCalculation == cal.intPk).ToList();
                        darrfnlcostentity.Sort();
                        FnlcostentityFinalCostEntityDB fnlcostentity = darrfnlcostentity.Last();

                        //                                  //Variables to return.
                        double numQuantity = 0;
                        double numCost = 0.0;
                        double numFinalCost = numCost;
                        double numCostWithFinalQuantity = numCost;
                        double? numnFinalQuantity = 0.0;
                        int? intnPkFinalCost = null;
                        String strDescription = null;
                        int intPkAccountMovement = 0;

                        if (
                            //                              //Is the first time or nothing has been chaged.
                            darrfnlcostentity.Count == 1
                            )
                        {
                            numQuantity = (double)fnlcostentity.numnQuantity;
                            numCost = (double)fnlcostentity.numnCost;
                            numFinalCost = numCost;
                            numCostWithFinalQuantity = numCost;
                            numnFinalQuantity = (double)fnlcostentity.numnQuantity;
                            intnPkFinalCost = fnlcostentity.intPk;
                            strDescription = fnlcostentity.strDescription;
                            intPkAccountMovement = fnlcostentity.intPkAccountMovement;

                            //                              //There are two registers or more.
                            bool boolManyRowsInFinalTable = darrfnlcostentity.Count >= 2 ? true : false;

                            //                                  //Build output Json.
                            PqcostjsonPerQuantityCostJson pqcostjson = new PqcostjsonPerQuantityCostJson(intnPkFinalCost,
                                cal.intPk, null, null, null, numQuantity.Round(2), ((double)numnFinalQuantity).Round(2),
                                cal.strUnit, cal.strDescription, numCost.Round(2), numCostWithFinalQuantity.Round(2),
                                numFinalCost.Round(2), strDescription, boolManyRowsInFinalTable, intPkAccountMovement);
                            darrpqcostjson.Add(pqcostjson);
                        }
                        else
                        {
                            FnlcostentityFinalCostEntityDB fnlcostentityFirst = darrfnlcostentity.First();

                            numQuantity = (double)fnlcostentityFirst.numnQuantity;
                            numCost = (double)fnlcostentityFirst.numnCost;
                            numFinalCost = numCost;
                            numnFinalQuantity = (double)fnlcostentityFirst.numnQuantity;
                            numCostWithFinalQuantity = numCost;
                            intnPkFinalCost = fnlcostentity.intPk;
                            strDescription = fnlcostentity.strDescription;
                            intPkAccountMovement = fnlcostentity.intPkAccountMovement;

                            if (
                                //                          //Final quantity changed.
                                fnlcostentity.numnQuantity != null
                                )
                            {
                                numnFinalQuantity = (double)fnlcostentity.numnQuantity;

                                if (
                                    numCost > 0 &&
                                    numQuantity > 0 &&
                                    numnFinalQuantity > 0
                                    )
                                {
                                    numCostWithFinalQuantity = (numCost * (double)numnFinalQuantity) / numQuantity;
                                    numFinalCost = numCostWithFinalQuantity;
                                }
                                else
                                {
                                    numFinalCost = 0;
                                    numCostWithFinalQuantity = 0;
                                }
                            }
                            else
                            {
                                //                          //Final cost changed.
                                numFinalCost = (double)fnlcostentity.numnCost;
                                numnFinalQuantity = null;
                            }

                            //                              //There are two registers or more.
                            bool boolManyRowsInFinalTable = darrfnlcostentity.Count >= 2 ? true : false;

                            //                                  //Build output Json.
                            PqcostjsonPerQuantityCostJson pqcostjson = new PqcostjsonPerQuantityCostJson(intnPkFinalCost,
                                cal.intPk, null, null, null, numQuantity.Round(2), numnFinalQuantity == null ?
                                numnFinalQuantity : ((double)numnFinalQuantity).Round(2), cal.strUnit,
                                cal.strDescription, numCost.Round(2), numCostWithFinalQuantity.Round(2),
                                numFinalCost.Round(2), strDescription, boolManyRowsInFinalTable, intPkAccountMovement);
                            darrpqcostjson.Add(pqcostjson);
                        }
                    }
                }
            }

            return darrpqcostjson.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static PqcostjsonPerQuantityCostJson[] arrpqcostjsonByResource(
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            JobentityJobEntityDB jobentity_I
            )
        {
            //                                              //Array of Jsons which will contain each calculation.
            List<PqcostjsonPerQuantityCostJson> darrpqcostjson = new List<PqcostjsonPerQuantityCostJson>();

            int intPkJob = jobentity_I.intPk;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Get all register from final cost that belongs to a final
            //                                              //      cost by resource.
            List<FnlcostentityFinalCostEntityDB> darrfnlcostentity = context.FinalCost.Where(fnlcost =>
                        fnlcost.intPkProcessInWorkflow == piwentity_I.intPk &&
                        fnlcost.intPkJob == intPkJob && fnlcost.intnPkResource != null &&
                        (fnlcost.intnPkElementElementType != null || fnlcost.intnPkElementElement != null)).ToList();

            List<FnlcostentityFinalCostEntityDB> darrfnlcostentityClean = new List<FnlcostentityFinalCostEntityDB>();
            foreach (FnlcostentityFinalCostEntityDB fnlcostentity in darrfnlcostentity)
            {
                if (
                    !darrfnlcostentityClean.Exists(fnl => fnl.intPkProcessInWorkflow == piwentity_I.intPk &&
                        fnl.intPkJob == intPkJob && fnl.intnPkResource == fnlcostentity.intnPkResource &&
                        fnl.intnPkElementElementType == fnlcostentity.intnPkElementElementType &&
                        fnl.intnPkElementElement == fnlcostentity.intnPkElementElement)
                    )
                {
                    darrfnlcostentityClean.Add(fnlcostentity);
                }
            }

            foreach (FnlcostentityFinalCostEntityDB fnlcostentity in darrfnlcostentityClean)
            {
                List<FnlcostentityFinalCostEntityDB> darrfnlcostentityList = context.FinalCost.Where(fnlcost =>
                        fnlcost.intPkProcessInWorkflow == fnlcostentity.intPkProcessInWorkflow &&
                        fnlcost.intPkJob == fnlcostentity.intPkJob &&
                        fnlcost.intnPkResource == fnlcostentity.intnPkResource &&
                        fnlcost.intnPkElementElementType == fnlcostentity.intnPkElementElementType &&
                        fnlcost.intnPkElementElement == fnlcostentity.intnPkElementElement).ToList();
                darrfnlcostentity.Sort();

                FnlcostentityFinalCostEntityDB fnlcostentityFirst = darrfnlcostentityList.First();

                int? intnPkFinalCost = fnlcostentityFirst.intPk;
                double numQuantity = (double)fnlcostentityFirst.numnQuantity;
                double? numnFinalQuantity = fnlcostentityFirst.numnQuantity;
                double numCost = (double)fnlcostentityFirst.numnCost;
                double numFinalCost = numCost;
                double numCostWithFinalQuantity = numCost;
                String strDescription = fnlcostentityFirst.strDescription;

                int? intnPkEleet = fnlcostentityFirst.intnPkElementElementType;
                int? intnPkEleEle = fnlcostentityFirst.intnPkElementElement;
                int? intnPkEleetOrEleele = intnPkEleet != null ? intnPkEleet : intnPkEleEle;
                bool boolIsEleet = intnPkEleet != null ? true : false;

                bool boolManyRowsInFinalTable = false;

                //                                          //Get resource's name.
                EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                    ele.intPk == fnlcostentityFirst.intnPkResource);
                String strResName = eleentity.strElementName;

                //                                          //Get resource's unit.
                //                                          //Get job in order to get date.
                String strUnit = "";
                ValentityValueEntityDB valentity = null;
                if (
                    jobentity_I != null
                    )
                {
                    //                                      //Get unit depends job's date.
                    valentity = ProdtypProductType.valentityGetUnitDependJobDate(jobentity_I,
                        eleentity);
                }
                else
                {
                    //                                      //Get the current unit of measurement.
                    valentity = ResResource.GetResourceUnitOfMeasurement(eleentity.intPk);
                }

                if (
                    valentity != null
                    )
                {
                    strUnit = valentity.strValue;
                }

                if (
                    //                                      //Is the first time or nothing has been chaged.
                    darrfnlcostentityList.Count == 1
                    )
                {
                    //                                      //Build output Json.
                    PqcostjsonPerQuantityCostJson pqcostjson = new PqcostjsonPerQuantityCostJson(intnPkFinalCost, null,
                        fnlcostentityFirst.intnPkResource, intnPkEleetOrEleele, boolIsEleet, numQuantity.Round(2),
                        ((double)numnFinalQuantity).Round(2), strUnit, strResName, numCost.Round(2),
                        numCostWithFinalQuantity.Round(2), numFinalCost.Round(2), strDescription,
                        boolManyRowsInFinalTable, fnlcostentityFirst.intPkAccountMovement);
                    darrpqcostjson.Add(pqcostjson);
                }
                else
                {
                    //                                      //                    
                    FnlcostentityFinalCostEntityDB fnlcostentityLast = darrfnlcostentityList.Last();

                    intnPkFinalCost = fnlcostentityLast.intPk;
                    strDescription = fnlcostentityLast.strDescription;

                    if (
                        //                                  //Final quantity changed.
                        fnlcostentityLast.numnQuantity != null
                        )
                    {
                        numnFinalQuantity = ((double)fnlcostentityLast.numnQuantity).Round(2);

                        if (
                            numCost > 0 &&
                            numQuantity > 0 &&
                            numnFinalQuantity > 0
                            )
                        {
                            //                              //Get final cost with new quantity.
                            numCostWithFinalQuantity = ((double)numnFinalQuantity * numCost) / numQuantity;
                            numFinalCost = numCostWithFinalQuantity;
                        }
                        else
                        {
                            numFinalCost = 0;
                            numCostWithFinalQuantity = 0;
                        }
                    }
                    else
                    {
                        //                                  //Final cost was changed.
                        numFinalCost = (double)fnlcostentityLast.numnCost;
                        numnFinalQuantity = fnlcostentityLast.numnQuantity;
                    }

                    if (
                        //                                  //There are two registers or more.
                        darrfnlcostentity.Count >= 2
                        )
                    {
                        boolManyRowsInFinalTable = true;
                    }

                    //                                      //Build output Json.
                    PqcostjsonPerQuantityCostJson pqcostjson = new PqcostjsonPerQuantityCostJson(intnPkFinalCost, null,
                        fnlcostentityFirst.intnPkResource, intnPkEleetOrEleele, boolIsEleet, numQuantity.Round(2),
                        numnFinalQuantity, strUnit, strResName, numCost.Round(2),
                        numCostWithFinalQuantity.Round(2), numFinalCost.Round(2), strDescription,
                        boolManyRowsInFinalTable, fnlcostentityFirst.intPkAccountMovement);
                    darrpqcostjson.Add(pqcostjson);
                }
            }

            return darrpqcostjson.ToArray();
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strGetJobSubStage(
            //                                              //Get the substage for a given job.

            int intJobId_I,
            String strPrintshopId_I
            )
        {
            String strJobSubStage = "";

            //                                          //Update job's subStage at wisnet.
            String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection(
                "Odyssey2Settings")["urlWisnetApi"];
            Task<String> Task_strResult = HttpTools<TjsonTJson>.GetJobSubStage(strUrlWisnet +
                "/Job/getStage/" + strPrintshopId_I + "/" + intJobId_I);
            Task_strResult.Wait();

            if (
                //                                      //There was access to the service of Wisnet.
                Task_strResult.Result != null
                )
            {
                //                                          //We obtained the substage.
                strJobSubStage = Task_strResult.Result;
            }

            return strJobSubStage;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetDueDateLog(
            String strPrintshopId_I,
            int intJobId_I,
            IConfiguration configuration_I,
            out DuedatejsonDueDateJson[] arrduedatejson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            arrduedatejson_O = null;
            JobjsonJobJson jobjson;
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                Odyssey2Context context = new Odyssey2Context();
                List<DuedateentityDueDateEntityDB> darrduedateentity = context.DueDate.Where(due =>
                    due.intJobId == intJobId_I).ToList();
                darrduedateentity.Sort();

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "No due dates found.";
                if (
                    darrduedateentity.Count >= 0
                    )
                {
                    PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                    //                                      //Get all employee from printshop.
                    List<ContactjsonContactJson> darrcontactjson = ResResource.darrcontactjsonGetAllEmployee(strPrintshopId_I);

                    List<DuedatejsonDueDateJson> darrduedatejson = new List<DuedatejsonDueDateJson>();
                    foreach (DuedateentityDueDateEntityDB duedateentity in darrduedateentity)
                    {
                        String strFirstName = "";
                        String strLastName = "Undefined";
                        ResResource.subGetFirstAndLastNameOfEmployee(strPrintshopId_I, duedateentity.intContactId,
                            darrcontactjson, ref strFirstName, ref strLastName);

                        //                                  //To easy code.
                        String strTime = duedateentity.strHour + ":" + duedateentity.strMinute + ":" +
                            duedateentity.strSecond;
                        //                                  //Convert base date to printshop date
                        ZonedTime ztimeDate = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                            duedateentity.strDate.ParseToDate(), strTime.ParseToTime(), ps.strTimeZone);

                        //                                  //Convert base date to printshop date
                        ZonedTime ztimeStart = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                            duedateentity.strStartDate.ParseToDate(), duedateentity.strStartTime.ParseToTime(),
                            ps.strTimeZone);

                        DuedatejsonDueDateJson duedatejson = new DuedatejsonDueDateJson(duedateentity.strDescription,
                            ztimeDate.Date.ToString(), ztimeDate.Time.ToString(), ztimeStart.Date.ToString(),
                            ztimeStart.Time.ToString(), strFirstName, strLastName);
                        darrduedatejson.Add(duedatejson);
                    }

                    arrduedatejson_O = darrduedatejson.ToArray();
                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetPriceLog(
            String strPrintshopId_I,
            int intJobId_I,
            int intPkWorkflow_I,
            //                                              //It is null when the log is from the job´s wf.
            int? intnEstimateId_I,
            int? intnCopyNumber_I,
            IConfiguration configuration_I,
            out PricejsonPriceJson[] arrpricejson_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            arrpricejson_O = null;
            JobjsonJobJson jobjson;
            intStatus_IO = 401;
            if (
                JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjson,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                PsPrintShop ps = PsPrintShop.psGet(strPrintshopId_I);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Printshop not found.";
                if (
                    ps != null
                    )
                {
                    //                                      //Establish connection.
                    Odyssey2Context context = new Odyssey2Context();

                    //                                      //Find workflow.
                    WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                            wf.intPk == intPkWorkflow_I &&
                            wf.boolDeleted == false);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Workflow not found.";
                    if (
                        wfentity != null
                        )
                    {
                        int? intnPkEstimate = null;
                        bool boolIsDataValid = false;
                        int? intnPkWorkflowToUse = intPkWorkflow_I;
                        if (
                            //                              //Estimate price log.
                            intnEstimateId_I != null
                            )
                        {
                            //                              //Find estimate.
                            EstentityEstimateEntityDB estentity = context.Estimate.FirstOrDefault(est =>
                                est.intId == intnEstimateId_I && est.intnCopyNumber == intnCopyNumber_I &&
                                est.intJobId == intJobId_I && est.intPkWorkflow == intPkWorkflow_I);

                            if (
                                estentity != null
                                )
                            {
                                intnPkEstimate = estentity.intPk;
                                boolIsDataValid = true;
                                intnPkWorkflowToUse = null;
                            }
                        }
                        else
                        {
                            //                              //Estimate == null.
                            //                              //Workflow price log.
                            boolIsDataValid = true;
                        }

                        intStatus_IO = 404;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Data not valid.";
                        if (
                            boolIsDataValid
                            )
                        {
                            List<PriceentityPriceEntityDB> darrpriceentity = context.Price.Where(price =>
                                price.intJobId == intJobId_I && price.intnPkWorkflow == intnPkWorkflowToUse &&
                                price.intnPkEstimate == intnPkEstimate).ToList();

                            intStatus_IO = 405;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "No prices found.";
                            if (
                                darrpriceentity.Count >= 0
                                )
                            {
                                darrpriceentity.Sort();

                                //                          //Get all employee from printshop.
                                List<ContactjsonContactJson> darrcontactjson = ResResource.darrcontactjsonGetAllEmployee(strPrintshopId_I);

                                List<PricejsonPriceJson> darrpricejson = new List<PricejsonPriceJson>();
                                foreach (PriceentityPriceEntityDB priceentity in darrpriceentity)
                                {
                                    String strFirstName = "";
                                    String strLastName = "Undefined";
                                    ResResource.subGetFirstAndLastNameOfEmployee(strPrintshopId_I,
                                        priceentity.intContactId, darrcontactjson,
                                        ref strFirstName, ref strLastName);

                                    //                      //Convert base date to printshop date
                                    ZonedTime ztimeStart = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                        priceentity.strStartDate.ParseToDate(), priceentity.strStartTime.ParseToTime(),
                                        ps.strTimeZone);

                                    PricejsonPriceJson pricejson = new PricejsonPriceJson(priceentity.numnPrice,
                                        priceentity.strDescription, ztimeStart.Date.ToString(), 
                                        ztimeStart.Time.ToString(), strFirstName, strLastName);
                                    darrpricejson.Add(pricejson);
                                }

                                arrpricejson_O = darrpricejson.ToArray();

                                intStatus_IO = 200;
                                strUserMessage_IO = "";
                                strDevMessage_IO = "";
                            }
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static Resjson4ResourceJson4[] arrresjson4GetResourcesFromIoGroup(
            //                                              //Returns an array of resources that can be selected from a
            //                                              //      gruop for a IO.

            String strPrintshopId_I,
            int intPkProcessInWorkflow_I,
            int intJobId_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            IConfiguration configuration_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Array of res to return.
            Resjson4ResourceJson4[] arrresjson4Resources = null;

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find process in workflow.
            PiwentityProcessInWorkflowEntityDB piwentity = context.ProcessInWorkflow.FirstOrDefault(piw =>
                piw.intPk == intPkProcessInWorkflow_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "PIW not found.";
            if (
                piwentity != null
                )
            {
                //                                          //Find workflow.
                WfentityWorkflowEntityDB wfentity = context.Workflow.FirstOrDefault(wf =>
                    wf.intPk == piwentity.intPkWorkflow && wf.boolDeleted == false);

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Workflow not found.";
                if (
                    wfentity != null
                    )
                {
                    //                                      //Find product.
                    int intPkProduct = wfentity.intnPkProduct != null ? (int)wfentity.intnPkProduct : 0;

                    //                                      //Job data.
                    JobjsonJobJson jobjsonJob;

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Invalid job.";
                    if (
                        JobJob.boolIsValidJobId(intJobId_I, strPrintshopId_I, configuration_I, out jobjsonJob,
                        ref strUserMessage_IO, ref strDevMessage_IO)
                        )
                    {
                        //                                  //Find job.
                        JobentityJobEntityDB jobentity = context.Job.FirstOrDefault(job => job.intJobID == intJobId_I);

                        int? intnPkEleet = null;
                        int? intnPkEleEle = null;

                        //                                  //Easy code.
                        if (
                            boolIsEleet_I
                            )
                        {
                            intnPkEleet = intPkEleetOrEleele_I;
                        }
                        else
                        {
                            intnPkEleEle = intPkEleetOrEleele_I;
                        }

                        //                                  //Find IO.
                        IoentityInputsAndOutputsEntityDB ioentity = context.InputsAndOutputs.FirstOrDefault(io =>
                            io.intnPkElementElementType == intnPkEleet &&
                            io.intnPkElementElement == intnPkEleEle &&
                            io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId &&
                            io.intPkWorkflow == piwentity.intPkWorkflow);

                        intStatus_IO = 405;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Input or output not found, either does not exist or does not " +
                            "have link and res.";
                        if (
                            ioentity != null
                            )
                        {
                            intStatus_IO = 405;
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "No group, can be no resource (only link) and no group " +
                                "or only one resource.";
                            if (
                                //                          //Io with group.
                                ioentity.intnPkResource == null &&
                                ioentity.intnGroupResourceId != null
                                )
                            {
                                int intGroupResourceId = (int)ioentity.intnGroupResourceId;

                                //                          //Find groups that belong to the workflow.
                                List<GpresentityGroupResourceEntityDB> darrgpresentityResources =
                                    context.GroupResource.Where(gpres => gpres.intId == intGroupResourceId &&
                                    gpres.intPkWorkflow == piwentity.intPkWorkflow).ToList();

                                intStatus_IO = 406;
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "There´s a group with only one element.";
                                if (
                                    darrgpresentityResources.Count >= 2
                                    )
                                {
                                    JobJob.subVerifyCalculationsOfTheGroupAndGetNames(piwentity.intPkWorkflow,
                                        intPkProduct, intnPkEleet, intnPkEleEle, piwentity.intProcessInWorkflowId,
                                        darrgpresentityResources, jobjsonJob, out arrresjson4Resources);

                                    intStatus_IO = 200;
                                    strUserMessage_IO = "";
                                    strDevMessage_IO = "";
                                }
                            }
                        }
                    }
                }
            }
            return arrresjson4Resources;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subVerifyCalculationsOfTheGroupAndGetNames(
            //                                              //This method finds resources´calculations in a workflow.
            //                                              //If there are not calculations, the resource can appear
            //                                              //      in the dropdown. If there are calculation, then the
            //                                              //      "boolCalculationApply" method is used.

            int intPkWorkflow_I,
            int intPkProduct_I,
            int? intnPkEleet_I,
            int? intnPkEleEle_I,
            int intProcessInWorkflowId_I,
            List<GpresentityGroupResourceEntityDB> darrgpresentityResources_I,
            JobjsonJobJson jobjsonJob_I,
            out Resjson4ResourceJson4[] arrresjson4Resources_O
            )
        {
            //                                              //List to return.
            List<Resjson4ResourceJson4> darrresjson4 = new List<Resjson4ResourceJson4>();

            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            foreach (GpresentityGroupResourceEntityDB gpresentity in darrgpresentityResources_I)
            {
                //                                          //Find per quantity by resource in workflow calculations.
                List<CalentityCalculationEntityDB> darrcalentity = context.Calculation.Where(cal =>
                    cal.boolIsEnable && cal.numnNeeded != null && cal.numnPerUnits != null &&
                    cal.strCalculationType == CalCalculation.strPerQuantity &&
                    cal.strByX == CalCalculation.strByResource && cal.strEndDate == null && cal.strEndTime == null &&
                    cal.numnPercentWaste >= 0 && cal.numnQuantityWaste >= 0 && cal.intnPkProduct == intPkProduct_I &&
                    cal.intnPkResource == gpresentity.intPkResource && cal.intnPkElementElementType == intnPkEleet_I &&
                    cal.intnPkElementElement == intnPkEleEle_I && cal.intnPkWorkflow == intPkWorkflow_I &&
                    cal.intnProcessInWorkflowId == intProcessInWorkflowId_I).ToList();

                //                                          //If one calculation applies, the resource is considered
                //                                          //      to appear in the dropdown.
                bool boolCalculationsApply = false;
                int intI = 0;
                /*WHILE-DO*/
                while (
                    intI < darrcalentity.Count &&
                    !boolCalculationsApply
                    )
                {
                    boolCalculationsApply = Tools.boolCalculationOrLinkApplies(darrcalentity[intI].intPk, null, null, 
                        null, jobjsonJob_I);

                    intI = intI + 1;
                }

                if (
                    //                                      //There are not calculations or there´s at least one that
                    //                                      //      applies to the job
                    (darrcalentity.Count == 0) ||
                    boolCalculationsApply
                    )
                {
                    //                                      //Find resource or template.
                    ResResource resResourceInGroup = ResResource.resFromDB(gpresentity.intPkResource, false);
                    //                                      //Create Json.
                    Resjson4ResourceJson4 resjson4ResourceToDropdown =
                        new Resjson4ResourceJson4(resResourceInGroup.intPk, resResourceInGroup.strName);
                    darrresjson4.Add(resjson4ResourceToDropdown);
                }
            }

            darrresjson4 = darrresjson4.OrderBy(res => res.strName).ToList();

            arrresjson4Resources_O = darrresjson4.ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subGetBudgetEstimation(
            //                                              //Get budget estimation from jobEstimate.

            int intJobId_I,
            int intPkWorkflow_I,
            int? intnEstimationId_I,
            int? intnCopyNumber_I,
            String strBaseDate_I,
            String strBaseTime_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            out BdgestjsonBudgetEstimationJson bdgestjson_O,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bdgestjson_O = null;

            JobjsonJobJson jobjson;
             if (
                //                                          //Valid Job and Get data from Order form.
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                    ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {
                intStatus_IO = 401;
                if (
                    JobJob.boolValidBaseDate(strBaseDate_I, strBaseTime_I, ps_I.strTimeZone, ref strUserMessage_IO,
                        ref strDevMessage_IO)
                    )
                {
                    ProdtypProductType prodtyp = ProdtypProductType.GetProductTypeUpdated(ps_I,
                        jobjson.strProductName, (int)jobjson.intnProductKey, true);

                    intStatus_IO = 402;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "No product found.";
                    if (
                        prodtyp != null
                        )
                    {
                        EstentityEstimateEntityDB estentity = context_M.Estimate.FirstOrDefault(est =>
                            est.intId == intnEstimationId_I && est.intnCopyNumber == intnCopyNumber_I &&
                            est.intJobId == jobjson.intJobId && est.intPkWorkflow == intPkWorkflow_I);

                        //                                  //the the quantity for the estimateCopy.
                        jobjson.intnQuantity = intnCopyNumber_I != null ? estentity.intnQuantity :
                            jobjson.intnQuantity;


                        intStatus_IO = 403;
                        strUserMessage_IO = "Estimation not found.";
                        strDevMessage_IO = "";
                        if (
                            (estentity != null) ||
                            (estentity == null && intnEstimationId_I == -1)
                            )
                        {
                            //                              //Variables to use as BaseDate and BaseTime.
                            String strBaseDate = strBaseDate_I;
                            String strBaseTime = strBaseTime_I;
                            /*CASE*/
                            if (
                                (intnEstimationId_I == -1) &&
                                ((strBaseDate_I == null) || (strBaseTime_I == null))
                                )
                            {
                                //                          //When recieve the Partial Estimate, re-assing variables,
                                //                          //      adding 1 hour.
                                int intOneHour = 60 * 60 * 1000;
                                ZonedTime ztimeBase = ZonedTimeTools.ztimeNow + intOneHour;
                                strBaseDate = ztimeBase.Date.ToString();
                                strBaseTime = ztimeBase.Time.ToString();
                            }
                            else if (
                                (estentity != null) &&
                                ((strBaseDate_I == null) || (strBaseTime_I == null))
                                )
                            {
                                //                          //Re-assing variables to database's value if needed.
                                strBaseDate = estentity.strBaseDate;
                                strBaseTime = estentity.strBaseTime;
                            }
                            else if (
                                (strBaseDate_I != null) && (strBaseTime_I != null)
                                )
                            {
                                ZonedTime ztimeBaseDate =
                                    ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(strBaseDate_I.ParseToDate(),
                                    strBaseTime_I.ParseToTime(), ps_I.strTimeZone);

                                strBaseDate = ztimeBaseDate.Date.ToString();
                                strBaseTime = ztimeBaseDate.Time.ToString();
                            }
                            /*END-CASE*/

                            intStatus_IO = 404;
                            strUserMessage_IO = "The product workflow is incomplete. Complete the product workflow" +
                                " to see the budget estimation.";
                            strDevMessage_IO = "";

                            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityWithFinalProduct;
                            bool boolWorkflowIsReady;
                            bool? boolnNotUsed;
                            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityProcessesNotReady;
                            ProdtypProductType.subfunWorkflowIsReady(intPkWorkflow_I, out darrpiwentityWithFinalProduct,
                                out boolWorkflowIsReady, out boolnNotUsed, out darrpiwentityProcessesNotReady);
                            if (
                                boolWorkflowIsReady
                                )
                            {
                                //                          //Get all the processes
                                List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses;
                                List<DynLkjsonDynamicLinkJson> darrdynlkjson;
                                ProdtypProductType.subGetWorkflowValidWay(intPkWorkflow_I, jobjson,
                                    out darrpiwentityAllProcesses, out darrdynlkjson);

                                intStatus_IO = 404;
                                strUserMessage_IO = "Many workflow possibilities found.";
                                strDevMessage_IO = "";
                                if (
                                    darrpiwentityAllProcesses.Count > 0
                                    )
                                {
                                    ProdtypProductType.subUpdateResourceForAJob(prodtyp, null, darrpiwentityAllProcesses,
                                        jobjson);

                                    //                      //List of normal piw.
                                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityNormalProcess =
                                        darrpiwentityAllProcesses.Where(piw =>
                                        piw.boolIsPostProcess == false).ToList();

                                    //                      //List of post piw.
                                    List<PiwentityProcessInWorkflowEntityDB> darrpiwentityPostProcess =
                                        darrpiwentityAllProcesses.Where(piw =>
                                        piw.boolIsPostProcess == true).ToList();

                                    //                      //List process'budget. 
                                    List<ProcbdgjsonProcessBudgetJson> darrprocbdgjson =
                                        new List<ProcbdgjsonProcessBudgetJson>();

                                    //                      //List to Add piws for support deliveryDate.
                                    List<Piwjson2ProcessInWorkflowJson2> darrpiwjson2 =
                                        new List<Piwjson2ProcessInWorkflowJson2>();

                                    //                      //Data to be returned by following methods.
                                    double numSumOfProcessCost = 0.0;
                                    bool boolHasOptionBudget = false;
                                    bool boolHasAllResourceSetted = true;
                                    bool boolCanBeEstimate = true;
                                    bool boolHasResourceIncomplete = false;

                                    double numExtraCost = 0.0;

                                    //                      //Dictionary to store inputs and outputs of a process.
                                    prodtyp.dicProcessIOs = new Dictionary<int, List<Iofrmpiwjson2IOFromPIWJson2>>();
                                    prodtyp.darrresthkjsonResThickness = new List<ResthkjsonResourceThicknessJson>();
                                    //                      //Get job stage.
                                    JobentityJobEntityDB jobentity = context_M.Job.FirstOrDefault(job => 
                                        job.intJobID == jobjson.intJobId);

                                    //                      //Get normal processes' budget json.
                                    JobJob.subGetNormalProcessBudget(intnEstimationId_I, jobentity, jobjson, prodtyp, 
                                        ps_I, darrdynlkjson, darrpiwentityAllProcesses, darrpiwentityNormalProcess, 
                                        configuration_I, darrpiwjson2, darrprocbdgjson, ref numExtraCost, 
                                        ref numSumOfProcessCost, ref boolCanBeEstimate, ref boolHasAllResourceSetted,
                                        ref boolHasOptionBudget, ref boolHasResourceIncomplete);

                                    //                      //Get post processes' budget json.
                                    JobJob.subGetPostProcessBudget(intnEstimationId_I, jobentity, jobjson, prodtyp,
                                        ps_I, darrdynlkjson, darrpiwentityAllProcesses, darrpiwentityPostProcess,
                                        configuration_I, darrpiwjson2, darrprocbdgjson, ref numExtraCost, 
                                        ref numSumOfProcessCost, ref boolCanBeEstimate, ref boolHasAllResourceSetted,
                                        ref boolHasOptionBudget,  ref boolHasResourceIncomplete);

                                    intStatus_IO = 405;
                                    strUserMessage_IO = "There must be at least one resource set in each " +
                                        "resource type or template.";
                                    strDevMessage_IO = "";
                                    if (
                                        true
                                        //boolHasAllResourceSetted
                                        )
                                    {
                                        //                  //Get Calculation by Product.
                                        List<CostbycaljsonCostByCalculationJson> darrcostbycaljsonByProduct;
                                        bool boolWorkflowJobIsReadyNotUsed = true;
                                        double numCostByProduct = prodtyp.numGetCostByProduct(jobjson, ps_I,
                                            out darrcostbycaljsonByProduct, ref boolWorkflowJobIsReadyNotUsed);

                                        double? numnJobEstimateCost = null;
                                        double? numnJobEstimatePrice = null;

                                        if (
                                            //              //The Job can be estimate.
                                            boolCanBeEstimate
                                            )
                                        {
                                            numnJobEstimateCost = numSumOfProcessCost + numCostByProduct;
                                            numnJobEstimatePrice = JobJob.numGetPriceEstimate((double)numnJobEstimateCost,
                                                prodtyp, jobjson.intJobId, estentity, numExtraCost, context_M);
                                        }

                                        numnJobEstimateCost = numnJobEstimateCost != null ?
                                            ((double)numnJobEstimateCost).Round(2) : (double?)null;

                                        bool boolIsConfirmable = false;

                                        if (
                                            //              //it has options and already choose one option.
                                            boolHasOptionBudget && boolCanBeEstimate && !boolHasResourceIncomplete
                                            )
                                        {
                                            //              //Therefore, the estimation is confirmable.
                                            boolIsConfirmable = true;
                                        }

                                        ZonedTime ztimeBase = ZonedTimeTools.NewZonedTime(strBaseDate.ParseToDate(),
                                                strBaseTime.ParseToTime());
                                        String strName = "";
                                        if (
                                            estentity != null
                                            )
                                        {
                                            ztimeBase = JobJob.ztimeUpdateBase(estentity.intPk, strBaseDate,
                                                strBaseTime, context_M);
                                            strName = estentity.strName;
                                        }

                                        //                  //Convert base date
                                        ZonedTime ztimeBaseDateNew = ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                            ztimeBase.Date, ztimeBase.Time, ps_I.strTimeZone);
                                        String strBaseDateNew = ztimeBaseDateNew.Date.ToString();
                                        String strBaseTimeNew = ztimeBaseDateNew.Time.ToString();

                                        //                  //Get Delivery Date.
                                        String strDeliveryDate = "Undefined.";
                                        String strDeliveryTime = "";
                                        bool boolAllResourcesAreAvailable;

                                        if (
                                            intnEstimationId_I != -1
                                            )
                                        {
                                            //              //Delete old periods related to workflow and job.
                                            JobJob.subDeleteOldTemporaryPeriods(intPkWorkflow_I, intJobId_I);

                                            JobJob.subDeliveryDateBudgetEstimate((int)intnEstimationId_I, jobjson, ps_I,
                                                ztimeBase, darrpiwentityAllProcesses, darrpiwjson2, configuration_I,
                                                out boolAllResourcesAreAvailable, out strDeliveryDate,
                                                out strDeliveryTime);

                                            if (
                                                //          //Delivery date exists.
                                                strDeliveryDate.Length > 0 &&
                                                strDeliveryTime.Length > 0
                                                )
                                            {
                                                //          //Get date.
                                                Date dateDelivaryDate = strDeliveryDate.ParseToDate();
                                                //          //Get time.
                                                Time timeDeliveryTime = strDeliveryTime.ParseToTime();
                                                //          //Build ztime object for delivery date.
                                                ZonedTime ztimeDeliveryDate = 
                                                    ZonedTimeTools.ztimeCSTToASpecificTimeZone(
                                                    dateDelivaryDate, timeDeliveryTime, ps_I.strTimeZone);

                                                //          //Assign delivery date converted
                                                strDeliveryDate = ztimeDeliveryDate.Date.ToString();
                                                strDeliveryTime = ztimeDeliveryDate.Time.ToString();
                                            }
                                        }

                                        //                  //Get Due Date.
                                        String strDueDate = "";
                                        String strDueTime = "";
                                        bool boolIsDueDateReachableNotUsed;
                                        ProdtypProductType.subGetWorkflowDueDateAndCompareItToDeliveryDate(
                                            jobjson.intJobId, "", ps_I.strTimeZone, out strDueDate, out strDueTime,
                                            out boolIsDueDateReachableNotUsed);

                                        //                  //The default estimation is null === 0.
                                        //                  //      is null in table estimationData, 
                                        //                  //      is zero in table estimate.
                                        int intEstimationId = intnEstimationId_I == null ? 0 : (int)intnEstimationId_I;

                                        //                  //Workflow name
                                        String strNameWf = ProdtypProductType.strGetWorkflowName(intPkWorkflow_I,
                                            ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);

                                        //                          //Get strJobNumber
                                        String strJobNumber = "";
                                        int intOrderId = 0;
                                        if (
                                            jobjson.intnOrderId != null
                                            )
                                        {
                                            strJobNumber = JobJob.strGetJobNumber((int)jobjson.intnOrderId,
                                            jobjson.intJobId, ps_I.strPrintshopId, context_M);
                                            intOrderId = (int)jobjson.intnOrderId;
                                        }                                        

                                        //                  //Json to Send.
                                        bdgestjson_O = new BdgestjsonBudgetEstimationJson(intEstimationId,
                                            strName, intPkWorkflow_I, strNameWf, boolHasOptionBudget,
                                            intOrderId, jobjson.intJobId, jobjson.strJobTicket,
                                            (int)jobjson.intnProductKey, jobjson.strProductCategory, jobjson.intnQuantity,
                                            jobjson.dateLastUpdate, strBaseDateNew, strBaseTimeNew, strDeliveryDate,
                                            strDeliveryTime, strDueDate, strDueTime, jobjson.darrattrjson,
                                            prodtyp.intPk, prodtyp.strCustomTypeId, darrcostbycaljsonByProduct,
                                            darrprocbdgjson, numnJobEstimateCost, numnJobEstimatePrice,
                                            boolIsConfirmable, strJobNumber);

                                        intStatus_IO = 200;
                                        strUserMessage_IO = boolHasAllResourceSetted ? "" :
                                            "Note: There are processes with Inputs that are not being used.";
                                        strDevMessage_IO = boolHasAllResourceSetted ? "" :
                                            "Note: There are processes with Inputs that are not being used.";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subGetNormalProcessBudget(
            //                                              //A normal process has a final product.
            //                                              //arrbdgresjsonFromType and arrbdgresjsonFromTemplate
            //                                              //      methods will look for the final product and they
            //                                              //      will start propagating cost from that IO to the 
            //                                              //      start of the workflow.

            int? intnEstimationId_I,
            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjson_I,
            ProdtypProductType prodtyp_I,
            PsPrintShop ps_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityNormalProcess_I,
            IConfiguration configuration_I,
            List<Piwjson2ProcessInWorkflowJson2> darrpiwjson2_M,
            List<ProcbdgjsonProcessBudgetJson> darrprocbdgjson_M,
            ref double numExtraCost_IO,
            ref double numSumOfProcessCost_IO,
            ref bool boolCanBeEstimate_IO,
            ref bool boolHasAllResourceSetted_IO,
            ref bool boolHasOptionBudget_IO,
            ref bool boolHasResourceIncomplete_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            prodtyp_I.subAddCalculationsBasedOnJobStatus(jobentity_I, darrpiwentityAllProcesses_I, context);

            //                                              //List waste to propagate.                          
            List<WstpropjsonWasteToPropagateJson> darrwstpropjson = new List<WstpropjsonWasteToPropagateJson>();

            //                                              //List of quantityInputs and quantityOutputs.
            //                                              //    for optimization.
            List<IoqytjsonIOQuantityJson> darrioqytjsonIOQuantity = new List<IoqytjsonIOQuantityJson>();

            //                                              //Get the inputs of every process.
            foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentityNormalProcess_I)
            {
                //                                          //The lists are for optimization

                //                                          //Get eleet-s.
                List<EleetentityElementElementTypeEntityDB> darreleetentityAllEleEt =
                context.ElementElementType.Where(eleet => eleet.intPkElementDad == piwentity.intPkProcess).ToList();

                //                                          //Get eleele-s.
                List<EleeleentityElementElementEntityDB> darreleeleentityAllEleEle = context.ElementElement.Where(
                    eleele => eleele.intPkElementDad == piwentity.intPkProcess).ToList();

                //                                          //Get io-s.
                List<IoentityInputsAndOutputsEntityDB> darrioentityAllIO = context.InputsAndOutputs.Where(io =>
                    io.intPkWorkflow == piwentity.intPkWorkflow &&
                    io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId).ToList();

                //                                          //Get ioj-s.
                List<IojentityInputsAndOutputsForAJobEntityDB> darriojentityAllIOJ =
                    context.InputsAndOutputsForAJob.Where(ioj => ioj.intPkProcessInWorkflow == piwentity.intPk &&
                    ioj.intJobId == jobjson_I.intJobId).ToList();

                if (
                    !prodtyp_I.dicProcessIOs.ContainsKey(piwentity.intPk)
                    )
                {
                    List<Iofrmpiwjson2IOFromPIWJson2> darrioinfrmpiwjson2IosFromPIW;
                    ProdtypProductType.subGetProcessInputsAndOutputs(jobjson_I, piwentity, prodtyp_I,
                        darreleeleentityAllEleEle, darreleetentityAllEleEt, out darrioinfrmpiwjson2IosFromPIW);

                    prodtyp_I.dicProcessIOs.Add(piwentity.intPk, darrioinfrmpiwjson2IosFromPIW);
                }

                //                                          //List to Add IO Inputs.
                List<RecbdgjsonResourceBudgetJson> darrrecbdgjson = new List<RecbdgjsonResourceBudgetJson>();

                //                                          //Get cost resources by each Eleet IO.
                darrrecbdgjson.AddRange(prodtyp_I.arrbdgresjsonFromType(intnEstimationId_I, ps_I.strPrintshopId,
                    jobentity_I, jobjson_I, piwentity, darrdynlkjson_I, darreleetentityAllEleEt, darrioentityAllIO, 
                    darriojentityAllIOJ, darrpiwentityAllProcesses_I, configuration_I, darrioqytjsonIOQuantity,
                    darrwstpropjson, ref boolCanBeEstimate_IO, ref boolHasAllResourceSetted_IO, 
                    ref boolHasResourceIncomplete_IO));

                //                                          //Get cost resources by each Eleele IO.
                darrrecbdgjson.AddRange(prodtyp_I.arrbdgresjsonFromTemplate(intnEstimationId_I, ps_I.strPrintshopId,
                    jobentity_I, jobjson_I, piwentity, darrdynlkjson_I, darreleeleentityAllEleEle, darrioentityAllIO,
                    darriojentityAllIOJ, darrpiwentityAllProcesses_I, configuration_I, darrioqytjsonIOQuantity,
                    darrwstpropjson, ref boolCanBeEstimate_IO, ref boolHasAllResourceSetted_IO, 
                    ref boolHasResourceIncomplete_IO));

                //                                          //Get Next PIW for know if already was analized.
                IoqytjsonIOQuantityJson ioqytjsonWasPropagate = darrioqytjsonIOQuantity.FirstOrDefault(ioqyt =>
                    ioqyt.intPkProcessInWorkflow == piwentity.intPk);

                //                                          //Get index of the current PIW
                int index = Array.IndexOf(darrpiwentityNormalProcess_I.ToArray(), piwentity);

                if (
                    //                                      //This PIW was not analized or is the first PIW.
                    ioqytjsonWasPropagate == null || index == 0
                    )
                {
                    JobJob.subPropagateWasteEstimation(jobjson_I, piwentity, darrwstpropjson,
                        configuration_I, ps_I.strPrintshopId, intnEstimationId_I, ref darrrecbdgjson);
                }

                //                                          //Total extra cost per process.
                //                                          //This variable must be use for inputs and outputs.
                double numProcessExtraCost = 0.0;

                //                                          //Increase cost to input resources that contain an hourly 
                //                                          //      rate.
                JobJob.subCalculateResourcesHourlyRatesEstimateNormalProcess(ref darrrecbdgjson, 
                    ref numProcessExtraCost);

                numExtraCost_IO = numExtraCost_IO + numProcessExtraCost;

                //                                          //Remove from list, resources with link
                darrrecbdgjson.RemoveAll(recbdg => recbdg.strLink != null);

                //                                          //Sum total of resource cost.
                double numCostEstimateResourcesByProcess = 0;
                foreach (RecbdgjsonResourceBudgetJson recbdgjsonResources in darrrecbdgjson)
                {
                    if (
                        recbdgjsonResources.boolHasOption
                        )
                    {
                        //                                  //The budget has options.
                        boolHasOptionBudget_IO = true;
                    }

                    //                                      //Add cost resources.
                    numCostEstimateResourcesByProcess = numCostEstimateResourcesByProcess +
                        recbdgjsonResources.numCost;
                }

                double numJobNotUse = 0;

                //                                          //Get Calculation per process.
                List<CostbycaljsonCostByCalculationJson> darrcostbycaljsonPerProcess;
                bool boolWorkflowJobIsReadyNotUsed = true;
                double numCostByProcess = prodtyp_I.numGetCostByProcess(jobjson_I, piwentity.intPkProcess, 
                    piwentity.intPk, ps_I, out darrcostbycaljsonPerProcess, ref numJobNotUse, 
                    ref boolWorkflowJobIsReadyNotUsed);

                numSumOfProcessCost_IO = numSumOfProcessCost_IO +
                    numCostEstimateResourcesByProcess + numCostByProcess;
                //                                          //Json process'budget.

                //                                          //To easy code.
                String strProcessName = ProProcess.proFromDB(piwentity.intPkProcess).strName;
                strProcessName = piwentity.intnId != null ?
                    strProcessName + " (" + piwentity.intnId + ")" : strProcessName;
                ProcbdgjsonProcessBudgetJson procbdjjson = new ProcbdgjsonProcessBudgetJson(
                    piwentity.intPkProcess, piwentity.intPk, strProcessName,
                    darrcostbycaljsonPerProcess, darrrecbdgjson);

                //                                          //json for support
                Piwjson2ProcessInWorkflowJson2 piwjson2 = new Piwjson2ProcessInWorkflowJson2(
                    piwentity.intPk, darrrecbdgjson.ToArray());

                darrpiwjson2_M.Add(piwjson2);
                darrprocbdgjson_M.Add(procbdjjson);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subGetPostProcessBudget(

            int? intnEstimationId_I,
            JobentityJobEntityDB jobentity_I,
            JobjsonJobJson jobjson_I,
            ProdtypProductType prodtyp_I,
            PsPrintShop ps_I,
            List<DynLkjsonDynamicLinkJson> darrdynlkjson_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityPostProcess_I,
            IConfiguration configuration_I,
            List<Piwjson2ProcessInWorkflowJson2> darrpiwjson2_M,
            List<ProcbdgjsonProcessBudgetJson> darrprocbdgjson_M,
            ref double numExtraCost_IO,
            ref double numSumOfProcessCost_IO,
            ref bool boolCanBeEstimate_IO,
            ref bool boolHasAllResourceSetted_IO,
            ref bool boolHasOptionBudget_IO,
            ref bool boolHasResourceIncomplete_IO
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //List waste to propagate.                          
            List<WstpropjsonWasteToPropagateJson> darrwstpropjson = new List<WstpropjsonWasteToPropagateJson>();

            //                                              //List of quantityInputs and quantityOutputs.
            //                                              //    for optimization.
            List<IoqytjsonIOQuantityJson> darrioqytjsonIOQuantity = new List<IoqytjsonIOQuantityJson>();

            //                                              //Get the inputs of every process.
            foreach (PiwentityProcessInWorkflowEntityDB piwentity in darrpiwentityPostProcess_I)
            {
                //                                          //The lists are for optimization

                //                                          //Get eleet-s.
                List<EleetentityElementElementTypeEntityDB> darreleetentityAllEleEt =
                context.ElementElementType.Where(eleet => eleet.intPkElementDad == piwentity.intPkProcess).ToList();

                //                                          //Get eleele-s.
                List<EleeleentityElementElementEntityDB> darreleeleentityAllEleEle = context.ElementElement.Where(
                    eleele => eleele.intPkElementDad == piwentity.intPkProcess).ToList();

                //                                          //Get io-s.
                List<IoentityInputsAndOutputsEntityDB> darrioentityAllIO = context.InputsAndOutputs.Where(io =>
                    io.intPkWorkflow == piwentity.intPkWorkflow &&
                    io.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId).ToList();

                //                                          //Get ioj-s.
                List<IojentityInputsAndOutputsForAJobEntityDB> darriojentityAllIOJ =
                    context.InputsAndOutputsForAJob.Where(ioj => ioj.intPkProcessInWorkflow == piwentity.intPk &&
                    ioj.intJobId == jobjson_I.intJobId).ToList();

                if (
                    !prodtyp_I.dicProcessIOs.ContainsKey(piwentity.intPk)
                    )
                {
                    List<Iofrmpiwjson2IOFromPIWJson2> darrioinfrmpiwjson2IosFromPIW;
                    ProdtypProductType.subGetProcessInputsAndOutputs(jobjson_I, piwentity, prodtyp_I,
                        darreleeleentityAllEleEle, darreleetentityAllEleEt, out darrioinfrmpiwjson2IosFromPIW);

                    prodtyp_I.dicProcessIOs.Add(piwentity.intPk, darrioinfrmpiwjson2IosFromPIW);
                }

                //                                          //List to Add Input IOs.
                List<RecbdgjsonResourceBudgetJson> darrrecbdgjson = new List<RecbdgjsonResourceBudgetJson>();

                //                                          //Get cost resources by each Eleet IO.
                darrrecbdgjson.AddRange(prodtyp_I.arrbdgresjsonFromTypePostProcess(intnEstimationId_I, true,
                    ps_I.strPrintshopId, jobentity_I, jobjson_I, piwentity, darrdynlkjson_I, darreleetentityAllEleEt,
                    darrioentityAllIO, darriojentityAllIOJ, darrpiwentityAllProcesses_I, configuration_I, 
                    darrioqytjsonIOQuantity, ref boolCanBeEstimate_IO, ref boolHasAllResourceSetted_IO,
                    ref boolHasResourceIncomplete_IO));

                //                                          //Get cost resources by each Eleele IO.
                darrrecbdgjson.AddRange(prodtyp_I.arrbdgresjsonFromTemplatePostProcess(intnEstimationId_I, true,
                    ps_I.strPrintshopId, jobentity_I, jobjson_I, piwentity, darrdynlkjson_I, darreleeleentityAllEleEle,
                    darrioentityAllIO, darriojentityAllIOJ, darrpiwentityAllProcesses_I, configuration_I,
                    darrioqytjsonIOQuantity, ref boolCanBeEstimate_IO, ref boolHasAllResourceSetted_IO, 
                    ref boolHasResourceIncomplete_IO));

                //                                          //List to Add Output IOs.
                List<RecbdgjsonResourceBudgetJson> darrrecbdgjsonOutput = new List<RecbdgjsonResourceBudgetJson>();

                //                                          //Get cost resources by each Eleet IO.
                darrrecbdgjsonOutput.AddRange(prodtyp_I.arrbdgresjsonFromTypePostProcess(intnEstimationId_I, false,
                    ps_I.strPrintshopId, jobentity_I, jobjson_I, piwentity, darrdynlkjson_I, darreleetentityAllEleEt,
                    darrioentityAllIO, darriojentityAllIOJ, darrpiwentityAllProcesses_I, configuration_I,
                    darrioqytjsonIOQuantity, ref boolCanBeEstimate_IO, ref boolHasAllResourceSetted_IO,
                    ref boolHasResourceIncomplete_IO));

                //                                          //Get cost resources by each Eleele IO.
                darrrecbdgjsonOutput.AddRange(prodtyp_I.arrbdgresjsonFromTemplatePostProcess(intnEstimationId_I, false,
                    ps_I.strPrintshopId, jobentity_I, jobjson_I, piwentity, darrdynlkjson_I, darreleeleentityAllEleEle,
                    darrioentityAllIO, darriojentityAllIOJ, darrpiwentityAllProcesses_I, configuration_I,
                    darrioqytjsonIOQuantity, ref boolCanBeEstimate_IO, ref boolHasAllResourceSetted_IO,
                    ref boolHasResourceIncomplete_IO));

                //                                          //Total extra cost per process.
                //                                          //This variable must be use for inputs and outputs.
                double numProcessExtraCost_IO = 0.0;

                //                                          //Increase cost to input resources that contain an hourly 
                //                                          //      rate.
                JobJob.subCalculateResourcesHourlyRatesEstimatePostProcess(jobjson_I, piwentity,
                    configuration_I, ps_I.strPrintshopId, intnEstimationId_I, ref darrrecbdgjson,
                    ref numProcessExtraCost_IO);

                numExtraCost_IO = numExtraCost_IO + numProcessExtraCost_IO;

                //                                          //Remove from list, resources with link
                darrrecbdgjson.RemoveAll(recbdg => recbdg.strLink != null);

                //                                          //Sum total of resource cost.
                double numCostEstimateResourcesByProcess = 0;
                foreach (RecbdgjsonResourceBudgetJson recbdgjsonResources in darrrecbdgjson)
                {
                    if (
                        recbdgjsonResources.boolHasOption
                        )
                    {
                        //                                  //The budget has options.
                        boolHasOptionBudget_IO = true;
                    }

                    //                                      //Add cost resources.
                    numCostEstimateResourcesByProcess = numCostEstimateResourcesByProcess +
                        recbdgjsonResources.numCost;
                }

                double numJobNotUse = 0;

                //                                          //Get Calculation per process.
                List<CostbycaljsonCostByCalculationJson> darrcostbycaljsonPerProcess;
                bool boolWorkflowJobIsReadyNotUsed = false;
                double numCostByProcess = prodtyp_I.numGetCostByProcess(jobjson_I,
                    piwentity.intPkProcess, piwentity.intPk, ps_I,
                    out darrcostbycaljsonPerProcess, ref numJobNotUse, ref boolWorkflowJobIsReadyNotUsed);

                numSumOfProcessCost_IO = numSumOfProcessCost_IO +
                    numCostEstimateResourcesByProcess + numCostByProcess;

                //                                          //Json process'budget.

                //                                          //To easy code.
                String strProcessName = ProProcess.proFromDB(piwentity.intPkProcess).strName;
                strProcessName = piwentity.intnId != null ?
                    strProcessName + " (" + piwentity.intnId + ")" : strProcessName;
                ProcbdgjsonProcessBudgetJson procbdjjson = new ProcbdgjsonProcessBudgetJson(
                    piwentity.intPkProcess, piwentity.intPk, strProcessName,
                    darrcostbycaljsonPerProcess, darrrecbdgjson);

                //                                          //json for support
                Piwjson2ProcessInWorkflowJson2 piwjson2 = new Piwjson2ProcessInWorkflowJson2(
                    piwentity.intPk, darrrecbdgjson.ToArray());

                darrpiwjson2_M.Add(piwjson2);
                darrprocbdgjson_M.Add(procbdjjson);
                
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subPropagateWasteEstimation(
            //                                              //Propagate waste to IO correspondent.

            JobjsonJobJson jobjson_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            List<WstpropjsonWasteToPropagateJson> darrwstpropjson_I,
            IConfiguration configuration_I,
            String strPrintshopId_I,
            int? intnEstimateIdThatInvokeThisMethod_I,
            ref List<RecbdgjsonResourceBudgetJson> darrrecbdgjson_M
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            //                                              //take each IO Input from a current PIW.
            foreach (RecbdgjsonResourceBudgetJson recbdgjson in darrrecbdgjson_M)
            {
                //                                          //EleetOrEleele of each IO input.
                int? intnPkEleet = recbdgjson.boolIsEleet ? (int?)recbdgjson.intPkEleetOrEleele : null;
                int? intnPkEleeele = !recbdgjson.boolIsEleet ? (int?)recbdgjson.intPkEleetOrEleele : null;

                //                                          //Find waste for this IO input.
                List<WstpropjsonWasteToPropagateJson> darrwstpropjson = darrwstpropjson_I.Where(wsttoprop =>
                    wsttoprop.intnPkEleetTarget == intnPkEleet && wsttoprop.intnPkEleeleTarget == intnPkEleeele &&
                    wsttoprop.intPkProcessInWorkflow == piwentity_I.intPk).ToList();

                //                                          //List additional waste.
                List<WstaddjsonWasteAdditionalJson> darrwstjsonWasteAdditional =
                    new List<WstaddjsonWasteAdditionalJson>();

                //                                          //Init the waste that added at the IO Input.
                double numWaste = 0;

                //                                          //Get all waste apply for the IO input.
                foreach (WstpropjsonWasteToPropagateJson wastetoprop in darrwstpropjson)
                {
                    if (
                        wastetoprop.numWaste > 0
                        )
                    {
                        numWaste = (wastetoprop.numWaste / wastetoprop.numFactor).Round(2);
                    }

                    if (
                        //                              //Unit allow Decimal.
                        recbdgjson.boolAllowDecimal
                        )
                    {
                        numWaste = numWaste.Round(2);
                        recbdgjson.numQuantity = (recbdgjson.numQuantity + numWaste).Round(2);
                    }
                    else
                    //                                  //Unit not allow Decimal
                    {
                        numWaste = Math.Ceiling(numWaste);
                        recbdgjson.numQuantity = Math.Ceiling(recbdgjson.numQuantity + numWaste);
                    }

                    int? intnPkSourceRes = null;

                    //                                      //Find Source IO input Resource.
                    IoentityInputsAndOutputsEntityDB ioentitySourceRes = context.InputsAndOutputs.FirstOrDefault(
                        io => io.intPkWorkflow == piwentity_I.intPkWorkflow &&
                        io.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId &&
                        io.intnPkElementElementType == wastetoprop.intnPkEleetSource &&
                        io.intnPkElementElement == wastetoprop.intnPkEleeleSource);

                    if (
                        //                                  //If QFrom resource is not in a GroupResource
                        ioentitySourceRes != null
                        )
                    {
                        if (
                            //                              //If the IO has a resource group, mean the already choose a options
                            //                              //    resource or the IO has a resourceGroup and the res was setted  
                            //                              //    in the WFJob.
                            ioentitySourceRes.intnGroupResourceId != null
                            )
                        {
                            IojentityInputsAndOutputsForAJobEntityDB ioentityQfromResForAJob =
                            context.InputsAndOutputsForAJob.FirstOrDefault(iofaj =>
                            iofaj.intPkProcessInWorkflow == piwentity_I.intPk &&
                            iofaj.intnPkElementElementType == wastetoprop.intnPkEleetSource &&
                            iofaj.intnPkElementElement == wastetoprop.intnPkEleeleSource);

                            if (
                                //                              //If QFrom resource is set in the job
                                ioentityQfromResForAJob != null
                                )
                            {
                                intnPkSourceRes = ioentityQfromResForAJob.intPkResource;
                            }
                            else
                            {
                                //                              //The resource was selecte how a option in estimation.
                                EstdataentityEstimationDataEntityDB estdata = context.EstimationData.FirstOrDefault(
                                estdata => estdata.intPkProcessInWorkflow == piwentity_I.intPk &&
                                estdata.intnPkElementElementType == wastetoprop.intnPkEleetSource &&
                                estdata.intnPkElementElement == wastetoprop.intnPkEleeleSource);

                                intnPkSourceRes = estdata.intPkResource;
                            }
                        }
                        else
                        {
                            intnPkSourceRes = (int)ioentitySourceRes.intnPkResource;
                        }
                    }
                    //                                      //If QFrom resource is in a GroupResource
                    else
                    {
                        IojentityInputsAndOutputsForAJobEntityDB ioentityQfromResForAJob =
                            context.InputsAndOutputsForAJob.FirstOrDefault(iofaj =>
                            iofaj.intPkProcessInWorkflow == piwentity_I.intPk &&
                            iofaj.intnPkElementElementType == wastetoprop.intnPkEleetSource &&
                            iofaj.intnPkElementElement == wastetoprop.intnPkEleeleSource);

                        if (
                            //                              //If QFrom resource is set in the job
                            ioentityQfromResForAJob != null
                            )
                        {
                            intnPkSourceRes = ioentityQfromResForAJob.intPkResource;
                        }
                    }

                    //                                      //Find Source resource.
                    EleentityElementEntityDB eleentitySource = context.Element.FirstOrDefault(ele =>
                        ele.intPk == intnPkSourceRes);

                    String strSourceResName = eleentitySource.strElementName;

                    //                                      //Find Source resource type.
                    EtElementTypeAbstract eletSource = EletemElementType.etFromDB(eleentitySource.intPkElementType);

                    String strSource;
                    //                                      //QFrom Resource is ElementElementType
                    if (
                        wastetoprop.intnPkEleetSource != null
                        )
                    {
                        //                                  //Type and resource concatenation.
                        strSource = "(" + eletSource.strXJDFTypeId + ") " + strSourceResName;
                    }
                    //                                      //QFrom Resource is ElementElement
                    else
                    {
                        //                                  //Find ElementElement resource.
                        EleeleentityElementElementEntityDB eleeleentity = context.ElementElement.FirstOrDefault(
                            eleele => eleele.intPk == ioentitySourceRes.intnPkElementElement);

                        //                                  //Find template of QFrom resource.
                        EleentityElementEntityDB eleentitytemplate = context.Element.FirstOrDefault(ele =>
                            ele.intPk == eleeleentity.intPkElementSon);

                        //                                  //Type, template and resource concatenation.
                        strSource = "(" + eletSource.strXJDFTypeId + " : " +
                            eleentitytemplate.strElementName + ") " + strSourceResName;
                    }

                    WstaddjsonWasteAdditionalJson wstaddjson = new WstaddjsonWasteAdditionalJson(
                        numWaste, strSource);

                    darrwstjsonWasteAdditional.Add(wstaddjson);
                }

                //                                          //Recalculate cost with new quantity.
                if (
                    recbdgjson.intnPkResource != null
                    )
                {
                    ResResource resTarget = ResResource.resFromDB(recbdgjson.intnPkResource, false);
                    recbdgjson.numCost = ProdtypProductType.numGetCostAdditional(jobjson_I, recbdgjson.numQuantity,
                        resTarget);

                    //                                      //Calculate new Time with new Quantity.
                    int intHours = 0;
                    int intMinutes = 0;
                    int intSeconds = 0;
                    ProdtypProductType.subCalculateResTimeFromQuantity((int)recbdgjson.intnPkResource, jobjson_I,
                        piwentity_I, recbdgjson.intPkEleetOrEleele, recbdgjson.boolIsEleet,
                        recbdgjson.numQuantity, configuration_I, strPrintshopId_I, intnEstimateIdThatInvokeThisMethod_I,
                        null, ref intHours, ref intMinutes, ref intSeconds);

                    recbdgjson.intHours = intHours;
                    recbdgjson.intMinutes = intMinutes;
                    recbdgjson.intSeconds = intSeconds;
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolValidBaseDate(
            String strBaseDate_I,
            String strBaseTime_I,
            String strPrintshopTimeZone_I,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolValidBaseDate = false;

            if (
                strBaseDate_I == null &&
                strBaseTime_I == null
                )
            {
                boolValidBaseDate = true;
            }
            else
            {
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Date or time format is not valid.";
                if (
                    //                                      //Validate date and time strings are parseable.
                    strBaseDate_I.IsParsableToDate() &&
                    strBaseTime_I.IsParsableToTime()
                    )
                {
                    //ZonedTime ztimeBasePeriodToAdd = ZonedTimeTools.NewZonedTime(strBaseDate_I.ParseToDate(),
                    //    strBaseTime_I.ParseToTime());
                    ZonedTime ztimeBasePeriodToAdd = 
                        ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(strBaseDate_I.ParseToDate(),
                        strBaseTime_I.ParseToTime(), strPrintshopTimeZone_I);

                    strUserMessage_IO = "Select valid date or time.";
                    strDevMessage_IO = "Date or time are in the past";
                    if (
                        //                                  //Date and time are in the future.
                        ztimeBasePeriodToAdd >= ZonedTimeTools.NewZonedTime(Date.Now(ZonedTimeTools.timezone),
                            Time.Now(ZonedTimeTools.timezone))
                        )
                    {
                        boolValidBaseDate = true;
                    }
                }
            }

            return boolValidBaseDate;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subDeliveryDateBudgetEstimate(
            //                                              //Calculate delivery date, based on temporary periods.

            int intEstimateId_I,
            JobjsonJobJson jobjson_I,
            PsPrintShop ps_I,
            ZonedTime ztimeBase_I,
            List<PiwentityProcessInWorkflowEntityDB> darrpiwentityAllProcesses_I,
            List<Piwjson2ProcessInWorkflowJson2> darrpiwjson2_I,
            IConfiguration configuration_I,
            //                                              //True if all resources are calendar or available.
            //                                              //If false, estimation will be unavailable.
            out bool boolAllResourcesAreAvailable_O,
            out String strDeliveryDate_O,
            out String strDeliveryTime_O
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            boolAllResourcesAreAvailable_O = true;

            ZonedTime ztimeBaseNew = ztimeBase_I;

            strDeliveryDate_O = ztimeBaseNew.Date.ToString();
            strDeliveryTime_O = ztimeBaseNew.Time.ToString();

            //                                              //Array of Process in workflow.
            Piwjson2ProcessInWorkflowJson2[] arrpiw = darrpiwjson2_I.ToArray();

            ZonedTime ztimeNextProcess = ztimeBaseNew;

            List<PerentityPeriodEntityDB> darrperentityAllTemporary = new List<PerentityPeriodEntityDB>();

            int intI = 0;
            /*WHILE-DO*/
            while (
                intI < arrpiw.Length &&
                boolAllResourcesAreAvailable_O
                )
            {
                //                                          //List of temporary periods.
                List<PerentityPeriodEntityDB> darrperentityTemporary = new List<PerentityPeriodEntityDB>();

                //                                          //Inputs.
                RecbdgjsonResourceBudgetJson[] arrrecbdgjson = arrpiw[intI].arrrecbdgjsonInput;

                TimesjsonTimesJson timesjson = new TimesjsonTimesJson(ztimeBaseNew.Date.ToString(),
                    ztimeBaseNew.Time.ToString(), ztimeBaseNew.Date.ToString(), ztimeBaseNew.Time.ToString());

                //                                          //Get PIW.
                PiwentityProcessInWorkflowEntityDB piwentity = darrpiwentityAllProcesses_I.FirstOrDefault(
                    piw => piw.intPk == arrpiw[intI].intPkProcessInWorkflow);

                int intJ = 0;
                /*WHILE-DO*/
                while (
                    intJ < arrrecbdgjson.Length &&
                    boolAllResourcesAreAvailable_O
                    )
                {
                    ResResource res = ResResource.resFromDB(arrrecbdgjson[intJ].intnPkResource, false);
                    if (
                        (res != null) &&
                        arrrecbdgjson[intJ].boolnIsCalendar != null &&
                        ((bool)arrrecbdgjson[intJ].boolnIsCalendar ||
                        !(bool)arrrecbdgjson[intJ].boolnIsCalendar && (bool)arrrecbdgjson[intJ].boolnIsAvailable)
                        )
                    {
                        if (
                            res.boolnIsCalendar == true
                            )
                        {
                            bool boolIsEleet = arrrecbdgjson[intJ].boolIsEleet;

                            //                              //To easy code.
                            int? intnPkElementElementType = boolIsEleet ?
                                arrrecbdgjson[intJ].intPkEleetOrEleele : (int?)null;

                            int? intnPkElementElement = !boolIsEleet ?
                                arrrecbdgjson[intJ].intPkEleetOrEleele : (int?)null;

                            int intOffsetTimeMinuteAfterDateNow = 0;

                            ResResource.subGetAvailableTime(intEstimateId_I, intOffsetTimeMinuteAfterDateNow,
                                arrrecbdgjson[intJ], jobjson_I, piwentity, ps_I, res, ztimeNextProcess,
                                darrperentityTemporary, out timesjson);

                            //                              //Calculate tolerance time based on the temporary period
                            //                              //      just created.
                            int intMinsBeforeDelete;
                            String strDeleteDate;
                            String strDeleteHour;
                            String strDeleteMinute;
                            JobJob.subCalculateTemporaryToleranceTime(timesjson.strStartDate, timesjson.strStartTime,
                                 timesjson.strEndDate, timesjson.strEndTime, out intMinsBeforeDelete,
                                 out strDeleteDate, out strDeleteHour, out strDeleteMinute);

                            PerentityPeriodEntityDB perentityTemp = new PerentityPeriodEntityDB
                            {
                                strStartDate = timesjson.strStartDate,
                                strStartTime = timesjson.strStartTime,
                                strEndDate = timesjson.strEndDate,
                                strEndTime = timesjson.strEndTime,
                                intJobId = jobjson_I.intJobId,
                                boolIsException = false,
                                intnContactId = null,
                                intnEstimateId = intEstimateId_I,
                                intPkWorkflow = piwentity.intPkWorkflow,
                                intProcessInWorkflowId = piwentity.intProcessInWorkflowId,
                                intnPkElementElementType = intnPkElementElementType,
                                intnPkElementElement = intnPkElementElement,
                                intPkElement = res.intPk,
                                intnPkCalculation = null,
                                intMinsBeforeDelete = intMinsBeforeDelete,
                                strDeleteDate = strDeleteDate,
                                strDeleteHour = strDeleteHour,
                                strDeleteMinute = strDeleteMinute
                            };

                            darrperentityTemporary.Add(perentityTemp);
                            darrperentityAllTemporary.Add(perentityTemp);
                        }
                    }
                    else
                    {
                        boolAllResourcesAreAvailable_O = false;
                        strDeliveryDate_O = "Unavailable";
                        strDeliveryTime_O = "";
                    }
                    intJ = intJ + 1;
                }

                ZonedTime ztimeThisProcess = ztimeNextProcess;
                //                                          //Get ztime period temporary from PIW.
                ZonedTime ztimeLongerProcessPeriodTemporaryFromPIW = JobJob.ztimePeriodTemporaryFromPIW(ps_I,
                    jobjson_I, piwentity, ztimeThisProcess, configuration_I);

                //                                          //Get ztime period temporary from Resource.
                ZonedTime ztimeLongerResourcePeriodTemporaryFromPIW = ZonedTimeTools.NewZonedTime(
                    timesjson.strEndDate.ParseToDate(), timesjson.strEndTime.ParseToTime());

                //                                          //Get ztimeBase for the next ProcessInWorkflow.
                ztimeNextProcess =
                    ztimeLongerProcessPeriodTemporaryFromPIW > ztimeLongerResourcePeriodTemporaryFromPIW ?
                    ztimeLongerProcessPeriodTemporaryFromPIW : ztimeLongerResourcePeriodTemporaryFromPIW;

                intI = intI + 1;
            }

            if (
                //                                          //All resources available.
                boolAllResourcesAreAvailable_O
                )
            {
                strDeliveryDate_O = ztimeNextProcess.Date.ToString();
                strDeliveryTime_O = ztimeNextProcess.Time.ToString();

                if (
                    intEstimateId_I > 0
                    )
                {
                    foreach (PerentityPeriodEntityDB perentityTemp in darrperentityAllTemporary)
                    {
                        context.Period.Add(perentityTemp);
                        context.SaveChanges();
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subDeleteOldTemporaryPeriods(
            //                                              //Delete temporary periods that belong to the workflow 
            //                                              //      and the job.
            //                                              //A temporary period is generated when opening one of the 
            //                                              //      estimates.

            int intPkWorkflow_I,
            int intJobId_I
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find periods.
            List<PerentityPeriodEntityDB> darrperentity = context.Period.Where(per =>
                per.intPkWorkflow == intPkWorkflow_I && per.intJobId == intJobId_I &&
                per.intnEstimateId != null).ToList();

            foreach (PerentityPeriodEntityDB perentity in darrperentity)
            {
                //                                          //Delete period.
                context.Period.Remove(perentity);
                context.SaveChanges();
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subCalculateTemporaryToleranceTime(
            //                                              //Calculate tolerance time of a temporary period based on
            //                                              //      its duration.

            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            out int intMinsBeforeDelete_O,
            out String strDeleteDate_O,
            out String strDeleteHour_O,
            out String strDeleteMinute_O
            )
        {
            //                                              //Calculate temporary period duration.

            ZonedTime ztimeTemporaryStartPer = ZonedTimeTools.NewZonedTime(strStartDate_I.ParseToDate(),
                strStartTime_I.ParseToTime());
            ZonedTime ztimeTemporaryEndPer = ZonedTimeTools.NewZonedTime(strEndDate_I.ParseToDate(),
                strEndTime_I.ParseToTime());
            double numDurationMilliseconds = ztimeTemporaryEndPer - ztimeTemporaryStartPer;

            //                                              //Tolerance duration = 5 mins, only if period duration is 
            //                                              //      larger than 5 mins. Otherwise, tolerance is equals
            //                                              //      to the period duration.
            int intFiveMinutesToMilliseconds = 5 * 60000;
            if (
                numDurationMilliseconds >= intFiveMinutesToMilliseconds
                )
            {
                intMinsBeforeDelete_O = 5;

                //                                          //Calculate delete date.

                ZonedTime ztimeDeletePer = ztimeTemporaryStartPer + intFiveMinutesToMilliseconds;
                strDeleteDate_O = ztimeDeletePer.Date.ToString();

                //                                          //Calculate delete time.

                strDeleteHour_O = ztimeDeletePer.Time.ToString().Substring(0, 2);
                strDeleteMinute_O = ztimeDeletePer.Time.ToString().Substring(3, 2);
            }
            else
            {
                //                                          //Milliseconds to minutes.
                intMinsBeforeDelete_O = (int)(numDurationMilliseconds / 60000);

                //                                          //Calculate delete date.

                ZonedTime ztimeDeletePer = ztimeTemporaryStartPer + (int)numDurationMilliseconds;
                strDeleteDate_O = ztimeDeletePer.Date.ToString();

                //                                          //Calculate delete time.

                strDeleteHour_O = ztimeDeletePer.Time.ToString().Substring(0, 2);
                strDeleteMinute_O = ztimeDeletePer.Time.ToString().Substring(3, 2);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subCalculateResourcesHourlyRatesEstimateNormalProcess(
            //                                              //Calculates hourly rates per resource in normal processes.

            //                                              //List containing IOs.
            ref List<RecbdgjsonResourceBudgetJson> darrrecbdgjson_M,
            //                                              //This variable must be the same when calculating inputs
            //                                              //      and outputs.
            //                                              //Do not create a new one when calculating output costs or 
            //                                              //      will be lost.
            ref double numProcessExtraCost_IO
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            foreach (RecbdgjsonResourceBudgetJson recbdgjson in darrrecbdgjson_M)
            {
                //                                          //Find resource.
                ResResource res = ResResource.resFromDB(recbdgjson.intnPkResource, !recbdgjson.boolIsEleet);
                if (
                    res != null
                    )
                {
                    //                                          //Get current cost.
                    CostentityCostEntityDB costentity = ResResource.costentityCurrentResourceCost(context, res);

                    if (
                        //                                      //Cost and hourly rate exists.
                        costentity != null &&
                        costentity.numnHourlyRate != null
                        )
                    {
                        //                                      //Get total hours.

                        //                                      //Transform mins and secs to hours.
                        double numHours = (double)recbdgjson.intHours;
                        double numMinsToHours = (double)recbdgjson.intMinutes / 60d;
                        double numSecsToHours = (double)recbdgjson.intSeconds / 3600d;
                        //                                      //Sum up hours.
                        double numTotalHours = numHours + numMinsToHours + numSecsToHours;

                        //                                      //Calculate hourly rate.
                        double numResourceExtraCost = numTotalHours * (double)costentity.numnHourlyRate;

                        //                                      //Sum up hourly rate to resource's cost.
                        recbdgjson.numCost = recbdgjson.numCost + numResourceExtraCost;

                        //                                      //Add extra cost to process extra cost counter.
                        numProcessExtraCost_IO = numProcessExtraCost_IO + numResourceExtraCost;
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public static void subCalculateResourcesHourlyRatesEstimatePostProcess(
            //                                              //Calculates hourly rates per resource in post processes.

            JobjsonJobJson jobjson_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            IConfiguration configuration_I,
            String strPrintshopId_I,
            int? intnEstimationId_I,

            //                                              //List containing IOs.
            ref List<RecbdgjsonResourceBudgetJson> darrrecbdgjson_M,
            //                                              //This variable must be the same when calculating inputs
            //                                              //      and outputs.
            //                                              //Do not create a new one when calculating output costs or 
            //                                              //      will be lost.
            ref double numProcessExtraCost_IO
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            foreach (RecbdgjsonResourceBudgetJson recbdgjson in darrrecbdgjson_M)
            {
                //                                      //Find resource.
                ResResource res = ResResource.resFromDB(recbdgjson.intnPkResource, !recbdgjson.boolIsEleet);
                if (
                    res != null
                    )
                {
                    //                                      //Calculate new Time with new Quantity.
                    int intHours = 0;
                    int intMinutes = 0;
                    int intSeconds = 0;

                    ProdtypProductType.subCalculateResTimeFromQuantity((int)recbdgjson.intnPkResource, jobjson_I,
                        piwentity_I, recbdgjson.intPkEleetOrEleele, recbdgjson.boolIsEleet,
                        recbdgjson.numQuantity, configuration_I, strPrintshopId_I, intnEstimationId_I, null,
                        ref intHours, ref intMinutes, ref intSeconds);

                    recbdgjson.intHours = intHours;
                    recbdgjson.intMinutes = intMinutes;
                    recbdgjson.intSeconds = intSeconds;

                    //                                      //Get current cost.
                    CostentityCostEntityDB costentity = ResResource.costentityCurrentResourceCost(context, res);

                    if (
                        //                                  //Cost and hourly rate exists.
                        costentity != null &&
                        costentity.numnHourlyRate != null
                        )
                    {
                        //                                  //Get total hours.

                        //                                  //Transform mins and secs to hours.
                        double numHours = (double)recbdgjson.intHours;
                        double numMinsToHours = (double)recbdgjson.intMinutes / 60d;
                        double numSecsToHours = (double)recbdgjson.intSeconds / 3600d;
                        //                                  //Sum up hours.
                        double numTotalHours = numHours + numMinsToHours + numSecsToHours;

                        //                                  //Calculate hourly rate.
                        double numResourceExtraCost = numTotalHours * (double)costentity.numnHourlyRate;

                        //                                  //Sum up hourly rate to resource's cost.
                        recbdgjson.numCost = recbdgjson.numCost + numResourceExtraCost;

                        //                                  //Add extra cost to process extra cost counter.
                        numProcessExtraCost_IO = numProcessExtraCost_IO + numResourceExtraCost;
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        private static ZonedTime ztimePeriodTemporaryFromPIW(
            PsPrintShop ps_I,
            JobjsonJobJson jobjson_I,
            PiwentityProcessInWorkflowEntityDB piwentity_I,
            ZonedTime ztimeBase_I,
            IConfiguration configuration_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            ZonedTime ztimePeriodLargerForThisPIWTemporary = ztimeBase_I;

            //                                              //Get Calculation fron this PIW.
            List<CalentityCalculationEntityDB> darrcalentityCalFromPIW = context.Calculation.Where(
                cal => cal.intnPkWorkflow == piwentity_I.intPkWorkflow &&
                cal.intnProcessInWorkflowId == piwentity_I.intProcessInWorkflowId &&
                cal.intnPkElementElement == null && cal.intnPkElementElementType == null &&
                cal.strEndDate == null && cal.strEndTime == null && cal.intnHours != null && cal.intnMinutes != null &&
                cal.intnSeconds != null && cal.boolIsEnable).ToList();

            //                                              //NotUsed beacuase 
            //                                              //    this has already been validated.
            int intStatus = 0;
            String strUserMessage = "";
            String strDevMessage = "";

            foreach (CalentityCalculationEntityDB calentity in darrcalentityCalFromPIW)
            {
                EndperjsonEndOfPeriodJson endperjson;

                if (
                    //                                  //Verify calculations conditions.
                    Tools.boolCalculationOrLinkApplies(calentity.intPk, null, null, null, jobjson_I)
                    )
                {
                    //                                      //Get period temporary from a piw.
                    ProProcess.subGetEndOfPeriod(ps_I.strPrintshopId, jobjson_I.intJobId, piwentity_I.intPk,
                        calentity.intPk, ztimeBase_I.Date.ToString(), ztimeBase_I.Time.ToString(),
                        configuration_I, out endperjson, ref intStatus, ref strDevMessage,
                        ref strUserMessage);

                    if (
                        endperjson != null
                        )
                    {
                        ZonedTime ztimePerTemp = ZonedTimeTools.NewZonedTime(endperjson.strEndDate.ParseToDate(),
                            endperjson.strEndTime.ParseToTime());

                        if (
                            ztimePerTemp > ztimePeriodLargerForThisPIWTemporary
                            )
                        {
                            //                              //Get period temporary largest from a PIW.
                            ztimePeriodLargerForThisPIWTemporary = ztimePerTemp;
                        }
                    }
                }
            }

            return ztimePeriodLargerForThisPIWTemporary;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static ZonedTime ztimeUpdateBase(
            //                                              //Update the Base date and time in the database.
            //                                              //Returns the ztime.

            int intPkEstimate_I,
            String strBaseDate_I,
            String strBaseTime_I,
            Odyssey2Context context_M
            )
        {
            ZonedTime ztimeBase = new ZonedTime();

            EstentityEstimateEntityDB estentity = context_M.Estimate.FirstOrDefault(est =>
                est.intPk == intPkEstimate_I);

            if (
                //                                          //Date received.
                !String.IsNullOrEmpty(strBaseDate_I) &&
                !String.IsNullOrEmpty(strBaseTime_I)
                )
            {
                if (
                    strBaseDate_I.IsParsableToDate() &&
                    strBaseTime_I.IsParsableToTime()
                    )
                {
                    ztimeBase = ZonedTimeTools.NewZonedTime(strBaseDate_I.ParseToDate(), strBaseTime_I.ParseToTime());

                    estentity.strBaseDate = strBaseDate_I;
                    estentity.strBaseTime = strBaseTime_I;

                    context_M.Update(estentity);
                    context_M.SaveChanges();
                }
                else
                {
                    //                                      //Date or time not valid.
                }

                ztimeBase = ZonedTimeTools.NewZonedTime(estentity.strBaseDate.ParseToDate(),
                    estentity.strBaseTime.ParseToTime());
            }

            return ztimeBase;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static double numGetPriceEstimate(
            //                                              //Get price of the estimate.

            double numCost_I,
            ProdtypProductType prodtyp_I,
            int intJobId_I,
            EstentityEstimateEntityDB estentity_I,
            double numExtraCost_I,
            Odyssey2Context context_I
            )
        {
            //                                              //Get last price.
            List<PriceentityPriceEntityDB> darrpriceentity = context_I.Price.Where(price =>
                price.intJobId == intJobId_I && price.intnPkEstimate == estentity_I.intPk).ToList();
            darrpriceentity.Sort();

            double numPrice = 0;
            /*CASE*/
            if (
                //                                          //There are a price.
                (darrpriceentity.Count() > 0) &&
                //                                          //It was'nt reset the current price.
                (darrpriceentity[0].numnPrice != null &&
                darrpriceentity[0].boolIsReset == false)
                )
            {
                numPrice = (double)darrpriceentity[0].numnPrice;

                //                                          //Update the lastPrice for estimate confirmed.
                estentity_I.numnLastPrice = estentity_I.intId == 0 ? (numPrice) : (double?)null; 
                context_I.Estimate.Update(estentity_I);
                context_I.SaveChanges();
            }
            else
            {
                //                                          //
                numPrice = numCost_I;
                double numCostProfiatble = numCost_I - numExtraCost_I;
                double numCostNonProfiatble = numExtraCost_I;

                CalentityCalculationEntityDB calentity = context_I.Calculation.FirstOrDefault(cal =>
                    cal.intnPkProduct == prodtyp_I.intPk && cal.strCalculationType == CalCalculation.strProfit &&
                    cal.boolIsEnable == true);

                if (
                    calentity != null
                    )
                {
                    double numProfit = (double)calentity.numnProfit / 100;
                    numPrice = (numCostProfiatble * (1 + numProfit) + numCostNonProfiatble).Round(2);
                }

                //                                      //Update the lastPrice for estimate confirmed.
                estentity_I.numnLastPrice = estentity_I.intId == 0 ? (numPrice) : (double?)null;
                context_I.Estimate.Update(estentity_I);
                context_I.SaveChanges();
            }

            return numPrice;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subNotifyNewOrder(
            //                                              //Adds the notifications to the Alert Table and calls the 
            //                                              //      front end method AlertForAll to notified all the
            //                                              //      contacts from the printshop.

            XPathNavigator xpathnav_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO,
            IConfiguration iConfiguration_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            //                                              //Get the PrintshopId and JobId.
            String strPrintShopId = JobJob.strGetInfoFromXPath(xpathnav_I,
                "/cXML/Header/From/Credential/Identity");
            String strJobId = JobJob.strGetInfoFromXPath(xpathnav_I,
                "/cXML/Request/OrderRequest/ItemOut/ID");
            String strContactId = JobJob.strGetInfoFromXPath(xpathnav_I,
                "/cXML/Request/OrderRequest/OrderRequestHeader/Contact/ID");
            int intOtherAttributes = JobJob.intOtherAttributes(xpathnav_I,
                "/cXML/Request/OrderRequest/ItemOut/ItemDetail/Extrinsic");

            intStatus_IO = 401;
            strUserMessage_IO = "Something went wrong.";
            strDevMessage_IO = "Incorrect job id.";
            if (
                strJobId.IsParsableToInt() &&
                strContactId != null &&
                strContactId.IsParsableToInt()
                )
            {
                //                                          //Get job id.
                int intJobId = strJobId.ParseToInt();

                intStatus_IO = 402;
                if (
                    JobJob.boolIsValidCustomer(strPrintShopId, strContactId, ref intStatus_IO,
                        ref strUserMessage_IO, ref strDevMessage_IO)
                    ||
                    ResResource.boolEmployeeOrOwnerIsFromPrintshop(strPrintShopId, strContactId.ParseToInt())
                    )
                {
                    //                                      //Establish connection.
                    Odyssey2Context context = new Odyssey2Context();

                    //                                      //Get the Printshop entity.
                    PsentityPrintshopEntityDB psentity = context.Printshop.FirstOrDefault(ps =>
                        ps.strPrintshopId == strPrintShopId);


                    intStatus_IO = 404;
                    strUserMessage_IO = "Something went wrong.";
                    strDevMessage_IO = "Printshop not found.";
                    if (
                        psentity != null
                        )
                    {
                        //                                  //Get the Customer Representatives from Wisnet.
                        List<int> darrintCustomerReps = JobJob.darrintGetCustomerReps(strPrintShopId, strContactId,
                            iConfiguration_I);

                        //                                  //Get the Superviser from the printshop.
                        List<int> darrintCustomerSuperviser = (from roleentity in context.Role
                                                               where roleentity.intPkPrintshop == psentity.intPk &&
                                                               roleentity.boolSupervisor == true
                                                               select roleentity.intContactId).ToList();

                        //                                  //Union: join the two lists excluding repeats
                        List<int> darrintCustomerRepsAndSuperviser = darrintCustomerReps.Union(
                            darrintCustomerSuperviser != null ? darrintCustomerSuperviser :
                            new List<int>()).ToList();

                        intStatus_IO = 404;
                        strUserMessage_IO = "Something went wrong.";
                        strDevMessage_IO = "Customer Representatives not found from Wiznet.";
                        if (
                            darrintCustomerRepsAndSuperviser != null &&
                            darrintCustomerRepsAndSuperviser.Count > 0
                            )
                        {
                            PsPrintShop ps = PsPrintShop.psGet(strPrintShopId);

                            //                              //Add alerts for the representatives.
                            AlerttypeentityAlertTypeEntityDB alerttypeentity = context.AlertType.FirstOrDefault(
                                at => at.strType == AlerttypeentityAlertTypeEntityDB.strNewOrder);

                            JobjsonJobJson jobjsonJob;
                            JobJob.boolIsValidJobId(strJobId.ParseToInt(), psentity.strPrintshopId, iConfiguration_I,
                                out jobjsonJob, ref strUserMessage_IO, ref strDevMessage_IO);

                            //                              //Build the message.
                            String strMessage = JobJob.strMessageFromAlertEstimateOrOrder(jobjsonJob, intOtherAttributes,
                                ps, alerttypeentity, iConfiguration_I, ref context);

                            foreach (int intrep in darrintCustomerRepsAndSuperviser)
                            {
                                AlertentityAlertEntityDB alertentity = new AlertentityAlertEntityDB
                                {
                                    intPkPrintshop = psentity.intPk,
                                    intPkAlertType = alerttypeentity.intPk,
                                    intnJobId = strJobId.ParseToInt(),
                                    intnContactId = intrep,
                                    intnOtherAttributes = intOtherAttributes,
                                    strMessage = strMessage
                                };
                                context.Alert.Add(alertentity);
                                context.SaveChanges();
                            }

                            //                              //Notify the customer representatives and superviser. 
                            AlnotAlertNotification.subSendToAFew(strPrintShopId,
                                darrintCustomerRepsAndSuperviser.ToArray(), "New Order Requested.", iHubContext_I);

                            //                              //Update the price.
                            //                              //Verify if the price was calculate from odyssey2;
                            if (
                                jobjsonJob.boolnOdyssey2Pricing == true
                                )
                            {
                                //                      //Get the jobjson entity.
                                JobjsonentityJobJsonEntityDB jobjsonentity = context.JobJson.FirstOrDefault(jobjson => 
                                    jobjson.intJobID == strJobId.ParseToInt());

                                if (
                                    jobjsonentity.strPrice == null
                                    )
                                {
                                    //                      //Add the new price.
                                    jobjsonentity.strPrice = jobjsonJob.numnWisnetPrice == null ? null :
                                        jobjsonJob.numnWisnetPrice + "";
                                    context.JobJson.Update(jobjsonentity);
                                    context.SaveChanges();
                                }
                            }

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subNotifyNewEstimate(
            //                                              //Adds the notifications to the Alert Table and calls the 
            //                                              //      front end method AlertForAll to notified all the
            //                                              //      contacts from the printshop.

            XPathNavigator xpathnav_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO,
            IConfiguration iConfiguration_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            //                                              //Get the PrintshopId and JobId.
            String strPrintShopId = JobJob.strGetInfoFromXPath(xpathnav_I,
                "/cXML/Header/From/Credential/Identity");
            String strJobId = JobJob.strGetInfoFromXPath(xpathnav_I,
                "/cXML/Request/RFQ/ItemOut/ID");
            String strContactId = JobJob.strGetInfoFromXPath(xpathnav_I,
                "/cXML/Request/RFQ/RFQHeader/Contact/ID");
            int intOtherAttributes = JobJob.intOtherAttributes(xpathnav_I,
                "/cXML/Request/RFQ/ItemOut/ItemDetail/Extrinsic");

            PsPrintShop ps = PsPrintShop.psGet(strPrintShopId);

            intStatus_IO = 401;
            strUserMessage_IO = "Something went wrong.";
            strDevMessage_IO = "Incorrect job id.";
            if (
                strJobId.IsParsableToInt() &&      
                strContactId != null && 
                strContactId.IsParsableToInt()
                )
            {
                //                                          //Get job id.
                int intJobId = strJobId.ParseToInt();

                intStatus_IO = 402;
                if (
                    JobJob.boolIsValidCustomer(strPrintShopId, strContactId, ref intStatus_IO,
                        ref strUserMessage_IO, ref strDevMessage_IO)
                    ||
                    ResResource.boolEmployeeOrOwnerIsFromPrintshop(strPrintShopId, strContactId.ParseToInt())
                    )
                {
                    //                                      //Establish connection.
                    Odyssey2Context context = new Odyssey2Context();

                    //                                      //Get the Printshop entity.
                    PsentityPrintshopEntityDB psentity = context.Printshop.FirstOrDefault(ps =>
                        ps.strPrintshopId == strPrintShopId);

                    intStatus_IO = 403;
                    strUserMessage_IO = "Something went wrong.";
                    strDevMessage_IO = "Printshop not found.";
                    if (
                        psentity != null
                        )
                    {
                        //                                  //Get the Customer Representatives from Wiznet.
                        List<int> darrintCustomerReps = JobJob.darrintGetCustomerReps(strPrintShopId, strContactId,
                            iConfiguration_I);

                        //                                  //Get the Superviser from the printshop.
                        List<int> darrintCustomerSuperviser = (from roleentity in context.Role
                                                               where roleentity.intPkPrintshop == psentity.intPk &&
                                                               roleentity.boolSupervisor == true
                                                               select roleentity.intContactId).ToList();

                        //                                  //Union: join the two lists excluding repeats
                        List<int> darrintCustomerRepsAndSuperviser = darrintCustomerReps.Union(
                            darrintCustomerSuperviser != null ? darrintCustomerSuperviser :
                            new List<int>()).ToList();

                        intStatus_IO = 404;
                        strUserMessage_IO = "Something is wrong.";
                        strDevMessage_IO = "Customer Representatives not found from Wiznet.";
                        if (
                            darrintCustomerRepsAndSuperviser != null &&
                            darrintCustomerRepsAndSuperviser.Count > 0
                            )
                        {
                            //                              //Add alerts for the representatives.
                            AlerttypeentityAlertTypeEntityDB alerttypeentity = context.AlertType.FirstOrDefault(
                                at => at.strType == AlerttypeentityAlertTypeEntityDB.strNewEstimate);

                            JobjsonJobJson jobjsonJob;
                            JobJob.boolIsValidJobId(strJobId.ParseToInt(), psentity.strPrintshopId, iConfiguration_I,
                                out jobjsonJob, ref strUserMessage_IO, ref strDevMessage_IO);

                            //                              //Delete jobjson's entity.
                            //                              //This is necessary due to at this moment we dont have the
                            //                              //      estimate number. 
                            JobjsonentityJobJsonEntityDB jobjsonEntityToDelete = context.JobJson.FirstOrDefault(job =>
                                job.strPrintshopId == ps.strPrintshopId &&
                                job.intJobID == strJobId.ParseToInt() && job.intnEstimateNumber == null);
                            if (
                                jobjsonEntityToDelete != null
                                )
                            {
                                context.JobJson.Remove(jobjsonEntityToDelete);
                                context.SaveChanges();
                            }

                            //                              //Build the message.
                            String strMessage = JobJob.strMessageFromAlertEstimateOrOrder(jobjsonJob, intOtherAttributes,
                                 ps, alerttypeentity, iConfiguration_I, ref context);

                            foreach (int intrep in darrintCustomerRepsAndSuperviser)
                            {
                                AlertentityAlertEntityDB alertentity = new AlertentityAlertEntityDB
                                {
                                    intPkPrintshop = psentity.intPk,
                                    intPkAlertType = alerttypeentity.intPk,
                                    intnJobId = strJobId.ParseToInt(),
                                    intnContactId = intrep,
                                    intnOtherAttributes = intOtherAttributes, 
                                    strMessage = strMessage
                                };
                                context.Alert.Add(alertentity);
                                context.SaveChanges();
                            }

                            //                              //Notify the customer representatives. 
                            AlnotAlertNotification.subSendToAFew(strPrintShopId,
                                darrintCustomerRepsAndSuperviser.ToArray(), "New Estimate Requested.", iHubContext_I);

                            intStatus_IO = 200;
                            strUserMessage_IO = "";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static String strMessageFromAlertEstimateOrOrder(
            //                                              //Return the message for the alert.

            JobjsonJobJson jobjsonJob_I,
            int intOtherAttributes,
            PsPrintShop ps_I,
            AlerttypeentityAlertTypeEntityDB alerttypeentity_I,
            IConfiguration iConfiguration_I,
            ref Odyssey2Context context_M
            )
        {
            String strMessage = "";

            //                                              //Get complement description to new order or estimate.
            AlerttypeentityAlertTypeEntityDB alerttypeentityItemsToAnswer = context_M.AlertType.FirstOrDefault(
                at => at.strType == AlerttypeentityAlertTypeEntityDB.strItemsToAnswer);

            AlerttypeentityAlertTypeEntityDB alerttypeentityReadyToGo = context_M.AlertType.FirstOrDefault(
                at => at.strType == AlerttypeentityAlertTypeEntityDB.strReadyToGo);

            //                                      //Get customer name and job/estimate number.

            //                                      //To know if is an alert for an order or estimate.
            bool boolEstimate = alerttypeentity_I.strType ==
                AlerttypeentityAlertTypeEntityDB.strNewEstimate ? true : false;

            bool boolJob = boolEstimate ? false : true;
            int intJobId = jobjsonJob_I.intJobId;

            int intStatus_IO = 0;
            String strDevMessage_IO = "";
            String strUserMessage_IO = "";

            //                                      //To get the customer's name and Order/Estimate number.
            String[] strOrderOrEstimInfo = new String[3];
            strOrderOrEstimInfo[0] = jobjsonJob_I.strCustomerName;
            strOrderOrEstimInfo[1] = "";
            strOrderOrEstimInfo[2] = "";

            //                              //Get order/estimate number.
            strOrderOrEstimInfo[1] = EmplEmployee.strGetOrderOrEstimateNumber(intJobId,
                    (int)jobjsonJob_I.intnOrderId, boolEstimate, ps_I.strPrintshopId, iConfiguration_I, 
                    context_M, ref intStatus_IO, ref strDevMessage_IO, ref strUserMessage_IO);

            //                              //Get order-estima date.
            strOrderOrEstimInfo[2] = EmplEmployee.strGetOrderOrEstimateDate(jobjsonJob_I.dateLastUpdate);



            if (
                //                                  //If there is not OrderNumber or EstimateNumber, means that
                //                                  //      the job is not valid in Wisnet anymore. We do not
                //                                  //      need to send this alert.
                strOrderOrEstimInfo[1].Length > 0
                )
            {
                //                                  //There is a customer name, add description, othewise just
                //                                  //      an empty description.
                String strCustomerName = strOrderOrEstimInfo[0].Length > 0 ? "from " +
                    strOrderOrEstimInfo[0] : strOrderOrEstimInfo[0];

                //                                  //Assign Order/Estimate number.
                String strJobNumber = strOrderOrEstimInfo[1];

                //                                  //Get date.
                String strOrderOrEstimateDate = strOrderOrEstimInfo[2].Length > 0 ? "requested at " +
                strOrderOrEstimInfo[2] : "";

                //                                  //Assing the complement description.
                strMessage = intOtherAttributes > 0 ?
                    alerttypeentity_I.strDescription + strJobNumber + " " + strOrderOrEstimateDate + " " +
                    strCustomerName + " " + ". " + intOtherAttributes +
                        alerttypeentityItemsToAnswer.strDescription :
                    alerttypeentity_I.strDescription + strJobNumber + " " + strOrderOrEstimateDate + " " +
                    strCustomerName + " " + alerttypeentityReadyToGo.strDescription;

                bool? boolnAllResourceSetted = null;
                if (
                    //                                      //It is a estimate.
                    boolEstimate
                    ) 
                {
                    bool? boolnAreResourcesAvailable;
                    boolnAllResourceSetted = JobJob.boolAreAllResourceSettedAutomatically(ps_I, jobjsonJob_I,
                            context_M, out boolnAreResourcesAvailable);

                    //                                      //Assing the complement description.
                    strMessage = intOtherAttributes > 0 ?
                        alerttypeentity_I.strDescription + strJobNumber + " " + strOrderOrEstimateDate + " " +
                        strCustomerName + " " + ". " + intOtherAttributes +
                            alerttypeentityItemsToAnswer.strDescription :
                        alerttypeentity_I.strDescription + strJobNumber + " " + strOrderOrEstimateDate + " " +
                        strCustomerName;

                    strMessage = strMessage + JobJob.strBuildMessageFromBooleans(boolnAllResourceSetted, boolnAreResourcesAvailable);

                    //                                          //It is reday to go?
                    strMessage = strMessage + (intOtherAttributes <= 0 && boolnAllResourceSetted == true &&
                        boolnAreResourcesAvailable == true ?
                        (" " + alerttypeentityReadyToGo.strDescription) : ".");
                }
                else
                {
                    //                                      //It is a new order.
                    if (
                        //                                  //Price was calculate with odyssey2
                        jobjsonJob_I.boolnOdyssey2Pricing == true
                        )
                    {
                        bool? boolnAreResourcesAvailable;
                        boolnAllResourceSetted = JobJob.boolAreAllResourceSettedAutomatically(ps_I, jobjsonJob_I,
                            context_M, out boolnAreResourcesAvailable);

                        //                                  //Assing the complement description.
                        strMessage = intOtherAttributes > 0 ?
                            alerttypeentity_I.strDescription + strJobNumber + " " + strOrderOrEstimateDate + " " +
                            strCustomerName + " " + ". " + intOtherAttributes +
                                alerttypeentityItemsToAnswer.strDescription :
                            alerttypeentity_I.strDescription + strJobNumber + " " + strOrderOrEstimateDate + " " +
                            strCustomerName;

                        strMessage = strMessage + JobJob.strBuildMessageFromBooleans(boolnAllResourceSetted, boolnAreResourcesAvailable);

                        //                                          //It is reday to go?
                        strMessage = strMessage + (intOtherAttributes <= 0 && boolnAllResourceSetted == true &&
                            boolnAreResourcesAvailable == true ?
                            (" " + alerttypeentityReadyToGo.strDescription) : ".");
                    }
                    else
                    {
                        //                                  //Price was calculate with odyssey1.

                        //                                  //Assing the complement description.
                        strMessage = intOtherAttributes > 0 ?
                            alerttypeentity_I.strDescription + strJobNumber + " " + strOrderOrEstimateDate + " " +
                            strCustomerName + " " + ". " + intOtherAttributes +
                                alerttypeentityItemsToAnswer.strDescription :
                            alerttypeentity_I.strDescription + strJobNumber + " " + strOrderOrEstimateDate + " " +
                            strCustomerName + " " + alerttypeentityReadyToGo.strDescription;
                    }
                }
            }

            return strMessage;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static string strBuildMessageFromBooleans(
            //                                      //Return message built from booleans.

            bool? boolnAllResourceSetted,
            bool? boolnAreResourcesAvailable
            )
        {
            String strMessage = "";

            /*CASE*/
            if (
                boolnAllResourceSetted == null &&
                boolnAreResourcesAvailable == null
                )
            {
                //                                          //The reason can be beacause there arent a workflow
                //                                          //  default and the price was calculate with odyseey1.
                strMessage = strMessage + ". There is no workflow";
            }
            else if (
                boolnAllResourceSetted == true &&
                boolnAreResourcesAvailable == true
                )
            {
                //                                          //Do not something. It is Ready.   
            }
            else if (
                boolnAllResourceSetted == true &&
                boolnAreResourcesAvailable == false                 
                )
            {
                strMessage = strMessage + ". Some resources are not availables";
            }
            else if (
                boolnAllResourceSetted == false &&
                boolnAreResourcesAvailable == true
                )
            {
                strMessage = strMessage + ". Some resources were not set automatically";
            }
            else
            {
                strMessage = strMessage + ". Some resources were not set automatically and are not availables";
            }
            /*END-CASE*/

            return strMessage;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool? boolAreAllResourceSettedAutomatically(
            //                                              //Validate the all resources are setted.
            
            PsPrintShop ps_I,
            JobjsonJobJson jobjsonJob_I,
            Odyssey2Context context_I, 
            out bool? boolnAreResourcesAvailable_O
            )
        {
            bool? boolnAreAllResourceSetted = null;
            boolnAreResourcesAvailable_O = null; 

            //                                              //Get the product.
            ProdtypProductType prodtyp = ProdtypProductType.GetProductTypeUpdated(ps_I, 
                jobjsonJob_I.strProductName, (int)jobjsonJob_I.intnProductKey, true);

            WfentityWorkflowEntityDB wfentityDefault = prodtyp.wfentityGetDefaultWorkflowDependingDate(
                jobjsonJob_I.dateLastUpdate);

            if (
                //                                          //Find the workflow default.
                wfentityDefault != null
                )
            {
                boolnAreAllResourceSetted = true;
                boolnAreResourcesAvailable_O = true; 

                //                                          //Get all IOs with resources groups.
                List<IoentityInputsAndOutputsEntityDB> darrioentity = context_I.InputsAndOutputs.Where(io =>
                    io.intPkWorkflow == wfentityDefault.intPk && io.intnGroupResourceId != null &&
                    io.intnPkResource == null).ToList();

                int intI = 0;
                /*REPEAT-WHILE*/
                while (
                    //                                      //Take each IO.
                    intI < darrioentity.Count() &&
                    //                                      //Continue find any resource not setted automatically.
                    boolnAreAllResourceSetted == true &&
                    boolnAreResourcesAvailable_O == true
                    )
                {
                    if (
                        //                                  //Any resource can be setted automatically.
                        JobJob.boolIsThereOneResourceValidFromResourceGroup(jobjsonJob_I, darrioentity[intI], 
                        ref boolnAreResourcesAvailable_O)
                        )
                    {

                    }
                    else
                    {
                        boolnAreAllResourceSetted = false;
                    }

                    intI = intI + 1;
                }
            }
            
            return boolnAreAllResourceSetted;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static bool boolIsThereOneResourceValidFromResourceGroup(
            //                                              //Verify if there are a one resource valid for the IO.

            JobjsonJobJson jobjson_I,
            IoentityInputsAndOutputsEntityDB ioentityWithGrpResource_I,
            ref bool? boolnAreResourcesAvailable_IO
            )
        {
            bool boolIsThereOneResourceValidForAResourcesGrp = false;

            List<EleentityElementEntityDB> darreleentity = ProdtypProductType.darreleentityGetValidResources(
                    ioentityWithGrpResource_I, jobjson_I, ioentityWithGrpResource_I.intPkWorkflow);

            if (
                //                                          //There is only one resource from the group that can be set.
                darreleentity.Count == 1
                )
            {
                boolIsThereOneResourceValidForAResourcesGrp = true;

                //                                  //Verified that resource is be available.
                //                                  //That resource of the type calendar is considered available.
                boolnAreResourcesAvailable_IO = darreleentity[0].boolnIsAvailable == null ? true : 
                    (bool)darreleentity[0].boolnIsAvailable;
            }

            return boolIsThereOneResourceValidForAResourcesGrp;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static String strGetInfoFromXPath(
            //                                              //Get the info from an XPath navigator given a Path 
            //                                              //      otherwise it will return a empty string.

            XPathNavigator xpathnav_I,
            String strPath_I
            )
        {
            XPathExpression xpathexp = xpathnav_I.Compile(strPath_I);
            XPathNodeIterator xpniterator = xpathnav_I.Select(xpathexp);

            String strValue = null;
            if (
                xpniterator.MoveNext()
                )
            {
                strValue = xpniterator.Current.Value.Trim();
            }

            return strValue;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolIsValidCustomer(
            //                                              //This method validates if a contact id from a costumer is 
            //                                              //      part of the printshop specified.

            String strPrintShopId_I,
            String strContactId_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsValidCustom = false;

            intStatus_IO = 400;
            strUserMessage_IO = "Something went wrong.";
            strDevMessage_IO = "Invalid Contact Id.";
            if (
                strContactId_I != null && strContactId_I.IsParsableToInt()
                )
            {
                CusjsonCustomerJson cusjson;
                PsPrintShop ps = PsPrintShop.psGet(strPrintShopId_I);
                CusCustomer.subGetOneCustomerFromPrintshop(ps, strContactId_I.ParseToInt(), out cusjson,
                    ref intStatus_IO, ref strUserMessage_IO, ref strDevMessage_IO);
                if (
                    cusjson != null
                    )
                {
                    boolIsValidCustom = true;
                }

            }

            return boolIsValidCustom;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static bool boolIsValidContact(
            //                                              //This method validates if a contact id from a costumer is 
            //                                              //      part of the printshop specified.

            String strPrintShopId_I,
            String strContactId_I,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsValidCustom = false;

            intStatus_IO = 400;
            strUserMessage_IO = "Something went wrong.";
            strDevMessage_IO = "Invalid Contact Id.";
            if (
                strContactId_I != null && strContactId_I.IsParsableToInt()
                )
            {
                //                                              //Get data from Wisnet.
                String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                    GetSection("Odyssey2Settings")["urlWisnetApi"];

                PsPrintShop ps = PsPrintShop.psGet(strPrintShopId_I);

                Task<bool?> Task_boolValidContactInWisnet = HttpTools<TjsonTJson>.
                    GetBoolAsyncToEndPoint(strUrlWisnet + "/ValidContact/" + ps.strPrintshopId + "/" +
                    strContactId_I.ParseToInt());

                Task_boolValidContactInWisnet.Wait();

                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Wisnet database connection lost.";
                if (
                    Task_boolValidContactInWisnet.Result != null
                    )
                {
                    boolIsValidCustom = true;

                    intStatus_IO = 200;
                    strUserMessage_IO = "";
                    strDevMessage_IO = "Success.";
                }
            }
            return true; //boolIsValidCustom;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static List<int> darrintGetCustomerReps(
            //                                              //Gets representatives for the customer specified by the 
            //                                              //      contact id from a printshop.

            String strPrintshopId_I,
            String strContactId_I,
            IConfiguration iConfiguration_I
            )
        {
            List<int> darrintCustomerReps = null;

            //                                              //Get Rep from Wisnet. Now we only get one response
            //                                              //      but this part of the code will change when we get a 
            //                                              //      list of reps from Wiznet.
            Task<CusjsonCustomerJson> Task_cusjsonFromWisnet = HttpTools<CusjsonCustomerJson>.GetOneAsyncToEndPoint(
                 iConfiguration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") +
                  "/Customer/CustomerRep/" + strPrintshopId_I + "/" + strContactId_I);
            Task_cusjsonFromWisnet.Wait();
            if (
                Task_cusjsonFromWisnet.Result != null
                )
            {
                //                                          //Get the result from Wiznet. Now we only get one response
                //                                          //      but this part of the code is ready when we get a 
                //                                          //      list of reps from Wiznet.
                List<CusjsonCustomerJson> darrcusjsonFromWisnet = new List<CusjsonCustomerJson>();
                darrcusjsonFromWisnet.Add(Task_cusjsonFromWisnet.Result);

                //                                          //Get the representatives contact ids.
                darrintCustomerReps = new List<int>();
                foreach (CusjsonCustomerJson cusjson in darrcusjsonFromWisnet)
                {
                    darrintCustomerReps.Add(cusjson.intContactId);
                }
            }

            return darrintCustomerReps;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static int intOtherAttributes(
            //                                              //Validates the attributes in an order or estimate to look        
            //                                              //      for Other fields. In case, at least, an Other field 
            //                                              //      is found the method will return true.

            XPathNavigator xpathnav_I,
            String strPath_I
            )
        {
            XPathExpression xpathexp = xpathnav_I.Compile(strPath_I);
            XPathNodeIterator xpniterator = xpathnav_I.Select(xpathexp);

            Regex regex = new Regex(@"^.*Other\s--.*$");

            int intOtherAttributes = 0;
            while (
                xpniterator.MoveNext()
                )
            {
                if (
                    regex.IsMatch(xpniterator.Current.Value.Trim())
                    )
                {
                    intOtherAttributes++;
                }
            }

            return intOtherAttributes;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subCopyConfirmedEstimate(
            //                                              //Copy a estimate confirmed from the job.

            int intJobId_I,
            int intPkWorkflow_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "The job is not valid.";

            //                                              //Verify job.
            JobjsonJobJson jobjsonJob;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjsonJob,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "The job has not a estimate confirmed.";

                //                                          //Get the estimate confirmed for this Job.
                EstentityEstimateEntityDB estentity = context_M.Estimate.FirstOrDefault(est =>
                    est.intId == JobJob.intIdConfirmedEstimate && est.intJobId == intJobId_I &&
                    est.intPkWorkflow == intPkWorkflow_I && est.intnCopyNumber == null);

                if (
                    estentity != null
                    )
                {
                    //                                      //Get the copy max.
                    int? intnCopyNumberMax =
                        (from estentityMax in context_M.Estimate
                         where estentityMax.intJobId == intJobId_I &&
                         estentityMax.intPkWorkflow == intPkWorkflow_I
                         select estentityMax).Max(estMax => estMax.intnCopyNumber);

                    int intCopyNumberNew = (intnCopyNumberMax == null ? 0 : (int)intnCopyNumberMax) + 1;

                    intStatus_IO = 403;
                    strUserMessage_IO = "Only three estimates are allowed.";
                    strDevMessage_IO = "Only three estimates are allowed.";
                    if (
                        intCopyNumberNew < 3
                        )
                    {
                        //                                  //Copy the estimate confirmed.
                        EstentityEstimateEntityDB estentityToAdd = new EstentityEstimateEntityDB
                        {
                            intJobId = estentity.intJobId,
                            intId = estentity.intId,
                            strBaseDate = estentity.strBaseDate,
                            strBaseTime = estentity.strBaseTime,
                            intPkWorkflow = estentity.intPkWorkflow,
                            strName = estentity.strName,
                            intnCopyNumber = intCopyNumberNew,
                            intnQuantity = jobjsonJob.intnQuantity
                        };

                        context_M.Estimate.Add(estentityToAdd);
                        context_M.SaveChanges();

                        intStatus_IO = 200;
                        strUserMessage_IO = "Success.";
                        strDevMessage_IO = "";
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetQuantityForEstimate(
            //                                              //Copy a estimate confirmed from the job.

            int intJobId_I,
            int intPkWorkflow_I,
            int intQuatity_I,
            int intCopyNumber_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "The job is not valid.";

            //                                              //Verify job.
            JobjsonJobJson jobjsonJob;
            if (
                JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjsonJob,
                ref strUserMessage_IO, ref strDevMessage_IO)
                )
            {

                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "The job has not this estimate copy.";

                //                                          //Get the estimate copy confirmed for this Job.
                EstentityEstimateEntityDB estentity = context_M.Estimate.FirstOrDefault(est =>
                    est.intId == JobJob.intIdConfirmedEstimate && est.intJobId == intJobId_I &&
                    est.intPkWorkflow == intPkWorkflow_I && est.intnCopyNumber == intCopyNumber_I);

                if (
                    estentity != null
                    )
                {
                    //                                      //Set the quantity.
                    estentity.intnQuantity = intQuatity_I;
                    estentity.numnLastPrice = null;
                    context_M.Estimate.Update(estentity);
                    context_M.SaveChanges();

                    intStatus_IO = 200;
                    strUserMessage_IO = "Success.";
                    strDevMessage_IO = "";
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subCreateNewEstimate(
            //                                              //Create an estimate from scratch.
            //                                              //Get a jobId from wisnet and create a workflow for the
            //                                              //      jobId (this jobId is an estimate's id).

            int intQuantity_I,
            PsPrintShop ps_I,
            String strName_I,
            int intContactId_I,
            Odyssey2Context context_M,
            out int intJobId_O,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            intJobId_O = 0;
            intStatus_IO = 401;
            strUserMessage_IO = "Name can not be empty.";
            strDevMessage_IO = "Name can not be empty.";
            if (
                strName_I.Length > 0
                )
            {
                if (
                    JobJob.boolIsValidCustomer(ps_I.strPrintshopId, intContactId_I.ToString(), ref intStatus_IO,
                        ref strUserMessage_IO, ref strDevMessage_IO)
                    )
                {
                    //                                      //Add to wisnet DB the new estimate required from
                    //                                      //      odyssey2.0
                    String strUrlWisnet = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
                                GetSection("Odyssey2Settings")["urlWisnetApi"];
                    Task<int> Task_PostEstimateFromWisnet =
                    HttpTools<TjsonTJson>.PostAddEstimateAsyncToEndPoint(intContactId_I, intQuantity_I, strUrlWisnet +
                        "/Estimates/PostAddEstimate", ps_I.strPrintshopId, strName_I);
                    Task_PostEstimateFromWisnet.Wait();

                    intStatus_IO = 402;
                    strUserMessage_IO = "Something is wrong.";
                    strDevMessage_IO = "Wisnet connection lost.";
                    if (
                        //                                          //If true, estimate was created succesfully.
                        Task_PostEstimateFromWisnet.Result > 0
                        )
                    {
                        //                                          //We use the estimate id created at wisnet to create a new
                        //                                          //      workflow.
                        int intJobId = Task_PostEstimateFromWisnet.Result;

                        JobJob.subCreateProductDummy(ps_I.strPrintshopId);

                        //                                      //Get pkProduct of dummy product.
                        int intProductKeyDummy = ("9999" + ps_I.strPrintshopId).ParseToInt();
                        EtentityElementTypeEntityDB etentity = context_M.ElementType.FirstOrDefault(et =>
                            et.intWebsiteProductKey == intProductKeyDummy);

                        //                                          //Create workflow.
                        WfentityWorkflowEntityDB wfentityEstimate = new WfentityWorkflowEntityDB
                        {
                            intnPkProduct = etentity.intPk,
                            strName = "Workflow for Estimate " + strName_I,
                            intWorkflowId = 1,
                            strStartDate = Date.Now(ZonedTimeTools.timezone).ToString(),
                            strStartTime = Time.Now(ZonedTimeTools.timezone).ToString(),
                            intPkPrintshop = ps_I.intPk,
                            boolDeleted = false,
                            boolDefault = false,
                            intnJobId = intJobId
                        };
                        context_M.Workflow.Add(wfentityEstimate);
                        context_M.SaveChanges();

                        intJobId_O = intJobId;
                        intStatus_IO = 200;
                        strUserMessage_IO = "Success.";
                        strDevMessage_IO = "";
                    }
                }                    
            }            
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSetResourceEstimate(
            //                                              //Set a resource for an IO in a process for an estimate's
            //                                              //      workflow.

            int intJobId_I,
            int intPkProcessInWorkflow_I,
            int intPkResource_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            PsPrintShop ps_I,
            IConfiguration configuration_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Find PIW.
            PiwentityProcessInWorkflowEntityDB piwentity = context_M.ProcessInWorkflow.FirstOrDefault(piw =>
                piw.intPk == intPkProcessInWorkflow_I);

            intStatus_IO = 401;
            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "No PIW found";
            if (
                //                                      //The process we want to add a resource, exists.
                piwentity != null
                )
            {
                intStatus_IO = 402;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Invalid Job";
                JobjsonJobJson jobjson;
                if (
                    //                                      //Valid Job and Get data from Order form.
                    JobJob.boolIsValidJobId(intJobId_I, ps_I.strPrintshopId, configuration_I, out jobjson,
                        ref strUserMessage_IO, ref strDevMessage_IO)
                      //true
                    )
                {
                    //                                      //Find PkWorkflow.
                    int intPkWorkflow = piwentity.intPkWorkflow;

                    //                                  //Validate if the given resource and io, exists and are
                    //                                  //      same type.

                    if (
                        ProdtypProductType.boolDataValid(intPkWorkflow, piwentity.intProcessInWorkflowId,
                            intPkResource_I, intPkEleetOrEleele_I, boolIsEleet_I, ref intStatus_IO,
                            ref strUserMessage_IO, ref strDevMessage_IO)
                        )
                    {
                        //                                              //To easy code.
                        int? intnElementElementType = boolIsEleet_I ? (int?)intPkEleetOrEleele_I : null;
                        int? intnElementElement = boolIsEleet_I ? null : (int?)intPkEleetOrEleele_I;

                        //                              //Find register in IO table.
                        //                                              //Get the IO.
                        IoentityInputsAndOutputsEntityDB ioentity =
                            context_M.InputsAndOutputs.FirstOrDefault(ioentity =>
                            ioentity.intnPkElementElementType == intnElementElementType &&
                            ioentity.intnPkElementElement == intnElementElement &&
                            ioentity.intPkWorkflow == intPkWorkflow &&
                            ioentity.intnProcessInWorkflowId == piwentity.intProcessInWorkflowId);

                        if (
                            //                                          //There is not an entry.
                            ioentity == null
                            )
                        {
                            //                          //The IO has not a resource and has not link.
                            //                          //Set resource.
                            IoentityInputsAndOutputsEntityDB ioentityToSet = new IoentityInputsAndOutputsEntityDB
                            {
                                intPkWorkflow = intPkWorkflow,
                                intnProcessInWorkflowId = piwentity.intProcessInWorkflowId,
                                intnPkElementElementType = intnElementElementType,
                                intnPkElementElement = intnElementElement,
                                intnPkResource = intPkResource_I,
                                intnGroupResourceId = null,
                                boolnIsFinalProduct = null
                            };
                            context_M.InputsAndOutputs.Add(ioentityToSet);
                            context_M.SaveChanges();

                        }
                        else
                        {
                            //                              //If we are here, is because the io exists for two reasons.
                            //                              //1.-IO has already a resource and has link.
                            //                              //2.-IO only has link.

                            //                              //Update/Set resource for an  IO.
                            ioentity.intnPkResource = intPkResource_I;
                            context_M.InputsAndOutputs.Update(ioentity);
                            context_M.SaveChanges();

                            //                              //If the IO has a link, we have to update the resource of
                            //                              //      the other side of the link.
                            if (
                                ioentity.strLink != null
                                )
                            {
                                //                      //Update other side link.
                                JobJob.subUpdateOtherSideLinkForEstimateWorkflow(ioentity, context_M);
                            }

                            intStatus_IO = 200;
                            strUserMessage_IO = "Success.";
                            strDevMessage_IO = "";
                        }
                    }
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private static void subUpdateOtherSideLinkForEstimateWorkflow(
            //                                              //Update the other side of the link for an IO in a
            //                                              //      estimate's workflow.

            IoentityInputsAndOutputsEntityDB ioentity_I,
            Odyssey2Context context_M
            )
        {
            //                                              //Get other side of the link.
            IoentityInputsAndOutputsEntityDB ioentityOtherSideLink = context_M.InputsAndOutputs.FirstOrDefault(io =>
                io.intPkWorkflow == ioentity_I.intPkWorkflow &&
                io.intPk != ioentity_I.intPk && io.strLink == ioentity_I.strLink);

            //                                              //Update resource at other side link.
            ioentityOtherSideLink.intnPkResource = ioentity_I.intnPkResource;
            context_M.InputsAndOutputs.Update(ioentityOtherSideLink);
            context_M.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeletePeriodsNotStartedOnTime(
            //                                              //Delete periods that reached its tolerance time and were 
            //                                              //      not started.
            //                                              //Send notifications to supervisors when a period is deleted
            //                                              //      and this may cause the job's estimated date to be 
            //                                              //      unavailable.

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

            //                                              //To easy code.
            String strCurrentHour = ztimeDateNow.Time.ToString();
            strCurrentHour = strCurrentHour.Substring(0, 2);
            String strCurrentMinute = ztimeDateNow.Time.ToString();
            strCurrentMinute = strCurrentMinute.Substring(3, 2);
            //                                              //Get all periods that are going to be deleted because its
            //                                              //      tolerance time was not reached.
            List<PerentityPeriodEntityDB> darrperiodToNotify = context.Period.Where(per =>
                per.strDeleteDate == ztimeDateNow.Date.ToString() &&
                per.strDeleteHour == strCurrentHour &&
                per.strDeleteMinute == strCurrentMinute &&
                //                                          //Exclude temporary periods.
                per.intnEstimateId == null &&
                //                                          //Period not started.
                per.strFinalStartDate == null &&
                //                                          //Period not finished.
                per.strFinalEndDate == null).ToList();

            foreach (PerentityPeriodEntityDB perentity in darrperiodToNotify)
            {
                //                                          //Find job.
                JobJob job = jobFromDB(perentity.intJobId);
                if (
                    //                                      //Only jobs in progress get these notifications.
                    job != null &&
                    job.intStage == JobJob.intInProgressStage
                    )
                {
                    JobJob.subSendDueDateAtRiskNotification(perentity, iHubContext_I);

                    //                                      //Delete alert related to period about to start.
                    JobJob.subDeletePeriodAboutToStartAlert(perentity.intPk, iHubContext_I);

                    context.Period.Remove(perentity);
                    context.SaveChanges();
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subSendDueDateAtRiskNotification(
            //                                              //Period that caused the notification.
            PerentityPeriodEntityDB perentity_I,
            //                                              //Connection object that will send the notifications.
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find PkPrintshop.

            //                                              //Find process or resource.
            EleentityElementEntityDB eleentity = context.Element.FirstOrDefault(ele =>
                ele.intPk == perentity_I.intPkElement);
            //                                              //Find ElementType.
            EtElementTypeAbstract et = EletemElementType.etFromDB(eleentity.intPkElementType);
            int intPkPrintshop = (int)et.intPkPrintshop;

            //                                              //Find printshop's supervisors.
            List<RolentityRoleEntityDB> darrrolentity = context.Role.Where(role =>
                role.intPkPrintshop == intPkPrintshop && role.boolSupervisor).ToList();
            if (
                darrrolentity.Count > 0
                )
            {
                //                                          //Notification.

                //                                          //Find PkAltertType for due date at risk.
                AlerttypeentityAlertTypeEntityDB alerttypeentity = context.AlertType.FirstOrDefault(
                    type => type.strType == AlerttypeentityAlertTypeEntityDB.strDueDateAtRisk);
                //                                          //Create notification.
                AlertentityAlertEntityDB alertentityNew = new AlertentityAlertEntityDB
                {
                    intPkPrintshop = intPkPrintshop,
                    intPkAlertType = alerttypeentity.intPk,
                    intnJobId = perentity_I.intJobId
                };
                context.Alert.Add(alertentityNew);
                context.SaveChanges();

                //                                          //Send notification.
                //                                          //Get strJobNumber
                //                                          //We dont have the orderId, get the info from jobjson 
                //                                          //      table.
                PsentityPrintshopEntityDB psentity = context.Printshop.FirstOrDefault(ps =>
                    ps.intPk == et.intPkPrintshop);
                String strJobNumber = JobJob.strGetJobNumber(null, perentity_I.intJobId, psentity.strPrintshopId,
                    context);

                foreach (RolentityRoleEntityDB rolentity in darrrolentity)
                {
                    //                                      //Contacts who will have notifications
                    List<int> darrintContactId = new List<int>();
                    darrintContactId.Add(rolentity.intContactId);

                    //                                      //Support method to send notifications to front.
                    String strMessage = "A period was deleted. Due date for this job is at risk: " + strJobNumber;
                    AlnotAlertNotification.subSendTaskToAFew(darrintContactId.ToArray(), strMessage, iHubContext_I);
                }
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static void subDeletePeriodAboutToStartAlert(
            //                                              //Find and delete alert created when the period was about
            //                                              //      to start.

            int intPkPeriod_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            //                                              //Establish connection.
            Odyssey2Context context = new Odyssey2Context();

            //                                              //Find alert.

            //                                              //Find alert type related to periods.
            AlerttypeentityAlertTypeEntityDB alerttypeentity = context.AlertType.FirstOrDefault(alerttype =>
                alerttype.strType == AlerttypeentityAlertTypeEntityDB.strPeriod);
            //                                              //Find alert.
            AlertentityAlertEntityDB alertentity = context.Alert.FirstOrDefault(alert =>
                alert.intnPkPeriod == intPkPeriod_I && alert.intPkAlertType == alerttypeentity.intPk);
            if (
                alertentity != null
                )
            {
                if (
                    //                                      //Notification not read.
                    PsPrintShop.boolNotificationReadByUser(alertentity, (int)alertentity.intnContactId)
                    )
                {
                    AlnotAlertNotification.subReduceToOne((int)alertentity.intnContactId, iHubContext_I);
                }

                context.Alert.Remove(alertentity);
                context.SaveChanges();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSendDueDateInThePastNotification(
            //                                              //Send notification to all of the printshop's
            //                                              //      supervisors when a pending job's due date 
            //                                              //      is before current date.

            //                                              //Connection object that will send the notifications.
            IHubContext<ConnectionHub> iHubContext_I,
            IConfiguration configuration_I
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

            //                                              //To easy code.
            String strCurrentHour = ztimeDateNow.Time.ToString();
            strCurrentHour = strCurrentHour.Substring(0, 2);
            String strCurrentMinute = ztimeDateNow.Time.ToString();
            strCurrentMinute = strCurrentMinute.Substring(3, 2);
            //                                              //Current due dates that expire in current date and time.
            List<DuedateentityDueDateEntityDB> darrduedateentity = context.DueDate.Where(due =>
                due.boolCurrent && due.strDate == ztimeDateNow.Date.ToString() &&
                due.strHour == strCurrentHour && due.strMinute == strCurrentMinute).ToList();

            foreach (DuedateentityDueDateEntityDB duedateentity in darrduedateentity)
            {
                //                                      //Find job.
                JobJob job = JobJob.jobFromDB(duedateentity.intJobId);
                if (
                    //                                      //Pending job.
                    job == null
                    )
                {
                    int? intnProductKey = JobJob.intnJobProductKey(duedateentity.intJobId, configuration_I);
                    if (
                         intnProductKey != null
                        )
                    {
                        //                                  //Find PkPrintshop.

                        //                                  //Find Element type.
                        EtentityElementTypeEntityDB etentity = context.ElementType.FirstOrDefault(et =>
                            et.intWebsiteProductKey == intnProductKey);
                        int intPkPrintshop = (int)etentity.intPrintshopPk;

                        //                                  //Find printshop's supervisors.
                        List<RolentityRoleEntityDB> darrrolentity = context.Role.Where(role =>
                            role.intPkPrintshop == intPkPrintshop && role.boolSupervisor).ToList();
                        if (
                            darrrolentity.Count > 0
                            )
                        {
                            //                              //Notification.

                            //                              //Find PkAltertType for due date int the past.
                            AlerttypeentityAlertTypeEntityDB alerttypeentity = context.AlertType.FirstOrDefault(type =>
                                type.strType == AlerttypeentityAlertTypeEntityDB.strDueDateInThePast);
                            //                              //Create notification.
                            AlertentityAlertEntityDB alertentityNew = new AlertentityAlertEntityDB
                            {
                                intPkPrintshop = intPkPrintshop,
                                intPkAlertType = alerttypeentity.intPk,
                                intnJobId = duedateentity.intJobId
                            };
                            context.Alert.Add(alertentityNew);
                            context.SaveChanges();

                            //                              //Get ps entity.
                            PsentityPrintshopEntityDB psentity = context.Printshop.FirstOrDefault(ps =>
                                ps.intPk == intPkPrintshop);

                            //                              //Send notification.
                            foreach (RolentityRoleEntityDB roleentity in darrrolentity)
                            {
                                //                          //Contacts who will have notifications
                                List<int> darrintContactId = new List<int>();
                                darrintContactId.Add(roleentity.intContactId);

                                //                      //Get strJobNumber.
                                String strJobNumber = JobJob.strGetJobNumber(null, duedateentity.intJobId,
                                    psentity.strPrintshopId, context);

                                //                          //Support method to send notifications 
                                //                          //      to front.
                                String strMessage = "Due date for this job is at risk: " + strJobNumber;
                                AlnotAlertNotification.subSendTaskToAFew(darrintContactId.ToArray(), strMessage,
                                    iHubContext_I);
                            }
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolIsThereATemporalJobForAPrintshop(

            int? intnJobId_I,
            String strPrintshopId_I,
            int intProductKey_I,
            int intQuantity_I,
            List<AttrjsonAttributeJson> darrattrjson_I,
            out JobjsonentityJobJsonEntityDB jobjsonentity_O,
            out JobjsonJobJson jobjsonTemporal_O,
            IConfiguration configuration_I,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsThereATemporalJobForAPrintshop = false;
            jobjsonTemporal_O = null;
            jobjsonentity_O = null;

            //                                              //Go to wisnet.
            Task<List<JobjsonJobJson>> Task_darrjobjsonTemporal = HttpTools<JobjsonJobJson>.GetListAsyncToEndPoint(
                    configuration_I.GetValue<String>("Odyssey2Settings:urlWisnetApi") + "/PrintshopData/GetTemporalJob?" 
                    + $"intPrintshopId_I={strPrintshopId_I}&intProductKey_I={intProductKey_I}&intnQuantity_I=" +
                    $"{intQuantity_I}");
            Task_darrjobjsonTemporal.Wait();

            strUserMessage_IO = "Something is wrong.";
            strDevMessage_IO = "There is not a connection to Wisnet.";
            if (
                Task_darrjobjsonTemporal.Result != null
                )
            {
                Odyssey2Context context = new Odyssey2Context();

                List<JobjsonJobJson> darrjobjson = Task_darrjobjsonTemporal.Result;

                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "There is more than a result from wisnet.";
                if (
                    darrjobjson.Count == 1
                    ) {
                    jobjsonTemporal_O = darrjobjson[0];

                    jobjsonTemporal_O.darrattrjson = darrattrjson_I;
                    jobjsonTemporal_O.intnReorderedFromJobID = intnJobId_I;

                    //                              //Add job to DB.
                    JobjsonentityJobJsonEntityDB jobjsonentity = new JobjsonentityJobJsonEntityDB
                    {
                        intJobID = jobjsonTemporal_O.intJobId,
                        strPrintshopId = strPrintshopId_I,
                        jobjson = JsonSerializer.Serialize(jobjsonTemporal_O),
                        intOrderId = 0
                    };
                    context.JobJson.Add(jobjsonentity);

                    context.SaveChanges();

                    jobjsonentity_O = jobjsonentity;

                    boolIsThereATemporalJobForAPrintshop = true;
                }
            }

            return boolIsThereATemporalJobForAPrintshop;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subBuiltMessageForEstimateAndOrderAlerts(
            //                                              //Build the message and update the alerts.

            IConfiguration iConfiguration_I,
            Odyssey2Context context_M,
            ref int intStatus_IO,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            //                                              //Get the Alert types for the Order.
            AlerttypeentityAlertTypeEntityDB alerttypeentityNewOrder = context_M.AlertType.FirstOrDefault(
                alerttype => alerttype.strType == AlerttypeentityAlertTypeEntityDB.strNewOrder);
            //                                              //Get the Alert types for the Estimate.
            AlerttypeentityAlertTypeEntityDB alerttypeentityNewEstimate =
                context_M.AlertType.FirstOrDefault(alerttype =>
                alerttype.strType == AlerttypeentityAlertTypeEntityDB.strNewEstimate);

            //                                              //Get the alert for update.
            List<AlertentityAlertEntityDB> darralertOrderEstimate = context_M.Alert.Where(alert => 
                (alert.intPkAlertType == alerttypeentityNewOrder.intPk || 
                alert.intPkAlertType == alerttypeentityNewEstimate.intPk) &&
                alert.intnJobId != null && alert.strMessage == null).ToList();

            bool allJobJsonExist = true;

            //                                              //Secuencial.
            foreach (AlertentityAlertEntityDB alert in darralertOrderEstimate)
            {
                JobjsonentityJobJsonEntityDB jobjson = context_M.JobJson.FirstOrDefault(jj => 
                    jj.intJobID == (int)alert.intnJobId);

                PsPrintShop ps = PsPrintShop.psGet(jobjson.strPrintshopId);

                JobjsonJobJson jobjsonJob;
                JobJob.boolIsValidJobId((int)alert.intnJobId, jobjson.strPrintshopId, iConfiguration_I,
                    out jobjsonJob, ref strUserMessage_IO, ref strDevMessage_IO);

                if (
                    jobjsonJob != null
                    )
                {
                    int intOtherAtribute = alert.intnOtherAttributes == null ? 0 : (int)alert.intnOtherAttributes;
                    String strMessage;
                    if (
                        //                                  //Order.
                        alerttypeentityNewOrder.intPk == alert.intPkAlertType
                        )
                    {
                        //                                  //Build the message.
                        strMessage = JobJob.strMessageFromAlertEstimateOrOrder(jobjsonJob,
                            intOtherAtribute, ps, alerttypeentityNewOrder, iConfiguration_I, 
                            ref context_M);
                    }
                    else
                    {
                        //                                  //Estimate..
                        //                                  //Build the message.
                        strMessage = JobJob.strMessageFromAlertEstimateOrOrder(jobjsonJob,
                            intOtherAtribute, ps, alerttypeentityNewEstimate, iConfiguration_I, 
                            ref context_M);

                    }

                    alert.strMessage = strMessage;
                    context_M.Alert.Update(alert);
                    context_M.SaveChanges();
                }
                else
                {
                    allJobJsonExist = false;
                    break;
                }
            }

            if (
                allJobJsonExist
                )
            {
                intStatus_IO = 200;
                strUserMessage_IO = "Success.";
                strDevMessage_IO = "";
            }
            else
            {
                intStatus_IO = 401;
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "There jobjson that no exist.";
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subDeleteTemporalJob(
            JobjsonentityJobJsonEntityDB jobjsonentity_I
            )
        {
            Odyssey2Context context = new Odyssey2Context();

            context.JobJson.Remove(jobjsonentity_I);

            context.SaveChanges();
        }

        //--------------------------------------------------------------------------------------------------------------

    }

    //==================================================================================================================
}
/*END-TASK*/
