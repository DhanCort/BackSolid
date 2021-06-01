/*TASK History Set of type to handle histories*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz, LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: August 29, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public sealed class HistoryWith<TOwner, TTarget> : BsysAbstract
        where TOwner : Entity, new()
        where TTarget : Entity, new()
    {
        //                                                  //HistoryWith & DateDateValueTrio are close related (like
        //                                                  //      Dictionary & KeyValuePair are close related).
        //                                                  //HistoryWith<TOwner, TValue> is an ordered collection of
        //                                                  //      DateDateValueTrio<TValue>.
        //                                                  //See also Life and HistoryCrossed.

        //                                                  //Example 1, each collaborator hixbo ____________________
        //                                                  //      (HistoryCrossed<Coll, Bo>) witch holds his bo
        //                                                  //      assignment in sequence, but also each bo needs to
        //                                                  //      know how is assigned.
        //                                                  //1. In business object declare and create "members"
        //                                                  //      object:
        //                                                  //[
        //                                                  //HistoryWith<Bo, Coll> hiwcollMembers = _______________
        //                                                  //      new HistoryWith<Bo, Coll>;
        //                                                  //]
        //                                                  //2. Add to "members" object each collaborator
        //                                                  //      (boX.hiwcollMembers.Add(collA)) at the SAME TIME
        //                                                  //      he/she is assigned to a "business object"
        //                                                  //      (collA.hixbo.Add(boX)), this will form an ordered
        //                                                  //      sequence of <dateStartA, dateEndA, collA>,
        //                                                  //      <dateStartB, dateEndB, collB>, ..., ____________
        //                                                  //      <dateStartN, dateEndN, collN>, where each
        //                                                  //      "members entry" is a
        //                                                  //      DateDateValueTrio<Coll>:
        //                                                  //[
        //                                                  //hiwcollMembers.Add(dateStartA, dateEndA, collA);
        //                                                  //hiwcollMembers.Add(dateStartB, dateEndB, collB);
        //                                                  //...
        //                                                  //hiwcollMembers.Add(dateStartN, dateEndN, collN);
        //                                                  //]
        //                                                  //These means:
        //                                                  //1. This object contains ALL collaborators assigned to
        //                                                  //      these business object, each entry includes the
        //                                                  //      period he/she was assigned.
        //                                                  //2. Many collaborators are assigned at any time.

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public static readonly HistoryWith<TOwner, TTarget> DummyValue = new HistoryWith<TOwner, TTarget>();

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Entity with a "life" that own this "history with".
        //                                                  //Example, "bo" that have defined a hiwcollMembers.
        public readonly ReferenceTo<TOwner> rowner; 

        //                                                  //History entries, sorted by End & Start. (End before 
        //                                                  //      Start).
        //                                                  //This order (End+Start) is useful to the keep most recent
        //                                                  //      entries at the end of array.
        private DateDateValueTrio<ReferenceTo<TTarget>>[] arrddvtrtarget;

        //                                                  //This information will be used to verity that
        //                                                  //      HistoryWith is updated inmediately after
        //                                                  //      HistoryCrossed update.
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
                Test.ToLog(this.arrddvtrtarget, "this.arrddvtvalue", LogArrOptionEnum.VERTICAL) + ", " + 
                Test.GetNewLine() + Test.ToLog(this.ztimeLastUpdate) + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        private HistoryWith() : base() { }

        //--------------------------------------------------------------------------------------------------------------
        public HistoryWith(

            //                                              //Example boX. (will be the owner of hiwcollMembers)
            TOwner Owner_I
            )
            : base()
        {
            Test.AbortIfNull(Owner_I, "Owner_I");

            //                                              //Get ReferenceTo owner in Table
            this.rowner = (ReferenceTo<TOwner>)Owner_I.GetReferenceToEntity(Owner_I.primaryKey);

            this.arrddvtrtarget = new DateDateValueTrio<ReferenceTo<TTarget>>[0];
            this.ztimeLastUpdate = ZonedTime.Now();
        }

        //--------------------------------------------------------------------------------------------------------------
        private HistoryWith(
            //                                              //FOR EXCLUSIVE USE OF Deserialization. 

            ReferenceTo<TOwner> ReferenceToOwner_T,
            DateDateValueTrio<ReferenceTo<TTarget>>[] arrddvtrtarget_T,
            ZonedTime ztimeLastUpdate_I
            )
            : base()
        {
            this.rowner = ReferenceToOwner_T;
            this.arrddvtrtarget = arrddvtrtarget_T;
            this.ztimeLastUpdate = ztimeLastUpdate_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*TRANSFORMATION METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public void z_TowaPRIVATE_subAdd(
            //                                              //SHOULD BE EXECUTED ONLY BY HistoryCrossed class.
            //                                              //Add a new Entry (ddvtrtarget).
            //                                              //Should add keeping sequenceat the end.

            Date dateStart_I,
            HistoryCrossed<TTarget, TOwner> hixownerToAdd_T
            )
        {
            //                                              //No fuses required, was call from HistoryCross

            //                                              //Copy opening space for one more entry, add new entry &
            //                                              //      sort
            this.arrddvtrtarget = this.arrddvtrtarget.Copy(0, this.arrddvtrtarget.Length + 1);
            this.arrddvtrtarget[this.arrddvtrtarget.Length - 1] = 
                new DateDateValueTrio<ReferenceTo<TTarget>>(dateStart_I, Date.MaxValue, hixownerToAdd_T.rowner);
            this.subSortArrddvtrtargetEndPlusStart();

            //                                              //Update timestamp and verify
            this.ztimeLastUpdate = ZonedTime.Now();

            this.subAbortIfNotInmediateAfterHixLastUpdate(hixownerToAdd_T.ztimeLastUpdate);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void z_TowaPRIVATE_subReverseAdd(
            //                                              //SHOULD BE EXECUTED ONLY BY HistoryCrossed class.
            //                                              //Add a new Entry (ddvtrtarget).
            //                                              //Should add at the end.

            Date dateStart_I, 
            HistoryCrossed<TTarget, TOwner> hixownerToReverseAdd_T
            )
        {
            //                                              //No fuses required, was call from HistoryCross

            //                                              //Look for position
            int intIndex = this.intSearchEntry(
                new DateDateValueTrio<ReferenceTo<TTarget>>(dateStart_I, Date.MaxValue, hixownerToReverseAdd_T.rowner));


            //                                              //Separate array.
            DateDateValueTrio<ReferenceTo<TTarget>>[] arrddvtrtargetA = this.arrddvtrtarget.Copy(0, intIndex);
            DateDateValueTrio<ReferenceTo<TTarget>>[] arrddvtrtargetB =
                this.arrddvtrtarget.Copy(intIndex + 1, this.arrddvtrtarget.Length - (intIndex + 1));

            this.arrddvtrtarget = Std.ConcatenateArrays(arrddvtrtargetA, arrddvtrtargetB);

            //                                              //Update timestamp and verify
            this.ztimeLastUpdate = ZonedTime.Now();

            this.subAbortIfNotInmediateAfterHixLastUpdate(hixownerToReverseAdd_T.ztimeLastUpdate);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void z_TowaPRIVATE_subClose(
            //                                              //SHOULD BE EXECUTED ONLY BY HistoryCrossed class.
            //                                              //CLose Entry (entry should be open).
            //                                              //Entry to close should have same Start & Value

            //                                              //Start <= End (period include 1 day or many)
            Date dateStart_I,
            Date dateEnd_I,
            HistoryCrossed<TTarget, TOwner> hixownerToUpdate_T
            )
        {
            //                                              //No fuses required, was call from HistoryCross

            //                                              //Look for position
            int intIndex = this.intSearchEntry(
                new DateDateValueTrio<ReferenceTo<TTarget>>(dateStart_I, Date.MaxValue, hixownerToUpdate_T.rowner));

            //                                              //Close entry and sort
            this.arrddvtrtarget[intIndex] = 
                new DateDateValueTrio<ReferenceTo<TTarget>>(dateStart_I, dateEnd_I, hixownerToUpdate_T.rowner);
            this.subSortArrddvtrtargetEndPlusStart();

            //                                              //Update timestamp and verify
            this.ztimeLastUpdate = ZonedTime.Now();

            this.subAbortIfNotInmediateAfterHixLastUpdate(hixownerToUpdate_T.ztimeLastUpdate);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private int intSearchEntry(

            DateDateValueTrio<ReferenceTo<TTarget>> ddvtrtargetToSearch_I
            )
        {
            //                                              //Find entry to remove, search backward:
            int intSearch = this.arrddvtrtarget.Length - 1;

            //                                              //1. Look for End-Start dates.
            /*UNTIL-DO*/
            while (!(
                //                                          //There is no more entries to check.
                (intSearch < 0) ||
                //                                          //End+Start position found
                (this.longEndPlusStart(this.arrddvtrtarget[intSearch]) <= this.longEndPlusStart(ddvtrtargetToSearch_I))
                ))
            {
                intSearch = intSearch - 1;
            }

            //                                              //2. In End+Start date, look for Value.
            /*UNTIL-DO*/
            while (!(
                //                                          //There is no more entries to check.
                (intSearch < 0) ||
                //                                          //End-Start dates is over
                (this.longEndPlusStart(this.arrddvtrtarget[intSearch]) < this.longEndPlusStart(ddvtrtargetToSearch_I)) ||
                //                                          //Entry found
                this.arrddvtrtarget[intSearch].value.Equals(ddvtrtargetToSearch_I.value)
                ))
            {
                intSearch = intSearch - 1;
            }

            if (
                (intSearch < 0) ||
                //                                          //End-Start dates is over
                (this.longEndPlusStart(this.arrddvtrtarget[intSearch]) < this.longEndPlusStart(ddvtrtargetToSearch_I))
                )
                Test.Abort(Test.z_TowaPRIVATE_ToLogXT(ddvtrtargetToSearch_I, "ddvtvalueToRemove") + " not found",
                    Test.ToLog(this, "this"));

            return intSearch;
        }

        //--------------------------------------------------------------------------------------------------------------
        public void z_TowaPRIVATE_subReverseClose(
            //                                              //SHOULD BE EXECUTED ONLY BY HistoryCrossed class.
            //                                              //CLose Entry (entry should have an end date).
            //                                              //Entry to close should have same Start & Value

            Date dateStart_I,
            HistoryCrossed<TTarget, TOwner> hixownerToUpdate_T
            )
        {
            //                                              //No fuses required, was call from HistoryCross

            //                                              //Search backward for entry
            int intSearch = this.arrddvtrtarget.Length - 1;
            /*UNTIL-DO*/
            while (!(
                (intSearch < 0) ||
                (this.arrddvtrtarget[intSearch].start == dateStart_I) &&
                    this.arrddvtrtarget[intSearch].value.Equals(hixownerToUpdate_T.rowner)
                ))
            {
                intSearch = intSearch - 1;
            }

            if (
                intSearch < 0
                )
                Test.Abort(
                    Test.ToLog(dateStart_I, "dateStart_I") + ", " +
                        Test.ToLog(hixownerToUpdate_T.rowner, "hixownerToUpdate_T.rowner") + " not found",
                    Test.ToLog(this, "this"));
            if (
                //                                          //Entry to reverse close is already open
                this.arrddvtrtarget[intSearch].end == Date.MaxValue
                )
                Test.Abort(
                    Test.ToLog(this.arrddvtrtarget[intSearch], "this.arrddvtrtarget[" + intSearch + "]") + 
                        " entry to ReverseClose is already open",
                    Test.ToLog(dateStart_I, "dateStart_I"),
                    Test.ToLog(hixownerToUpdate_T.rowner, "hixownerToUpdate_T.rowner"), Test.ToLog(this, "this"));


            //                                              //Open entry and sort
            this.arrddvtrtarget[intSearch] =
                new DateDateValueTrio<ReferenceTo<TTarget>>(dateStart_I, Date.MaxValue, hixownerToUpdate_T.rowner);
            this.subSortArrddvtrtargetEndPlusStart();

            //                                              //Update timestamp and verify
            this.ztimeLastUpdate = ZonedTime.Now();

            this.subAbortIfNotInmediateAfterHixLastUpdate(hixownerToUpdate_T.ztimeLastUpdate);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private long longEndPlusStart(
            //                                              //This method could be an static method, it was designed
            //                                              //      as instance method to easy code
            //                                              //Concatenate End+Start to compare.
            //                                              //long, to compare

            DateDateValueTrio<ReferenceTo<TTarget>> ddvtrtargetToCompate
            )
        {
            return ((long)ddvtrtargetToCompate.end.TotalDays) * Date.MaxValue.TotalDays +
                ddvtrtargetToCompate.start.TotalDays;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private void subSortArrddvtrtargetEndPlusStart(
            )
        {
            long[] arrlongIndex = new long[this.arrddvtrtarget.Length]; 
            for (int intX = 0; intX < this.arrddvtrtarget.Length; intX = intX + 1)
            {
                arrlongIndex[intX] = this.longEndPlusStart(this.arrddvtrtarget[intX]);
            }
            Std.Sort(arrlongIndex, this.arrddvtrtarget);
        }

        //--------------------------------------------------------------------------------------------------------------
        private void subAbortIfNotInmediateAfterHixLastUpdate(
            //                                              //Update of hiw corresponding should be done immedialy (at
            //                                              //      most 1 millisecond after) after hix update.
            //                                              //Abort if not ok.

            ZonedTime ztimeHixLastUpdate_I
            )
        {
            const long longLIMIT = 100;

            long longLapsMillisecond = this.ztimeLastUpdate - ztimeHixLastUpdate_I;

            if (
                //                                          //Lapse time is OK.
                //                                          //CHANGE THIS TO 1, DEBUG TEST COULD TAKE LONGER
                longLapsMillisecond > longLIMIT
                )
                Test.Abort(
                    Test.ToLog(longLapsMillisecond, "longLapsMillisecond") + " should be at most " + longLIMIT +
                        " millisecond",
                    Test.ToLog(ztimeHixLastUpdate_I, "ztimeHixLastUpdate_I"), Test.ToLog(this, "this"));
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public DateDateValueTrio<ReferenceTo<TTarget>>[] GetEntries(
            //                                              //Lookup for historywith entries (ddvtrtarget) corresponding
            //                                              //      to period.
            //                                              //arrddvtrtarget, HistoryWith Entries that include at least
            //                                              //      one day in period.

            //                                              //Start <= End (period include 1 day or many)
            Date Start_I,
            Date End_I
            )
        {
            if (!(
                Start_I <= End_I
                ))
                Test.Abort(
                    Test.ToLog(Start_I, "Start_I") + ", " + Test.ToLog(End_I, "End_I") + " should be in sequence",
                    Test.ToLog(this, "this"));

            List<DateDateValueTrio<ReferenceTo<TTarget>>> darrddvtrtargetSelected =
                new List<DateDateValueTrio<ReferenceTo<TTarget>>>();

            //                                              //Search and select backward, until End < Start_I 
            int intX = this.arrddvtrtarget.Length - 1;
            /*UNTIL-DO*/
            while (!(
                (intX < 0) ||
                //                                          //No more posible entries to select
                (this.arrddvtrtarget[intX].end < Start_I)
                ))
            {
                if (
                    //                                      //At least 1 day of entry is within period
                    (this.arrddvtrtarget[intX].end >= Start_I) && (this.arrddvtrtarget[intX].start <= End_I)
                    )
                {
                    darrddvtrtargetSelected.Add(this.arrddvtrtarget[intX]);
                }

                intX = intX - 1;
            }

            DateDateValueTrio<ReferenceTo<TTarget>>[] arrddvtrtargetSelected = darrddvtrtargetSelected.ToArray();

            //                                              //Sort by Start+End
            Std.Sort(arrddvtrtargetSelected);

            return arrddvtrtargetSelected;
        }

        //--------------------------------------------------------------------------------------------------------------
        public DateDateValueTrio<ReferenceTo<TTarget>>[] GetEntries(
            //                                              //Lookup for historywith entries (ddvtrtarget) corresponding
            //                                              //      to date.
            //                                              //arrddvtrtarget, HistoryWith Entries that include date.

            Date Date_I
            )
        {
            return this.GetEntries(Date_I, Date_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public DateDateValueTrio<ReferenceTo<TTarget>>[] GetAllEntries(
            //                                              //arrddvtrtarget, all Entries, but should include the Entity
            //                                              //      (hiw.owner)
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
            byte[] arrbyteRtarget = this.arrddvtrtarget.Serialize();
            byte[] arrbyteZtimeLastUpdate = this.ztimeLastUpdate.Serialize();

            return Std.ConcatenateArrays(arrbyteRowner, arrbyteRtarget, arrbyteZtimeLastUpdate);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void CreateEmptyOrDeserialize(
            //                                              //Returns a history with object.
            //                                              //For some tests, to mean empty history, it was easy accept
            //                                              //      bytes_IO = null.

            //                                              //Paramenter required to construct an empty history with
            TOwner ownerBelongTo_I,
            //                                              //The object created or deserialized.
            out HistoryWith<TOwner, TTarget> historyWith_O,
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
                historyWith_O = new HistoryWith<TOwner, TTarget>(ownerBelongTo_I);
            }
            else
            {
                HistoryWith<TOwner, TTarget>.DummyValue.Deserialize(out historyWith_O, ref bytes_IO);

                if (
                    bytes_IO.Length > 0
                    )
                    Test.Abort(Test.ToLog(bytes_IO, "bytes_IO") + " was not used in bytes_IO deserialization",
                        Test.ToLog(historyWith_O, "historyWith_O"));
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Deserialize(
            //                                              //Returns a deserialized object.

            //                                              //The object to deserialize.
            out HistoryWith<TOwner, TTarget> historyWith_O,
            //                                              //The serialized bytes.
            ref byte[] Bytes_IO
            )
        {
            ReferenceTo<TOwner> rownerDeserialized;
            (new ReferenceTo<TOwner>()).Deserialize(out rownerDeserialized, ref Bytes_IO);

            DateDateValueTrio<ReferenceTo<TTarget>>[] arrddvtrtargetDeserialized;
            Std.DeserializeArray(out arrddvtrtargetDeserialized, ref Bytes_IO,
                new DateDateValueTrio<ReferenceTo<TTarget>>());

            ZonedTime timeLastUpdateDeserialized;
            ZonedTime.DummyValue.Deserialize(out timeLastUpdateDeserialized, ref Bytes_IO);

            historyWith_O = new HistoryWith<TOwner, TTarget>(rownerDeserialized, arrddvtrtargetDeserialized,
                timeLastUpdateDeserialized);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
