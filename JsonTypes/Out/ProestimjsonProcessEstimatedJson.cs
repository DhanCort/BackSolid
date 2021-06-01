/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: July 02, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ProestimjsonProcessEstimatedJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strName { get; set; }
        public int intPkProcessInWorkflow { get; set; }
        public List<ResestimjsonResourceEstimatedJson> arrres { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ProestimjsonProcessEstimatedJson(
            String strName_I,
            int intPkProcessInWorkflow_I,
            List<ResestimjsonResourceEstimatedJson> darrresestimjson_I
            )
        {
            this.strName = strName_I;
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.arrres = darrresestimjson_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
