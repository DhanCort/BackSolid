/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 17, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class WnrjsonWorkflowNotReadyJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strTips { get; set; }
        public String [] arrstrProcessName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public WnrjsonWorkflowNotReadyJson(
            String strTips_I,
            String [] arrstrProcessName_I
            )
        {
            this.strTips = strTips_I;
            this.arrstrProcessName = arrstrProcessName_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
