/*TASK RP.JDF*/
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: December 9, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class CaljsonCalculationJson : IComparable
    {
        public int intPk { get; set; }
        public double? numnQuantity { get; set; }
        public double? numnNeeded { get; set; }
        public double? numnPerUnits { get; set; }
        public double? numnCost { get; set; }
        public int? intnHours { get; set; }
        public int? intnMinutes { get; set; }
        public int? intnSeconds { get; set; }
        public String strValue { get; set; }
        public String[] arrAscendantName { get; set; }
        public int[] arrintAscendantPk { get; set; }
        public double? numnProfit { get; set; }
        public String strDescription { get; set; }
        public double? numnMin { get; set; }
        public double? numnQuantityWaste { get; set; }
        public double? numnPercentWaste { get; set; }
        public double? numnBlock { get; set; }
        public bool boolIsEnable { get; set; }
        public String strConditionToApplyCoded { get; set; }
        public String strBy { get; set; }
        public int? intnPkProduct { get; set; }
        public int? intnPkProcess { get; set; }
        public String strCalculationType { get; set; }
        public int? intnGroupId { get; set; }
        public bool? boolnIsWorkflow { get; set; }
        public String strProcessName { get; set; }
        public int? intnPkResourceI { get; set; }
        public String strUnitI { get; set; }
        public String strTypeTemplateAndResourceO { get; set; }
        public int? intnPkResourceO { get; set; }
        public int? intnPkEleetOrEleeleI { get; set; }
        public bool? boolnIsEleetI { get; set; }
        public int? intnPkEleetOrEleeleO { get; set; }
        public bool? boolnIsEleetO { get; set; }
        public String strUnitO { get; set; }
        public int? intnPkPaTrans { get; set; }
        public int? intnPkProcessInWorkflow { get; set; }
        public String strResourceName { get; set; }
        public String strQtyFromResourceName { get; set; }
        public bool boolIsInPostProcess { get; set; }
        public bool boolIsEditable { get; set; }
        public String strAccountName { get; set; }
        public int? intnPkAccount { get; set; }
        public bool boolFromThickness { get; set; }
        public bool boolIsBlock { get; set; }

        public bool? boolnByArea { get; set; }
        public String strAreaUnitO { get; set; }
        public bool boolHasCondition { get; set; }



        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            CaljsonCalculationJson caljson = (CaljsonCalculationJson)obj_I;

            int intGroup = 0;
            if (
                this.intnGroupId != null
                )
            {
                intGroup = (int)this.intnGroupId;
            }

            int intGroupB = 0;
            if (
                caljson.intnGroupId != null
                )
            {
                intGroupB = (int)caljson.intnGroupId;
            }

            return intGroupB.CompareTo(intGroup);
        }
        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASk*/
