/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (JLBD - José Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: September 14, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class WstaddjsonWasteAdditionalJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public double numWasteAdditional { get; set; }
        public String strSource { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public WstaddjsonWasteAdditionalJson(
            double numWasteAdditional_I,
            String strSource_I
            )
        {
            this.numWasteAdditional = numWasteAdditional_I;
            this.strSource = strSource_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
