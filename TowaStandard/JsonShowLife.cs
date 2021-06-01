/*TASK Database Database All-in-Memory*/
using System;

//                                                          //AUTHOR: Towa (LGCR-Leoncio Chiunty).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: October 22, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public sealed class JsonShowLife : JsonAbstract
    {
        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        public JsonShowDaysPeriod[] periods;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "{" + Test.ToLog(this.periods) + "}";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            return "{" + Test.ToLog(this.periods, "periods") + "}";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public JsonShowLife(

            Life life_I
            )
            : base()
        {
            DaysPeriod[] arrdperiod = life_I.GetAllPeriods();
            this.periods = new JsonShowDaysPeriod[arrdperiod.Length];

            for (int intP = 0; intP < arrdperiod.Length; intP = intP + 1)
            {
                this.periods[intP] = new JsonShowDaysPeriod(arrdperiod[intP]);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
