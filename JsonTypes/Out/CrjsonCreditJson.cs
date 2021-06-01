/*TASK RP.JDF*/
using System;
//                                                          //AUTHOR: Towa (JLBD - Luis Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierre).
//                                                          //DATE: December 09, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class CrjsonCreditJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkCredit { get; set; }
        public String strCreditNumber { get; set; }
        public double numOriginalAmount { get; set; }
        public double numOpenBalance { get; set; }
        public bool boolIsCreditMemo { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public CrjsonCreditJson(
            int intPkCredit_I,
            String strCreditNumber_I,
            double numOriginalAmount_I,
            double numOpenBalance_I,
            bool boolIsCreditMemo_I
            )
        {
            this.intPkCredit = intPkCredit_I;
            this.strCreditNumber = strCreditNumber_I;
            this.numOriginalAmount = numOriginalAmount_I;
            this.numOpenBalance = numOpenBalance_I;
            this.boolIsCreditMemo = boolIsCreditMemo_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
