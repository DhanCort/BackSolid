/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 12th, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class BscostjsonBaseCostJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int? intnPkFinalCost { get; set; }
        public int intPkCalculation { get; set; }
        public String strCalName { get; set; }
        public double numCost { get; set; }
        public double numFinalCost { get; set; }
        public String strDescription { get; set; }
        public bool boolManyRowsInFinalTable { get; set; }
        public int intPkAccountMovement { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public BscostjsonBaseCostJson(
            int? intnPkFinalCost_I,
            int intPkCalculation_I,
            String strCalName_I,
            double numCost_I,
            double numFinalCost_I,
            String strDescription_I,
            bool boolManyRowsInFinalTable_I,
            int intPkAccountMovement_I
            )
        {
            this.intnPkFinalCost = intnPkFinalCost_I;
            this.intPkCalculation = intPkCalculation_I;
            this.strCalName = strCalName_I;
            this.numCost = numCost_I;
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
