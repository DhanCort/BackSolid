/*TASK Database Database All-in-Memory*/
using System;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz, LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: October 22, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public sealed class JsonHistorySimple<TOwner, TValue> : JsonAbstract
        where TOwner : Entity, new()
        where TValue : SerializableInterface<TValue>, new()
    {
        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        public JsonHistorySimpleEntry[] items;

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "{" + Test.ToLog(this.items) + "}";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            return "{" + Test.ToLog(this.items, "items") + "}";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public JsonHistorySimple(

            HistoryS<TOwner, TValue> history_I
            )
            : base()
        {
            DateDateValueTrio<TValue>[] arrddvtvalue = history_I.GetAllEntries();
            this.subGetItems(arrddvtvalue);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private void subGetItems(

            DateDateValueTrio<TValue>[] arrddvtvalue_I
            )
        {
            this.items = new JsonHistorySimpleEntry[arrddvtvalue_I.Length];
            for (int intI = 0; intI < arrddvtvalue_I.Length; intI = intI + 1)
            {
                //                                          //To easy code
                DateDateValueTrio<TValue> ddvtrtarget = arrddvtvalue_I[intI];

                this.items[intI] = new JsonHistorySimpleEntry(ddvtrtarget.start, ddvtrtarget.end, ddvtrtarget.value);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
