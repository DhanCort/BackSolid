/*TASK RP.JDF*/
using Newtonsoft.Json;
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 21, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ProdjsonProductJson : TjsonTJson
    {
        [JsonProperty(PropertyName = "category")]
        public String strCategory { get; set; }

        [JsonProperty(PropertyName = "productKey")]
        public int intProductKey { get; set; }

        [JsonProperty(PropertyName = "productName")]
        public String strProductName { get; set; }

        [JsonProperty(PropertyName = "boolIsPublic")]
        public bool boolIsPublic { get; set; }
    }

    //==================================================================================================================
}
/*END-TASK*/
