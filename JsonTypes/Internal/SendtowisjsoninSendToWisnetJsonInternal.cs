/*TASK RP.JSON*/
using System;
using System.Collections.Generic;
using TowaStandard;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia Aguazul).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: February 16, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class SendtowisjsoninSendToWisnetJsonInternal
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intJobId { get; set; }
        public String strPrintshopId { get; set; }
        public int intContactId { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public SendtowisjsoninSendToWisnetJsonInternal(
            int intJobId_I,
            String strPrintshopId_I,
            int intContactId_I
            )
        {
            this.intJobId = intJobId_I;
            this.strPrintshopId = strPrintshopId_I;
            this.intContactId = intContactId_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
