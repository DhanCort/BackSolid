/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa(DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: November 11, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class InvjobinfojsonInvoiceJobInformationJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int? intnJobId { get; set; }
        public String strJobNumber { get; set; }
        public String strName { get; set; }
        public int intQuantity { get; set; }
        public double numPrice { get; set; }
        public int? intnPkAccount { get; set; }
        public int? intnPkAccountMov { get; set; }
        public String strAccount { get; set; }
        public bool boolIsExempt { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public InvjobinfojsonInvoiceJobInformationJson()
        {

        }

        //-------------------------------------------------------------------------------------------------------------
        public InvjobinfojsonInvoiceJobInformationJson(
            int? intnJobId_I,
            String strJobNumber_I,
            String strName_I,
            int intQuantity_I,
            double numPrice_I,
            int? intnPkAccount_I,
            int? intnPkAccountMov_I,
            String strAccount_I,
            bool boolIsExempt_I
            )
        {
            this.intnJobId = intnJobId_I;
            this.strJobNumber = strJobNumber_I;
            this.strName = strName_I;
            this.intQuantity = intQuantity_I;
            this.numPrice = numPrice_I;
            this.intnPkAccount = intnPkAccount_I;
            this.intnPkAccountMov = intnPkAccountMov_I;
            this.strAccount = strAccount_I;
            this.boolIsExempt = boolIsExempt_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
