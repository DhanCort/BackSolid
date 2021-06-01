﻿/*TASK Database Database All-in-Memory*/
using System;

//                                                          //AUTHOR: Towa (LGCR-Leoncio Chiunty).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: November 5, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public sealed class JsonGetStringId : JsonAbstract
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //There are 6 type of entity key; String, long, int, char,
        //                                                  //      Date & Time.
        //                                                  //There is a specific JsonGetXxxxxId for each type.

        //                                                  //Controllers create this objects.

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        public String stringId;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "{" + Test.ToLog(this.stringId) + "}";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            return "{" + Test.ToLog(this.stringId, "stringId") + "}";
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
