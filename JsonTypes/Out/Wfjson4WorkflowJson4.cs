/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: October, 2020. 

namespace Odyssey2Backend.JsonTypes.Out
{
    //=================================================================================================================  
    public class Wfjson4WorkflowJson4
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkWorkflow { get; set; }
        public int? intnPkProduct { get; set; }
        public String strName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Wfjson4WorkflowJson4(
            int intPkWorkflow_I,
            int? intnPkProduct_I,
            String strName_I
            )
        {
            this.intPkWorkflow = intPkWorkflow_I;
            this.intnPkProduct = intnPkProduct_I;
            this.strName = strName_I;
        }
        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

