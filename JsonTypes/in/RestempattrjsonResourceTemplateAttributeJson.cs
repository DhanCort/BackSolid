/*TASK RP.JDF*/
using System;
using System.Collections.Generic;
//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierre).
//                                                          //DATE: March 18, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class RestempattrjsonResourceTemplateAttributeJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strTemplateName { get; set; }
        public String strAliasTemplate { get; set; }
        public String strResourceName { get; set; }
        public List<String> darrstrValuesFromAttribute { get; set; }
        public String strPricePerThousand { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public RestempattrjsonResourceTemplateAttributeJson(
            String strTemplateName_I,
            String strAliasTemplate_I,
            String strResourceName_I,
            List<String> darrstrValuesFromAttribute_I,
            String strPricePerThousand_I
            )
        {
            this.strTemplateName = strTemplateName_I;
            this.strAliasTemplate = strAliasTemplate_I;
            this.strResourceName = strResourceName_I;
            this.darrstrValuesFromAttribute = darrstrValuesFromAttribute_I;
            this.strPricePerThousand = strPricePerThousand_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
