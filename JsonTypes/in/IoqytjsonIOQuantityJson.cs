/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: August 08, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class IoqytjsonIOQuantityJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public int? intPkProcessInWorkflow { get; set; }
        public int? intnPkEleet { get; set; }
        public int? intnPkEleele { get; set; }
        public int? intnPkResource { get; set; }
        public double numQuantityIO { get; set; }
        public double numCostResource { get; set; }
        public bool boolAreInput { get; set; }
        public String strLink { get; set; }
        public WstjsonWasteJson[] arrwstjsonWaste { get; set; }
        public WstaddjsonWasteAdditionalJson[] arrwstaddWasteAdditional { get; set; }
        public int intHours { get; set; }
        public int intMinutes { get; set; }
        public int intSeconds{ get; set; }

//-------------------------------------------------------------------------------------------------------------
//                                                  //CONSTRUCTORS.

//-------------------------------------------------------------------------------------------------------------
public IoqytjsonIOQuantityJson(
            int? intPkProcessInWorkflow_I,
            int? intnPkEleet_I,
            int? intnPkEleele_I,
            int? intnPkResource_I,
            double numQuantityIO_I,
            double numCostResource_I,
            bool boolAreInput_I,
            String strLink_I,
            WstjsonWasteJson[] arrwstjsonWaste_I
            )
        {
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.intnPkEleet = intnPkEleet_I;
            this.intnPkEleele = intnPkEleele_I;
            this.intnPkResource = intnPkResource_I;
            this.numQuantityIO = numQuantityIO_I;
            this.numCostResource = numCostResource_I;
            this.boolAreInput = boolAreInput_I;
            this.strLink = strLink_I;
            this.arrwstjsonWaste = arrwstjsonWaste_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
