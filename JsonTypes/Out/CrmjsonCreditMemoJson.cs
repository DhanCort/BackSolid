/*TASK RP.JDF*/
using System;
//                                                          //AUTHOR: Towa (JLBD - Luis Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierre).
//                                                          //DATE: December 08, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class CrmjsonCreditMemoJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkCreditMemo { get; set; }
        public String strCreditMemoNumber { get; set; }
        public String strCustomerFullName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public CrmjsonCreditMemoJson(
            int intPkCreditMemo_I,
            String strCreditMemoNumber_I,
            String strCustomerFullName_I
            )
        {
            this.intPkCreditMemo = intPkCreditMemo_I;
            this.strCreditMemoNumber = strCreditMemoNumber_I;
            this.strCustomerFullName = strCustomerFullName_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
