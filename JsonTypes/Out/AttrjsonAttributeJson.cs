/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: February 24, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Attrjson10AttributeJson10
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strName { get; set; }
        public String strValue { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Attrjson10AttributeJson10(
            int intPk_I,
            String strName_I,
            String strValue_I
            )
        {
            this.intPk = intPk_I;
            this.strName = strName_I;
            this.strValue = strValue_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
