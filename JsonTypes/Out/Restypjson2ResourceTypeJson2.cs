/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 17, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Restypjson2ResourceTypeJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strTypeId { get; set; }
        public bool boolHasIt { get; set; }
        public String strClassification { get; set; }
        public bool? boolnUsage { get; set; }
        public bool boolIsPhysical { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Restypjson2ResourceTypeJson2(
            int intPk_I,
            String strTypeId_I,
            bool boolHasIt_I,
            String strClassification_I,
            bool? boolnUsage_I,
            bool boolIsPhysical_I
            )
        {
            this.intPk = intPk_I;
            this.strTypeId = strTypeId_I;
            this.boolHasIt = boolHasIt_I;
            this.strClassification = strClassification_I;
            this.boolnUsage = boolnUsage_I;
            this.boolIsPhysical = boolIsPhysical_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASk*/
