/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: May 26, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Attrjson5AttributeJson5
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strAscendant { get; set; }
        public String strValue { get; set; }
        public int? intnInheritedValuePk { get; set; }
        public bool boolChangeable { get; set; }
        public int? intnPkValue { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Attrjson5AttributeJson5(
            String strAscendant_I,
            String strValue_I,
            int? intnInheritedValuePk_I,
            bool boolChangeable_I,
            int? intnPkValue_I
            )
        {
            this.strAscendant = strAscendant_I;
            this.strValue = strValue_I;
            this.intnInheritedValuePk = intnInheritedValuePk_I;
            this.boolChangeable = boolChangeable_I;
            this.intnPkValue = intnPkValue_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
