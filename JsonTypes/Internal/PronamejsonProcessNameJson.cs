/*TASK RP.JSON*/
using System;
using System.Collections.Generic;
using TowaStandard;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: March 03, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class PronamejsonProcessNameJson
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intProcessInWorkflowId { get; set; }
        public int? intnId { get; set; }
        public String strProcessName { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public PronamejsonProcessNameJson(
            int intProcessInWorkflowId_I,
            int? intnId_I,
            String strProcessName_I
            )
        {
            this.intProcessInWorkflowId = intProcessInWorkflowId_I;
            this.intnId = intnId_I;
            this.strProcessName = strProcessName_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
