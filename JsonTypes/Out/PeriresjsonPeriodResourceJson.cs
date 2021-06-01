/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: June 25, 2020.  

namespace Odyssey2Backend.JsonTemplates.Out
{
    //==================================================================================================================
    public class PeriresjsonPeriodResourceJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkPeriod { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strDateSunday { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PeriresjsonPeriodResourceJson(
            int intPkPeriod_I,
            String strStartDate_I,
            String strStartTime_I,
            String strDateSunday_I
            )
        {
            this.intPkPeriod = intPkPeriod_I;
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strDateSunday = strDateSunday_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

