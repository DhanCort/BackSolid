/*TASK RP.JSON*/
using Odyssey2Backend.Utilities;
using System;
using TowaStandard;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF -Liliana Gutierrez).
//                                                          //DATE: April 01, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ResattrjsonResourceAttributesJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkAttribute { get; set; }
        public String strAttrName { get; set; }
        public String strAttrValue { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ResattrjsonResourceAttributesJson(
            int intPkAttribute_I,
            String strAttrName_I,
            String strAttrValue_I
            )
        {
            this.intPkAttribute = intPkAttribute_I;
            this.strAttrName = strAttrName_I;
            this.strAttrValue = strAttrValue_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
