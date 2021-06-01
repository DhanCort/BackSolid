/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: July 20, 2020.  

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class Piwjson2ProcessInWorkflowJson2
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkProcessInWorkflow { get; set; }
        public RecbdgjsonResourceBudgetJson[] arrrecbdgjsonInput { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public Piwjson2ProcessInWorkflowJson2(
            int intPkProcessInWorkflow_I,
            RecbdgjsonResourceBudgetJson[] arrrecbdgjsonInput_I
            )
        {
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.arrrecbdgjsonInput = arrrecbdgjsonInput_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
