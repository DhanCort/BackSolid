/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (DTC-Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: October 01, 2020.  

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PronodjsonProcessNodesJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int? intnPkProcessInWorkflow { get; set; }
        public int? intnPkNode { get; set; }
        public String strName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PronodjsonProcessNodesJson(
            int? intnPkProcessInWorkflow_I,
            int? intnPkNode_I,
            String strName_I
            )
        {
            this.intnPkProcessInWorkflow = intnPkProcessInWorkflow_I;
            this.intnPkNode = intnPkNode_I;
            this.strName = strName_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
