/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 12th, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PqcostjsonPerQuantityCostJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int? intnPkFinalCost { get; set; }
        public int? intnPkCalculation { get; set; }
        public int? intnPkResource { get; set; }
        public int? intnPkEleetOrEleele { get; set; }
        public bool? boolnIsEleet { get; set; }
        public double numQuantity { get; set; }
        public double? numnFinalQuantity { get; set; }
        public String strUnit { get; set; }
        public String strResOrCalName { get; set; }
        public double numCost { get; set; }
        public double numCostWithFinalQuantity { get; set; }
        public double numFinalCost { get; set; }
        public String strDescription { get; set; }
        public bool boolManyRowsInFinalTable { get; set; }
        public int intPkAccountMovement { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PqcostjsonPerQuantityCostJson(
            int? intnPkFinalCost_I,
            int? intnPkCalculation_I,
            int? intnPkResource_I,
            int? intnPkEleetOrEleele_I,
            bool? boolnIsEleet_I,
            double numQuantity_I,
            double? numnFinalQuantity_I,
            String strUnit_I,
            String strResOrCalName_I,
            double numCost_I,
            double numCostWithFinalQuantity_I,
            double numFinalCost_I, 
            String strDescription_I,
            bool boolManyRowsInFinalTable_I,
            int intPkAccountMovement_I
            )
        {
            this.intnPkFinalCost = intnPkFinalCost_I;
            this.intnPkCalculation = intnPkCalculation_I;
            this.intnPkResource = intnPkResource_I;
            this.intnPkEleetOrEleele = intnPkEleetOrEleele_I;
            this.boolnIsEleet = boolnIsEleet_I;
            this.numQuantity = numQuantity_I;
            this.numnFinalQuantity = numnFinalQuantity_I;
            this.strUnit = strUnit_I;
            this.strResOrCalName = strResOrCalName_I;
            this.numCost = numCost_I;
            this.numCostWithFinalQuantity = numCostWithFinalQuantity_I;
            this.numFinalCost = numFinalCost_I;
            this.strDescription = strDescription_I;
            this.boolManyRowsInFinalTable = boolManyRowsInFinalTable_I;
            this.intPkAccountMovement = intPkAccountMovement_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
