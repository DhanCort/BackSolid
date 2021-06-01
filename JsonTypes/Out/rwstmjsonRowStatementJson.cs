/*TASK RP.JSON*/
using Odyssey2Backend.Utilities;
using System;
using TowaStandard;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: December 15 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class rwstmjsonRowStatementJson : IComparable
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //-------------------------------------------------------------------------------------------------------------

        public String strDate { get; set; }
        public String strType { get; set; }
        public String strNumber { get; set; }
        public double? numnCharge { get; set; }
        public double? numnPayment { get; set; }
        public double? numnAmount { get; set; }
        public double? numnBalance { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public rwstmjsonRowStatementJson(
            String strDate_I,
            String strType_I,
            String strNumber_I,
            double? numnCharge_I,
            double? numnPayment_I,
            double? numnAmount_I,
            double? numnBalance_I
            )
        {
            this.strDate = strDate_I;
            this.strType = strType_I;
            this.strNumber = strNumber_I;
            this.numnCharge = numnCharge_I;
            this.numnPayment = numnPayment_I;
            this.numnAmount = numnAmount_I;
            this.numnBalance = numnBalance_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            rwstmjsonRowStatementJson rwstmjsonRows = (rwstmjsonRowStatementJson)obj_I;

            //                                              //To easy code.
            Date dateThis = this.strDate.ParseToDate();
            Time timeThis = "12:00:00".ParseToTime();

            ZonedTime ztimeThis = ZonedTimeTools.NewZonedTime(dateThis, timeThis);

            Date dateToCompare = rwstmjsonRows.strDate.ParseToDate();
            Time timeToCompare = "12:00:00".ParseToTime();

            ZonedTime ztimeToCompare = ZonedTimeTools.NewZonedTime(dateToCompare, timeToCompare);

            return ztimeThis.CompareTo(ztimeToCompare);
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
