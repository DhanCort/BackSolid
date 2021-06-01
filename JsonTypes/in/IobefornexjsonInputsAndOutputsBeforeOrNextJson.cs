/*TASK RP.JSON*/
using Odyssey2Backend.DB_Odyssey2;
using System;

//                                                          //AUTHOR: Towa (CLGA - Cesar Garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: October 19, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class IobefornexjsonInputsAndOutputsBeforeOrNextJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public IoentityInputsAndOutputsEntityDB ioentityBelongAProcessOrNode { get; set; }
        public IoentityInputsAndOutputsEntityDB ioentityBeforeOrNext { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public IobefornexjsonInputsAndOutputsBeforeOrNextJson(
            IoentityInputsAndOutputsEntityDB ioentityBelongAProcessOrNode_I,
            IoentityInputsAndOutputsEntityDB ioentityBeforeOrNext_I
            )
        {
            this.ioentityBelongAProcessOrNode = ioentityBelongAProcessOrNode_I;
            this.ioentityBeforeOrNext = ioentityBeforeOrNext_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
