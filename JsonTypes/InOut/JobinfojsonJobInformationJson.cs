/*TASK RP.JDF*/
using System;
using System.Collections.Generic;
//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 21, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class JobinfojsonJobInformationJson : TjsonTJson
    {
        public int intJobId { get; set; }
        public String strJobTicket { get; set; }
        public bool boolIsCompleted { get; set; }
        public String strJobNumber { get; set; }

    }

    //==================================================================================================================
}
/*END-TASK*/
