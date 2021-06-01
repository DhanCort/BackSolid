/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 17, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Protypjson5ProcessTypeJson5
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strTypeId { get; set; }
        public Restypjson2ResourceTypeJson2[] arrres { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Protypjson5ProcessTypeJson5(
            int intPk_I,
            String strTypeId_I,
            Restypjson2ResourceTypeJson2[] arrres_I
            )
        {
            this.intPk = intPk_I;
            this.strTypeId = strTypeId_I;
            this.arrres = arrres_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
