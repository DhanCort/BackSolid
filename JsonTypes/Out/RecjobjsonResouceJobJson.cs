/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (DTC-Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: September 15, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class RecjobjsonResourceJobJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        
        public String strName { get; set; }        
        public double numQuantity { get; set; }
        public String strUnit { get; set; }
        public double numCost { get; set; }
        public String strEmployee { get; set; }
        public bool boolAllowDecimal { get; set; }
        public int? intnPkEleetOrEleele { get; set; }
        public bool? boolnIsEleet { get; set; }
        public int? intnPkResource { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public RecjobjsonResourceJobJson(
            String strName_I,
            double numQuantity_I,
            String strUnit_I,
            double numCostByResource_I,
            String strEmployee_I,
            bool boolAllowDecimal_I,
            int? intnPkEleetOrEleele_I,
            bool? boolnIsEleet_I,
            int? intnPkResource_I
            )
        {
            this.strName = strName_I;
            this.numQuantity = numQuantity_I;
            this.strUnit = strUnit_I;
            this.numCost = numCostByResource_I;
            this.strEmployee = strEmployee_I;
            this.boolAllowDecimal = boolAllowDecimal_I;
            this.intnPkEleetOrEleele = intnPkEleetOrEleele_I;
            this.boolnIsEleet = boolnIsEleet_I;
            this.intnPkResource = intnPkResource_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
