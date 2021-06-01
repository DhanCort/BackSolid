/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: July 13, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class EstdetjsonEstimationDetailsJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES. 

        public String strEstimationName { get; set; }
        public int intQuantity { get; set; }
        public double numPrice { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public EstdetjsonEstimationDetailsJson(
            String strEstimationName_I,
            int intQuantity_I,
            double numPrice_I
            )
        {
            this.strEstimationName = strEstimationName_I;
            this.intQuantity = intQuantity_I;
            this.numPrice = numPrice_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

