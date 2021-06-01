/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: April 13, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class LogjsonLoginJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strToken { get; set; }
        public String strUserFirstName { get; set; }
        public String strUserLastName { get; set; }
        public String strPrintshopId { get; set; }
        public String strPrintshopName { get; set; }
        public bool boolIsAdmin { get; set; }
        public bool boolIsOwner { get; set; }
        public bool boolIsSuperAdmin { get; set; }
        public bool boolIsSupervisor { get; set; }
        public bool boolIsAccountant { get; set; }
        public int intUnreadAlerts { get; set; }
        public int intContactId { get; set; }
        public String strCustomerUrl { get; set; }
        public String strSendAProofUrl { get; set; }
        public bool boolOffset { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public LogjsonLoginJson(
            String strToken_I,
            String strUserFirstName_I,
            String strUserLastName_I,
            String strPrintshopId_I,
            String strPrintshopName_I,
            bool boolIsAdmin_I,
            bool boolIsOwner_I,
            bool boolIsSuperAdmin_I,
            bool boolIsSupervisor_I,
            bool boolIsAccountant_I,
            int intUnreadAlerts_I,
            int intContactId_I,
            String strCustomerUrl_I,
            String strSendAProofUrl_I,
            bool boolOffset_I
            )
        {
            this.strToken = strToken_I;
            this.strUserFirstName = strUserFirstName_I;
            this.strUserLastName = strUserLastName_I;
            this.strPrintshopId = strPrintshopId_I;
            this.strPrintshopName = strPrintshopName_I;
            this.boolIsAdmin = boolIsAdmin_I;
            this.boolIsOwner = boolIsOwner_I;
            this.boolIsSuperAdmin = boolIsSuperAdmin_I;
            this.boolIsSupervisor = boolIsSupervisor_I;
            this.boolIsAccountant = boolIsAccountant_I;
            this.intUnreadAlerts = intUnreadAlerts_I;
            this.intContactId = intContactId_I;
            this.strCustomerUrl = strCustomerUrl_I;
            this.strSendAProofUrl = strSendAProofUrl_I;
            this.boolOffset = boolOffset_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
