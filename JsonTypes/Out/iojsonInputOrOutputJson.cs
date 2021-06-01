/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: February 28, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class IojsonInputOrOutputJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkType { get; set; }
        public int? intnPkTemplate { get; set; }
        public int intPkEleetOrEleele { get; set; }
        public bool boolIsEleet { get; set; }
        public String strTypeAndTemplate { get; set; }
        public int? intnPkResource { get; set; }
        public String strResource { get; set; }
        public String strLink { get; set; }
        public bool boolIsPhysical { get; set; }
        public bool boolIsDeviceToolOrCustom { get; set; }
        public bool boolIsCustom { get; set; }
        public bool? boolnIsFinalProduct { get; set; }
        public bool boolSize { get; set; }
        public bool boolComponent { get; set; }
        public bool boolThickness { get; set; }
        public bool boolMedia { get; set; }
        public bool boolIsPaper { get; set; }
        public bool? boolnIsDeviceOrMiscConsumable { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public IojsonInputOrOutputJson(
            int intPkType_I,
            int? intnPkTemplate_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            String strTypeAndTemplate_I,
            int? intnPkResource_I,
            String strResource_I,
            String strLink_I,
            bool boolIsPhysical_I,
            bool boolIsDeviceToolOrCustom_I,
            bool boolIsCustom_I,
            bool? boolnIsFinalProduct_I,
            bool boolSize_I,
            bool boolComponent_I,
            bool boolThickness_I,
            bool boolMedia_I,
            bool boolIsPaper_I,
            bool? boolnIsDeviceOrMiscConsumable_I
            )
        {
            this.intPkType = intPkType_I;
            this.intnPkTemplate = intnPkTemplate_I;
            this.intPkEleetOrEleele = intPkEleetOrEleele_I;
            this.boolIsEleet = boolIsEleet_I;
            this.strTypeAndTemplate = strTypeAndTemplate_I;
            this.intnPkResource = intnPkResource_I;
            this.strResource = strResource_I;
            this.strLink = strLink_I;
            this.boolIsPhysical = boolIsPhysical_I;
            this.boolIsDeviceToolOrCustom = boolIsDeviceToolOrCustom_I;
            this.boolIsCustom = boolIsCustom_I;
            this.boolnIsFinalProduct = boolnIsFinalProduct_I;
            this.boolSize = boolSize_I;
            this.boolComponent = boolComponent_I;
            this.boolThickness = boolThickness_I;
            this.boolMedia = boolMedia_I;
            this.boolIsPaper = boolIsPaper_I;
            this.boolnIsDeviceOrMiscConsumable = boolnIsDeviceOrMiscConsumable_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
