/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: February 24, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Ascvaljson1AscendantsValueJson1
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String[] arrstrAscendant;
        public String strValue;

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Ascvaljson1AscendantsValueJson1(
            String[] arrstrAscendant_I,
            String strValue_I
            )
        {
            this.arrstrAscendant = arrstrAscendant_I;
            this.strValue = strValue_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
