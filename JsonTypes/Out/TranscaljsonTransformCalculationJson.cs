/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: August 03, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class TranscaljsonTransformCalculationJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intnPk { get; set; }
        public int intPkProcessInWorkflow { get; set; }
        public double numNeeded { get; set; }
        public double numPerUnit { get; set; }
        public String strTypeTemplateAndResourceI { get; set; }
        public int intPkEleetOrEleeleI { get; set; }
        public bool boolIsEleetI { get; set; }
        public int intPkResourceI { get; set; }
        public String strUnitI { get; set; }
        public int intPkEleetOrEleeleO { get; set; }
        public bool boolIsEleetO { get; set; }
        public int intPkResourceO { get; set; }
        public String strUnitO { get; set; }
        public String strTypeTemplateAndResourceO { get; set; }
        public bool boolHasCondition { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public TranscaljsonTransformCalculationJson(
            int intnPk_I,
            int intPkProcessInWorkflow_I,
            double numNeeded_I,
            double numPerUnit_I,
            String strTypeTemplateAndResourceI_I,
            int intPkEleetOrEleeleI_I,
            bool boolIsEleetI_I,
            int intPkResourceI_I,
            String strUnitI_I,
            int intPkEleetOrEleeleO_I,
            bool boolIsEleetO_I,
            int intPkResourceO_I,
            String strUnitO_I,
            String strTypeTemplateAndResourceO_I
            )
        {
            this.intnPk = intnPk_I;
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.numNeeded = numNeeded_I;
            this.numPerUnit = numPerUnit_I;
            this.strTypeTemplateAndResourceI = strTypeTemplateAndResourceI_I;
            this.intPkEleetOrEleeleI = intPkEleetOrEleeleI_I;
            this.boolIsEleetI = boolIsEleetI_I;
            this.intPkResourceI = intPkResourceI_I;
            this.strUnitI = strUnitI_I;
            this.intPkEleetOrEleeleO = intPkEleetOrEleeleO_I;
            this.boolIsEleetO = boolIsEleetO_I;
            this.intPkResourceO = intPkResourceO_I;
            this.strUnitO = strUnitO_I;
            this.strTypeTemplateAndResourceO = strTypeTemplateAndResourceO_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
