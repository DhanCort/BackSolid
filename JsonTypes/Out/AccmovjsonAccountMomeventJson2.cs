/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: November 13, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class AccmovjsonAccountMomeventJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //-------------------------------------------------------------------------------------------------------------

        public String strDate { get; set; }
        public String strTransacctionType { get; set; }
        public String strNumber { get; set; }
        public String strName { get; set; }
        public String strMemo { get; set; }
        public double? numnChargeOrIncrease { get; set; }
        public double? numnPaymentOrDecrease { get; set; }
        public double numBalance { get; set; }


        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public AccmovjsonAccountMomeventJson2(
            String strDate_I,
            String strTransacctionType_I,
            String strNumber_I,
            String strName_I,
            String strMemo_I,
            double? numnChargeOrIncrease_I,
            double? numnPaymentOrDecrease_I,
            double numBalance_I
            )
        {
            this.strDate = strDate_I;
            this.strTransacctionType = strTransacctionType_I;
            this.strNumber = strNumber_I;
            this.strName = strName_I;
            this.strMemo = strMemo_I;
            this.numnChargeOrIncrease = numnChargeOrIncrease_I;
            this.numnPaymentOrDecrease = numnPaymentOrDecrease_I;
            this.numBalance = numBalance_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
