/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 28, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class DayjsonDayJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strDay { get; set; }
        public String strDate { get; set; }
        public PorjsonPeriodOrRuleJson[] arrperPeriods { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public DayjsonDayJson(
            String strDay_I,
            String strDate_I,
            PorjsonPeriodOrRuleJson[] arrperPeriods_I
            )
        {
            this.strDay = strDay_I;
            this.strDate = strDate_I;
            this.arrperPeriods = arrperPeriods_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
