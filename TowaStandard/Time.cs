/*TASK Time*/
using System;

//                                                          //AUTHOR: Towa (FOJ-Felipe Osornio).
//                                                          //CO-AUTHOR: Towa (RGL-Rodrigo García).
//                                                          //DATE: 27-Septiembre-2018.

namespace TowaStandard
{
    //==================================================================================================================
    public struct Time : BsysInterface, IComparable
    {
        //                                                  //Time is an structure which handles time information with
        //                                                  //      reference to a particular day and location not known
        //                                                  //      by the object.

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public const int SecondsPerMinute = 60;
        public const int SecondsPerHour = 60 * SecondsPerMinute;
        public const int SecondsPerDay = 24 * SecondsPerHour;

        //                                                  //Time (Clock) range
        private const int intCLOCK_MIN_VALUE_TOTAL_SECONDS = -9 * Time.SecondsPerHour;
        private const int intCLOCK_MAX_VALUE_TOTAL_SECONDS = 10 * Time.SecondsPerDay - 1;

        //                                                  //00:00:00
        public static readonly Time MinValue = new Time(0);

        //                                                  //23:59:59
        public static readonly Time MaxValue = new Time(Time.SecondsPerDay - 1);

        //                                                  //MIN_TIME and MAX_TIME is the "normal" range of time.
        //                                                  //Eventually, when DaylightSavingTime starts ________
        //                                                  //      (TypeOfDay = START_DAYLIGHT_SAVING_TIME) is a
        //                                                  //      shorter day (24 - delta)-hours, this day, time runs
        //                                                  //      from +delta up to 23:59:59. {Ex. Mexico City, ____
        //                                                  //      Apr 1, 2018, from 01:00:00 to 23:59:59}.
        //                                                  //On the other side, when DaylightSavingTime ends ________
        //                                                  //      (TypeOfDay = END_DAYLIGHT_SAVING_TIME) is a longer
        //                                                  //      day (24 + delta)-hours, this day, time runs from
        //                                                  //      -delta up to 23:59:59. {Ex. Mexico City, ________
        //                                                  //      Oct 28, 2018, from (-1):00:00 to 23:59:59}.
        //                                                  //Also, when activities cross midnight, it will be
        //                                                  //      convenient to "keep" time for several hours with
        //                                                  //      reference to previous day. {Ex. time could be
        //                                                  //      1.03:30:00 for 3:30am of next day}.
        //                                                  //Also, when traveling (moving accros different Time Zone),
        //                                                  //      it will be convenient to "keep" time with reference
        //                                                  //      to the Day-TimeZone I started the day. {Ex. when
        //                                                  //      moving from Tokyo (Nov 14, 1am) to Hawaii in a few
        //                                                  //      hours (maybe 3 hours) I will arrive to Hawaii at
        //                                                  //      Nov 13, 11pm (the day before I left!!), then it will
        //                                                  //      be convenient to "keep" time with reference to ____
        //                                                  //      Tokyo-Nov 14 (4am).

        //                                                  //(-9):00:00 (MinValue - 9 hours)
        private static readonly Time timeCLOCK_MIN_VALUE = new Time(-9 * Time.SecondsPerHour);

        //                                                  //9.23:59:59 (MaXValue + 9 days)
        private static readonly Time timeCLOCK_MAX_VALUE = new Time(10 * Time.SecondsPerDay - 1);

        //                                                  //12:00:00
        private static readonly Time timeTWELVE_OCLOCK_MIDDAY = new Time(12 * Time.SecondsPerHour);

        //                                                  //Options should be in ascending sequence
        private static readonly int[] arrintROUND_DOWN_OPTION =
        {
            //                                              //Seconds
            1, 2, 3, 4, 5, 6, 10, 12, 15, 20, 30,
            //                                              //Minutes
            1 * Time.SecondsPerMinute, 2 * Time.SecondsPerMinute, 3 * Time.SecondsPerMinute, 4 * Time.SecondsPerMinute,
            5 * Time.SecondsPerMinute, 6 * Time.SecondsPerMinute, 10 * Time.SecondsPerMinute,
            12 * Time.SecondsPerMinute, 15 * Time.SecondsPerMinute, 20 * Time.SecondsPerMinute,
            30 * Time.SecondsPerMinute,
            //                                              //Hours
            1 * Time.SecondsPerHour, 2 * Time.SecondsPerHour, 3 * Time.SecondsPerHour, 4 * Time.SecondsPerHour,
            6 * Time.SecondsPerHour, 8 * Time.SecondsPerHour, 12 * Time.SecondsPerHour
        };

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static Time(
            )
        {
            //                                              //Prepare constants

            Std.Sort(Time.arrintROUND_DOWN_OPTION);

            //                                              //Verify constants
            Time.subVerifyConstantsRoundDownOptions();
        }

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        private static void subVerifyConstantsRoundDownOptions(
            )
        {
            Test.AbortIfNullOrEmpty(Time.arrintROUND_DOWN_OPTION, "Time.arrintROUND_DOWN_OPTION");
            Test.AbortIfDuplicate(Time.arrintROUND_DOWN_OPTION, "Time.arrintROUND_DOWN_OPTION");
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Additional days + hours + minutes + seconds (all added in
        //                                                  //      seconds)
        public readonly int TotalSeconds;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        //                                                  //With "normal" range, this value is 0, but it can be from
        //                                                  //      -1 up to 9.
        public int AdditionalDays
        {
            get
            {
                //                                          //To make a positive number
                int intDaysX = 10;
                int intAdditionalDaysPlusDayX =
                    (this.TotalSeconds + intDaysX * Time.SecondsPerDay) / Time.SecondsPerDay;

                return intAdditionalDaysPlusDayX - intDaysX;
            }
        }

        public int Hours
        {
            get
            {
                //                                          //To make a positive number
                int intDaysX = 10;
                int intHoursPlusDayX = (this.TotalSeconds + intDaysX * Time.SecondsPerDay) / Time.SecondsPerHour;

                return intHoursPlusDayX % 24;
            }
        }

        public int Minutes
        {
            get
            {
                //                                          //To make a positive number
                int intDaysX = 10;
                int intMinutesPlusDayX = (this.TotalSeconds + intDaysX * Time.SecondsPerDay) / Time.SecondsPerMinute;

                return intMinutesPlusDayX % 60;
            }
        }

        public int Seconds
        {
            get
            {
                //                                          //To make a positive number
                int intDaysX = 10;
                int intSecondsPlusDayX = this.TotalSeconds + intDaysX * Time.SecondsPerDay;

                return intSecondsPlusDayX % 60;
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
        public Time(
            //                                              //this.*[O], assing values. 

            int TotalSeconds_I
            )
        {
            if (
                !TotalSeconds_I.IsBetween(Time.intCLOCK_MIN_VALUE_TOTAL_SECONDS, Time.intCLOCK_MAX_VALUE_TOTAL_SECONDS)
                )
                Test.Abort(Test.ToLog(TotalSeconds_I, "TotalSeconds_I") + " should be in CLOCK range " +
                    Time.intCLOCK_MIN_VALUE_TOTAL_SECONDS + "-" + Time.intCLOCK_MAX_VALUE_TOTAL_SECONDS);

            this.TotalSeconds = TotalSeconds_I;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public Time(
            //                                              //this.*[O], assing values. 

            int Hours_I,
            int Minutes_I,
            int Seconds_I
            )
        {
            if (
                !Hours_I.IsBetween(0, 23)
                )
                Test.Abort(
                    Test.ToLog(Hours_I, "Hours_I") + " should be in range 0-23", Test.ToLog(Hours_I, "Hours_I"),
                    Test.ToLog(Minutes_I, "Minutes_I"), Test.ToLog(Seconds_I, "Seconds_I"));
            if (
                !Minutes_I.IsBetween(0, 59)
                )
                Test.Abort(
                    Test.ToLog(Minutes_I, "Minutes_I") + " should be in range 0-59", Test.ToLog(Hours_I, "Hours_I"),
                    Test.ToLog(Minutes_I, "Minutes_I"), Test.ToLog(Seconds_I, "Seconds_I"));
            if (
                !Seconds_I.IsBetween(0, 59)
                )
                Test.Abort(
                    Test.ToLog(Seconds_I, "Seconds_I") + " should be in range 0-59", Test.ToLog(Hours_I, "Hours_I"),
                    Test.ToLog(Minutes_I, "Minutes_I"), Test.ToLog(Seconds_I, "Seconds_I"));

            this.TotalSeconds = Hours_I * Time.SecondsPerHour + Minutes_I * Time.SecondsPerMinute + Seconds_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static Time Now(
            )
        {
            return Time.Now(TimeZoneX.Here(), 1);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Time Now(
            int SecondToRoundDown_I
            )
        {
            return Time.Now(TimeZoneX.Here(), SecondToRoundDown_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Time Now(
            TimeZoneX timezoneBase_I
            )
        {
            return Time.Now(timezoneBase_I, 1);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Time Now(
            //                                              //Take time from clock and current timezone and round down.
            //                                              //Example: Time.Now(TimeZoneX.Here(), 300), if "local time"
            //                                              //       is 11:34pm ,the result will be 23:30:00 because
            //                                              //      clock "ticks" every 5 minutes.

            TimeZoneX timezoneBase_I,
            int SecondToRoundDown_I
            )
        {
            Time timeNow = ZonedTime.Now(timezoneBase_I).Time;

            if (
                //                                          //Need to round down
                SecondToRoundDown_I != 1
                )
            {
                int intTotalSecondsRoundDown = Time.intTotalSecondRoundDown(timeNow.TotalSeconds, SecondToRoundDown_I);
                timeNow = new Time(intTotalSecondsRoundDown);
            }

            return timeNow;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Time Now(

            Date dateBase_I
            )
        {
            return Time.Now(dateBase_I, TimeZoneX.Here(), 1);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Time Now(

            Date dateBase_I,
            int SecondToRoundDown_I
            )
        {
            return Time.Now(dateBase_I, TimeZoneX.Here(), SecondToRoundDown_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Time Now(

            Date dateBase_I,
            TimeZoneX timezoneBase_I
            )
        {
            return Time.Now(dateBase_I, timezoneBase_I, 1);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Time Now(

            //                                              //Should be Date.Now or before
            Date dateBase_I,
            TimeZoneX timezoneBase_I,
            int SecondToRoundDown_I
            )
        {
            ZonedTime ztimeNow = ZonedTime.Now(timezoneBase_I);

            if (!(
                ztimeNow.Date >= dateBase_I
                ))
                Test.Abort(
                    Test.ToLog(ztimeNow.Date, "ztimeNow.Date") + " should in the same day or after " +
                        Test.ToLog(dateBase_I, "dateBase_I"), 
                    Test.ToLog(ztimeNow, "ztimeNow"), Test.ToLog(timezoneBase_I, "timezoneBase_I"), 
                    Test.ToLog(SecondToRoundDown_I, "SecondToRoundDown_I"));

            int intTimeTotalSeconds;
            if (
                ztimeNow.Date == dateBase_I
                )
            {
                //                                          //No additional days
                intTimeTotalSeconds = ztimeNow.Time.TotalSeconds;
            }
            else
            {
                //                                          //Compute dateBase 12:00:00, this time will be in NORMAL
                //                                          //      or DAYLIGHT_SAVING_TIME not afected by start or end
                //                                          //      of daylight saving time.
                //                                          //Note: date ztimeNow could be a different TypeOfDay
                ZonedTime ztimeBase12pm = new ZonedTime(dateBase_I, Time.timeTWELVE_OCLOCK_MIDDAY, timezoneBase_I);

                long longTimeTotalSecondsAfter12pm =
                    (ztimeNow.UtcTotalMilliseconds - ztimeBase12pm.UtcTotalMilliseconds) / 
                    ZonedTime.MillisecondsPerSecond;
                intTimeTotalSeconds = 12 * Time.SecondsPerHour + (int)longTimeTotalSecondsAfter12pm;
            }

            if (
                //                                          //Need to round down
                SecondToRoundDown_I != 1
                )
            {
                intTimeTotalSeconds = Time.intTotalSecondRoundDown(intTimeTotalSeconds, SecondToRoundDown_I);
            }

            return new Time(intTimeTotalSeconds);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static int intTotalSecondRoundDown(

            int intTotalSeconds_I,
            int intSecondToRoundDown_I
            )
        {
            if (!(
                intSecondToRoundDown_I.IsInSortedSet(Time.arrintROUND_DOWN_OPTION)
                ))
                Test.Abort(Test.ToLog(intSecondToRoundDown_I, "intSecondToRoundDown_I") + " is not a valid option",
                    Test.ToLog(Time.arrintROUND_DOWN_OPTION, "Time.arrintROUND_DOWN_OPTION"));

            //                                              //To make a positive number
            int intDaysXTotalSeconds = 10 * Time.SecondsPerDay;

            int intTotalSecondsRoundDown =
                ((intTotalSeconds_I + intDaysXTotalSeconds) / intSecondToRoundDown_I) * intSecondToRoundDown_I -
                intDaysXTotalSeconds;

            return intTotalSecondsRoundDown;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Time operator +(

            //                                              //time, new Time adding seconds

            Time time_I,
            int intSeconds_I
            )
        {
            return new Time(time_I.TotalSeconds + intSeconds_I);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Time operator -(

            //                                              //time, new Time subtracting seconds

            Time time_I,
            int intSeconds_I
            )
        {
            return new Time(time_I.TotalSeconds - intSeconds_I);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static int operator -(
            //                                              //int, lapse time in seconds

            Time timeLeft_I,
            Time timeRight_I
            )
        {
            return timeLeft_I.TotalSeconds - timeRight_I.TotalSeconds;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator <(
            Time timeLeft_I,
            Time timeRight_I
            )
        {
            return (
                timeLeft_I.TotalSeconds < timeRight_I.TotalSeconds
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator <=(
            Time timeLeft_I,
            Time timeRight_I
            )
        {
            return (
                timeLeft_I.TotalSeconds <= timeRight_I.TotalSeconds
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator >(
            Time timeLeft_I,
            Time timeRight_I
            )
        {
            return (
                timeLeft_I.TotalSeconds > timeRight_I.TotalSeconds
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator >=(
            Time timeLeft_I,
            Time timeRight_I
            )
        {
            return (
                timeLeft_I.TotalSeconds >= timeRight_I.TotalSeconds
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public bool IsBetween(
            Time TimeStart_I,
            Time TimeEnd_I
            )
        {
            return this.TotalSeconds.IsBetween(TimeStart_I.TotalSeconds, TimeEnd_I.TotalSeconds);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator ==(
            Time timeLeft_I,
            Time timeRight_I
            )
        {
            return (
                timeLeft_I.TotalSeconds == timeRight_I.TotalSeconds
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator !=(
            Time timeLeft_I,
            Time timeRight_I
            )
        {
            return (
                timeLeft_I.TotalSeconds != timeRight_I.TotalSeconds
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public override String ToString(
            //                                              //Convert to text.
            //                                              //Options:
            //                                              //00:00:00-23:59:59 use "HH:mm:ss".
            //                                              //Negative, (-9):00:00-(-1):59:59 use picture (-h):mm:ss
            //                                              //      where "-h" will be from -9 to -1.
            //                                              //Aditional Days, 1.00:00:00-9:23:59:59 use picture
            //                                              //      d.HH:mm:ss where "d" will be from 1 to 9.
            )
        {
            //                                              //Format: ":mm:ss"
            String strMinutesAndSeconds = ":" + this.Minutes.ToString("00") + ":" + this.Seconds.ToString("00");

            String strToText;
            if (
                this.TotalSeconds < 0
                )
            {
                //                                          //AdditionalDays = -1, convert "d.HH" to "(-h)".

                //                                          //Format: "(-h)" + ":mm:ss"
                strToText = "(" + (this.Hours - 24) + ")" + strMinutesAndSeconds;
            }
            else
            {
                //                                              //Format: "HH" + ":mm:ss"
                strToText = this.Hours.ToString("00") + strMinutesAndSeconds;

                if (
                    this.AdditionalDays > 0
                    )
                {
                    //                                      //Format: "Days." + "HH:mm:ss"
                    strToText = this.AdditionalDays + "." + strToText;
                }
            }

            return strToText;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public String ToString(
            String Picture_I,
            System.Globalization.CultureInfo Culture_I
            )
        {
            String strToString;
            if (
                this.TotalSeconds.IsBetween(Time.MinValue.TotalSeconds, Time.MaxValue.TotalSeconds)
                )
            {
                DateTime dt = new DateTime(this.TotalSeconds * TimeSpan.TicksPerSecond);
                strToString = dt.ToString(Picture_I, Culture_I);
            }
            else
            {
                //                                      //Need to use picture "d.HH:mm:ss"/"d.HH:mm" or
                //                                      //      "(-h):mm:ss"/"(-h):mm", depends on picture with "s".

                //                                      //Compute time in full 24-hour picture
                strToString = this.ToString();

                //                                      //Cut seconds if not present in required picture
                if (
                    //                                  //Picture do not include seconds
                    !Picture_I.Contains('s')
                    )
                {
                    int intLastColon = strToString.LastIndexOf(':');
                    strToString = strToString.Substring(0, intLastColon);
                }
            }

            return strToString;
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            //                                              //Required for Sort, BinarySearch and CompareTo.

            //                                              //this[I], object key info.

            //                                              //syspath or str
            Object obj_L
            )
        {
            if (!(
                obj_L is Time
                ))
                Test.Abort(Test.ToLog(obj_L.GetType(), "obj_L.GetType") + " should be Time");

            int intTotalSecondsToCompare = ((Time)obj_L).TotalSeconds;

            return (this.TotalSeconds < intTotalSecondsToCompare) ? -1 : 
                (this.TotalSeconds > intTotalSecondsToCompare) ? 1 : 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //NEXT 2 METHODS ARE TO AVOID COMPILE WARNINGS.

        //-------------------------------------------------------------------------------------------------------------
        public override bool Equals(
            Object obj_L
            )
        {
            if (!(
                obj_L is Time
                ))
                Test.Abort(Test.ToLog(obj_L.GetType(), "obj_L.GetType") + " should be Time");

            Time timeRight = (Time)obj_L;

            return (
                this == timeRight
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
