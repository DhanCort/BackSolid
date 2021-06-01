/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: September 03, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class BrajsonBranchJson : TjsonTJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intBranchId { get; set; }
        public String strName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public BrajsonBranchJson(
            int intBranchId_I,
            String strName_I
            )
        {
            this.intBranchId = intBranchId_I;
            this.strName = strName_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
