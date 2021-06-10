using Odyssey2Backend.JsonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odyssey2Backend.PrintShop.sOlid_Cal
{
    public class ProfitCalModel : ICalculation
    {
        public string strUnit { get; set; } = null;
        public double? numnQuantity { get; set; } = null;
        public double? numnCost { get; set; } = null;
        public double? numnBlock { get; set; } = null;
        public bool boolIsEnable { get; set; }
        public string strValue { get; set; } = null;
        public string strAscendantElements { get; set; } = null;
        public string strDescription { get; set; }
        public double? numnProfit { get; set; }
        public string strCalculationType { get; set; } = CalCalculation.strProfit;
        public GpcondjsonGroupConditionJson gpcondCondition { get; set; } = null;
        public int? intnPkProduct { get; set; }
        public int? intnPkProcess { get; set; } = null;
        public int? intnPkResourceI { get; set; } = null;
        public int? intnPkProcessInWorkflow { get; set; } = null;
        public int? intnPkEleetOrEleeleI { get; set; } = null;
        public bool? boolnIsEleetI { get; set; } = null;
        public double? numnNeeded { get; set; } = null;
        public double? numnPerUnits { get; set; } = null;
        public string strByX { get; set; }
        public double? numnMin { get; set; } = null;
        public double? numnQuantityWaste { get; set; } = null;
        public double? numnPercentWaste { get; set; } = null;
        public int? intnPkEleetOrEleeleO { get; set; } = null;
        public bool? boolnIsEleetO { get; set; } = null;
        public int? intnPkResourceO { get; set; } = null;
        public int? intnPkAccount { get; set; } = null;
        public bool? boolnFromThickness { get; set; } = null;
        public bool? boolnIsBlock { get; set; } = null;
        public bool? boolnByArea { get; set; } = null;
        public IAddCal addProfitCal { get; set; } = new AddProfitCal();

    }
}
