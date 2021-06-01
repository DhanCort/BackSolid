/*TASK RP.JDF*/
using Newtonsoft.Json;
using System;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: September 05, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ProdprijsonProducPrivatetJson : TjsonTJson
    {
        [JsonProperty(PropertyName = "category")]
        public String strCategory { get; set; }

        [JsonProperty(PropertyName = "productKey")]
        public int intProductKey { get; set; }

        [JsonProperty(PropertyName = "productName")]
        public String strProductName { get; set; }

        [JsonProperty(PropertyName = "boolIsPublic")]
        public bool boolIsPublic { get; set; }

        [JsonProperty(PropertyName = "intncompanyId")]
        public long? intnCompanyId { get; set; }

        [JsonProperty(PropertyName = "intnbranchId")]
        public long? intnBranchId { get; set; }

        [JsonProperty(PropertyName = "intncontactId")]
        public long? intnContactId { get; set; }
    }

    //==================================================================================================================
}
/*END-TASK*/
