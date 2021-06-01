/*TASK RP.JDF*/
using System;
//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierre).
//                                                          //DATE: April 12, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class GpcondjsonGroupConditionJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strOperator { get; set; }
        public CondjsonConditionJson[] arrcond { get; set; }
        public GpcondjsonGroupConditionJson[] arrgpcond { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public GpcondjsonGroupConditionJson(
            )
        {
        }

        public GpcondjsonGroupConditionJson(
            String strOperator_I,
            CondjsonConditionJson[] arrcond_I,
            GpcondjsonGroupConditionJson[] arrgpcond_I
            )
        {
            this.strOperator = strOperator_I;
            this.arrcond = arrcond_I;
            this.arrgpcond = arrgpcond_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/



