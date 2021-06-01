/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: June 25, 2020.  

namespace Odyssey2Backend.JsonTemplates.Out
{
    //==================================================================================================================
    public class PerfrmpiwjsonPeriodFromPIWJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkProcessInWorkflow { get; set; }
        public String strName { get; set; }
        public List<ProperjsonProcessPeriodJson> arrproper { get; set; }
        public List<ResperjsonResourcePeriodJson> arrresper { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PerfrmpiwjsonPeriodFromPIWJson(
            int intPkProcessInWorkflow_I,
            String strName_I,
            List<ProperjsonProcessPeriodJson> arrproper_I,
            List<ResperjsonResourcePeriodJson> arrresper_I
            )
        {
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.strName = strName_I;
            this.arrproper = arrproper_I;
            this.arrresper = arrresper_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

