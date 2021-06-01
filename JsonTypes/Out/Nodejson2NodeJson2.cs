/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CCC - José Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: October 02, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Nodejson2NodeJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int? intnPkProcessInWorkflow { get; set; }
        public int? intnPkNode { get; set; }
        public String strName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Nodejson2NodeJson2(
            int? intnPkProcessInWorkflow_I,
            int? intnPkNode_I,
            String strName_I
            )
        {
            this.intnPkProcessInWorkflow = intnPkProcessInWorkflow_I;
            this.intnPkNode = intnPkNode_I;
            this.strName = strName_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
