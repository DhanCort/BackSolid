/*TASK History Set of type to handle histories*/
using System;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz, LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: August 29, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public sealed class HistoryE<TOwner, TTarget> : BsysAbstract
        where TOwner : Entity, new()
        where TTarget : Entity, new()
    {
        //                                                  //HistoryEntity & DateDateValueTrio are close related (like
        //                                                  //      Dictionary & KeyValuePair are close related).
        //                                                  //HistoryEntity<TOwner, TTarget> is an ordered collection
        //                                                  //      of DateDateValueTrio<ReferenceTo<TTarget>>.
        //                                                  //See also Life.
        //                                                  //HistoryEntity is similar to HistoryCross but, it TTarget
        //                                                  //      wants to know his relationship with an owner he
        //                                                  //      needs to findout.

        //                                                  //Example, organization's manager.
        //                                                  //1. Declare and create manager object within orgA object
        //                                                  //      construction:
        //                                                  //[
        //                                                  //this.hiscollManager = new HistoryEntity<Org, Coll>(this);
        //                                                  //]
        //                                                  //2. Add to "manager history" a sequence of entries:
        //                                                  //[
        //                                                  //this.hiscollManager.Add(date20180810, collX);
        //                                                  //this.hiscollManager.Add(date20181201, collY);
        //                                                  //this.hiscollManager.Close(date20181231);
        //                                                  //this.hiscollManager.Add(date20190201, collX);
        //                                                  //]
        //                                                  //These means:
        //                                                  //1. orgA is management is assigned to collX  August 10,
        //                                                  //      2018 (probably the date organization was created),
        //                                                  //      This manager will be the same until some other date
        //                                                  //      it is changed (or the organization ends operations).
        //                                                  //2. orgA management is assigned to collY on December 1,
        //                                                  //      2018. 
        //                                                  //3. orgA cease to operate on December 31, 2018. 
        //                                                  //4. orgA is re-open for operations and management is
        //                                                  //      assigned to collX on February 1, 2019.
        //                                                  //5. Management is not defined before August 10, 2018.
        //                                                  //6. "HistoryEntity" information should be an instance
        //                                                  //      variable of an object that has a Life (example
        //                                                  //      <<20180810, 20181231>, <20190201, 99991231>>), or,
        //                                                  //      if not, Life will be assumed <<00010101, 99991231>>.
        //                                                  //6a. For each life period, in hiscollManager we need to
        //                                                  //      have one or more entries, start date in life period
        //                                                  //      should be exactly the date of first entry that
        //                                                  //      correspond to that period. (in example, first life
        //                                                  //      period has 2 enties and second life period has just
        //                                                  //      1).
        //                                                  //6b. No history entry should cover a date outside life.
        //                                                  //6c. HistoryEntity information has no meening outside
        //                                                  //      organization life).

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public static readonly HistoryE<TOwner, TTarget> DummyValue = new HistoryE<TOwner, TTarget>();

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Entity with a "life" that own this "history entity".
        //                                                  //Example, "collA" that have defined this "hixbo".
        public readonly ReferenceTo<TOwner> rowner;

        //                                                  //History entries, sorted by Start.
        private DateDateValueTrio<ReferenceTo<TTarget>>[] arrddvtrtarget;

        public ZonedTime ztimeLastUpdate;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTE VARIABLES*/

        public int Size { get { return this.arrddvtrtarget.Length; } }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            String strObjId = Test.GetObjId(this);

            return strObjId + "[" + Test.ToLog(this.rowner) + ", " + Test.ToLog(this.arrddvtrtarget) + "]";
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            String strObjId = Test.GetObjId(this);

            return strObjId + "{" + Test.ToLog(this.rowner) + ", " + Test.GetNewLine() +
                Test.ToLog(this.arrddvtrtarget, "arrddvtrtarget", LogArrOptionEnum.VERTICAL) + ", " + 
                Test.GetNewLine() + Test.ToLog(this.ztimeLastUpdate) + "}";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        private HistoryE() : base() { }

        //--------------------------------------------------------------------------------------------------------------
        public HistoryE(

            //                                              //Example orgA. (orgA contains hiscollManager)
            TOwner owner_I
            )
            : base()
        {
            Test.AbortIfNull(owner_I, "owner_I");

            //                                              //Get ReferenceTo owner in Table
            this.rowner = (ReferenceTo<TOwner>)owner_I.GetReferenceToEntity(owner_I.primaryKey);

            this.arrddvtrtarget = new DateDateValueTrio<ReferenceTo<TTarget>>[0];
            this.ztimeLastUpdate = ZonedTime.Now();
        }

        //--------------------------------------------------------------------------------------------------------------
        private HistoryE(
            //                                              //FOR EXCLUSIVE USE OF Deserialization. 

            ReferenceTo<TOwner> rowner_T,
            DateDateValueTrio<ReferenceTo<TTarget>>[] arrddvtrtarget_T,
            ZonedTime ztimeLastUpdate_I
            )
            : base()
        {
            this.rowner = rowner_T;
            this.arrddvtrtarget = arrddvtrtarget_T;
            this.ztimeLastUpdate = ztimeLastUpdate_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*TRANSFORMATION METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public void Add(
            //                                              //Add a new entry (ddvtrtarget).
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
            TTarget targetToAdd_I
            )
        {
            if (
                !this.rowner.v.lifeX.IsValidDateToAddHistory(start_I)
                )
                Test.Abort(
                    Test.ToLog(start_I, "start_I") + " is not a valid start date to add history, " +
                        "it is not within last owner's life (or life is not open)",
                    Test.ToLog(this.rowner.v.lifeX, "this.rowner.v.lifeX"), Test.ToLog(this, "this"));
            Test.AbortIfNull(targetToAdd_I, "targetToAdd_I");
            if (
                !targetToAdd_I.lifeX.IsValidDateToAddHistory(start_I)
                )
                Test.Abort(
                    Test.ToLog(start_I, "start_I") + " is not a valid start date to add history, " +
                        "it is not within last target's life (or life is not open)",
                    Test.ToLog(targetToAdd_I.lifeX, "targetToAdd_I.lifeX"), Test.ToLog(this, "this"));

            //                                              //To easy code
            DateDateValueTrio<ReferenceTo<TTarget>> ddvtrtargetLast = (this.arrddvtrtarget.Length > 0) ?
                this.arrddvtrtarget[this.arrddvtrtarget.Length - 1] : default(DateDateValueTrio<ReferenceTo<TTarget>>);

            //                                              //Last entry (within open life period)
            if (
                (this.arrddvtrtarget.Length > 0) &&
                //                                          //Last entry is also in open life period
                this.rowner.v.lifeX.IsValidDateToAddHistory(ddvtrtargetLast.start) &&
                //                                          //Last entry is closed
                (ddvtrtargetLast.end != Date.MaxValue)
                )
                Test.Abort(Test.ToLog(ddvtrtargetLast, "ddvtrtargetLast") + " should be open",
                    Test.ToLog(start_I, "start_I"), Test.ToLog(this.rowner.v.lifeX, "this.owner.v.lifeX"),
                    Test.ToLog(this, "this"));

            //                                              //Start should be ON or AFTER last start
            if (
                //                                          //Last entry is also in open life period
                (this.arrddvtrtarget.Length > 0) &&
                //                                          //Entry to add is not in sequence
                (start_I < ddvtrtargetLast.start)
                )
                Test.Abort(Test.ToLog(start_I, "start_I") + " should be ON or AFTER open life period",
                    Test.ToLog(ddvtrtargetLast, "ddvtrtargetLast"), Test.ToLog(this.rowner.v.lifeX, "this.rowner.v.lifeX"),
                    Test.ToLog(this, "this"));

            //                                              //Close last entry if required
            if (
                (this.arrddvtrtarget.Length > 0) &&
                //                                          //Last entry is also in open life period
                this.rowner.v.lifeX.IsValidDateToAddHistory(ddvtrtargetLast.start)
                )
            {
                //                                          //Need to close last entry.
                this.Close(start_I - 1);
            }

            //                                              //Assign a copy of array & add new entry
            this.arrddvtrtarget = this.arrddvtrtarget.Copy(0, this.arrddvtrtarget.Length + 1);
            ReferenceTo<TTarget> rtarget = (ReferenceTo<TTarget>)targetToAdd_I.GetReferenceToEntity(
                targetToAdd_I.primaryKey);
            this.arrddvtrtarget[this.arrddvtrtarget.Length - 1] =
                new DateDateValueTrio<ReferenceTo<TTarget>>(start_I, Date.MaxValue, rtarget);

            this.ztimeLastUpdate = ZonedTime.Now();
        }

        //--------------------------------------------------------------------------------------------------------------
        public void ReverseAdd(
            //                                              //Reverse Add a las entry.
            //                                              //Should be an open entry, not the first in life period.
            )
        {
            if (
                this.arrddvtrtarget.Length == 0
                )
                Test.Abort(Test.ToLog(this.arrddvtrtarget, "this.arrddvtvalue") + " has no entries",
                    Test.ToLog(this, "this"));

            //                                              //To easy code
            DateDateValueTrio<ReferenceTo<TTarget>> ddvtrtargetLast =
                this.arrddvtrtarget[this.arrddvtrtarget.Length - 1];

            if (
                ddvtrtargetLast.start == this.rowner.v.lifeX.GetLastPeriod().Start
                )
                Test.Abort(
                    Test.ToLog(ddvtrtargetLast.start, "ddvtrtargetLast.Start") + 
                        " can not reverse add of first entry in open life period",
                    Test.ToLog(this.rowner.v.lifeX, "this.rowner.v.lifeX"), Test.ToLog(this, "this"));

            //                                              //To Reverse Add:
            //                                              //1. Remove last entry.
            //                                              //2. May require to Reverse Close of previous to last entry
            //                                              //      (will be last entry).

            //                                              //1. Remove last entry.
            this.arrddvtrtarget = this.arrddvtrtarget.Copy(0, this.arrddvtrtarget.Length - 1);

            //                                              //Update timestamp before reverse adding history with
            this.ztimeLastUpdate = ZonedTime.Now();

            //                                              //2. May require Reverse Close of previous to last entry
            //                                              //      (will be last entry).

            //                                              //To easy code
            DateDateValueTrio<ReferenceTo<TTarget>> ddvtrtargetNewLast =
                this.arrddvtrtarget[this.arrddvtrtarget.Length - 1];

            //                                              //May require to open last entry (was previous to last)
            if (
                (this.arrddvtrtarget.Length > 0) &&
                //                                          //Last entry is also in open life period
                this.rowner.v.lifeX.IsValidDateToAddHistory(ddvtrtargetNewLast.start)
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
                this.arrddvtrtarget.Length == 0
                )
                Test.Abort(Test.ToLog(this.arrddvtrtarget, "this.arrddvtvalue") + " has no entries",
                    Test.ToLog(end_I, "end_I"), Test.ToLog(this, "this"));

            //                                              //To easy code
            DateDateValueTrio<ReferenceTo<TTarget>> ddvtrtargetLast =
                this.arrddvtrtarget[this.arrddvtrtarget.Length - 1];

            if (
                ddvtrtargetLast.end != Date.MaxValue
                )
                Test.Abort(Test.ToLog(ddvtrtargetLast, "ddvtrtargetLast") + " is not a open entry",
                    Test.ToLog(end_I, "end_I"), Test.ToLog(this, "this"));
            if (
                //                                          //Days in period is NEGATIVE
                (end_I - ddvtrtargetLast.start + 1) < 0
                )
                Test.Abort(
                    Test.ToLog(end_I, "end_I") + " should be at most one day before " + 
                        Test.ToLog(ddvtrtargetLast.start, "ddvtrtargetLast.Start"),
                    Test.ToLog(this, "this"));

            //                                              //Close last entry
            this.arrddvtrtarget[this.arrddvtrtarget.Length - 1] =
                new DateDateValueTrio<ReferenceTo<TTarget>>(ddvtrtargetLast.start, end_I, ddvtrtargetLast.value);

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
                this.arrddvtrtarget.Length == 0
                )
                Test.Abort(Test.ToLog(this.arrddvtrtarget, "this.arrddvtvalue") + " has no entries",
                    Test.ToLog(this, "this"));

            //                                              //To easy code
            DateDateValueTrio<ReferenceTo<TTarget>> ddvtrtargetLast =
                this.arrddvtrtarget[this.arrddvtrtarget.Length - 1];

            if (
                //                                          //Enry to reverse close (to open again) is open
                ddvtrtargetLast.end == Date.MaxValue
                )
                Test.Abort(Test.ToLog(ddvtrtargetLast, "ddvtrtargetLast") + " is not a close entry",
                    Test.ToLog(this, "this"));
            if (
                //                                          //Last entry IS NOT within open life period 
                !this.rowner.v.lifeX.IsValidDateToAddHistory(ddvtrtargetLast.start)
                )
                Test.Abort(
                    Test.ToLog(ddvtrtargetLast, "ddvtrtargetLast") + " should be within open life period",
                    Test.ToLog(this.rowner.v.lifeX, "this.rowner.v.lifeX"));

            //                                              //Open last entry
            this.arrddvtrtarget[this.arrddvtrtarget.Length - 1] = new DateDateValueTrio<ReferenceTo<TTarget>>(
                ddvtrtargetLast.start, Date.MaxValue, ddvtrtargetLast.value);

            //                                              //Update timestamp before updating history with
            this.ztimeLastUpdate = ZonedTime.Now();
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public DateDateValueTrio<ReferenceTo<TTarget>> GetEntry(
            //                                              //Lookup for entry (ddvtrtarget) corresponding to date.
            //                                              //ddvtrtarget, Entry corresponding to date, but should
            //                                              //      include the Entity (hiw.owner)

            Date date_I
            )
        {
            //                                              //Search backward
            int intX = this.arrddvtrtarget.Length - 1;
            /*UNTIL-DO*/
            while (!(
                //                                          //There are no more history entries to check.
                (intX < 0) ||
                //                                          //Histroy entry has been found.
                (this.arrddvtrtarget[intX].start <= date_I)
                ))
            {
                intX = intX - 1;
            }

            if (
                (intX < 0) ||
                //                                          //Is not within entry
                (this.arrddvtrtarget[intX].end < date_I)
                )
                Test.Abort(Test.ToLog(date_I, "date_I") + " is not a history entry for date",
                    Test.ToLog(this.arrddvtrtarget, "this.arrddvtvalue"));


            return this.arrddvtrtarget[intX];
        }

        //--------------------------------------------------------------------------------------------------------------
        public ReferenceTo<TTarget> GetTarget(
            //                                              //Lookup for history entry (ddvtrtarget) corresponding to a
            //                                              //      date.
            //                                              //rtarget, corresponding to history entry

            Date date_I
            )
        {
            return this.GetEntry(date_I).value;
        }

        //--------------------------------------------------------------------------------------------------------------
        public ReferenceTo<TTarget> GetOpenTarget(
            //                                              //Lookup for OPEN (should be the last one) history entry 
            //                                              //      (ddvtrtarget).
            //                                              //rtarget, corresponding to that entry
            )
        {
            //                                              //To easy code
            DateDateValueTrio<ReferenceTo<TTarget>> ddvtrtargetLast =
                this.arrddvtrtarget[this.arrddvtrtarget.Length - 1];

            if (
                ddvtrtargetLast.end != Date.MaxValue
                )
                Test.Abort(Test.ToLog(ddvtrtargetLast, "ddvtrtargetLast") + " should be an open entry",
                    Test.ToLog(this, "this"));

            return ddvtrtargetLast.value;
        }

        //--------------------------------------------------------------------------------------------------------------
        public DateDateValueTrio<ReferenceTo<TTarget>>[] GetAllEntries(
            //                                              //arrddvtrtarget, all Entries, but should include the
            //                                              //      Entity (hiw.owner)
            )
        {
            //                                              //Need to create a copy
            DateDateValueTrio<ReferenceTo<TTarget>>[] arrddvtrtargetToReturn = 
                new DateDateValueTrio<ReferenceTo<TTarget>>[this.arrddvtrtarget.Length];
            Array.Copy(this.arrddvtrtarget, 0, arrddvtrtargetToReturn, 0, this.arrddvtrtarget.Length);

            return arrddvtrtargetToReturn;
        }

        //--------------------------------------------------------------------------------------------------------------
        public byte[] Serialize(
            //                                              //Returns a serialized version of the object.
            )
        {
            byte[] arrbyteRowner = this.rowner.Serialize();
            byte[] arrbyteArrddvtrtarget = this.arrddvtrtarget.Serialize();
            byte[] arrbyteZtimeLastUpdate = this.ztimeLastUpdate.Serialize();

            return Std.ConcatenateArrays(arrbyteRowner, arrbyteArrddvtrtarget, arrbyteZtimeLastUpdate);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void CreateEmptyOrDeserialize(
            //                                              //Returns a history object.
            //                                              //For some tests, to mean empty history, it was easy accept
            //                                              //      bytes_IO = null.

            //                                              //Paramenter required to construct an empty history entity
            TOwner ownerBelongTo_I,
            //                                              //The object created or deserialized.
            out HistoryE<TOwner, TTarget> historyEntity_O,
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
                historyEntity_O = new HistoryE<TOwner, TTarget>(ownerBelongTo_I);
            }
            else
            {
                HistoryE<TOwner, TTarget>.DummyValue.Deserialize(out historyEntity_O, ref bytes_IO);

                if (
                    bytes_IO.Length > 0
                    )
                    Test.Abort(Test.ToLog(bytes_IO, "bytes_IO") + " was not used in bytes_IO deserialization",
                        Test.ToLog(historyEntity_O, "historyEntity_O"));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Deserialize(
            //                                              //Returns a deserialized object.

            //                                              //The object to deserialize.
            out HistoryE<TOwner, TTarget> historyEntity_O,
            //                                              //The serialized bytes.
            ref byte[] bytes_IO
            )
        {
            ReferenceTo<TOwner> rownerDeserialized;
            (new ReferenceTo<TOwner>()).Deserialize(out rownerDeserialized, ref bytes_IO);

            DateDateValueTrio<ReferenceTo<TTarget>>[] arrddvtrtargetDeserialized;
            Std.DeserializeArray(out arrddvtrtargetDeserialized, ref bytes_IO,
                new DateDateValueTrio<ReferenceTo<TTarget>>());

            ZonedTime timeLastUpdateDeserialized;
            ZonedTime.DummyValue.Deserialize(out timeLastUpdateDeserialized, ref bytes_IO);

            historyEntity_O = new HistoryE<TOwner, TTarget>(rownerDeserialized, arrddvtrtargetDeserialized,
                timeLastUpdateDeserialized);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
