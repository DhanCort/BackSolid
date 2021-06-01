/*TASK RP.JDF*/
using System;
//                                                          //AUTHOR: Towa (JLBD - Luis Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierre).
//                                                          //DATE: December 10, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Crjson2CreditJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkCredit { get; set; }
        public bool boolIsCreditMemo { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Crjson2CreditJson2(
            int intPkCredit_I,
            bool boolIsCreditMemo_I
            )
        {
            this.intPkCredit = intPkCredit_I;
            this.boolIsCreditMemo = boolIsCreditMemo_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
