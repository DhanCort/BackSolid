/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: July 13, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class EstjsonEstimationJson 
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES. 

        public List<Estjson2EstimationDataJson2> arrest { get; set; }
        public bool boolIsDownloadable { get; set; }
        public bool boolIsFromJob { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public EstjsonEstimationJson(
            List<Estjson2EstimationDataJson2> arrest_I,
            bool boolIsDownloadable_I,
            bool boolIsFromJob_I
            )
        {
            this.arrest = arrest_I;
            this.boolIsDownloadable = boolIsDownloadable_I;
            this.boolIsFromJob = boolIsFromJob_I;
        }


        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
