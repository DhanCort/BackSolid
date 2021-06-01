/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 08, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class LeveljsonLevelJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public Rulejson1RuleJson1[] arrrule { get; set; }
        public List<ProjsonProcessJson> arrpro { get; set; }
        public List<Perjson1PeriodJson1> arrper { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public LeveljsonLevelJson(
            Rulejson1RuleJson1[] arrrulejson1_I
            )
        {
            this.arrrule = arrrulejson1_I;
            this.arrpro = new List<ProjsonProcessJson>();
            this.arrper = new List<Perjson1PeriodJson1>();
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
