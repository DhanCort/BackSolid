/*TASK Database Database All-in-Memory*/
using System;

//                                                          //AUTHOR: Towa (LGCR-Leoncio Chiunty).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: October 22, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public sealed class JsonHistorySimpleEntry : JsonAbstract
    {
        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        public String start;
        public String end;
        public String value;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "{" + Test.ToLog(this.start) + ", " + Test.ToLog(this.end) + ", " + Test.ToLog(this.value) + "}";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            return "{" + Test.ToLog(this.start, "start") + ", " + Test.ToLog(this.end, "end") + ", " +
                Test.ToLog(this.value, "value") + "}";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public JsonHistorySimpleEntry(

            Date start_I,
            Date end_I,
            Object object_I
            )
            : base()
        {
            this.start = start_I.ToText();
            this.end = end_I.ToText();
            this.value = object_I.ToString();
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
