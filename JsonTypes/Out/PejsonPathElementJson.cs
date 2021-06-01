/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: February 24, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class PejsonPathElementJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strName { get; set; }
        public bool boolIsType { get; set; }
        public bool boolIsResource { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public PejsonPathElementJson(
            int intPK_I,
            String strName_I,
            bool boolIsType_I,
            bool boolIsResource_I
            )
        {
            this.intPk = intPK_I;
            this.strName = strName_I;
            this.boolIsType = boolIsType_I;
            this.boolIsResource = boolIsResource_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
