/*TASK RP.JDF*/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: Febraury 16, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class DeljobsjsonDeletedJobsJson : TjsonTJson
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int[] darrintJobsDeleted { get; set; }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
