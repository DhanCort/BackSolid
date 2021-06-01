/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: November 13, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Accjson2AccountJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //-------------------------------------------------------------------------------------------------------------

        public int intPk { get; set; }
        public String strNumber { get; set; }
        public String strName { get; set; }
        public double numAmount { get; set; }
        public String strAccountType { get; set; }


        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Accjson2AccountJson2(
            int intPk_I,
            String strNumber_I,
            String strName_I,
            double numAmount_I,
            String strAccountType_I
            )
        {
            this.intPk = intPk_I;
            this.strNumber = strNumber_I;
            this.strName = strName_I;
            this.numAmount = numAmount_I;
            this.strAccountType = strAccountType_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
