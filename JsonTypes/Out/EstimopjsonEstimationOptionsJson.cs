/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: July 06, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class EstimopjsonEstimationOptionsJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public String strBaseDate { get; set; }
        public String strBaseTime { get; set; }
        public ResestimjsonResourceEstimatedJson[] arrresSelected { get; set; }
        public CombestimjsonCombinationEstimatedJson[] arrop { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public EstimopjsonEstimationOptionsJson(
            String strBaseDate_I,
            String strBaseTime_I,
            ResestimjsonResourceEstimatedJson[] arrresestimjson2_I,
            CombestimjsonCombinationEstimatedJson[] arrcombestimjson_I
            )
        {
            this.strBaseDate = strBaseDate_I;
            this.strBaseTime = strBaseTime_I;
            this.arrresSelected = arrresestimjson2_I;
            this.arrop = arrcombestimjson_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
