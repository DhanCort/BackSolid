/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: February 24, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ResortemjsonResourceOrTemplateJsonsourceJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strName { get; set; }
        public String strUnit { get; set; }
        public bool boolIsType { get; set; }
        public bool? boolnIsCalendar { get; set; }
        public bool? boolnIsAvailable { get; set; }
        public bool? boolnCalendarIsChangeable { get; set; }
        public bool? boolnCostIsChangeable { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ResortemjsonResourceOrTemplateJsonsourceJson(
            int intPk_I,
            String strName_I,
            String strUnit_I,
            bool boolIsType_I,
            bool? boolnIsCalendar_I,
            bool? boolnIsAvailable_I,
            bool? boolnCalendarIsChangeable_I,
            bool? boolnCostIsChangeable_I
            )
        {
            this.intPk = intPk_I;
            this.strName = strName_I;
            this.strUnit = strUnit_I;
            this.boolIsType = boolIsType_I;
            this.boolnIsCalendar = boolnIsCalendar_I;
            this.boolnIsAvailable = boolnIsAvailable_I;
            this.boolnCalendarIsChangeable = boolnCalendarIsChangeable_I;
            this.boolnCostIsChangeable = boolnCostIsChangeable_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
