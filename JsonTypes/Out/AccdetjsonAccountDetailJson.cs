/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (DTC-Daniel Texon).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: December 23, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class AccdetjsonAccountDetailJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //-------------------------------------------------------------------------------------------------------------

        public bool boolIsAsset { get; set; }
        public AccmovjsonAccountMomeventJson2[] arrAccountMovements { get; set; }


        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public AccdetjsonAccountDetailJson(
            bool boolIsAsset_I,
            AccmovjsonAccountMomeventJson2[] arrAccountMovements_I

            )
        {
            this.boolIsAsset = boolIsAsset_I;
            this.arrAccountMovements = arrAccountMovements_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
