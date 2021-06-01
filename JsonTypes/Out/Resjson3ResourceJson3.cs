/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 14, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Resjson3ResourceJson3
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkWorkflow { get; set; }
        public int intPk { get; set; }
        public String strName { get; set; }
        public String strUnit { get; set; }
        public bool boolIsPhysical { get; set; }
        public int intPkProcessInWorkflow { get; set; }
        public bool boolIsPaper { get; set; }
        public bool boolIsDeviceOrMiscConsumable { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Resjson3ResourceJson3(
            int intPkWorkflow_I,
            int intPk_I,
            String strName_I,
            String strUnit_I,
            bool boolIsPhysical_I,
            int intPkProcessInWorkflow_I,
            bool boolIsPaper_I,
            bool boolIsDeviceOrMiscConsumable_I
            )
        {
            this.intPkWorkflow = intPkWorkflow_I;
            this.intPk = intPk_I;
            this.strName = strName_I;
            this.boolIsPhysical = boolIsPhysical_I;
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.strUnit = strUnit_I;
            this.boolIsPaper = boolIsPaper_I;
            this.boolIsDeviceOrMiscConsumable = boolIsDeviceOrMiscConsumable_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
