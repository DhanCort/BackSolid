/*TASK RP.JSON*/
using System;
//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: April 14, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class Userjson1UserJson1 : TjsonTJson
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strEmail { get; set; }
        public String strPassword { get; set; }
        public String strName { get; set; }
        public String strLastName { get; set; }
        public int intActive { get; set; }
        public int intPrintshopId { get; set; }
        public int intPrintshopEmployee { get; set; }
        public int intPrintshopAdmin { get; set; }
        public int intPrintshopOwner { get; set; }
        public int intContactId { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
