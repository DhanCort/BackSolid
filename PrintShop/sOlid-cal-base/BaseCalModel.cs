using Odyssey2Backend.JsonTypes;
using Odyssey2Backend.PrintShop.sOlid_Cal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odyssey2Backend.PrintShop.sOlid_cal_base
{
    public class BaseCalModel : ICalculation
    {
        public string strUnit { get; set; } = null;
        public double? numnQuantity { get; set; } = null;
        public double? numnCost { get; set; } 
        public double? numnBlock { get; set; } = null;
        public bool boolIsEnable { get; set; }
        public string strValue { get; set; } 
        public string strAscendantElements { get; set; } 
        public string strDescription { get; set; }
        public double? numnProfit { get; set; } = null;
        public string strCalculationType { get; set; } = CalCalculation.strBase;
        public GpcondjsonGroupConditionJson gpcondCondition { get; set; }
        public int? intnPkProduct { get; set; }
        public int? intnPkProcess { get; set; }
        public int? intnPkResourceI { get; set; } = null;
        public int? intnPkProcessInWorkflow { get; set; }
        public int? intnPkEleetOrEleeleI { get; set; } 
        public bool? boolnIsEleetI { get; set; } 
        public double? numnNeeded { get; set; } = null;
        public double? numnPerUnits { get; set; } = null;
        public string strByX { get; set; }
        public double? numnMin { get; set; } = null;
        public double? numnQuantityWaste { get; set; } = null;
        public double? numnPercentWaste { get; set; } = null;
        public int? intnPkEleetOrEleeleO { get; set; } = null;
        public bool? boolnIsEleetO { get; set; } = null;
        public int? intnPkResourceO { get; set; } = null;
        public int? intnPkAccount { get; set; } 
        public bool? boolnFromThickness { get; set; } = null;
        public bool? boolnIsBlock { get; set; } = null;
        public bool? boolnByArea { get; set; }
        public bool? boolnWorkflowIsBase { get; set; }
        public IAddCal addBaseCal { get; set; } = new AddBaseCal();
    }
}
