/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: September 29, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class CellcutdatajsonCellDataJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public double numwidth { get; set; }
        public bool boolIsWaste { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public CellcutdatajsonCellDataJson(
            double numwidth_I,
            bool boolIsWaste_I
            )
        {
            this.numwidth = numwidth_I;
            this.boolIsWaste = boolIsWaste_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

