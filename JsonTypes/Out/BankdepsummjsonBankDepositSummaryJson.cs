/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: December 09, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class BankdepsummjsonBankDepositSummaryJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //-------------------------------------------------------------------------------------------------------------
        public String strBankAccountName { get; set; }
        public String strDepositDate { get; set; }
        public String strDate { get; set; }
        public PayjsonPaymentJson[] arrPayments { get; set; }
        public double numTotal { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public BankdepsummjsonBankDepositSummaryJson(
            String strBankAccountName_I,
            String strDepositDate_I,
            String strDate_I,
            PayjsonPaymentJson[] arrPayments_I,
            double numTotal_I
            )
        {
            this.strBankAccountName = strBankAccountName_I;
            this.strDepositDate = strDepositDate_I;
            this.strDate = strDate_I;
            this.arrPayments = arrPayments_I;
            this.numTotal = numTotal_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
