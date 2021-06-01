/*TASK TimePeriod Time Period*/
using System;

//                                                          //AUTHOR: Towa (AQG-Andrea Quiroz, LGF-Liliana Gutiérrez).
//                                                          //CO-AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //DATE: August, 29, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public struct TimePeriod : BsysInterface, IComparable
    {
        //                                                  //Time period is a start and end time

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Ex. <01:00:04, 01:00:10> is a 6 minutes period.
        //                                                  //(Start is included in the period, End time IS NOT).
        //                                                  //Notice. this IS DIFFERENT for dperiod.
        public readonly Time Start;
        public readonly Time End;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        //                                                  //Lapse time in seconds
        public int Lapse
        {
            get
            {
                return this.End - this.Start;
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
        public TimePeriod(Time Start_I, Time End_I)
        {
            this.Start = Start_I;
            this.End = End_I;

            if (
                this.Start > this.End
                )
                Test.Abort(this.ToText() + " is not a valid period, times are not in sequence");
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
            Time timeToCompare;
            /*CASE*/
            if (
                obj_I is TimePeriod
                )
            {
                timeToCompare = ((TimePeriod)obj_I).Start; ;
            }
            else if (
                obj_I is Date
                )
            {
                timeToCompare = (Time)obj_I;
            }
            else
            {
                Test.Abort(
                    Test.ToLog(obj_I.GetType(), "obj_I.type") +
                        " is not a compatible CompareTo argument, the options are: TimePeriod & Time",
                    Test.ToLog(this.GetType(), "this.type"));

                timeToCompare = default(Time);
            }
            /*END-CASE*/

            int intCompareTo = this.Start.CompareTo(timeToCompare);

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
                obj_L is TimePeriod
                ))
                Test.Abort(Test.ToLog(obj_L.GetType(), "obj_L.GetType") + " should be TimePeriod");

            TimePeriod dperiodRight = (TimePeriod)obj_L;

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
    }
 
    //==================================================================================================================
}
/*END-TASK*/
