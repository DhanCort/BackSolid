/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: April 1, 2020.  

namespace Odyssey2Backend.JsonTemplates.Out
{
    //==================================================================================================================
    public class Proelejson3ProcessElementJson3
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strElementName { get; set; }
        public bool boolIsXJDF { get; set; }


        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Proelejson3ProcessElementJson3(
            int intPk_I,
            String strElementName_I,
            bool boolIsXJDF_I
            )
        {
            this.intPk = intPk_I;
            this.strElementName = strElementName_I;
            this.boolIsXJDF = boolIsXJDF_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/

