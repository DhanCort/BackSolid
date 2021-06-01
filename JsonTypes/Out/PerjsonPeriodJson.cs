/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: April 28th, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PerjsonPeriodJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strEndDate { get; set; }
        public String strEndTime { get; set; }
        public int intJobId { get; set; }
        public String strJobNumber { get; set; }
        public int intPkResource { get; set; }
        public int? intnContactId { get; set; }
        public int intMinsBeforeDelete { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PerjsonPeriodJson(
            int intPk_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            int intJobId_I,
            String strJobNumber_I,
            int intPkResource_I,
            int? intnContactId_I,
            int intMinsBeforeDelete_I
            )
        {
            this.intPk = intPk_I;
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strEndDate = strEndDate_I;
            this.strEndTime = strEndTime_I;
            this.intJobId = intJobId_I;
            this.strJobNumber = strJobNumber_I;
            this.intPkResource = intPkResource_I;
            this.intnContactId = intnContactId_I;
            this.intMinsBeforeDelete = intMinsBeforeDelete_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/