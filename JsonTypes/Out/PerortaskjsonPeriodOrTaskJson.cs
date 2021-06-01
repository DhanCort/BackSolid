/*TASK RP.JSON*/
using System;
using TowaStandard;
using Odyssey2Backend.Utilities;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 22, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class PerortaskjsonPeriodOrTaskJson : IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
       
        public int? intnPkPeriod { get; set; }
        public int? intnPkTask { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strEndDate { get; set; }
        public String strEndTime { get; set; }
        public String strJobId { get; set; }
        public String strJobNumber { get; set; }
        public bool boolIsByResource { get; set; }
        public bool boolIsAbleToStart { get; set; }
        public bool boolIsAbleToEnd { get; set; }
        public int intMinsBeforeDelete { get; set; }
        public String strProcess { get; set; }
        public String strDescription { get; set; }
        public int intMinutesForNotification { get; set; }
        public bool boolIsNotifiedable { get; set; }
        public bool boolIsCompleted { get; set; }
        public String strJobName { get; set; }
        public String strCustomerName { get; set; }
        public String strCustomerLastName { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public PerortaskjsonPeriodOrTaskJson(
            int? intnPkPeriod_I,
            int? intnPkTask_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            String strJobId_I,
            String strJobNumber_I,
            bool boolIsByResource_I,
            bool boolIsAbleToStart_I,
            bool boolIsAbleToEnd_I,
            int intMinsBeforeDelete_I,
            String strProcess_I, 
            String strDescription_I,
            int intMinutesForNotification_I,
            bool boolIsNotifiedable_I,
            bool boolIsCompleted_I,
            String strJobName_I,
            String strCustomerName,
            String strCustomerLastName
            )
        {
            this.intnPkPeriod = intnPkPeriod_I;
            this.intnPkTask = intnPkTask_I;
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strEndDate = strEndDate_I;
            this.strEndTime = strEndTime_I;
            this.strJobId = strJobId_I;
            this.strJobNumber = strJobNumber_I;
            this.boolIsByResource = boolIsByResource_I;
            this.boolIsAbleToStart = boolIsAbleToStart_I;
            this.boolIsAbleToEnd = boolIsAbleToEnd_I;
            this.intMinsBeforeDelete = intMinsBeforeDelete_I;
            this.strProcess = strProcess_I;
            this.strDescription = strDescription_I;
            this.intMinutesForNotification = intMinutesForNotification_I;
            this.boolIsNotifiedable = boolIsNotifiedable_I;
            this.boolIsCompleted = boolIsCompleted_I;
            this.strJobName = strJobName_I;
            this.strCustomerName = strCustomerName;
            this.strCustomerLastName = strCustomerLastName;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            //                                              //This task
            Date thisDate = this.strStartDate.ParseToDate();
            Time thisTime = this.strStartTime.ParseToTime();
            ZonedTime ztimeStartThisTask = ZonedTimeTools.NewZonedTime(thisDate, thisTime);

            //                                              //Task to compare
            PerortaskjsonPeriodOrTaskJson porjson = (PerortaskjsonPeriodOrTaskJson)obj_I;
            Date newDate = porjson.strStartDate.ParseToDate();
            Time newTime = porjson.strStartTime.ParseToTime(); 
            ZonedTime ztimeStartNewTask = ZonedTimeTools.NewZonedTime(newDate, newTime);

            //                                              //SORT BY DATE
            //                                              //Compare the tasks date
            return ztimeStartThisTask.CompareTo(ztimeStartNewTask);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
