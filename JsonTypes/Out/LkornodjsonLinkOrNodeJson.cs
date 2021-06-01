/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: October 02, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class LkornodjsonLinkOrNodeJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public Nodejson2NodeJson2 nodeFrom { get; set; }
        public Nodejson2NodeJson2[] arrnodeTo { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public LkornodjsonLinkOrNodeJson(
            Nodejson2NodeJson2 nodeFrom_I,
            Nodejson2NodeJson2[] arrnodeTo_I
            )
        {
            this.nodeFrom = nodeFrom_I;
            this.arrnodeTo = arrnodeTo_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
