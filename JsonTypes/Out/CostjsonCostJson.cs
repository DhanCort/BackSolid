/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 1st, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class CostjsonCostJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkResource { get; set; }
        public String strUnit { get; set; }
        public double? numnQuantity { get; set; }
        public double? numnCost { get; set; }
        public double? numnMin { get; set; }
        public double? numnBlock { get; set; }
        public int? intnPkAccount { get; set; }
        public double? numnHourlyRate { get; set; }
        public bool? boolnArea { get; set; }
        public String strDimensionUnit { get; set; }
        public bool boolPaper { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public CostjsonCostJson(
            int intPkresource_I,
            String strUnit_I,
            double? numnQuantity_I,
            double? numnCost_I,
            double? numnMin_I,
            double? numnBlock_I,
            int? intnPkAccount_I,
            double? numnHourlyRate_I, 
            bool? boolnArea_I, 
            String strDimensionUnit_I, 
            bool boolPaper_I
            )
        {
            this.intPkResource = intPkresource_I;
            this.strUnit = strUnit_I;
            this.numnQuantity = numnQuantity_I;
            this.numnCost = numnCost_I;
            this.numnMin = numnMin_I;
            this.numnBlock = numnBlock_I;
            this.intnPkAccount = intnPkAccount_I;
            this.numnHourlyRate = numnHourlyRate_I;
            this.boolnArea = boolnArea_I;
            this.strDimensionUnit = strDimensionUnit_I;
            this.boolPaper = boolPaper_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
