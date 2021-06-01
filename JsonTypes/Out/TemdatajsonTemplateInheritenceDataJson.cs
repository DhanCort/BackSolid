/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 19, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class TemdatajsonTemplateInheritenceDataJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkTemplate { get; set; }
        public String strUnit { get; set; }
        public double? numnCost { get; set; }
        public bool boolIsDeviceOrTool { get; set; }
        public bool? boolnIsCalendar { get; set; }
        public bool? boolnIsAvailable { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public TemdatajsonTemplateInheritenceDataJson(
            int intPkTemplate_I,
            String strUnit_I,
            double? numnCost_I,
            bool boolIsDeviceOrTool_I,
            bool? boolnIsCalendar_I,
            bool? boolnIsAvailable_I
            )
        {
            this.intPkTemplate = intPkTemplate_I;
            this.strUnit = strUnit_I;
            this.numnCost = numnCost_I;
            this.boolIsDeviceOrTool = boolIsDeviceOrTool_I;
            this.boolnIsCalendar = boolnIsCalendar_I;
            this.boolnIsAvailable = boolnIsAvailable_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
