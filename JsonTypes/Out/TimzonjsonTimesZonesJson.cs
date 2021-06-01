/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: August 03, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class TimzonjsonTimesZonesJson

    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strTimeZoneId { get; set; }
        public String strTimeZone { get; set; }
        public bool boolSelected { get; set; }


        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public TimzonjsonTimesZonesJson
            (
            String strTimeZoneId_I,
            String strTimeZoneDescription_I,
            bool boolSelected_I
            )
        {
            this.strTimeZoneId = strTimeZoneId_I;
            this.strTimeZone = strTimeZoneDescription_I;
            this.boolSelected = boolSelected_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
