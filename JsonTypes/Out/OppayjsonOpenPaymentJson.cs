/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa(CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: December 09, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class OppayjsonOpenPaymentJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //-------------------------------------------------------------------------------------------------------------

        public int intPkPayment { get; set; }
        public String strCustomerFullName { get; set; }
        public String strDate { get; set; }
        public String strMethodName { get; set; }
        public String strReference { get; set; }
        public double numAmount { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------

        public OppayjsonOpenPaymentJson(
            int intPkPayment_I,
            String strCustomerFullName_I,
            String strDate_I,
            String strMethodName_I,
            String strReference_I,
            double numAmount_I
            )
        {
            this.intPkPayment = intPkPayment_I;
            this.strCustomerFullName = strCustomerFullName_I;
            this.strDate = strDate_I;
            this.strMethodName = strMethodName_I;
            this.strReference = strReference_I;
            this.numAmount = numAmount_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
