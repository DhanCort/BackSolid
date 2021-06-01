/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 28, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Restyportemjson1ResourceTypeOrTemplateJson1
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkEleetOrEleele { get; set; }
        public String strTypeOrTemplate { get; set; }
        public bool boolIsEleet { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Restyportemjson1ResourceTypeOrTemplateJson1(
            int intEleetOrEleel_I,
            String strTypeOrTemplate_I,
            bool boolIsEleet_I
            )
        {
            this.intPkEleetOrEleele = intEleetOrEleel_I;
            this.strTypeOrTemplate = strTypeOrTemplate_I;
            this.boolIsEleet = boolIsEleet_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
