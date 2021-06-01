/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 28, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Restypjson1ResourceTypeJson1
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strTypeId { get; set; }
        public bool boolIsType { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Restypjson1ResourceTypeJson1(
            int intPk_I,
            String strTypeId_I,
            bool boolIsType_I
            )
        {
            this.intPk = intPk_I;
            this.strTypeId = strTypeId_I;
            this.boolIsType = boolIsType_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
