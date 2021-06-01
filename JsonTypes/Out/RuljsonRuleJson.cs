/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: April 27, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class RuljsonRuleJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkRule { get; set; }
        public String strFrecuency { get; set; }
        public String strFrecuencyValue { get; set; }
        public String strStartTime { get; set; }
        public String strEndTime { get; set; }
        public int? intnPkResource { get; set; }
        public String strRangeStartDate { get; set; }
        public String strRangeStartTime { get; set; }
        public String strRangeEndDate { get; set; }
        public String strRangeEndTime { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public RuljsonRuleJson(
            int intPkRule_I,
            String strFrecuency_I,
            String strFrecuencyValue_I,
            String strStartTime_I,
            String strEndTime_I,
            int? intnPkResource_I,
            String strRangeStartDate_I,
            String strRangeStartTime_I,
            String strRangeEndDate_I,
            String strRangeEndTime_I
            )
        {
            this.intPkRule = intPkRule_I;
            this.strFrecuency = strFrecuency_I;
            this.strFrecuencyValue = strFrecuencyValue_I;
            this.strStartTime = strStartTime_I;
            this.strEndTime = strEndTime_I;
            this.intnPkResource = intnPkResource_I;
            this.strRangeStartDate = strRangeStartDate_I;
            this.strRangeStartTime = strRangeStartTime_I;
            this.strRangeEndDate = strRangeEndDate_I;
            this.strRangeEndTime = strRangeEndTime_I;
        }
        
        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
