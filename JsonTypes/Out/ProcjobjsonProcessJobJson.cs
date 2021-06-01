/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (DTC-Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF-Liliana Gutierrez).
//                                                          //DATE: September 15, 2020.  

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ProcjobjsonProcessJobJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strName { get; set; }
        public List<CostbycaljsonCostByCalculationJson> arrcal { get; set; }
        public List<RecjobjsonResourceJobJson> arrres { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ProcjobjsonProcessJobJson(
            String strName_I,
            List<CostbycaljsonCostByCalculationJson> arrcal_I,
            List<RecjobjsonResourceJobJson> arrresjobjson_I
            )
        {
            this.strName = strName_I;
            this.arrcal = arrcal_I;
            this.arrres = arrresjobjson_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
