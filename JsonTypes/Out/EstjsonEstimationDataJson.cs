/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: July 3rd, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class EstjsonEstimationDataJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public int intJobId { get; set; }
        public int intEstimationId { get; set; }
        public int intPkProcessInWorkflow { get; set; }
        public int intPkEleetOrEleele { get; set; }
        public bool boolIsEleet { get; set; }
        public int intPkResource { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public EstjsonEstimationDataJson(
            int intJobId_I,
            int intEstimationId_I,
            int intPkProcessInWorkflow_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            int intPkResource_I
            )
        {
            this.intJobId = intJobId_I;
            this.intEstimationId = intEstimationId_I;
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.intPkEleetOrEleele = intPkEleetOrEleele_I;
            this.boolIsEleet = boolIsEleet_I;
            this.intPkResource = intPkResource_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
