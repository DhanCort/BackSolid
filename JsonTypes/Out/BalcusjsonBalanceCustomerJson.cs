/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: December 10, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class BalcusjsonBalanceCustomerJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //-------------------------------------------------------------------------------------------------------------

        public int intContactId { get; set; }
        public String strFullName { get; set; }
        public double numBalance { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public BalcusjsonBalanceCustomerJson(
            int intContactId_I,
            String strFullName_I,
            double numBalance_I
            )
        {
            this.intContactId = intContactId_I;
            this.strFullName = strFullName_I;
            this.numBalance = numBalance_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
