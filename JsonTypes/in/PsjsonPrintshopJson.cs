/*TASK RP.JDF*/
using Newtonsoft.Json;
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 21, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PsjsonPrintshopJson : TjsonTJson
    {
        [JsonProperty(PropertyName = "printshopId")]
        public String strPrintshopId { get; set; }

        [JsonProperty(PropertyName = "printshopName")]
        public String strPrintshopName { get; set; }
        public String strUrl { get; set; }
    }

    //==================================================================================================================
}
/*END-TASK*/
