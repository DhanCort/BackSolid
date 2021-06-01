/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (CLGA - Cesar garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: July 3, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class CostbycaljsonCostByCalculationJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strDescription { get; set; }
        public double numCost { get; set; }
        public double numFinalCost { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public CostbycaljsonCostByCalculationJson(
            String strDescription_I,
            double numCost_I
            )
        {
            this.strDescription = strDescription_I;
            this.numCost = numCost_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
