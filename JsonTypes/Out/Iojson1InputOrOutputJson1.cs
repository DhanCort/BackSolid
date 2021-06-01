/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 17, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class Iojson1InputOrOutputJson1
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public int intPkType { get; set; }
        public int? intnPkTemplate { get; set; }
        public int intPkEleetOrEleele { get; set; }
        public bool boolIsEleet { get; set; }
        public String strTypeAndTemplate { get; set; }
        public String strResource { get; set; }
        public String strLink { get; set; }
        public int? intnPkResource { get; set; }
        public double numQuantity { get; set; }
        public String strUnit { get; set; }
        public double numCostByResource { get; set; }
        public bool boolIsPhysical { get; set; }
        public bool? boolnIsCalendar { get; set; }
        public bool? boolnIsAvailable { get; set; }
        public int intHours { get; set; }
        public int intMinutes { get; set; }
        public int intSeconds { get; set; }
        public bool boolAutomaticallySet { get; set; }
        public bool boolIsOneResource { get; set; }
        public bool boolHasNotResource { get; set; }
        public bool boolIsCustom { get; set; }
        public bool boolIsPaper { get; set; }
        public bool? boolnIsFinalProduct { get; set; }
        public bool boolUnitAllowDecimal { get; set; }
        public WstjsonWasteJson[] arrwstWaste { get; set;}
        public WstaddjsonWasteAdditionalJson[]  arrwstaddWasteAdditional { get; set; }
        public String strDimensions { get; set; }
        public bool? boolnIsDeviceOrMiscConsumable { get; set; }
        public bool? boolnSize { get; set; }
        public bool boolThickness { get; set; }
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public Iojson1InputOrOutputJson1(
            int intPkType_I,
            int? intnPkTemplate_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            String strTypeAndTemplate_I,
            String strResource_I,
            String strLink_I,
            int? intnPkResource_I,
            double numQuantity_I,
            String strUnit_I,
            double numCostByResource_I,
            bool boolIsPhysical_I,
            bool? boolnIsCalendar_I,
            bool? boolnIsAvailable_I,
            int intHours_I,
            int intMinutes_I,
            int intSeconds_I,
            bool boolAutomaticallySet_I,
            bool boolIsOneResource_I,
            bool boolHasNotResource_I,
            bool boolIsCustom_I,
            bool boolIsPaper_I,
            bool? boolnIsFinalProduct_I,
            bool boolUnitAllowDecimal_I,
            WstjsonWasteJson[] arrwstWaste_I,
            WstaddjsonWasteAdditionalJson[] arrwstaddWasteAdditional_I,
            String strDimensions_I,
            bool? boolnIsDeviceOrMiscConsumable_I,
            bool? boolnSize_I,
            bool boolThickness_I

            )
        {
            this.intPkType = intPkType_I;
            this.intnPkTemplate = intnPkTemplate_I;
            this.intPkEleetOrEleele = intPkEleetOrEleele_I;
            this.boolIsEleet = boolIsEleet_I;
            this.strTypeAndTemplate = strTypeAndTemplate_I;
            this.strResource = strResource_I;
            this.strLink = strLink_I;
            this.intnPkResource = intnPkResource_I;
            this.numQuantity = numQuantity_I;
            this.strUnit = strUnit_I;
            this.numCostByResource = numCostByResource_I;
            this.boolIsPhysical = boolIsPhysical_I;
            this.boolnIsCalendar = boolnIsCalendar_I;
            this.boolnIsAvailable = boolnIsAvailable_I;
            this.intHours = intHours_I;
            this.intMinutes = intMinutes_I;
            this.intSeconds = intSeconds_I;
            this.boolAutomaticallySet = boolAutomaticallySet_I;
            this.boolIsOneResource = boolIsOneResource_I;
            this.boolHasNotResource = boolHasNotResource_I;
            this.boolIsCustom = boolIsCustom_I;
            this.boolIsPaper = boolIsPaper_I;
            this.boolnIsFinalProduct = boolnIsFinalProduct_I;
            this.boolUnitAllowDecimal = boolUnitAllowDecimal_I;
            this.arrwstWaste = arrwstWaste_I;
            this.arrwstaddWasteAdditional = arrwstaddWasteAdditional_I;
            this.strDimensions = strDimensions_I;
            this.boolnIsDeviceOrMiscConsumable = boolnIsDeviceOrMiscConsumable_I;
            this.boolnSize = boolnSize_I;
            this.boolThickness = boolThickness_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
