/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa(CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 27, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class InvojsonInvoiceJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public int intOrderNumber { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------

        public InvojsonInvoiceJson(
            int intPk_I,
            int intOrderNumber_I
            )
        {
            this.intPk = intPk_I;
            this.intOrderNumber = intOrderNumber_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
