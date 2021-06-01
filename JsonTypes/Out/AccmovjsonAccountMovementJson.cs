/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF -Liliana Gutierrez).
//                                                          //DATE: November 13, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class AccmovjsonAccountMovementJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strDate { get; set; }
        public String strAccountNumber { get; set; }
        public String strAccountName { get; set; }
        public String strNumber { get; set; }
        public double? numnIncrease { get; set; }
        public double? numnDecrease { get; set; }
        public String strTransacctionType { get; set; }
        public String strMemo { get; set; }
        public double numBalance { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public AccmovjsonAccountMovementJson(
            String strDate_I,
            String strAccountNumber_I,
            String strAccountName_I,
            String strNumber_I,
            double? numnIncrease_I,
            double? numnDecrease_I,
            String strTransacctionType_I,
            String strMemo_I,
            double numBalance
            )
        {
            this.strDate = strDate_I;
            this.strAccountNumber = strAccountNumber_I;
            this.strAccountName = strAccountName_I;
            this.strNumber = strNumber_I;
            this.numnIncrease = numnIncrease_I;
            this.numnDecrease = numnDecrease_I;
            this.strTransacctionType = strTransacctionType_I;
            this.strMemo = strMemo_I;
            this.numBalance = numBalance;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
