/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 17, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class WfjsonWorkflowJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public int intPkWorkflow { get; set; }
        public PiwjsonProcessInWorkflowJson[] arrpro { get; set; }
        public bool boolIsReady { get; set; }
        public String strWorkflowName { get; set; }
        public bool boolHasFinalProduct { get; set; }
        public int intPkProduct { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public WfjsonWorkflowJson(
            int intPkWorkflow_I,
            PiwjsonProcessInWorkflowJson[] arrpro_I,
            bool boolIsReady_I,
            String strWorkflowName_I,
            bool boolHasFinalProduct_I,
            int intPkProduct_I
            )
        {
            this.intPkWorkflow = intPkWorkflow_I;
            this.arrpro = arrpro_I;
            this.boolIsReady = boolIsReady_I;
            this.strWorkflowName = strWorkflowName_I;
            this.boolHasFinalProduct = boolHasFinalProduct_I;
            this.intPkProduct = intPkProduct_I;
        }

            //-------------------------------------------------------------------------------------------------------------
        }

    //==================================================================================================================
}
/*END-TASK*/
