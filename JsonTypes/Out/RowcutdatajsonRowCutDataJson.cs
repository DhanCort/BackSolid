/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: September 29, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class RowcutdatajsonRowCutDataJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public double numheight { get; set; }
        public CellcutdatajsonCellDataJson[] arrcell { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public RowcutdatajsonRowCutDataJson(
            double numheight_I,
            CellcutdatajsonCellDataJson[] arrcellcutdatajson_I
            )
        {
            this.numheight = numheight_I;
            this.arrcell = arrcellcutdatajson_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

