/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: September 29, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class CutdatajsonCutDataJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public double numPerUnit { get; set; }
        public double numNeeded { get; set; }
        public bool boolIsReversed { get; set; }
        public RowcutdatajsonRowCutDataJson[] arrrow { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public CutdatajsonCutDataJson(
            double numPerUnit_I,
            bool boolIsReversed_I,
            RowcutdatajsonRowCutDataJson[] arrrowcutdatajson_I
            )
        {
            this.numPerUnit = numPerUnit_I;
            this.boolIsReversed = boolIsReversed_I;
            this.arrrow = arrrowcutdatajson_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

