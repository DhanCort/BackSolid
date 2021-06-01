/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 5, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Resjson1ResourceJson1
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkType { get; set; }
        public String strTypeName { get; set; }
        public int? intnPkInherited { get; set; }
        public String strInheritedName { get; set; }
        public int intPkResource { get; set; }
        public String strResourceName { get; set; }
        public String strUnit { get; set; }
        public bool boolIsTemplate { get; set; }
        public Attrjson4AttributeJson4[] arrattr { get; set; }
        public bool boolIsPhysical { get; set; }
        public bool boolnIsChangeable { get; set; }
        public UnitinhejsonUnitInheritanceJson unitinhe { get; set; }
        public CostinhejsonCostInheritanceJson costinhe { get; set; }
        public AvainhejsonAvailabilityInheritanceJson avainhe { get; set; }
        public bool? boolnIsDecimal { get; set; }



        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Resjson1ResourceJson1(
            int intPkType_I,
            String strTypeName_I,
            int? intnPkInherited_I,
            String strInheritedName_I,
            int intPkResource_I,
            String strResourceName_I,
            String strUnit_I,
            bool boolIsTemplate_I,
            Attrjson4AttributeJson4[] arrattr_I,
            bool boolIsPhysical_I,
            bool boolIsChangeable_I, 
            UnitinhejsonUnitInheritanceJson unitinhe_I,
            CostinhejsonCostInheritanceJson costinhe_I,
            AvainhejsonAvailabilityInheritanceJson avainhe_I,
            bool? boolnIsDecimal_I
            )
        {
            this.intPkType = intPkType_I;
            this.strTypeName = strTypeName_I;
            this.intnPkInherited = intnPkInherited_I;
            this.strInheritedName = strInheritedName_I;
            this.intPkResource = intPkResource_I;
            this.strResourceName = strResourceName_I;
            this.strUnit = strUnit_I;
            this.boolIsTemplate = boolIsTemplate_I;
            this.arrattr = arrattr_I;
            this.boolIsPhysical = boolIsPhysical_I;
            this.boolnIsChangeable = boolIsChangeable_I;
            this.unitinhe = unitinhe_I;
            this.costinhe = costinhe_I;
            this.avainhe = avainhe_I;
            this.boolnIsDecimal = boolnIsDecimal_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
