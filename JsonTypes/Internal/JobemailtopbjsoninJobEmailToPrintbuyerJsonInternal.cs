/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: April 27, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class JobemailtopbjsoninJobEmailToPrintbuyerJsonInternal
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String Action { get; set; }
        public int Initiated_By_Contact_ID { get; set; }
        public int Job_ID { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public JobemailtopbjsoninJobEmailToPrintbuyerJsonInternal(
            String strAction_I,
            int intEmployeeContactId_I,
            int intJobId_I
            )
        {
            this.Action = strAction_I;
            this.Initiated_By_Contact_ID = intEmployeeContactId_I;
            this.Job_ID = intJobId_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
