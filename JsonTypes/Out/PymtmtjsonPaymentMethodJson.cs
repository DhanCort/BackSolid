/*TASK RP.JDF*/
using System;
//                                                          //AUTHOR: Towa (JLBD - Luis Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierre).
//                                                          //DATE: November 30, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PymtmtjsonPaymentMethodJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PymtmtjsonPaymentMethodJson(
            int intPk_I,
            String strName_I
            )
        {
            this.intPk = intPk_I;
            this.strName = strName_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
