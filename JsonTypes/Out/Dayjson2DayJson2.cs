/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 22, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Dayjson2DayJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strDay { get; set; }
        public String strDate { get; set; }
        public PerortaskjsonPeriodOrTaskJson[] arrperortask { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Dayjson2DayJson2(
            String strDay_I,
            String strDate_I,
            PerortaskjsonPeriodOrTaskJson[] arrperortaskjson_I
            )
        {
            this.strDay = strDay_I;
            this.strDate = strDate_I;
            this.arrperortask = arrperortaskjson_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
