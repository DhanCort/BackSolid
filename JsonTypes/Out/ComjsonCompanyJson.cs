/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: September 03, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ComjsonCompanyJson : TjsonTJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intCompanyId { get; set; }
        public String strName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ComjsonCompanyJson(
            int intCompanyId_I,
            String strName_I
            )
        {
            this.intCompanyId = intCompanyId_I;
            this.strName = strName_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
