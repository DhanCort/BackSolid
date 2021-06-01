/*TASK RP.JDF*/
using Newtonsoft.Json;
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 21, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ValjsonValueJson : TjsonTJson
    {
        [JsonProperty(PropertyName = "value")]
        public String strValue { get; set; }
    }

    //==================================================================================================================
}
/*END-TASK*/
