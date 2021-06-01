/*TASK RP.JDF*/
using Newtonsoft.Json;
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 21, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class FieldjsonFieldJson : TjsonTJson
    {
        [JsonProperty(PropertyName = "elementId")]
        public int intElementId { get; set; }

        [JsonProperty(PropertyName = "attributeName")]
        public String strAttributeName { get; set; }
    }

    //==================================================================================================================
}
/*END-TASK*/
