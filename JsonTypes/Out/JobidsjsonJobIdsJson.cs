﻿/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa(DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: November 11, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class JobidsjsonJobIdsJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int[] darrintJobsIds { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public JobidsjsonJobIdsJson(
            int[] darrintJobsIds_I
            )
        {
            this.darrintJobsIds = darrintJobsIds_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/