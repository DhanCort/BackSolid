/*TASK RP.JDF*/
using Newtonsoft.Json;
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 21, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Field2jsonField2Json : TjsonTJson
    {
        [JsonProperty(PropertyName = "elementId")]
        public int intElementId { get; set; }

        [JsonProperty(PropertyName = "attributeName")]
        public String strAttributeName { get; set; }

        [JsonProperty(PropertyName = "values")]
        public ValjsonValueJson[] arrstrValues { get; set; }
        public Field2jsonField2Json(
            int intElementId_I, 
            String strAttributeName_I,
            ValjsonValueJson[] arrstrValues_I
            )
        {
            this.intElementId = intElementId_I;
            this.strAttributeName = strAttributeName_I;
            this.arrstrValues = arrstrValues_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
