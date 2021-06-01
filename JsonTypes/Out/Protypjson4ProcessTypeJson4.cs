/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 17, 2020.  

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class Piwjson1ProcessInWorkflowJson1
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public int intPkProcessInWorkflow { get; set; }
        public int intPkProcess { get; set; }
        public String strName { get; set; }
        public double numCostByProcess { get; set; }
        public Iojson1InputOrOutputJson1[] arrresortypInput { get; set; }
        public Iojson1InputOrOutputJson1[] arrresortypOutput { get; set; }
        public int intStage { get; set; }
        public bool? boolnCanStartProcess { get; set; }
        public bool? boolnCanBeCompleted { get; set; }
        public bool boolIsPostProcess { get; set; }
        public int intHours { get; set; }
        public int intMinutes { get; set; }
        public int intSeconds { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public Piwjson1ProcessInWorkflowJson1(
            int intPkProcessInWorkflow_I,
            int intPkProcess_I,
            String strName_I,
            double numCostByProcess_I,
            Iojson1InputOrOutputJson1[] arrresortypInput_I,
            Iojson1InputOrOutputJson1[] arrresortypOutput_I,
            int intStage_I,
            bool? boolnCanStartProcess_I,
            bool? boolnCanBeCompleted_I,
            bool boolIsPostProcess
            )
        {
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.intPkProcess = intPkProcess_I;
            this.strName = strName_I;
            this.numCostByProcess = numCostByProcess_I;
            this.arrresortypInput = arrresortypInput_I;
            this.arrresortypOutput = arrresortypOutput_I;
            this.intStage = intStage_I;
            this.boolnCanStartProcess = boolnCanStartProcess_I;
            this.boolnCanBeCompleted = boolnCanBeCompleted_I;
            this.boolIsPostProcess = boolIsPostProcess;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
