/*TASK RP.JDF*/
using System;
using System.Collections.Generic;
//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierre).
//                                                          //DATE: March 18, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class TempjsonTemplateJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strTemplateName { get; set; }
        public String strAliasTemplate { get; set; }
        public double? numnPrice { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public TempjsonTemplateJson(
            int intPk_I,
            String strTemplateName_I,
            String strAliasTemplate_I,
            double? numnPrice_I
            )
        {
            this.intPk = intPk_I;
            this.strTemplateName = strTemplateName_I;
            this.strAliasTemplate = strAliasTemplate_I;
            this.numnPrice = numnPrice_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
