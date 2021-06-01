/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 22, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ContactjsonContactJson : TjsonTJson
    {
        public int intContactId { get; set; }
        public int intPrintshopEmployee { get; set; }
        public int intPrintshopOwner { get; set; }
        public int intPrintshopAdmin { get; set; }
        public String strFirstName { get; set; }
        public String strLastName { get; set; }
        public int intActive { get; set; }
        public String strPhotoUrl { get; set; }
    }

    //==================================================================================================================
}
/*END-TASK*/
