/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: December 09, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class BankdepjsonBankDepositJson
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //--------------------------------------------------------------------------------------------------------------
        public int intPkBankDeposit { get; set; }
        public String strDate { get; set; }
        public double numAmount { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public BankdepjsonBankDepositJson(
            int intPkBankDeposit_I,
            String strDate_I,
            double numAmount_I
            )
        {
            this.intPkBankDeposit = intPkBankDeposit_I;
            this.strDate = strDate_I;
            this.numAmount = numAmount_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
