/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: July 3rd, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Estjson2EstimationDataJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public int? intnEstimationId { get; set; }
        public int? intnCopyNumber { get; set; }
        public String strBaseDate { get; set; }
        public String strBaseTime { get; set; }
        public String strName { get; set; }
        public int intQuantity { get; set; }
        public double numPrice { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Estjson2EstimationDataJson2(
            int? intnEstimationId_I,
            int? intnCopyNumber_I,
            String strBaseDate_I,
            String strBaseTime_I,
            String strName_I,
            int intQuantity_I,
            double numPrice_I
            )
        {
            this.intnEstimationId = intnEstimationId_I;
            this.intnCopyNumber = intnCopyNumber_I;
            this.strBaseDate = strBaseDate_I;
            this.strBaseTime = strBaseTime_I;
            this.strName = strName_I;
            this.intQuantity = intQuantity_I;
            this.numPrice = numPrice_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
