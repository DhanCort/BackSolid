/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF -Liliana Gutierrez).
//                                                          //DATE: August 03, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class IofrmpiwjsonIOFromPIWJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int? intnPkEleetOrEleele { get; set; }
        public bool boolIsEleet { get; set; }
        public int? intnPkResource { get; set; }
        public String strTypeTemplateAndResource { get; set; }
        public String strUnit { get; set; }
        public bool boolIsInput { get; set; }
        public bool boolIsComponet { get; set; }
        public bool boolIsMedia { get; set; }
        public bool boolIsRoll { get; set; }
        public bool boolSize { get; set; }
        public String strAreaUnit { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public IofrmpiwjsonIOFromPIWJson(
            int? intnPkEleetOrEleele_I,
            bool boolIsEleet_I,
            int? intnPkResource_I, 
            String strTypeTemplateAndResource_I,
            String strUnit_I,
            bool boolIsInput_I,
            bool boolIsComponent_I,
            bool boolIsMedia_I,
            bool boolIsRoll_I,
            bool boolSize_I,
            String strAreaUnit_I
            )
        {
            this.intnPkEleetOrEleele = intnPkEleetOrEleele_I;
            this.boolIsEleet = boolIsEleet_I;
            this.intnPkResource = intnPkResource_I;
            this.strTypeTemplateAndResource = strTypeTemplateAndResource_I;
            this.strUnit = strUnit_I;
            this.boolIsInput = boolIsInput_I;
            this.boolIsComponet = boolIsComponent_I;
            this.boolIsMedia = boolIsMedia_I;
            this.boolIsRoll = boolIsRoll_I;
            this.boolSize = boolSize_I;
            this.strAreaUnit = strAreaUnit_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
