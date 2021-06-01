/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF -Liliana Gutierrez).
//                                                          //DATE: December 09, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PayjsonPaymentJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //-------------------------------------------------------------------------------------------------------------
        public String strCustomerFullName { get; set; }
        public String strMethodName { get; set; }
        public String strReference { get; set; }
        public double numAmount { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PayjsonPaymentJson(
            String strCustomerFullName_I,
            String strMethodName_I,
            String strReference_I,
            double numAmount_I
            )
        {
            this.strCustomerFullName = strCustomerFullName_I;
            this.strMethodName = strMethodName_I;
            this.strReference = strReference_I;
            this.numAmount = numAmount_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
