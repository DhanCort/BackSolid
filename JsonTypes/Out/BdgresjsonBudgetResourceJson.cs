/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: July 07, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class BdgresjsonBudgetResourceJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES. 

        public int? intnPkResource { get; set; }
        public String strName { get; set; }
        public int intPkEleetOrEleele { get; set; }
        public bool boolIsEleet { get; set; }
        public int? intnGroupResourceId { get; set; }
        public bool boolHasOption { get; set; }
        public double? numnCost { get; set; }
        public double? numnQuantity { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public BdgresjsonBudgetResourceJson(
            int? intnPkResource_I,
            String strName_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            int? intnGroupResourceId_I,
            bool boolHasOption_I,
            double? numnCost_I,
            double? numnQuantity_I
            )
        {
            this.intnPkResource = intnPkResource_I;
            this.strName = strName_I;
            this.intPkEleetOrEleele = intPkEleetOrEleele_I;
            this.boolIsEleet = boolIsEleet_I;
            this.intnGroupResourceId = intnGroupResourceId_I;
            this.boolHasOption = boolHasOption_I;
            this.numnCost = numnCost_I;
            this.numnQuantity = numnQuantity_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
