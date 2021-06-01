/*TASK RP.XJDF*/
using Odyssey2Backend.Utilities;
using System;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 10, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Perisaddablejson2PeriodIsAddableJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public bool boolIsAddableAboutPeriods { get; set; }
        public bool boolIsAddableAboutRules { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Perisaddablejson2PeriodIsAddableJson2(
            bool boolIsAddableAboutPeriods_I,
            bool boolIsAddableAboutRules_I
            )
        {
            this.boolIsAddableAboutPeriods = boolIsAddableAboutPeriods_I;
            this.boolIsAddableAboutRules = boolIsAddableAboutRules_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
