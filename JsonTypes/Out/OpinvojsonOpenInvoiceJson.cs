/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa(CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: December 09, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class OpinvojsonOpenInvoiceJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //-------------------------------------------------------------------------------------------------------------

        public int intPkInvoice { get; set; }
        public int intInvoiceNumber { get; set; }
        public double numOriginalAmount { get; set; }
        public double numOpenBalance { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------

        public OpinvojsonOpenInvoiceJson(
            int intPkInvoice_I,
            int intInvoiceNumber_I,
            double numOriginalAmount_I,
            double numOpenBalance_I
            )
        {
            this.intPkInvoice = intPkInvoice_I;
            this.intInvoiceNumber = intInvoiceNumber_I;
            this.numOriginalAmount = numOriginalAmount_I;
            this.numOpenBalance = numOpenBalance_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
