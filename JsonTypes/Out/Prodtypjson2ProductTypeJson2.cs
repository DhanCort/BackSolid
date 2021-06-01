/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (VSTD - Victor Torres).
//                                                          //CO-AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //DATE: November 28, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================
    public class Prodtypjson2ProductTypeJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strTypeId { get; set; }
        public String strXJDFType { get; set; }
        public String strCategory { get; set; }
        public int? intnPkAccount { get; set; }
        public String strGuidedLink { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Prodtypjson2ProductTypeJson2(
            int intPk_I,
            String strTypeId_I,
            String strXJDFType_I,
            String strCategory_I,
            int? intnPkAccount_I,
            String strGuidedLink_I
            )
        {
            this.intPk = intPk_I;
            this.strTypeId = strTypeId_I;
            this.strXJDFType = strXJDFType_I;
            this.strCategory = strCategory_I;
            this.intnPkAccount = intnPkAccount_I;
            this.strGuidedLink = strGuidedLink_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
