/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (CLGA - Cesar garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: July 3, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ProcbdgjsonProcessBudgetJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkProcess { get; set; }
        public int intPkProcessInWorkflow { get; set; }
        public String strName { get; set; }
        public List<CostbycaljsonCostByCalculationJson> arrcal { get; set; }
        public List<RecbdgjsonResourceBudgetJson> arrres { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ProcbdgjsonProcessBudgetJson(
            int intPkProcess_I,
            int intPkProcessInWorkflow_I,
            String strName_I,
            List<CostbycaljsonCostByCalculationJson> arrcal_I,
            List<RecbdgjsonResourceBudgetJson> arrresbdgjson_I
            )
        {
            this.intPkProcess = intPkProcess_I;
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.strName = strName_I; 
            this.arrcal = arrcal_I;
            this.arrres = arrresbdgjson_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
