/*TASK RP.JDF*/
using System;
//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierre).
//                                                          //DATE: April 12, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class CondjsonConditionJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int? intnPkAttribute { get; set; }
        public String strCondition { get; set; }
        public String strValue { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public CondjsonConditionJson(
            )
        {
        }

        public CondjsonConditionJson(
            int? intnPkAttribute_I,
            String strCondition_I,
            String strValue_I
            )
        {
            this.intnPkAttribute = intnPkAttribute_I;
            this.strCondition = strCondition_I;
            this.strValue = strValue_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/

