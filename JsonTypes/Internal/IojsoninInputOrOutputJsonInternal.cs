/*TASK RP.JSON*/
using System;
using System.Collections.Generic;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: September 30, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class IojsoninInputOrOutputJsonInternal
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkPIW { get; set; }
        public int? intnPkElementElementType { get; set; }
        public int? intnPkElementElement { get; set; }
        public int intPkResource { get; set; }
        public double numQuantity { get; set; }
        public double numCost { get; set; }
        public double numWaste { get; set; }
        public double numQuantityAndWaste { get; set; }
        public bool boolIsFinalProduct { get; set; }
        public bool boolHasLink { get; set; }
        public String strLink { get; set; } 
        public bool boolUnitAllowsDecimals { get; set; }
        public bool? boolnHasAnyResourceOrGrpSetted { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public IojsoninInputOrOutputJsonInternal(
            int intPkPIW_I,
            int? intnPkElementelementType_I,
            int? intnPkElementelement_I
            )
        {
            this.intPkPIW = intPkPIW_I;
            this.intnPkElementElementType = intnPkElementelementType_I;
            this.intnPkElementElement = intnPkElementelement_I;
            this.intPkResource = 0;
            this.numQuantity = 0.0;
            this.numCost = 0.0;
            this.numWaste = 0.0;
            this.numQuantityAndWaste = 0.0;
            this.boolIsFinalProduct = false;
            this.boolHasLink = false;
            this.boolnHasAnyResourceOrGrpSetted = null;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
