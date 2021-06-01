/*TASK RP.JDF*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: March 5, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class LkjsonLinkJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkOut { get; set; }
        public int intPkIn { get; set; }
        public String strProcessFrom { get; set; }
        public String strOut { get; set; }
        public String strProcessTo { get; set; }
        public String strIn { get; set; }
        public bool boolSetCondition { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public LkjsonLinkJson(
            int intPkOut_I,
            int intPkIn_I,
            String strProcessFrom_I,
            String strOut_I,
            String strProcessTo_I,
            String strIn_I,
            bool boolSetCondition_I
            )
        {
            this.intPkOut = intPkOut_I;
            this.intPkIn = intPkIn_I;
            this.strProcessFrom = strProcessFrom_I;
            this.strOut = strOut_I;
            this.strProcessTo = strProcessTo_I;
            this.strIn = strIn_I;
            this.boolSetCondition = boolSetCondition_I;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
