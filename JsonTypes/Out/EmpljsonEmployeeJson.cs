/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 22, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class EmpljsonEmployeeJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strFirstName { get; set; }
        public String strLastName { get; set; }
        public int intContactId { get; set; }
        public String strPhotoUrl { get; set; }
        public bool boolIsSupervisor { get; set; }
        public bool boolIsAccountant { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public EmpljsonEmployeeJson(
            String strFirstName_I,
            String strLastName_I,
            int intContactId_I,
            String strPhotoUrl_I,
            bool boolIsSupervisor_I,
            bool boolIsAccountant_I
            )
        {
            this.strFirstName = strFirstName_I;
            this.strLastName = strLastName_I;
            this.intContactId = intContactId_I;
            this.strPhotoUrl = strPhotoUrl_I;
            this.boolIsSupervisor = boolIsSupervisor_I;
            this.boolIsAccountant = boolIsAccountant_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
