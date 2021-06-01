/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - César Cigarroa).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 30, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PricejsonPriceJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public double? numnPrice { get; set; }
        public String strDescription { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strFirstName { get; set; }
        public String strLastName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PricejsonPriceJson(
            double? numnPrice_I,
            String strDescription_I,
            String strStartDate_I,
            String strStartTime_I,
            String strFirstName_I,
            String strLastName_I
            )
        {
            this.numnPrice = numnPrice_I;
            this.strDescription = strDescription_I;
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strFirstName = strFirstName_I;
            this.strLastName = strLastName_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
