/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: May 26, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class JobandqtysonJobTypeAndQuantityJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strType { get; set; }
        public int intQuantity { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public JobandqtysonJobTypeAndQuantityJson(
            String strType_I,
            int intQuantity_I
            )
        {
            this.strType = strType_I;
            this.intQuantity = intQuantity_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
