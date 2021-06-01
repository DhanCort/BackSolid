/*TASK Database Database All-in-Memory*/
using System;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz, LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: October 22, 2019.

namespace TowaStandard
{
    //==================================================================================================================
    public sealed class JsonHistoryEntity<TOwner, TTarget> : JsonAbstract
        where TOwner : Entity, new()
        where TTarget : Entity, new()
    {
        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        public JsonHistoryEntityEntry[] items;

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
        public JsonHistoryEntity(

            HistoryCrossed<TOwner, TTarget> historyCrossed_I
            )
            : base()
        {
            this.items = this.arrjsonGetItems(historyCrossed_I.GetAllEntries());
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public JsonHistoryEntity(

            HistoryWith<TOwner, TTarget> historyWith_I
            )
            : base()
        {
            this.items = this.arrjsonGetItems(historyWith_I.GetAllEntries());
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public JsonHistoryEntity(

            HistoryE<TOwner, TTarget> historyE_I
            )
            : base()
        {
            this.items = this.arrjsonGetItems(historyE_I.GetAllEntries());
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private JsonHistoryEntityEntry[] arrjsonGetItems(

            DateDateValueTrio<ReferenceTo<TTarget>>[] arrddvtrtarget_I
            )
        {
            JsonHistoryEntityEntry[] arrjsonGetItems = new JsonHistoryEntityEntry[arrddvtrtarget_I.Length];
            for (int intI = 0; intI < arrddvtrtarget_I.Length; intI = intI + 1)
            {
                //                                          //To easy code
                DateDateValueTrio<ReferenceTo<TTarget>> ddvtrtarget = arrddvtrtarget_I[intI];

                arrjsonGetItems[intI] = new JsonHistoryEntityEntry(ddvtrtarget.start, ddvtrtarget.end,
                    ddvtrtarget.value.primaryKey, ddvtrtarget.value.v.key.ToString());
            }

            return arrjsonGetItems;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
