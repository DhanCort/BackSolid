/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 5, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Attrjson4AttributeJson4
    {
        //-------------------------------------------------------------------------------------------------------------
        //
        public int intPkValue { get; set; }
        public int? intnPkValueInherited { get; set; }
        public String strValue { get; set; }
        public int[] arrPkAscendant { get; set; }
        public bool? boolChangeable { get; set; }
        public bool boolIsBlocked { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Attrjson4AttributeJson4(
            int intPkValue_I,
            int? intnPkValueInherited_I,
            String strValue_I,
            int[] arrPkAscendant_I,
            bool? boolChangeable_I,
            bool boolIsBlocked_I
            )
        {
            this.intPkValue = intPkValue_I;
            this.intnPkValueInherited = intnPkValueInherited_I;
            this.strValue = strValue_I;
            this.arrPkAscendant = arrPkAscendant_I;
            this.boolChangeable = boolChangeable_I;
            this.boolIsBlocked = boolIsBlocked_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
