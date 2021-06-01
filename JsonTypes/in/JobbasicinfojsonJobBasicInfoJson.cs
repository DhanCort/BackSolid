/*TASK RP.JDF*/
using System;
using System.Collections.Generic;
using TowaStandard;
//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: October 29, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class JobbasicinfojsonJobBasicInfoJson : TjsonTJson
    {
        public int intJobId { get; set; }
        public int intOrderId { get; set; }
        public String strJobTicket { get; set; }
        public String strProductName { get; set; }
        public String dateLastUpdate { get; set; }
        public int intProductKey { get; set; }
        public String strCustomerName { get; set; }
        public bool? boolInEstimating { get; set; }
        public double? numnWisnetPrice { get; set; }
    }

    //==================================================================================================================
}
/*END-TASK*/
