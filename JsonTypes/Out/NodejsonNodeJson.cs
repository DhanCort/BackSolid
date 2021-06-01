/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (JLBD - José Basurto).
//                                                          //CO-AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //DATE: October 01, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class NodejsonNodeJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int? intnPkNode { get; set; }
        public String strName { get; set; }
        public int intPkWorkflowFinal { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public NodejsonNodeJson(
            int? intnPkNode_I,
            String strName_I,
            int intPkWorkflowFinal_I
            )
        {
            this.intnPkNode = intnPkNode_I;
            this.strName = strName_I;
            this.intPkWorkflowFinal = intPkWorkflowFinal_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
