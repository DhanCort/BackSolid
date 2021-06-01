/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: Augost 14, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class LvlpiwjsonLevelAndPIWJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public List<LeveljsonLevelJson> arrlevels { get; set; }
        public List<Piwjson3ProcessInWorkflowJson3> arrpiw { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public LvlpiwjsonLevelAndPIWJson(
            List<LeveljsonLevelJson> arrlevels_I,
            List<Piwjson3ProcessInWorkflowJson3> arrpiw_I
            )
        {
            this.arrlevels = arrlevels_I;
            this.arrpiw = arrpiw_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
