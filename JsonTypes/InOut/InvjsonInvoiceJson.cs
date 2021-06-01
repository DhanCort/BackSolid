/*TASK RP.JDF*/
using System;
using System.Collections.Generic;
//                                                          //AUTHOR: Towa(DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 13, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class InvjsonInvoiceJson : TjsonTJson
    {
        public int intOrderId { get; set; }
        public int? intnOrderNumber { get; set; }
        public String strOrderDate { get; set; }
        public int intJobsQuantity { get; set; }
        public String strLogoUrl { get; set; }
        public String strShippedTo { get; set; }
        public String strShippedToAddressLine1 { get; set; }
        public String strShippedToAddressLine2 { get; set; }
        public String strShippedToCity { get; set; }
        public String strShippedToState { get; set; }
        public String strShippedToCountry { get; set; }
        public String strShippedToFirstName { get; set; }
        public String strShippedToLastName { get; set; }
        public bool boolIsShipped { get; set; }
        public String strBilledTo { get; set; }
        public String strPayableTo { get; set; }
        public String strShippingMethod { get; set; }
        public String strComments { get; set; }
        public String strTerms { get; set; }
        public String strPO { get; set; }
        public int? intnInvoiceNumber { get; set; }

        //                                                  //The following attribute will be filled with 
        //                                                  //      job information
        public List<InvjobinfojsonInvoiceJobInformationJson> darrinvjobinfojson { get; set; }

        public double numSubtotalTotal { get; set; }
        public double numTaxes { get; set; }
        public double numTaxPercentage { get; set; }
        public double numTotal { get; set; }
        public String strPrintshopZipCode { get; set; }
        public String strShippedToZip { get; set; }
    }

    //==================================================================================================================
}
/*END-TASK*/
