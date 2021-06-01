/*TASK RP.JSON*/
using System;
using System.Collections.Generic;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: September 30, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class PiwjsoninProcessInWorkflowJsonInternal
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public int intPkWorkflow { get; set; }
        public List<IojsoninInputOrOutputJsonInternal> darriojsoninInputs { get; set; }
        public List<IojsoninInputOrOutputJsonInternal> darriojsoninOutputs { get; set; }
        public bool boolIsPostProcess { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public PiwjsoninProcessInWorkflowJsonInternal(
            int intPk_I,
            int intPkWorkflow_I,
            bool boolIsPostProcess_I
            )
        {
            this.intPk = intPk_I;
            this.intPkWorkflow = intPkWorkflow_I;
            this.darriojsoninInputs = new List<IojsoninInputOrOutputJsonInternal>();
            this.darriojsoninOutputs = new List<IojsoninInputOrOutputJsonInternal>();
            this.boolIsPostProcess = boolIsPostProcess_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
