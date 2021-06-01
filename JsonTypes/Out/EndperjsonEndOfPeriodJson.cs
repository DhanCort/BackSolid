/*TASK RP.XJDF*/
using Odyssey2Backend.Utilities;
using System;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 11, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class EndperjsonEndOfPeriodJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strEndDate { get; set; }
        public String strEndTime { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public EndperjsonEndOfPeriodJson(
            String strEndDate_I,
            String strEndTime_I
            )
        {
            this.strEndDate = strEndDate_I;
            this.strEndTime = strEndTime_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
