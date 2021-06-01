/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: Febraury 18, 2021.  

namespace Odyssey2Backend.JsonTemplates.Out
{
    //==================================================================================================================
    public class Resperjson3ResourcePeriodJson3
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int? intnPkPeriod { get; set; }
        public int? intnEstimatedDuration { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strEndDate { get; set; }
        public String strEndTime { get; set; }
        public String strFirstName { get; set; }
        public String strLastName { get; set; }
        public int? intnContactId { get; set; }
        public int? intnMinsBeforeDelete { get; set; }
        public bool boolPeriodStarted { get; set; }
        public bool boolPeriodCompleted { get; set; }


        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Resperjson3ResourcePeriodJson3(
            int? intnPkPeriod_I,
            int? intnEstimatedDuration_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            String strFirstName_I,
            String strLastName_I,
            int? intnContactId_I,
            int? intnMinsBeforeDelete_I,
            bool boolPeriodStarted_I,
            bool boolPeriodCompleted_I
            )
        {
            this.intnPkPeriod = intnPkPeriod_I;
            this.intnEstimatedDuration = intnEstimatedDuration_I;
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strEndDate = strEndDate_I;
            this.strEndTime = strEndTime_I;
            this.strFirstName = strFirstName_I;
            this.strLastName = strLastName_I;
            this.intnContactId = intnContactId_I;
            this.intnMinsBeforeDelete = intnMinsBeforeDelete_I;
            this.boolPeriodStarted = boolPeriodStarted_I;
            this.boolPeriodCompleted = boolPeriodCompleted_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

