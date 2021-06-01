/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //DATE: June 12th, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class FnlcostjsonFinalCostJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkJob { get; set; }
        public int intPkProcessInWorkflow { get; set; }
        public BscostjsonBaseCostJson[] arrcalBase { get; set; }
        public PqcostjsonPerQuantityCostJson[] arrcalPerQuantity { get; set; }
        public PqcostjsonPerQuantityCostJson[] arrcalPerQuantityByResource { get; set; }
        public String strSubStage { get; set; }
        public bool boolJobCompleted { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public FnlcostjsonFinalCostJson(
           int intPkJob_I,
           int intPkProcessInWorkflow_I,
           BscostjsonBaseCostJson[] arrbscostjsonBase_I,
           PqcostjsonPerQuantityCostJson[] arrpqcostjsonPerQuantity_I,
           PqcostjsonPerQuantityCostJson[] arrpqcostjsonPerQuantityByResource_I,
           String strSubStage_I,
           bool boolJobCompleted_I
           )
        {
            this.intPkJob = intPkJob_I;
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.arrcalBase = arrbscostjsonBase_I;
            this.arrcalPerQuantity = arrpqcostjsonPerQuantity_I;
            this.arrcalPerQuantityByResource = arrpqcostjsonPerQuantityByResource_I;
            this.strSubStage = strSubStage_I;
            this.boolJobCompleted = boolJobCompleted_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
