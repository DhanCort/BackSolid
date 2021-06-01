/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa(CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: December 09, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class OpinvosjsonOpenInvoicesJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //-------------------------------------------------------------------------------------------------------------
        public OpinvojsonOpenInvoiceJson[] arrOpenInvoices { get; set; }
        public int intContactId { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------

        public OpinvosjsonOpenInvoicesJson(
            OpinvojsonOpenInvoiceJson[] arrOpenInvoices_I,
            int intContactId_I
            )
        {
            this.arrOpenInvoices = arrOpenInvoices_I;
            this.intContactId = intContactId_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
