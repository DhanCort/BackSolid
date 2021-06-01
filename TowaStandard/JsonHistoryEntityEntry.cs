/*TASK Database Database All-in-Memory*/
using System;

//                                                          //AUTHOR: Towa (LGCR-Leoncio Chiunty).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: October 22, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public sealed class JsonHistoryEntityEntry : JsonAbstract
    {
        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        public String start;
        public String end;
        public long pk;
        public String id;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "{" + Test.ToLog(this.start) + ", " + Test.ToLog(this.end) + ", " + Test.ToLog(this.pk) + ", " +
                Test.ToLog(this.id) + "}";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            return "{" + Test.ToLog(this.start, "start") + ", " + Test.ToLog(this.end, "end") + ", " +
                Test.ToLog(this.pk, "pk") + ", " + Test.ToLog(this.id, "id") + "}";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public JsonHistoryEntityEntry(

            Date start_I,
            Date end_I,
            long pk_I,
            String id_I
            )
            : base()
        {
            this.start = start_I.ToText();
            this.end = end_I.ToText();
            this.pk = pk_I;
            this.id = id_I;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
