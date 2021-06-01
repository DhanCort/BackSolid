/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: May 25, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class WfandlinkjsonWorkflowPkAndLinkJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkWorkflow { get; set; }
        public String strLink { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public WfandlinkjsonWorkflowPkAndLinkJson(
            int intPkWorkflow_I,
            String strLink_I
            )
        {
            this.intPkWorkflow = intPkWorkflow_I;
            this.strLink = strLink_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
