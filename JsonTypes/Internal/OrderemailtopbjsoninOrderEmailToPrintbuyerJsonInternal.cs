/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: May 03, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class OrderemailtopbjsoninOrderEmailToPrintbuyerJsonInternal
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String Action { get; set; }
        public int Initiated_By_Contact_ID { get; set; }
        public int Order_ID { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public OrderemailtopbjsoninOrderEmailToPrintbuyerJsonInternal(
            String strAction_I,
            int intEmployeeContactId_I,
            int intOrderId_I
            )
        {
            this.Action = strAction_I;
            this.Initiated_By_Contact_ID = intEmployeeContactId_I;
            this.Order_ID = intOrderId_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

