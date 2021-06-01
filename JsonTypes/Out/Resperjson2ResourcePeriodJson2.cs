/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: Febraury 18, 2021.  

namespace Odyssey2Backend.JsonTemplates.Out
{
    //==================================================================================================================
    public class Resperjson2ResourcePeriodJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strResource { get; set; }
        public int intPkResource { get; set; }
        public int? intnPkEleetOrEleele { get; set; }
        public bool boolIsEleet { get; set; }
        public Resperjson3ResourcePeriodJson3[] arrresper { get; set; }


        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Resperjson2ResourcePeriodJson2(
            String strResource_I,
            int intPkResource_I,
            int? intnPkEleetOrEleele_I,
            bool boolIsEleet_I,
            Resperjson3ResourcePeriodJson3[] arrresper_I
            )
        {
            this.strResource = strResource_I;
            this.intPkResource = intPkResource_I;
            this.intnPkEleetOrEleele = intnPkEleetOrEleele_I;
            this.boolIsEleet = boolIsEleet_I;
            this.arrresper = arrresper_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/


