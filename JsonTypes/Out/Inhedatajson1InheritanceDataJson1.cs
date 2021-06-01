/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 26, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Inhedatajson1InheritanceDataJson1
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public UnitinhejsonUnitInheritanceJson unitinhe { get; set; }
        public CostinhejsonCostInheritanceJson costinhe { get; set; }
        public AvainhejsonAvailabilityInheritanceJson avainhe { get; set; }
        public Attrjson3AttributeJson3 attr { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Inhedatajson1InheritanceDataJson1(
            UnitinhejsonUnitInheritanceJson unitinhejson_I,
            CostinhejsonCostInheritanceJson costinhejson_I,
            AvainhejsonAvailabilityInheritanceJson avainhejson_I,
            Attrjson3AttributeJson3 attr_I
            )
        {
            this.unitinhe = unitinhejson_I;
            this.costinhe = costinhejson_I;
            this.avainhe = avainhejson_I;
            this.attr = attr_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
