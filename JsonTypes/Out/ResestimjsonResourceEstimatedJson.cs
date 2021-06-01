/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: July 06, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ResestimjsonResourceEstimatedJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkProcessInWorkflow { get; set; }
        public int intPkEleetOrEleele { get; set; }
        public bool boolIsEleet { get; set; }
        public int? intnPk { get; set; }
        public String strName { get; set; }
        public double? numnCost { get; set; }
        public double? numnQuantity { get; set; }
        public String strUnit { get; set; }
        public bool boolIsAvailable { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ResestimjsonResourceEstimatedJson(
            int intPkProcessInWorkflow_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            int? intnPkResource_I,
            String strName_I,
            double? numnCost_I,
            double? numnQuantity_I,
            String strUnit_I,
            bool boolIsAvailable_I
            )
        {
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.intPkEleetOrEleele = intPkEleetOrEleele_I;
            this.boolIsEleet = boolIsEleet_I;
            this.intnPk = intnPkResource_I;
            this.strName = strName_I;
            this.numnCost = numnCost_I;
            this.numnQuantity = numnQuantity_I;
            this.strUnit = strUnit_I;
            this.boolIsAvailable = boolIsAvailable_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
