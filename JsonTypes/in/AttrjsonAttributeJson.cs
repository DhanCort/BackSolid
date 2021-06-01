/*TASK RP.JDF*/
using System;
using System.ComponentModel.DataAnnotations;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 21, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class AttrjsonAttributeJson : TjsonTJson
    {
        [Required] 
        [Range(1, int.MaxValue)]
        public int intAttributeId { get; set; }

        [StringLength(250, MinimumLength = 0)]
        public String strAttributeName { get; set; }

        [StringLength(250, MinimumLength = 0)]
        public String strValue { get; set; }
    }

    //==================================================================================================================
}
/*END-TASK*/
