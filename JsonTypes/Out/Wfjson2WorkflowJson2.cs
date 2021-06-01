/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: May 14, 2020. 

namespace Odyssey2Backend.JsonTypes.Out
{
    //=================================================================================================================  
    public class Wfjson2WorkflowJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkWorkflow { get; set; }
        public String strName { get; set; }
        public bool boolIsDefault { get; set; }
        public bool boolUsed { get; set; }
        public String strWarningMessage { get; set; }
        public bool boolStillValid { get; set; }
        public bool boolNewVersion { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Wfjson2WorkflowJson2(
            int intPkWorkflow_I,
            String strName_I,
            bool boolIsDefault_I,
            bool boolUsed_I,
            String strWarningMessage_I,
            bool boolStillValid_I,
            bool boolNewVersion_I
            )
        {
            this.intPkWorkflow = intPkWorkflow_I;
            this.strName = strName_I;
            this.boolIsDefault = boolIsDefault_I;
            this.boolUsed = boolUsed_I;
            this.strWarningMessage = strWarningMessage_I;
            this.boolStillValid = boolStillValid_I;
            this.boolNewVersion = boolNewVersion_I;
        }
        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

