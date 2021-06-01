/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 19, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class InhedatajsonInheritanceDataJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int? intnPkTemplate { get; set; }
        public bool boolIsDeviceToolOrCustom { get; set; }
        public UnitinhejsonUnitInheritanceJson unitinhe { get; set; }
        public CostinhejsonCostInheritanceJson costinhe { get; set; }
        public AvainhejsonAvailabilityInheritanceJson avainhe { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public InhedatajsonInheritanceDataJson(
            int? intnPkTemplate_I,
            bool boolIsDeviceToolOrCustom_I,
            UnitinhejsonUnitInheritanceJson unitinhejson_I,
            CostinhejsonCostInheritanceJson costinhejson_I,
            AvainhejsonAvailabilityInheritanceJson avainhejson_I
            )
        {
            this.intnPkTemplate = intnPkTemplate_I;
            this.boolIsDeviceToolOrCustom = boolIsDeviceToolOrCustom_I;
            this.unitinhe = unitinhejson_I;
            this.costinhe = costinhejson_I;
            this.avainhe = avainhejson_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
