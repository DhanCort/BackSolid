/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: February 28, 2020.  

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PiwjsonProcessInWorkflowJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkProcessInWorkflow { get; set; }
        public int intPkProcess { get; set; }
        public String strName { get; set; }
        public bool boolHasCalculations { get; set; }
        public IojsonInputOrOutputJson[] arrresortypInput { get; set; }
        public IojsonInputOrOutputJson[] arrresortypOutput { get; set; }
        public int intPkProcessType { get; set; }
        public String strNameProcessType { get; set; }
        public bool boolIsNormal { get; set; }
        public bool boolContainsFinalProduct { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PiwjsonProcessInWorkflowJson(
            int intPkProcessInWorkflow_I,
            int intPkProcess_I,
            String strName_I,
            bool boolHasCalculations_I,
            IojsonInputOrOutputJson[] arrresortypInput_I,
            IojsonInputOrOutputJson[] arrresortypOutput_I,
            int intPkProcessType_I,
            String strNameProcessType_I,
            bool boolIsNormal_I,
            bool boolContainsFinalProduct_I
            )
        {
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.intPkProcess = intPkProcess_I;
            this.strName = strName_I;
            this.boolHasCalculations = boolHasCalculations_I;
            this.arrresortypInput = arrresortypInput_I;
            this.arrresortypOutput = arrresortypOutput_I;
            this.intPkProcessType = intPkProcessType_I;
            this.strNameProcessType = strNameProcessType_I;
            this.boolIsNormal = boolIsNormal_I;
            this.boolContainsFinalProduct = boolContainsFinalProduct_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
