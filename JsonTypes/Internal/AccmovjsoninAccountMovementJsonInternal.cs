/*TASK RP.JSON*/
using Odyssey2Backend.Utilities;
using System;
using TowaStandard;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF -Liliana Gutierrez).
//                                                          //DATE: December 22, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class AccmovjsoninAccountMovementJsonInternal : IComparable
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strAccountNumber { get; set; }
        public String strAccountName { get; set; }
        public double? numnIncrease { get; set; }
        public double? numnDecrease { get; set; }
        public String strTransacctionType { get; set; }
        public String strMemo { get; set; }
        public int? intnPkInvoice { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public AccmovjsoninAccountMovementJsonInternal(
            String strStartDate_I,
            String strStartTime_I,
            String strAccountNumber_I,
            String strAccountName_I,
            double? numnIncrease_I,
            double? numnDecrease_I,
            String strTransacctionType_I,
            String strMemo_I,
            int? intnPkInvoice_I
            )
        {
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strAccountNumber = strAccountNumber_I;
            this.strAccountName = strAccountName_I;
            this.numnIncrease = numnIncrease_I;
            this.numnDecrease = numnDecrease_I;
            this.strTransacctionType = strTransacctionType_I;
            this.strMemo = strMemo_I;
            this.intnPkInvoice = intnPkInvoice_I;
        }

        //--------------------------------------------------------------------------------------------------------------        
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            AccmovjsoninAccountMovementJsonInternal accmovjsonin = (AccmovjsoninAccountMovementJsonInternal)obj_I;

            ZonedTime ztime = ZonedTimeTools.NewZonedTime(this.strStartDate.ParseToDate(), 
                this.strStartTime.ParseToTime());
            ZonedTime ztimeB = ZonedTimeTools.NewZonedTime(accmovjsonin.strStartDate.ParseToDate(),
                accmovjsonin.strStartTime.ParseToTime());

            return ztime.CompareTo(ztimeB);
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
