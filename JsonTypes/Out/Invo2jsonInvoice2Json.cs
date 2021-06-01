/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa(CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: December 11, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Invo2jsonInvoice2Json
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public int intOrderNumber { get; set; }
        public double numOpenBalance { get; set; }
        public String strBilledTo { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------

        public Invo2jsonInvoice2Json(
            int intPk_I,
            int intOrderNumber_I,
            double numOpenBalance_I,
            String strBilledTo_I
            )
        {
            this.intPk = intPk_I;
            this.intOrderNumber = intOrderNumber_I;
            this.numOpenBalance = numOpenBalance_I;
            this.strBilledTo = strBilledTo_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
