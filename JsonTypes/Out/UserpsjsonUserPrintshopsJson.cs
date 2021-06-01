/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: May 14, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class UserpsjsonUserPrintshopsJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public Psjson1PrinthsopJson1[] arrps { get; set; }
        public bool boolIsAdmin { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public UserpsjsonUserPrintshopsJson(
            Psjson1PrinthsopJson1[] arrps_I,
            bool boolIsAdmin_I
            )
        {
            this.arrps = arrps_I;
            this.boolIsAdmin = boolIsAdmin_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
