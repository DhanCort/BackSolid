/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (JLBD - Luis Basurto).
//                                                          //CO-AUTHOR: Towa (LGF -Liliana Gutierrez).
//                                                          //DATE: March 03, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Iofrmpiwjson2IOFromPIWJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkEleetOrEleele { get; set; }
        public int intPkResource { get; set; }
        public bool boolIsEleet { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Iofrmpiwjson2IOFromPIWJson2(
            int intPkEleetOrEleele_I,
            int intPkResource_I,
            bool boolIsEleet_I
            )
        {
            this.intPkEleetOrEleele = intPkEleetOrEleele_I;
            this.intPkResource = intPkResource_I;
            this.boolIsEleet = boolIsEleet_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

