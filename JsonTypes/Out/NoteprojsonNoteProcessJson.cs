/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF -Liliana Gutierrez).
//                                                          //DATE: March 05, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class NoteprojsonNoteProcessJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkNote { get; set; }
        public String strNote { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public NoteprojsonNoteProcessJson(
            int intPkNote_I,
            String strNote_I
            )
        {
            this.intPkNote = intPkNote_I;
            this.strNote = strNote_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
