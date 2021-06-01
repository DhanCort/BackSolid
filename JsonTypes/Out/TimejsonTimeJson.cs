/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: May 11st, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class TimejsonTimeJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strUnit { get; set; }
        public double numQuantity { get; set; }
        public int intHours { get; set; }
        public int intMinutes { get; set; }
        public int intSeconds { get; set; }
        public double? numnMinThickness { get; set; }
        public double? numnMaxThickness { get; set; }
        public String strThicknessUnit { get; set; }
        public int intPkTime { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public TimejsonTimeJson(
            String strUnit_I,
            double numQuantity_I,
            int intHours_I,
            int intMinutes_I,
            int intSeconds_I,
            double? numnMinThickness_I,
            double? numnMaxThickness_I,
            String strThicknessUnit_I,
            int intPkTime_I
            )
        {
            this.strUnit = strUnit_I;
            this.numQuantity = numQuantity_I;
            this.intHours = intHours_I;
            this.intMinutes = intMinutes_I;
            this.intSeconds = intSeconds_I;
            this.numnMinThickness = numnMinThickness_I;
            this.numnMaxThickness = numnMaxThickness_I;
            this.strThicknessUnit = strThicknessUnit_I;
            this.intPkTime = intPkTime_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
