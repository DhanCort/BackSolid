/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF -Liliana Gutierrez).
//                                                          //DATE: November 13, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class AccsinaperjsonAccountsInAPeriodJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public Accjson2AccountJson2[] arrAccount { get; set; }
        public double numTotalExpenses { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public AccsinaperjsonAccountsInAPeriodJson(
            Accjson2AccountJson2[] arrAccount_I,
            double numTotalExpenses_I
            )
        {
            this.arrAccount = arrAccount_I;
            this.numTotalExpenses = numTotalExpenses_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
