/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: December 12, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class CredmemjsonCreditMemoJson
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //--------------------------------------------------------------------------------------------------------------
        public String strCustomerFullName { get; set; }
        public String strCreditMemoNumber { get; set; }
        public String strDate { get; set; }
        public String strBilledTo { get; set; }
        public String strLogoUrl { get; set; }
        public String strDescription { get; set; }
        public double numAmount { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public CredmemjsonCreditMemoJson(
            String strCustomerFullName_I,
            String strCreditMemoNumber_I,
            String strDate_I,
            String strBilledTo_I,
            String strLogoUrl_I,
            String strDescription_I,
            double numAmount_I
            )
        {
            this.strCustomerFullName = strCustomerFullName_I;
            this.strCreditMemoNumber = strCreditMemoNumber_I;
            this.strDate = strDate_I;
            this.strBilledTo = strBilledTo_I;
            this.strLogoUrl = strLogoUrl_I;
            this.strDescription = strDescription_I;
            this.numAmount = numAmount_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
