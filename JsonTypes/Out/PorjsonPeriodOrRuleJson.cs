/*TASK RP.JSON*/
using System;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 28, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class PorjsonPeriodOrRuleJson : IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
       
        public int? intnPkPeriod { get; set; }
        public String strStartTime { get; set; }
        public String strEndTime { get; set; }
        public String strJobId { get; set; }
        public String strJobNumber { get; set; }
        public bool boolIsAvailable { get; set; }
        public String strFirstName { get; set; }
        public String strLastName { get; set; }
        public String strProcess { get; set; }
        public String strJobName { get; set; }
        public int intMinsBeforeDelete { get; set; }
        public bool? boolnIsPeriodDone { get; set; }
        public bool boolPeriodStarted { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public PorjsonPeriodOrRuleJson(
            int? intnPkPeriod_I,
            String strStartTime_I,
            String strEndTime_I,
            String strJobId_I,
            String strJobNumber_I,
            bool boolIsAvailable_I,
            String strFirstName_I,
            String strLastName_I,
            String strProcess_I,
            String strJobName_I,
            int intMinsBeforeDelete_I,
            bool? boolnIsPeriodDone_I,
            bool boolPeriodStarted_I
            )
        {
            this.intnPkPeriod = intnPkPeriod_I;
            this.strStartTime = strStartTime_I;
            this.strEndTime = strEndTime_I;
            this.strJobId = strJobId_I;
            this.strJobNumber = strJobNumber_I;
            this.boolIsAvailable = boolIsAvailable_I;
            this.strFirstName = strFirstName_I;
            this.strLastName = strLastName_I;
            this.strProcess = strProcess_I;
            this.strJobName = strJobName_I;
            this.intMinsBeforeDelete = intMinsBeforeDelete_I;
            this.boolnIsPeriodDone = boolnIsPeriodDone_I;
            this.boolPeriodStarted = boolPeriodStarted_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            PorjsonPeriodOrRuleJson porjson = (PorjsonPeriodOrRuleJson)obj_I;

            Time time = this.strStartTime.ParseToTime();
            Time timeB = porjson.strStartTime.ParseToTime();

            return time.CompareTo(timeB);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
