/*TASK RP.XJDF*/
using Odyssey2Backend.Utilities;
using System;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 10, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PerisaddablejsonPeriodIsAddableJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public bool boolIsAddableAboutPeriods { get; set; }
        public bool boolIsAddableAboutRules { get; set; }
        public bool boolIsAddableAboutEmployeesPeriods { get; set; }
        public int? intPkCalculation { get; set; }
        public String strDescription { get; set; }
        public int? intHours { get; set; }
        public int? intMinutes { get; set; }
        public int? intSeconds { get; set; }
        public int? intnPkPeriod { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strEndDate { get; set; }
        public String strEndTime { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PerisaddablejsonPeriodIsAddableJson(
            bool boolIsAddableAboutPeriods_I,
            bool boolIsAddableAboutRules_I,
            bool boolIsAddableAboutEmployeesPeriods_I,
            int? intPkCalculation_I,
            String strDescription_I,
            int? intHours_I,
            int? intMinutes_I,
            int? intSeconds_I,
            int? intnPkPeriod_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I
            )
        {
            this.boolIsAddableAboutPeriods = boolIsAddableAboutPeriods_I;
            this.boolIsAddableAboutRules = boolIsAddableAboutRules_I;
            this.boolIsAddableAboutEmployeesPeriods = boolIsAddableAboutEmployeesPeriods_I;
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
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
