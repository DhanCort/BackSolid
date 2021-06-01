/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa(DTC-Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: October 09, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class DynLkjsonDynamicLinkJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int? intnPiwI { get; set; }
        public int? intnPkEleetOrEleeleI { get; set; }
        public bool boolIsEleetI { get; set; }
        public String strLinkI { get; set; }
        public int? intnPiwO { get; set; }
        public int? intnPkEleetOrEleeleO { get; set; }
        public bool boolIsEleetO { get; set; }
        public String strLinkO { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public DynLkjsonDynamicLinkJson(
         int? intnPiwI_I, 
         int? intnPkEleetOrEleeleI_I, 
         bool boolIsEleetI_I,
         String strLinkI_I,
         int? intnPiwO_I,
         int? intnPkEleetOrEleeleO_I, 
         bool boolIsEleetO_I,
         String strLinkO_I
            )
        {
            this.intnPiwI = intnPiwI_I;
            this.intnPkEleetOrEleeleI = intnPkEleetOrEleeleI_I;
            this.boolIsEleetI = boolIsEleetI_I;
            this.strLinkI = strLinkI_I;
            this.intnPiwO = intnPiwO_I;
            this.intnPkEleetOrEleeleO = intnPkEleetOrEleeleO_I;
            this.boolIsEleetO = boolIsEleetO_I;
            this.strLinkO = strLinkO_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/

