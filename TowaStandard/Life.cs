/*TASK Life*/
using System;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz, LGF-Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: August, 29, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public class Life : BsysAbstract, SerializableInterface<Life>
    {
        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public static readonly Life DummyValue = new Life();

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //0 or more (non-overlaping) days periods in sequence.
        //                                                  //Ex. <2018-08-29, 2018-11-30>, <2019-01-01, 9999-12-31>.
        //                                                  //Notice, in this example last period is not closed
        private DaysPeriod[] arrdperiod;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        public int Days
        {
            get
            {
                int intDaysInLife = 0;
                foreach (DaysPeriod dperiodX in this.arrdperiod)
                {
                    intDaysInLife = intDaysInLife + dperiodX.Days;
                }

                return intDaysInLife;
            }
        }

        public DaysPeriod LastPeriod
        {
            get
            {
                if (
                    this.arrdperiod.Length == 0
                    )
                    Test.Abort(Test.ToLog(this, "this") + " life has no periods");

                return this.arrdperiod[this.arrdperiod.Length - 1];
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return Test.ToLog(this);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public override String ToLogFull()
        {
            return Test.ToLog(this);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public Life(
            //                                              //Start a life with no periods
            )
            : base()
        {
            this.arrdperiod = new DaysPeriod[0];
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public Life(
            //                                              //Start a life with an open period.
            Date Start_I
            )
            : base()
        {
            if (
                Start_I == Date.MaxValue
                )
                Test.Abort(Test.ToLog(Start_I, "") + " is not a valid start date");

            DaysPeriod dperiodOpen = new DaysPeriod(Start_I, Date.MaxValue);

            this.arrdperiod = new DaysPeriod[] { dperiodOpen };
        }

        //--------------------------------------------------------------------------------------------------------------
        private Life(
            //                                              //Only to deserialize life

            DaysPeriod[] arrdperiod_L
            )
            : base()
        {
            this.arrdperiod = arrdperiod_L;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*TRANSFORMATION METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public void StartPeriod(
            //                                              //Add a new period of life.
            //                                              //The new period should follow the last, it will be a open
            //                                              //      period

            //                                              //Start date of the new period of life.
            Date Start_I
            )
        {
            //                                              //If not the first period, this period should follow the
            //                                              //      last
            if (
                this.arrdperiod.Length > 0
                )
            {
                //                                          //To easy code
                DaysPeriod dperiodLast = this.arrdperiod[this.arrdperiod.Length - 1];

                if (
                    //                                      //Is open
                    dperiodLast.End == Date.MaxValue
                    )
                    Test.Abort(Test.ToLog(dperiodLast, "dperiodLast") + " last period is open",
                        Test.ToLog(this, "this"));
                if (
                    //                                      //Is not a subsequent start
                    Start_I <= dperiodLast.End
                    )
                    Test.Abort(Test.ToLog(Start_I, "Start_I") + " is not after last period in life",
                        Test.ToLog(dperiodLast, "dperiodLast"), Test.ToLog(this, "this"));
            }

            //                                              //Assign a copy of array & add new entry
            this.arrdperiod = this.arrdperiod.Copy(0, this.arrdperiod.Length + 1);
            this.arrdperiod[this.arrdperiod.Length - 1] = new DaysPeriod(Start_I, Date.MaxValue);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void ReverseStartPeriod(
            //                                              //Remove last period of life (should be open).
            )
        {
            //                                              //If not the first period, this period should follow the
            //                                              //      last
            if (
                this.arrdperiod.Length > 0
                )
            {
                //                                          //To easy code
                DaysPeriod dperiodLast = this.arrdperiod[this.arrdperiod.Length - 1];

                if (
                    //                                      //Last periord is not an open period subsequent start
                    dperiodLast.End != Date.MaxValue
                    )
                    Test.Abort(Test.ToLog(dperiodLast, "dperiodLast") + " last period is not open",
                        Test.ToLog(this, "this"));
            }

            //                                              //Remove last period
            this.arrdperiod = this.arrdperiod.Copy(0, this.arrdperiod.Length - 1);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void End(
            //                                              //End the last period of life.

            Date End_I
            )
        {
            if (
                this.arrdperiod.Length == 0
                )
                Test.Abort(Test.ToLog(this, "this") + " life has no periods");

            //                                              //To easy code
            DaysPeriod dperiodLast = this.arrdperiod[this.arrdperiod.Length - 1];

            if (
                dperiodLast.End != Date.MaxValue
                )
                Test.Abort(Test.ToLog(dperiodLast, "dperiodLast") + " should be open", Test.ToLog(this, "this"));
            if (
                End_I < dperiodLast.Start
                )
                Test.Abort(Test.ToLog(End_I, "End_I") + " can not close the last period, it is not in sequence",
                    Test.ToLog(this, "this"));

            //                                              //End Period.
            this.arrdperiod[this.arrdperiod.Length - 1] = new DaysPeriod(dperiodLast.Start, End_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void ReverseEnd(
            )
        {
            if (
                this.arrdperiod.Length == 0
                )
                Test.Abort(Test.ToLog(this, "this") + " life has no periods");

            //                                              //To easy code
            DaysPeriod dperiodLast = this.arrdperiod[this.arrdperiod.Length - 1];

            if (
                dperiodLast.End == Date.MaxValue
                )
                Test.Abort(Test.ToLog(dperiodLast, "dperiodLast") + " should be closed", Test.ToLog(this, "this"));

            //                                              //Reverse end  (turn to open)
            this.arrdperiod[this.arrdperiod.Length - 1] = new DaysPeriod(dperiodLast.Start, Date.MaxValue);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public DaysPeriod GetPeriod(
            //                                              //period, period that include date

            //                                              //Date to find.
            Date Date_I
            )
        {
            //                                              //Search backward
            int intP = this.arrdperiod.Length - 1;
            /*UNTIL-DO*/
            while (!(
                //                                          //There is no more periods to check.
                (intP < 0) ||
                //                                          //The period has been found.
                (this.arrdperiod[intP].Start <= Date_I)
                ))
            {
                intP = intP - 1;
            }

            if (
                (intP < 0) ||
                //                                          //Is not in period.
                (Date_I > this.arrdperiod[intP].End)
                )
                Test.Abort(Test.ToLog(Date_I, "Date_I") + " is not in life", Test.ToLog(this, "this"));

            return this.arrdperiod[intP];
        }

        //--------------------------------------------------------------------------------------------------------------
        public DaysPeriod GetLastPeriod(
            //                                              //period, last
            )
        {
            Test.AbortIfNullOrEmpty(this.arrdperiod, "this.arrdperiod");

            return this.arrdperiod[this.arrdperiod.Length - 1];
        }

        //--------------------------------------------------------------------------------------------------------------
        public DaysPeriod[] GetAllPeriods(
            //                                              //arrperiod, all period included in the life. If in the
            //                                              //      life object the information is kept in an array,
            //                                              //      a COPY should be returned (These is necessary to
            //                                              //      avoid a later modification to returned array,
            //                                              //      also modify the information within the life object).
            )
        {
            DaysPeriod[] arrdperiodCopy = new DaysPeriod[this.arrdperiod.Length];
            Array.Copy(this.arrdperiod, 0, arrdperiodCopy, 0, this.arrdperiod.Length);

            //                                              //SHOULD return a copy, NOT array within life
            return arrdperiodCopy;
        }

        //--------------------------------------------------------------------------------------------------------------
        public Life GetClone(
            //                                              //life, a clone of this life (should be a DIFFERENT object).
            )
        {
            DaysPeriod[] arrdperiodCopy = this.GetAllPeriods();

            //                                              //SHOULD return a new object
            return new Life(arrdperiod);
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool IsAlive(
            //                                              //Find out if life is alive in a given date.

            Date Date_I
            )
        {
            //                                              //Search backward
            int intP = this.arrdperiod.Length - 1;
            /*UNTIL-DO*/
            while (!(
                //                                          //There is no more periods to check.
                (intP < 0) ||
                //                                          //The period has been found.
                (this.arrdperiod[intP].Start <= Date_I)
                ))
            {
                intP = intP - 1;
            }

            return (
                (intP >= 0) &&
                //                                          //Is in period.
                (Date_I <= this.arrdperiod[intP].End)
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool IsAlive(
            //                                              //Find out if life is alive during a given period.

            //                                              //Start<=End
            Date Start_I,
            Date End_I
            )
        {
            if (
                Start_I > End_I
                )
                Test.Abort(Test.ToLog(Start_I, "") + ", " +Test.ToLog(End_I, "") +
                    " is not a valid period, dates are not in sequence");

            //                                              //Search backward
            int intP = this.arrdperiod.Length - 1;
            /*UNTIL-DO*/
            while (!(
                //                                          //There is no more periods to check.
                (intP < 0) ||
                //                                          //The period has been found.
                (this.arrdperiod[intP].Start <= Start_I)
                ))
            {
                intP = intP - 1;
            }

            return (
                (intP >= 0) &&
                //                                          //Full period is in period (within life).
                (End_I <= this.arrdperiod[intP].End)
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool IsValidDateToStartPeriod(
            //                                              //A date is valid as a Start if chronologically this
            //                                              //      date is subsequent to the last end and is not Max
            //                                              //Notice, if life is empty, it is ok

            Date Start_I
            )
        {
            bool IsValidDateStartPeriod = (
                Start_I != Date.MaxValue
                );
            if (
                IsValidDateStartPeriod
                )
            {
                IsValidDateStartPeriod = (
                    (this.arrdperiod.Length == 0) ||
                    //                                      //Last period is CLOSED
                    (this.arrdperiod[this.arrdperiod.Length - 1].End != Date.MaxValue) &&
                        //                                  //Is after last period
                        (Start_I > this.arrdperiod[this.arrdperiod.Length - 1].End) ||
                    //                                      //Last period is OPEN
                    (this.arrdperiod[this.arrdperiod.Length - 1].End == Date.MaxValue) &&
                        //                                  //It is inside last life after start
                        (Start_I > this.arrdperiod[this.arrdperiod.Length - 1].Start)
                    );

            }

            return IsValidDateStartPeriod;
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool IsValidDateToEnd(
            //                                              //A date is valid as a Start if chronologically this
            //                                              //      date is subsequent to the last end and is not Max
            //                                              //Notice, if life is empty, it is ok

            Date End_I
            )
        {
            bool IsValidDateToEnd = (
                (End_I != Date.MaxValue) &&
                (this.arrdperiod.Length > 0)
                );
            if (
                IsValidDateToEnd
                )
            {
                //                                          //To easy code
                DaysPeriod dperiodLast = this.arrdperiod[this.arrdperiod.Length - 1];

                IsValidDateToEnd = (
                    //                                      //Is an OPEN period
                    (dperiodLast.End == Date.MaxValue) &&
                    //                                      //Is in last period life
                    (End_I >= dperiodLast.Start)
                    );

            }

            return IsValidDateToEnd;
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool IsValidDateToAddHistory( 
            //                                              //Find out if date is within last period (should be open)

            Date Date_I
            )
        {
            bool IsValidDateToAddHistory = (
                (Date_I != Date.MaxValue) &&
                (this.arrdperiod.Length > 0)
                );
            if (
                IsValidDateToAddHistory
                )
            {
                //                                          //To easy code
                DaysPeriod dperiodLast = this.arrdperiod[this.arrdperiod.Length - 1];

                IsValidDateToAddHistory = (
                    //                                      //Is an OPEN period
                    (dperiodLast.End == Date.MaxValue) &&
                    //                                      //Is in last period life
                    (Date_I >= dperiodLast.Start)
                    );
            }

            return IsValidDateToAddHistory;
        }

        //--------------------------------------------------------------------------------------------------------------
        public String ToText()
        {
            String[] arrstrPeriods = new String[this.arrdperiod.Length];

            for (int intP = 0; intP < this.arrdperiod.Length; intP = intP + 1)
            {
                arrstrPeriods[intP] = this.arrdperiod[intP].ToText();
            }

            return "<" + String.Join(", ", arrstrPeriods) + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public byte[] Serialize(
            //                                              //Get a serialized version of the object.

            )
        {
            return this.arrdperiod.Serialize();
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Deserialize(
            //                                              //Returns a deserialized object.

            //                                              //The object to deserialize.
            out Life Life_O,
            //                                              //The serialized bytes.
            ref byte[] Bytes_IO
            )
        {
            DaysPeriod[] arrdperiod;
            Std.DeserializeArray(out arrdperiod, ref Bytes_IO, DaysPeriod.DummyValue);

            Life_O = new Life(arrdperiod);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
