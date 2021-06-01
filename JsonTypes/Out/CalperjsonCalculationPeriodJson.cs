/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 08, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class CalperjsonCalculationPeriodJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkCalculation { get; set; }
        public String strDescription { get; set; }
        public int intHours { get; set; }
        public int intMinutes { get; set; }
        public int intSeconds { get; set; }
        public int? intnPkPeriod { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strEndDate { get; set; }
        public String strEndTime { get; set; }
        public String strFirstName { get; set; }
        public String strLastName { get; set; }
        public int? intnContactId { get; set; }
        public bool boolProcessCompleted { get; set; }
        public int intMinsBeforeDelete { get; set; }
        public bool boolPeriodStarted { get; set; }
        public bool boolPeriodCompleted { get; set; }
        public String strEstimatedDate { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public CalperjsonCalculationPeriodJson(
            int intPkCalculation_I,
            String strDescription_I,
            int intHours_I,
            int intMinutes_I,
            int intSeconds_I,
            int? intnPkPeriod_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            String strFirstName_I,
            String strLastName_I,
            int? intnContactId_I,
            bool boolProcessCompleted_I,
            int intMinsBeforeDelete_I,
            bool boolPeriodStarted_I,
            bool boolPeriodCompleted_I,
            String strEstimatedDate_I
            )
        {
            this.intPkCalculation = intPkCalculation_I;
            this.strDescription = strDescription_I;
            this.intHours = intHours_I;
            this.intMinutes = intMinutes_I;
            this.intSeconds = intSeconds_I;
            this.intnPkPeriod = intnPkPeriod_I;
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strEndDate = strEndDate_I;
            this.strEndTime = strEndTime_I;
            this.strFirstName = strFirstName_I;
            this.strLastName = strLastName_I;
            this.intnContactId = intnContactId_I;
            this.boolProcessCompleted = boolProcessCompleted_I;
            this.intMinsBeforeDelete = intMinsBeforeDelete_I;
            this.boolPeriodStarted = boolPeriodStarted_I;
            this.boolPeriodCompleted = boolPeriodCompleted_I;
            this.strEstimatedDate = strEstimatedDate_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
