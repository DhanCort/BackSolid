/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 17, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Wfjson3WorkflowJson3
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public bool boolIsReady { get; set; }
        public String strWorkflowName { get; set; }
        public bool boolHasFinalProduct { get; set; }
        public bool boolHasSize { get; set; }
        public bool boolGeneric { get; set; }
        public LkornodjsonLinkOrNodeJson[] arrlkornodjson { get; set; }
        
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Wfjson3WorkflowJson3(
            int intPk_I, 
            bool boolIsReady_I,
            String strWorkflowName_I,
            bool boolHasFinalProduct_I,
            bool boolHasSize_I,
            bool boolGeneric_I,
            LkornodjsonLinkOrNodeJson[] arrlkornodjson_I
            )
        {
            this.intPk = intPk_I;
            this.boolIsReady = boolIsReady_I;
            this.strWorkflowName = strWorkflowName_I;
            this.boolHasFinalProduct = boolHasFinalProduct_I;
            this.boolHasSize = boolHasSize_I;
            this.boolGeneric = boolGeneric_I;
            this.arrlkornodjson = arrlkornodjson_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
