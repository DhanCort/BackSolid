/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: Augost 17, 2020.  

namespace Odyssey2Backend.JsonTemplates.Out
{
    //==================================================================================================================
    public class ResperjsonResourcePeriodJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strResource { get; set; }
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
        public ResperjsonResourcePeriodJson(
            String strResource_I,
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
            this.strResource = strResource_I;
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

