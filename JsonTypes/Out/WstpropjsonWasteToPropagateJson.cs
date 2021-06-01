/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (JLBD - José Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: September 14, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class WstpropjsonWasteToPropagateJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public int intPkProcessInWorkflow { get; set; }
        public int? intnPkEleetSource { get; set; }
        public int? intnPkEleeleSource { get; set; }
        public int? intnPkEleetTarget { get; set; }
        public int? intnPkEleeleTarget { get; set; }
        public double numWaste { get; set; }
        public double numFactor { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public WstpropjsonWasteToPropagateJson(
            int intPkProcessInWorkflow_I,
            int? intnPkEleetSource_I,
            int? intnPkEleeleSource_I,
            int? intnPkEleetTarget_I,
            int? intnPkEleeleTarget_I,
            double numWaste_I,
            double numFactor_I
            )
        {
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.intnPkEleetSource = intnPkEleetSource_I;
            this.intnPkEleeleSource = intnPkEleeleSource_I;
            this.intnPkEleetTarget = intnPkEleetTarget_I;
            this.intnPkEleeleTarget = intnPkEleeleTarget_I;
            this.numWaste = numWaste_I;
            this.numFactor = numFactor_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

