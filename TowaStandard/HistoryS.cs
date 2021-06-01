/*TASK History Set of type to handle histories*/
using System;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz, LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: August 29, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public sealed class HistoryS<TOwner, TValue> : BsysAbstract
        where TOwner : Entity, new()
        where TValue : SerializableInterface<TValue>, new()
    {
        //                                                  //History & DateDateValueTrio are close related (like
        //                                                  //      Dictionary & KeyValuePair are close related).
        //                                                  //History<TOwner, TValue> is an ordered collection
        //                                                  //      of DateDateValueTrio<ReferenceTo<TValue>>.
        //                                                  //See also Life and HistoryWith.

        //                                                  //Example, collaborator's business object assignment.
        //                                                  //1. Declare and create business object assignment object
        //                                                  //      within collA object construction:
        //                                                  //[
        //                                                  //this.hixbo = new History<Coll, Bo>(this);
        //                                                  //]
        //                                                  //2. Add to "business object assignment history" a sequence
        //                                                  //      of entries:
        //                                                  //[
        //                                                  //this.hixbo.Add(date20180810, boX);
        //                                                  //this.subCloseAndAddBo(date20181201, boY);
        //                                                  //this.subCloseBo(date20181231);
        //                                                  //this.hixbo.Add(date20190201, BoX);
        //                                                  //]
        //                                                  //These means:
        //                                                  //1. collA is assigned boX on August 10, 2018 (probably the
        //                                                  //      date he/she was hired). This assignment will remain
        //                                                  //      the same until some other date it is changed (or
        //                                                  //      he/she leave the organization).
        //                                                  //2. collA is re-assigned to boY on December 1, 2018. 
        //                                                  //3. collA left the organization on December 31, 2018. 
        //                                                  //4. collA is re-hired and assigned to boX on February 1,
        //                                                  //      2019.
        //                                                  //5. Assignment is not defined before August 10, 2018
        //                                                  //      trying to get business object before this date will
        //                                                  //      fail.
        //                                                  //6. "History" information should be an instance
        //                                                  //      variable of an object that has a Life (example
        //                                                  //      <<20180810, 20181231>, <20190201, 99991231>>), or,
        //                                                  //      if not, Life will be assumed <<00010101, 99991231>>.
        //                                                  //6a. For each life period, in hixbo we need to have one
        //                                                  //      or more entries, start date in life period
        //                                                  //      should be exactly the date of first entry
        //                                                  //      that correspond to that period. (in example, first
        //                                                  //      life period has 2 enties and second life
        //                                                  //      period has just 1).
        //                                                  //6b. No history entry should cover a date outside life.
        //                                                  //6c. History information has no meening outside
        //                                                  //      collaborator life).

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public static readonly HistoryS<TOwner, TValue> DummyValue = new HistoryS<TOwner, TValue>();

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Entity with a "life" that own this "history".
        //                                                  //Example, "collA" that have defined this "hixbo".
        public readonly ReferenceTo<TOwner> rowner;

        //                                                  //History entries, sorted by Start.
        private DateDateValueTrio<TValue>[] arrddvtvalue;

        public ZonedTime ztimeLastUpdate;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTE VARIABLES*/

        public int Size { get { return this.arrddvtvalue.Length; } }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            String strObjId = Test.GetObjId(this);

            return strObjId + "[" + Test.ToLog(this.rowner) + ", " + Test.ToLog(this.arrddvtvalue) + "]";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            String strObjId = Test.GetObjId(this);

            return strObjId + "{" + Test.ToLog(this.rowner) + ", " + Test.GetNewLine() +
                Test.ToLog(this.arrddvtvalue, "arrddvtvalue", LogArrOptionEnum.VERTICAL) + ", " + Test.GetNewLine() +
                Test.ToLog(this.ztimeLastUpdate) + "}";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        private HistoryS() : base() { }

        //--------------------------------------------------------------------------------------------------------------
        public HistoryS(

            //                                              //Example collA. (collA contains hiscurrSalary)
            TOwner owner_I
            )
            : base()
        {
            Test.AbortIfNull(owner_I, "owner_I");

            //                                              //Get ReferenceTo owner in Table
            this.rowner = (ReferenceTo<TOwner>)(new TOwner()).GetReferenceToEntity(owner_I.primaryKey);

            this.arrddvtvalue = new DateDateValueTrio<TValue>[0];
            this.ztimeLastUpdate = ZonedTime.Now();
        }

        //--------------------------------------------------------------------------------------------------------------
        private HistoryS(
            //                                              //FOR EXCLUSIVE USE OF Deserialization. 

            ReferenceTo<TOwner> rowner_T,
            DateDateValueTrio<TValue>[] arrddvtvalue_T,
            ZonedTime ztimeLastUpdate_I
            )
            : base()
        {
            this.rowner = rowner_T;
            this.arrddvtvalue = arrddvtvalue_T;
            this.ztimeLastUpdate = ztimeLastUpdate_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*TRANSFORMATION METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public void Add(
            //                                              //Add a new entry (ddvtvalue).
            //                                              //If this entry IS NOT first in open life period, last
            //                                              //      entry should be closed.

            //                                              //Should be AFTER start of last Entry. (see class comments).
            //                                              //Should start on last start Entry or AFTER. (see class 
            //                                              //      comments).
            Date start_I,
            //                                              //HistoryWith that correspond to entry to add.
            //                                              //Example, for a hixbo within a collA, in boX we should have
            //                                              //      a hiwcollMembers (inside we have owner=boX, with is
            //                                              //      the business object to assign).
            TValue valueToAdd_I
            )
        {
            if (
                !this.rowner.v.lifeX.IsValidDateToAddHistory(start_I)
                )
                Test.Abort(
                    Test.ToLog(start_I, "start_I") + " is not a valid start date to add history, " +
                        "it is not within last owner's life (or life is not open)",
                    Test.ToLog(this.rowner.v.lifeX, "this.owner.v.lifeX"), Test.ToLog(this, "this"));
            Test.AbortIfNull(valueToAdd_I, "valueToAdd_I");

            //                                              //To easy code
            DateDateValueTrio<TValue> ddvtvalueLast = (this.arrddvtvalue.Length > 0) ?
                this.arrddvtvalue[this.arrddvtvalue.Length - 1] : default(DateDateValueTrio<TValue>);

            //                                              //Last entry (within open life period)
            if (
                (this.arrddvtvalue.Length > 0) &&
                //                                          //Last entry is also in open life period
                this.rowner.v.lifeX.IsValidDateToAddHistory(ddvtvalueLast.start) &&
                //                                          //Last entry is closed
                (ddvtvalueLast.end != Date.MaxValue)
                )
                Test.Abort(Test.ToLog(ddvtvalueLast, "ddvtvalueLast") + " should be open",
                    Test.ToLog(start_I, "start_I"), Test.ToLog(this.rowner.v.lifeX, "this.owner.v.lifeX"),
                    Test.ToLog(this, "this"));

            //                                              //Start should be ON or AFTER last start
            if (
                //                                          //Last entry is also in open life period
                (this.arrddvtvalue.Length > 0) &&
                //                                          //Entry to add is not in sequence
                (start_I < ddvtvalueLast.start)
                )
                Test.Abort(Test.ToLog(start_I, "start_I") + " should be ON or AFTER open life period",
                    Test.ToLog(ddvtvalueLast, "ddvtvalueLast"), Test.ToLog(this.rowner.v.lifeX, "this.owner.v.lifeX"),
                    Test.ToLog(this, "this"));

            //                                              //Close last entry if required
            if (
                (this.arrddvtvalue.Length > 0) &&
                //                                          //Last entry is also in open life period
                this.rowner.v.lifeX.IsValidDateToAddHistory(ddvtvalueLast.start)
                )
            {
                //                                          //Need to close last entry.
                this.Close(start_I - 1);
            }

            //                                              //Assign a copy of array & add new entry
            this.arrddvtvalue = this.arrddvtvalue.Copy(0, this.arrddvtvalue.Length + 1);
            this.arrddvtvalue[this.arrddvtvalue.Length - 1] =
                new DateDateValueTrio<TValue>(start_I, Date.MaxValue, valueToAdd_I);

            this.ztimeLastUpdate = ZonedTime.Now();
        }

        //--------------------------------------------------------------------------------------------------------------
        public void ReverseAdd(
            //                                              //Reverse Add a las entry.
            //                                              //Should be an open entry, not the first in life period.
            )
        {
            if (
                this.arrddvtvalue.Length == 0
                )
                Test.Abort(Test.ToLog(this.arrddvtvalue, "this.arrddvtvalue") + " has no entries",
                    Test.ToLog(this, "this"));

            //                                              //To easy code
            DateDateValueTrio<TValue> ddvtvalueLast = this.arrddvtvalue[this.arrddvtvalue.Length - 1];

            if (
                ddvtvalueLast.start == this.rowner.v.lifeX.GetLastPeriod().Start
                )
                Test.Abort(
                    Test.ToLog(ddvtvalueLast.start, "ddvtvalueLast.Start") +
                        " can not reverse add of first entry in open life period",
                    Test.ToLog(this.rowner.v.lifeX, "this.rowner.v.lifeX"), Test.ToLog(this, "this"));

            //                                              //To Reverse Add:
            //                                              //1. Remove last entry.
            //                                              //2. May require to Reverse Close of previous to last entry
            //                                              //      (will be last entry).

            //                                              //1. Remove last entry.
            this.arrddvtvalue = this.arrddvtvalue.Copy(0, this.arrddvtvalue.Length - 1);

            //                                              //Update timestamp before reverse adding history with
            this.ztimeLastUpdate = ZonedTime.Now();

            //                                              //2. May require Reverse Close of previous to last entry
            //                                              //      (will be last entry).

            //                                              //To easy code
            DateDateValueTrio<TValue> ddvtvalueNewLast = this.arrddvtvalue[this.arrddvtvalue.Length - 1];

            //                                              //May require to open last entry (was previous to last)
            if (
                (this.arrddvtvalue.Length > 0) &&
                //                                          //Last entry is also in open life period
                this.rowner.v.lifeX.IsValidDateToAddHistory(ddvtvalueNewLast.start)
                )
            {
                //                                          //Need to close last entry.
                this.ReverseClose();
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Close(
            //                                              //Close last History entry (ddvtvalue).
            //                                              //Close happen because:
            //                                              //a) Life closed, then all histories needs to close.
            //                                              //b) A new history entry is added, then last entry has to
            //                                              //      close.

            //                                              //Should be WITHIN last History Entry or, ONE DAY before
            //                                              //      this means, entry was canceled.
            //                                              //Ex. a collaborator was hired, but never show up, his life
            //                                              //      was closed before start and, all his histories.
            //                                              //Ex. a history entry was added to override last entry
            //                                              //      (starts the same date than previos entry).
            Date end_I
            )
        {
            if (
                end_I == Date.MaxValue
                )
                Test.Abort(Test.ToLog(end_I, "end_I") + " is not a valid End", Test.ToLog(this, "this"));

            if (
                this.arrddvtvalue.Length == 0
                )
                Test.Abort(Test.ToLog(this.arrddvtvalue, "this.arrddvtvalue") + " has no entries",
                    Test.ToLog(end_I, "end_I"), Test.ToLog(this, "this"));

            //                                              //To easy code
            DateDateValueTrio<TValue> ddvtvalueLast = this.arrddvtvalue[this.arrddvtvalue.Length - 1];

            if (
                ddvtvalueLast.end != Date.MaxValue
                )
                Test.Abort(Test.ToLog(ddvtvalueLast, "ddvtvalueLast") + " is not a open entry",
                    Test.ToLog(end_I, "end_I"), Test.ToLog(this, "this"));
            if (
                //                                          //Days in period is NEGATIVE
                (end_I - ddvtvalueLast.start + 1) < 0
                )
                Test.Abort(
                    Test.ToLog(end_I, "end_I") + " should be at most one day before " +
                        Test.ToLog(ddvtvalueLast.start, "ddvtvalueLast.Start"),
                    Test.ToLog(this, "this"));

            //                                              //Close last entry
            this.arrddvtvalue[this.arrddvtvalue.Length - 1] =
                new DateDateValueTrio<TValue>(ddvtvalueLast.start, end_I, ddvtvalueLast.value);

            //                                              //Update timestamp before updating history with
            this.ztimeLastUpdate = ZonedTime.Now();
        }

        //--------------------------------------------------------------------------------------------------------------
        public void ReverseClose(
            //                                              //Reverse Close last History entry (ddvtvalue).
            //                                              //Reverse Close happen because:
            //                                              //a) Life reverse close, then all histories needs to reverse
            //                                              //      close.
            //                                              //b) A history added, was reversed then previous entry
            //                                              //      (to be the last) should reverse close.
            )
        {
            if (
                this.arrddvtvalue.Length == 0
                )
                Test.Abort(Test.ToLog(this.arrddvtvalue, "this.arrddvtvalue") + " has no entries",
                    Test.ToLog(this, "this"));

            //                                              //To easy code
            DateDateValueTrio<TValue> ddvtvalueLast = this.arrddvtvalue[this.arrddvtvalue.Length - 1];

            if (
                //                                          //Enry to reverse close (to open again) is open
                ddvtvalueLast.end == Date.MaxValue
                )
                Test.Abort(Test.ToLog(ddvtvalueLast, "ddvtvalueLast") + " is not a close entry",
                    Test.ToLog(this, "this"));
            if (
                //                                          //Last entry IS NOT within open life period 
                !this.rowner.v.lifeX.IsValidDateToAddHistory(ddvtvalueLast.start)
                )
                Test.Abort(
                    Test.ToLog(ddvtvalueLast, "ddvtvalueLast") + " should be within open life period",
                    Test.ToLog(this.rowner.v.lifeX, "this.rowner.v.lifeX"));

            //                                              //Open last entry
            this.arrddvtvalue[this.arrddvtvalue.Length - 1] = new DateDateValueTrio<TValue>(
                ddvtvalueLast.start, Date.MaxValue, ddvtvalueLast.value);

            //                                              //Update timestamp before updating history with
            this.ztimeLastUpdate = ZonedTime.Now();
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public DateDateValueTrio<TValue> GetEntry(
            //                                              //Lookup for entry (ddvtvalue) corresponding to date.
            //                                              //ddvtvalue, Entry corresponding to date, but should
            //                                              //      include the Entity (hiw.owner)

            Date date_I
            )
        {
            //                                              //Search backward
            int intX = this.arrddvtvalue.Length - 1;
            /*UNTIL-DO*/
            while (!(
                //                                          //There are no more history entries to check.
                (intX < 0) ||
                //                                          //Histroy entry has been found.
                (this.arrddvtvalue[intX].start <= date_I)
                ))
            {
                intX = intX - 1;
            }

            if (
                (intX < 0) ||
                //                                          //Is not within entry
                (this.arrddvtvalue[intX].end < date_I)
                )
                Test.Abort(Test.ToLog(date_I, "date_I") + " is not a history entry for date",
                    Test.ToLog(this.arrddvtvalue, "this.arrddvtvalue"));


            return this.arrddvtvalue[intX];
        }

        //--------------------------------------------------------------------------------------------------------------
        public TValue GetValue(
            //                                              //Lookup for history entry (ddvtvalue) corresponding to a
            //                                              //      date.
            //                                              //value, corresponding to history entry

            Date date_I
            )
        {
            return this.GetEntry(date_I).value;
        }

        //--------------------------------------------------------------------------------------------------------------
        public TValue GetOpenValue(
            //                                              //Lookup for OPEN (should be the last one) history entry 
            //                                              //      (ddvtvalue).
            //                                              //value, corresponding to that entry
            )
        {
            //                                              //To easy code
            DateDateValueTrio<TValue> ddvtvalueLast = this.arrddvtvalue[this.arrddvtvalue.Length - 1];

            if (
                ddvtvalueLast.end != Date.MaxValue
                )
                Test.Abort(Test.ToLog(ddvtvalueLast, "ddvtvalueLast") + " should be an open entry",
                    Test.ToLog(this, "this"));

            return ddvtvalueLast.value;
        }

        //--------------------------------------------------------------------------------------------------------------
        public DateDateValueTrio<TValue>[] GetAllEntries(
            //                                              //arrddvtvalue, all Entries, but should include the
            //                                              //      Entity (hiw.owner)
            )
        {
            //                                              //Need to create a copy
            DateDateValueTrio<TValue>[] arrddvtvalueToReturn =
                new DateDateValueTrio<TValue>[this.arrddvtvalue.Length];
            Array.Copy(this.arrddvtvalue, 0, arrddvtvalueToReturn, 0, this.arrddvtvalue.Length);

            return arrddvtvalueToReturn;
        }

        //--------------------------------------------------------------------------------------------------------------
        public byte[] Serialize(
            //                                              //Returns a serialized version of the object.
            )
        {
            byte[] arrbyteRowner = this.rowner.Serialize();
            byte[] arrbyteArrddvtvalue = this.arrddvtvalue.Serialize();
            byte[] arrbyteZtimeLastUpdate = this.ztimeLastUpdate.Serialize();

            return Std.ConcatenateArrays(arrbyteRowner, arrbyteArrddvtvalue, arrbyteZtimeLastUpdate);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void CreateEmptyOrDeserialize(
            //                                              //Returns a history object.
            //                                              //For some tests, to mean empty history, it was easy accept
            //                                              //      bytes_IO = null.

            //                                              //Paramenter required to construct an empty history with
            TOwner ownerBelongTo_I,
            //                                              //The object created or deserialized.
            out HistoryS<TOwner, TValue> history_O,
            //                                              //The serialized bytes.
            //                                              //initial value null means an empty history
            ref byte[] bytes_IO
            )
        {
            if (
                //                                          //null means an empty history
                bytes_IO == null
                )
            {
                history_O = new HistoryS<TOwner, TValue>(ownerBelongTo_I);
            }
            else
            {
                HistoryS<TOwner, TValue>.DummyValue.Deserialize(out history_O, ref bytes_IO);

                if (
                    bytes_IO.Length > 0
                    )
                    Test.Abort(Test.ToLog(bytes_IO, "bytes_IO") + " was not used in bytes_IO deserialization",
                        Test.ToLog(history_O, "history_O"));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Deserialize(
            //                                              //Returns a deserialized object.

            //                                              //The object to deserialize.
            out HistoryS<TOwner, TValue> history_O,
            //                                              //The serialized bytes.
            ref byte[] bytes_IO
            )
        {
            ReferenceTo<TOwner> rownerDeserialized;
            (new ReferenceTo<TOwner>()).Deserialize(out rownerDeserialized, ref bytes_IO);

            DateDateValueTrio<TValue>[] arrddvtvalueDeserialized;
            Std.DeserializeArray(out arrddvtvalueDeserialized, ref bytes_IO, new DateDateValueTrio<TValue>());

            ZonedTime timeLastUpdateDeserialized;
            ZonedTime.DummyValue.Deserialize(out timeLastUpdateDeserialized, ref bytes_IO);

            history_O = new HistoryS<TOwner, TValue>(rownerDeserialized, arrddvtvalueDeserialized,
                timeLastUpdateDeserialized);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
