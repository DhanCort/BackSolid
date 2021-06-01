/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa(DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: November 11, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PsorderjsonPrintshopOrderJson : TjsonTJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intOrderId { get; set; }
        public int? intnOrderNumber { get; set; }
        public int? intnPkInvoice { get; set; }
        public bool boolAllJobsAreCompleted { get; set; }
        public List<JobinfojsonJobInformationJson> darrjobsinfo { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PsorderjsonPrintshopOrderJson(
            int intOrderId_I,
            int? intnOrderNumber_I,
            int? intnPkInvoice_I,
            List<JobinfojsonJobInformationJson> darrjobsinfo_I
            )
        {
            this.intOrderId = intOrderId_I;
            this.intnOrderNumber = intnOrderNumber_I;
            this.intnPkInvoice = intnPkInvoice_I;
            this.darrjobsinfo = darrjobsinfo_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
