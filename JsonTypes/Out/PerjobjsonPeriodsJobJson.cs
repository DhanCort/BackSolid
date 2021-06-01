/*TASK RP.XJDF*/
using Odyssey2Backend.JsonTemplates.Out;
using System;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: Febraury 18, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PerjobjsonPeriodsJobJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strEstimateDate { get; set; }
        public Projson3ProcessJson3[] darrpro { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PerjobjsonPeriodsJobJson(
            String strEstimateDate_I,
            Projson3ProcessJson3[] darrpro_I
            )
        {
            this.strEstimateDate = strEstimateDate_I;
            this.darrpro = darrpro_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
