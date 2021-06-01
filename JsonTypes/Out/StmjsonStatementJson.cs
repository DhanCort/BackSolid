/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF -Liliana Gutierrez).
//                                                          //DATE: Decemver 15, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class StmjsonStatementJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //-------------------------------------------------------------------------------------------------------------

        public String strLogoUrl { get; set; }
        public String strTitle { get; set; }
        public String strBilledTo { get; set; }
        public String strDate { get; set; }
        public String strDateFrom { get; set; }
        public String strDateTo { get; set; }
        public rwstmjsonRowStatementJson[] arrrow { get; set; }
        public double numCurrentDue { get; set; }
        public double num30DaysDue { get; set; }
        public double num60DaysDue { get; set; }
        public double num90DaysDue { get; set; }
        public double numMore90DaysDue { get; set; }
        public double numAmountDue { get; set; }
        public double? numnTotalCharge { get; set; }
        public double? numnTotalPayment { get; set; }
        public double? numnTotalAmount { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public StmjsonStatementJson(
            String strLogoUrl_I,
            String strTitle_I,
            String strBilledTo_I,
            String strDate_I,
            String strDateFrom_I,
            String strDateTo_I,
            rwstmjsonRowStatementJson[] arrrow_I,
            double? numnTotalCharge_I,
            double? numnTotalPayment_I,
            double? numnTotalAmount_I,
            double numCurrentDue_I,
            double num30DaysDue_I,
            double num60DaysDue_I,
            double num90DaysDue_I,
            double numMore90DaysDue_I, 
            double numAmountDue_I
            )
        {
            this.strLogoUrl = strLogoUrl_I;
            this.strTitle = strTitle_I;
            this.strBilledTo = strBilledTo_I;
            this.strDate = strDate_I;
            this.strDateFrom = strDateFrom_I;
            this.strDateTo = strDateTo_I;
            this.arrrow = arrrow_I;
            this.numnTotalCharge = numnTotalCharge_I;
            this.numnTotalPayment = numnTotalPayment_I;
            this.numnTotalAmount = numnTotalAmount_I;
            this.numCurrentDue = numCurrentDue_I;
            this.num30DaysDue = num30DaysDue_I;
            this.num60DaysDue = num60DaysDue_I;
            this.num90DaysDue = num90DaysDue_I;
            this.numMore90DaysDue = numMore90DaysDue_I;
            this.numAmountDue = numAmountDue_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
