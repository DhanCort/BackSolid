/*TASK Database Database All-in-Memory*/
using System;

//                                                          //AUTHOR: Towa (LGCR-Leoncio Chiunty).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: October 22, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public sealed class JsonShowDaysPeriod : JsonAbstract
    {
        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        public String start;
        public String end;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "{" + Test.ToLog(this.start) + ", " + Test.ToLog(this.end) + "}";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            return "{" + Test.ToLog(this.start, "start") + ", " + Test.ToLog(this.end, "end") + "}";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public JsonShowDaysPeriod(

            DaysPeriod period_I
            )
            : base()
        {
            this.start = period_I.Start.ToText();
            this.end = period_I.End.ToText();
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
