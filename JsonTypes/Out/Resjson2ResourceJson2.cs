/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 14, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Resjson2ResourceJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strName { get; set; }
        public bool boolIsPhysical { get; set; }
        public String strUnit { get; set; }
        public bool boolIsPaper { get; set; }
        public bool boolIsDeviceOrMiscConsumable { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Resjson2ResourceJson2(
            int intPk_I,
            String strName_I,
            bool boolIsPhysical_I,
            String strUnit_I,
            bool boolIsPaper_I,
            bool boolIsDeviceOrMiscConsumable_I
            )
        {
            this.intPk = intPk_I;
            this.strName = strName_I;
            this.boolIsPhysical = boolIsPhysical_I;
            this.strUnit = strUnit_I;
            this.boolIsPaper = boolIsPaper_I;
            this.boolIsDeviceOrMiscConsumable = boolIsDeviceOrMiscConsumable_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
