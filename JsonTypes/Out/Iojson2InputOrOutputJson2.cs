/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (JLBD - Luis Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: October 16, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class Iojson2InputOrOutputJson2
    {
        //--------------------------------------------------------------------------------------------------------------
        // 
        public int intPkEleetOrEleele { get; set; }
        public bool boolIsEleet { get; set; }
        public int? intnPkResource { get; set; }
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public Iojson2InputOrOutputJson2(
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            int? intnPkResource_I
            )
        {
            this.intPkEleetOrEleele = intPkEleetOrEleele_I;
            this.boolIsEleet = boolIsEleet_I;
            this.intnPkResource = intnPkResource_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
