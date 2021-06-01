/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: Augost 17, 2020.  

namespace Odyssey2Backend.JsonTemplates.Out
{
    //==================================================================================================================
    public class ProperjsonProcessPeriodJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkCalculation { get; set; }
        public String strDescription { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strEndDate { get; set; }
        public String strEndTime { get; set; }
        public String strFirstName { get; set; }
        public String strLastName { get; set; }
        public int? intnContactId { get; set; }
        public String strStatus { get; set; }
        public int intMinsBeforeDelete { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ProperjsonProcessPeriodJson(
            int intPkCalculation_I,
            String strDescription_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            String strFirstName_I,
            String strLastName_I,
            int? intnContactId_I,
            String strStatus_I,
            int intMinsBeforeDelete_I
            )
        {
            this.intPkCalculation = intPkCalculation_I;
            this.strDescription = strDescription_I;
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strEndDate = strEndDate_I;
            this.strEndTime = strEndTime_I;
            this.strFirstName = strFirstName_I;
            this.strLastName = strLastName_I;
            this.intnContactId = intnContactId_I;
            this.strStatus = strStatus_I;
            this.intMinsBeforeDelete = intMinsBeforeDelete_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

