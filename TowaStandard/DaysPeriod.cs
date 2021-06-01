/*TASK DaysPeriod Days Period*/
using System;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz, LGF-Liliana Gutiérrez).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: August, 29, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public struct DaysPeriod : BsysInterface, IComparable, SerializableInterface<DaysPeriod>
    {
        //                                                  //Days period is a start and end date:
        //                                                  //It can be uses by itself.
        //                                                  //Most of the time it is used as a set of consecutive days
        //                                                  //      periods within a Life

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public static DaysPeriod DummyValue = new DaysPeriod();

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Ex. <2019-08-30, 2019-08-31> is a 2 days period.
        //                                                  //(Start and End days ARE included in the period).
        public readonly Date Start;
        public readonly Date End;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        public int Days
        {
            get
            {
                //                                          //Remember, both Start and End days ARE included in the
                //                                          //      period.
                return this.End - this.Start + 1;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public String ToLogShort()
        {
            return Test.ToLog(this);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public String ToLogFull()
        {
            return Test.ToLog(this);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        public DaysPeriod(
            
            //                                              //End should >= Start
            Date Start_I,
            Date End_I
            )
        {
            this.Start = Start_I;
            this.End = End_I;

            if (!(
                this.Start <= this.End
                ))
                Test.Abort(Test.ToLog(this, "this") +
                    " is not a valid period, Start and End Dates should be in sequence");
        }

        //--------------------------------------------------------------------------------------------------------------
        public DaysPeriod(
            //                                              //Construct and open period
            
            Date Start_I
            )
        {
            this.Start = Start_I;
            this.End = Date.MaxValue;
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool Contains(
            //                                              //true if date is included in period

            Date Date_I
            )
        {
            return Date_I.IsBetween(this.Start, this.End);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS-METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public String ToText()
        {
            return "<" + this.Start.ToText() + ", " + this.End.ToText() + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            //                                              //Required for Sort, BinarySearch and CompareTo.

            //                                              //this[I], object key info.

            //                                              //syspath or str
            Object obj_I
            )
        {
            Date dateToCompare;
            /*CASE*/
            if (
                obj_I is DaysPeriod
                )
            {
                dateToCompare = ((DaysPeriod)obj_I).Start; ;
            }
            else if (
                obj_I is Date
                )
            {
                dateToCompare = (Date)obj_I;
            }
            else
            {
                Test.Abort(
                    Test.ToLog(obj_I.GetType(), "obj_I.type") +
                        " is not a compatible CompareTo argument, the options are: DaysPeriod & Date",
                    Test.ToLog(this.GetType(), "this.type"));

                dateToCompare = default(Date);
            }
            /*END-CASE*/

            int intCompareTo = this.Start.CompareTo(dateToCompare);

            return intCompareTo;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //NEXT 2 METHODS ARE TO AVOID COMPILE WARNINGS.

        //--------------------------------------------------------------------------------------------------------------
        public override bool Equals(
            Object obj_L
            )
        {
            if (!(
                obj_L is DaysPeriod
                ))
                Test.Abort(Test.ToLog(obj_L.GetType(), "obj_L.GetType") + " should be DaysPeriod");

            DaysPeriod dperiodRight = (DaysPeriod)obj_L;

            return (
                (this.Start == dperiodRight.Start) &&
                (this.End == dperiodRight.End)
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
            return this.GetHashCode();
        }

        //--------------------------------------------------------------------------------------------------------------
        public byte[] Serialize(
            //                                              //Get a serialized version of the object.

            )
        {
            byte[] arrbyteStart = this.Start.Serialize();
            byte[] arrbyteEnd = this.End.Serialize();

            return Std.ConcatenateArrays(arrbyteStart, arrbyteEnd);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Deserialize(
            //                                              //Returns a deserialized object.

            //                                              //The object to deserialize.
            out DaysPeriod DaysPeriod_O,
            //                                              //The serialized bytes.
            ref byte[] Bytes_IO
            )
        {
            Date dateStart;
            Date.DummyValue.Deserialize(out dateStart, ref Bytes_IO);

            Date dateEnd;
            Date.DummyValue.Deserialize(out dateEnd, ref Bytes_IO);

            DaysPeriod_O = new DaysPeriod(dateStart, dateEnd);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
