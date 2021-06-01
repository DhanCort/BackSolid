/*TASK RP.JSON*/
using System;
using System.Collections.Generic;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: February 11, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class JobpendjsoninJobPendingJsonInternal
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intJobId { get; set; }
        public String strPrintshopId { get; set; }
        public double numJobPrice1 { get; set; }
        public int intContactId { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public JobpendjsoninJobPendingJsonInternal(
            int intJobId_I,
            String strPrintshopId_I,
            double numPrice1_I,
            int intContactId_I
            )
        {
            this.intJobId = intJobId_I;
            this.strPrintshopId = strPrintshopId_I;
            this.numJobPrice1 = numPrice1_I;
            this.intContactId = intContactId_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
