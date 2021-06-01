/*TASK RP.JDF*/
using Newtonsoft.Json;
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 28, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Elejson1ElementJson1
    {
        [JsonProperty(PropertyName = "intPk")]
        public int intPk { get; set; }

        [JsonProperty(PropertyName = "strName")]
        public String strName { get; set; }

        [JsonProperty(PropertyName = "arrele")]
        public Elejson1ElementJson1[] arrele { get; set; }

        [JsonProperty(PropertyName = "arrattr")]
        public Attrjson2AttributeJson2[] arrattr { get; set; }
    }

    //==================================================================================================================
}
/*END-TASK*/
