/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CLGA. Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF, Liliana Gutierrez).
//                                                          //DATE: Jun 28, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ResdisjsonResourceIsDispensableJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkWorkflow { get; set; }
        public bool boolResourceIsDispensable { get; set; }
        public String strJobs { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ResdisjsonResourceIsDispensableJson(
            int intPkWorkflow_I,
            bool boolResourceIsDispensable_I, 
            String strJobs_I
            )
        {
            this.intPkWorkflow = intPkWorkflow_I;
            this.boolResourceIsDispensable = boolResourceIsDispensable_I;
            this.strJobs = strJobs_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
