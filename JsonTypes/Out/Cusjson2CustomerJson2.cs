/*TASK RP.JDF*/
using System;
//                                                          //AUTHOR: Towa (JLBD - Luis Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierre).
//                                                          //DATE: November 30, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Cusjson2CustomerJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intContactId { get; set; }
        public String strFullName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Cusjson2CustomerJson2(
            int intContactId_I,
            String strFullName_I
            )
        {
            this.intContactId = intContactId_I;
            this.strFullName = strFullName_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
