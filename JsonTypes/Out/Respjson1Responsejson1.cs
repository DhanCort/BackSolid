/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 28, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Respjson1ResponceJson1
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intStatus { get; set; }
        public String strUserMessage { get; set; }
        public String strDevMessage { get; set; }
        public Object objResponse { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Respjson1ResponceJson1(
            int intStatus_I,
            String strUserMessage_I,
            String strDevMessage_I,
            Object objResponse_I
            )
        {
            this.intStatus = intStatus_I;
            this.strUserMessage = strUserMessage_I;
            this.strDevMessage = strDevMessage_I;
            this.objResponse = objResponse_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
