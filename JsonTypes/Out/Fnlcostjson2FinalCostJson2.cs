/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 20th, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Fnlcostjson2FinalCostJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strDateTime { get; set; }
        public double? numnFinalQuantity { get; set; }
        public double? numnCostWithFinalQuantity { get; set; }
        public double? numnFinalCost { get; set; }
        public String strDescription { get; set; }
        public String strFirstName { get; set; }
        public String strLastName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Fnlcostjson2FinalCostJson2(
            String strDateTime_I,
            double? numFinalQuantity_I,
            double? numCostWithFinalQuantity_I,
            double? numFinalCost_I,
            String strDescription_I,
            String strFirstName_I,
            String strLastName_I
            )
        {
            this.strDateTime = strDateTime_I;
            this.numnFinalQuantity = numFinalQuantity_I;
            this.numnCostWithFinalQuantity = numCostWithFinalQuantity_I;
            this.numnFinalCost = numFinalCost_I;
            this.strDescription = strDescription_I;
            this.strFirstName = strFirstName_I;
            this.strLastName = strLastName_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
