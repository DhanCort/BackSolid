using Odyssey2Backend.JsonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odyssey2Backend.PrintShop.sOlid_Cal
{
    public interface ICalculation
    {
        String strUnit { get; set; }
        double? numnQuantity { get; set; }
        double? numnCost  { get; set; }
        double? numnBlock { get; set; }
        bool boolIsEnable { get; set; }
        String strValue { get; set; }
        String strAscendantElements { get; set; }
        String strDescription { get; set; }
        double? numnProfit { get; set; }
        String strCalculationType { get; set; }
        GpcondjsonGroupConditionJson gpcondCondition { get; set; }
        int? intnPkProduct { get; set; }
        int? intnPkProcess { get; set; }
        int? intnPkResourceI { get; set; }
        int? intnPkProcessInWorkflow { get; set; }
        int? intnPkEleetOrEleeleI { get; set; }
        bool? boolnIsEleetI { get; set; }
        double? numnNeeded { get; set; }
        double? numnPerUnits { get; set; }
        String strByX { get; set; }
        double? numnMin { get; set; }
        double? numnQuantityWaste { get; set; }
        double? numnPercentWaste { get; set; }
        int? intnPkEleetOrEleeleO { get; set; }
        bool? boolnIsEleetO { get; set; }
        int? intnPkResourceO { get; set; }
        int? intnPkAccount { get; set; }
        bool? boolnFromThickness { get; set; }
        bool? boolnIsBlock { get; set; }
        bool? boolnByArea { get; set; }
        bool? boolnWorkflowIsBase { get; set; }
    }
}
