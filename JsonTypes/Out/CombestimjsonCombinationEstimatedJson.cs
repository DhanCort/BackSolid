/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: July 02, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class CombestimjsonCombinationEstimatedJson : IComparable
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public double numJobCost { get; set; }
        public ProestimjsonProcessEstimatedJson[] arrpro { get; set; }
        public String strDeliveryDate { get; set; }
        public String strDeliveryTime { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public CombestimjsonCombinationEstimatedJson(
            double numJobCost_I,
            ProestimjsonProcessEstimatedJson[] arrproestimjson_I,
            String strDeliveryDate_I,
            String strDeliveryTime_I
            )
        {
            this.numJobCost = numJobCost_I;
            this.arrpro = arrproestimjson_I;
            this.strDeliveryDate = strDeliveryDate_I;
            this.strDeliveryTime = strDeliveryTime_I;
        }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //-------------------------------------------------------------------------------------------------------------
        public int CompareTo(
           Object obj_I
           )
        {
            CombestimjsonCombinationEstimatedJson combestimjson = (CombestimjsonCombinationEstimatedJson)obj_I;

            return this.numJobCost.CompareTo(combestimjson.numJobCost);
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
