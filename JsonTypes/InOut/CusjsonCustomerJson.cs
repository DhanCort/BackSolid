/*TASK RP.JDF*/
using System;
using System.Collections.Generic;
//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: August 07, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class CusjsonCustomerJson : TjsonTJson
    {
        public int intContactId { get; set; }
        public int? intnCompanyId { get; set; }
        public String strCompanyName { get; set; }
        public int? intnBranchId { get; set; }
        public String strBranchName { get; set; }
        public String strFirstName { get; set; }
        public String strLastName { get; set; }
        public String strEmail { get; set; }
        public String strPhone { get; set; }
        public String strCellPhone { get; set; }
    }

    //==================================================================================================================
}
/*END-TASK*/
