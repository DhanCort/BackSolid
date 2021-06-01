/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 15, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class FcsumjsonFinalCostsSummaryJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public double numEstimateCost { get; set; }
        public double numFinalCost { get; set; }
        public double numCostDifference { get; set; }
        public double numEstimatedProfit { get; set; }
        public double numFinalProfit { get; set; }
        public double numProfitDifference { get; set; }
        public double numCostByProduct { get; set; }
        public List<ProfcsumjsonProcessFinalCostSummaryJson> arrpro { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public FcsumjsonFinalCostsSummaryJson(
            double numEstimateCost_I,
            double numFinalCost_I,
            double numCostDifference_I,
            double numEstimateProfit_I,
            double numFinalProfit_I,
            double numProfitDifference_I,
            double numCostByProduct_I,
            List<ProfcsumjsonProcessFinalCostSummaryJson> arrpro_I
            )
        {
            this.numEstimateCost = numEstimateCost_I;
            this.numFinalCost = numFinalCost_I;
            this.numCostDifference = numCostDifference_I;
            this.numEstimatedProfit = numEstimateProfit_I;
            this.numFinalProfit = numFinalProfit_I;
            this.numProfitDifference = numProfitDifference_I;
            this.numCostByProduct = numCostByProduct_I;
            this.arrpro = arrpro_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //=================================================================================================================  
    public class ProfcsumjsonProcessFinalCostSummaryJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkprocessInWorkflow { get; set; }
        public String strProcessName { get; set; }
        public double numEstimateCost { get; set; }
        public double? numFinalCost { get; set; }
        public double? numCostDifference { get; set; }
        public List<CalfcsumjsonCalculationFinalCostSummaryJson> arrcalculationscost { get; set; }
        public List<ResfcsumjsonResourceFinalCostSummaryJson> arrresourcecost { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ProfcsumjsonProcessFinalCostSummaryJson(
            int intPkprocessInWorkflow_I,
            String strProcessName_I,
            double numEstimateCost_I,
            double numFinalCost_I,
            double numCostDifference_I,
            List<CalfcsumjsonCalculationFinalCostSummaryJson> arrcalculationscost_I,
            List<ResfcsumjsonResourceFinalCostSummaryJson> arrresourcecost_I
            )
        {
            this.intPkprocessInWorkflow = intPkprocessInWorkflow_I;
            this.strProcessName = strProcessName_I;
            this.numEstimateCost = numEstimateCost_I;
            this.numFinalCost = numFinalCost_I;
            this.numCostDifference = numCostDifference_I;
            this.arrcalculationscost = arrcalculationscost_I;
            this.arrresourcecost = arrresourcecost_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //=================================================================================================================  
    public class CalfcsumjsonCalculationFinalCostSummaryJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strCalculationName { get; set; }
        public double numEstimateCost { get; set; }
        public double? numFinalCost { get; set; }
        public double? numCostDifference { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public CalfcsumjsonCalculationFinalCostSummaryJson(
            String strCalculationName_I,
            double numEstimateCost_I,
            double numFinalCost_I,
            double numCostDifference_I
            )
        {
            this.strCalculationName = strCalculationName_I;
            this.numEstimateCost = numEstimateCost_I;
            this.numFinalCost = numFinalCost_I;
            this.numCostDifference = numCostDifference_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //=================================================================================================================  
    public class ResfcsumjsonResourceFinalCostSummaryJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strResourceName { get; set; }
        public double numEstimateCost { get; set; }
        public double? numFinalCost { get; set; }
        public double? numCostDifference { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ResfcsumjsonResourceFinalCostSummaryJson(
            String strResourceName_I,
            double numEstimateCost_I,
            double numFinalCost_I,
            double numCostDifference_I
            )
        {
            this.strResourceName = strResourceName_I;
            this.numEstimateCost = numEstimateCost_I;
            this.numFinalCost = numFinalCost_I;
            this.numCostDifference = numCostDifference_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
