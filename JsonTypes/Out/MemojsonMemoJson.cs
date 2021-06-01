/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: December 11, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class MemojsonMemoJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        //-------------------------------------------------------------------------------------------------------------
        public String strCustomerFullName { get; set; }
        public String strCreditMemoNumber { get; set; }
        public String strDate { get; set; }
        public String strBilledTo { get; set; }
        public String strDescription { get; set; }
        public double numAmount { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public MemojsonMemoJson(
            String strCustomerFullName_I,
            String strCreditMemoNumber_I,
            String strDate_I,
            String strBilledTo_I,
            String strDescription_I,
            double numAmount_I
            )
        {
            this.strCustomerFullName = strCustomerFullName_I;
            this.strCreditMemoNumber = strCreditMemoNumber_I;
            this.strDate = strDate_I;
            this.strBilledTo = strBilledTo_I;
            this.strDescription = strDescription_I;
            this.numAmount = numAmount_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
