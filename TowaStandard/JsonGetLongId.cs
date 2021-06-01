/*TASK Database Database All-in-Memory*/
using System;

//                                                          //AUTHOR: Towa (LGCR-Leoncio Chiunty).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: November 5, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public sealed class JsonGetLongId : JsonAbstract
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //There are 6 type of entity key; String, long, int, char,
        //                                                  //      Date & Time.
        //                                                  //There is a specific JsonGetXxxxxId for each type.

        //                                                  //Controllers create this objects.

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        public long longId;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "{" + Test.ToLog(this.longId) + "}";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            return "{" + Test.ToLog(this.longId, "longId") + "}";
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
