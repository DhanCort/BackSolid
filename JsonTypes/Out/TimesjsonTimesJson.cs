/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: May 26, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class TimesjsonTimesJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strEndDate { get; set; }
        public String strEndTime { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public TimesjsonTimesJson(
            String strDateStart_I,
            String strTimeStart_I,
            String strDateEnd_I,
            String strTimeEnd_I
            )
        {
            this.strStartDate = strDateStart_I;
            this.strStartTime = strTimeStart_I;
            this.strEndDate = strDateEnd_I;
            this.strEndTime = strTimeEnd_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
