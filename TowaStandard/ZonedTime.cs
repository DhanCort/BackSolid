/*TASK ZonedTime*/
using System;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa (RGL-Rodrigo García).
//                                                          //DATE: October 25, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public struct ZonedTime : BsysInterface, IComparable, SerializableInterface<ZonedTime>
    {
        //-------------------------------------------------------------------------------------------------------------
        public String ToText(
            )
        {
            return null;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ZonedTime is an structure which handles zoned time
        //                                                  //      information (date, time, etc.).
        //                                                  //Complexity of geografic location (TimeZoneX) and Daylight
        //                                                  //      Saving Time is handles internally.

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public static readonly ZonedTime DummyValue = new ZonedTime();

        public const long MillisecondsPerSecond = 1000;
        public const long MillisecondsPerMinute = 60 * MillisecondsPerSecond;
        public const long MillisecondsPerHour = 60 * MillisecondsPerMinute;
        public const long MillisecondsPerDay = 24 * MillisecondsPerHour;

        //                                                  //Date range.
        //                                                  //0001-01-01T14:00:00.000+00:00[UTC].
        private const long lonMIN_VALUE_TOTAL_MILLISECONDS = 12 * ZonedTime.MillisecondsPerHour;
        //                                                  //9999-12-31T09:59:59.999+00:00[UTC]
        private static readonly long longMAX_VALUE_TOTAL_MILLISECONDS =
            Date.MaxValue.TotalDays * ZonedTime.MillisecondsPerDay + 10 * ZonedTime.MillisecondsPerHour - 1;

        //                                                  //MIN_ZONED_TIME = 0001-01-01T14:00:00.000+00:00[UTC].
        //                                                  //14 hours is required to be able to express MIN_ZONED_TIME
        //                                                  //      in any time zone with offset -14 hours.
        public static readonly ZonedTime MinValue = new ZonedTime(ZonedTime.lonMIN_VALUE_TOTAL_MILLISECONDS);

        //                                                  //MAX_ZONED_TIME = 9999-12-31T09:59:59.999+00:00[UTC]
        //                                                  //09:59:59 time is required to be able to express
        //                                                  //      MAX_ZONED_TIME in any time zone with offset +14
        //                                                  //      hours.
        public static readonly ZonedTime MaxValue = new ZonedTime(ZonedTime.longMAX_VALUE_TOTAL_MILLISECONDS);

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static ZonedTime(
            )
        {
        }

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //"knows" offset and all starts-ends of DaylightSavingTime.
        public readonly TimeZoneX TimeZone;

        //                                                  //Internally zoned time is UTC time and is kept in
        //                                                  //      milliseconds since 0001-01-01T00:00:00.000 (this
        //                                                  //      zoned time will be 0 milliseconds).
        //                                                  //Notice, 0 is not valid zoned time (< MIN_ZONED_TIME).
        //                                                  //This time is comparable to any time in any time zone.
        public readonly long UtcTotalMilliseconds;

        //                                                  //Next values are redundant, all this info is in TimeZoneX

        public bool IsDaylightSavingTime;
        //                                                  //DST delta, even if this ztime is not in DST.
        private readonly int intDeltaDefinedMinutes;
        public readonly ZonedTimeDstTypeOfDayEnum DaylightSavingTimeTypeOfDay;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        //                                                  //Utc + offset
        public long ZtimeTotalMilliseconds
        {
            get
            {
                return this.UtcTotalMilliseconds + this.OffsetMinutes * ZonedTime.MillisecondsPerMinute;
            }
        }

        public Date Date
        {
            get
            {
                //                                          //Compute date in DaysBefore
                int intTotalDays = (int)(this.ZtimeTotalMilliseconds / ZonedTime.MillisecondsPerDay);

                return new Date(intTotalDays);
            }
        }

        //                                                  //This time is within Date,
        //                                                  //a) If Date is the start of DaylightSavingTime period,
        //                                                  //      (START_DAYLIGHT_SAVING_TIME) this time is from
        //                                                  //      01:00:00 up to 23:59:59 (23 hours day).
        //                                                  //b) If Date is the end of DaylightSavingTime period,
        //                                                  //      (END_DAYLIGHT_SAVING_TIME) this time is from
        //                                                  //      -1:00:00 up to 23:59:59. (25 hours day). 
        //                                                  //c) Any other day, in or out DaylightSavingTime period,
        //                                                  //      (DAYLIGHT_SAVING_TIME or NORMAL) is from 00:00:00 up
        //                                                  //      to 23:59:59. (24 hours day). 
        //                                                  //Notice that time is expressed "as is known" in the day. 
        public Time Time
        {
            get
            {
                //                                          //Compute time in Seconds
                long longTimeMilliseconds = (int)(this.ZtimeTotalMilliseconds % ZonedTime.MillisecondsPerDay);
                int intTotalSeconds = (int)(longTimeMilliseconds / ZonedTime.MillisecondsPerSecond);

                //                                          //Adjust Start and End DST
                /*CASE*/
                if (
                    (this.DaylightSavingTimeTypeOfDay == ZonedTimeDstTypeOfDayEnum.START_DAYLIGHT_SAVING_TIME) &&
                    //                                      //This is the start of the day (DST has not begin)
                    !this.IsDaylightSavingTime
                    )
                {
                    //                                      //+ DELTA to move time to DSL
                    intTotalSeconds = intTotalSeconds + this.intDeltaDefinedMinutes * 60;
                }
                else if (
                    (this.DaylightSavingTimeTypeOfDay == ZonedTimeDstTypeOfDayEnum.END_DAYLIGHT_SAVING_TIME) &&
                    //                                      //This is the start of the day (DST has not end)
                    this.IsDaylightSavingTime
                    )
                {
                    //                                      //- DELTA to move back time to NORMAL
                    intTotalSeconds = intTotalSeconds - this.intDeltaDefinedMinutes * 60;
                }
                {
                    //                                      //time is ok, no adjustment required
                }
                /*END-CASE*/

                return new Time(intTotalSeconds);
            }
        }

        public int Milliseconds
        {
            get
            {
                //                                          //To compute no adjusment is needed, offset and delta are in
                //                                          //      minutes
                return (int)(this.UtcTotalMilliseconds % ZonedTime.MillisecondsPerSecond);
            }
        }

        public int OffsetMinutes
        {
            get
            {
                return this.TimeZone.BaseUtcOffsetMinutes + this.DeltaMinutes;
            }
        }

        public int DeltaMinutes
        {
            get
            {
                return (this.IsDaylightSavingTime) ? this.intDeltaDefinedMinutes : 0;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR COMPUTED VARIABLES*/

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
        private ZonedTime(
            //                                              //USED TO CONSTRUCT MIN AND MAX VALUES.
            //                                              //this.*[O], assing values. 

            //                                              //true for MaxValue, false for MinValue
            long longUtcTotalMilliseconds_I
            )
        {
            if (
                !longUtcTotalMilliseconds_I.
                    IsBetween(ZonedTime.lonMIN_VALUE_TOTAL_MILLISECONDS, ZonedTime.longMAX_VALUE_TOTAL_MILLISECONDS)
                )
                Test.Abort(Test.ToLog(longUtcTotalMilliseconds_I, "longUtcTotalMilliseconds_I") +
                    " should be in the range " + ZonedTime.lonMIN_VALUE_TOTAL_MILLISECONDS + "-" +
                    ZonedTime.longMAX_VALUE_TOTAL_MILLISECONDS);

            this.TimeZone = TimeZoneX.UTC;
            this.UtcTotalMilliseconds = longUtcTotalMilliseconds_I;
            this.IsDaylightSavingTime = false;
            this.intDeltaDefinedMinutes = 0;
            this.DaylightSavingTimeTypeOfDay = ZonedTimeDstTypeOfDayEnum.NORMAL;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public ZonedTime(
            //                                              //this.*[O], assing values. 

            long UtcTotalMilliseconds_I,
            TimeZoneX timezone_I
            )
        {
            if (
                !UtcTotalMilliseconds_I.
                    IsBetween(ZonedTime.lonMIN_VALUE_TOTAL_MILLISECONDS, ZonedTime.longMAX_VALUE_TOTAL_MILLISECONDS)
                )
                Test.Abort(Test.ToLog(UtcTotalMilliseconds_I, "UtcTotalMilliseconds_I") + " is out of range for a ZonedTime",
                    Test.ToLog(ZonedTime.MinValue, "ZonedTime.MinValue"),
                    Test.ToLog(ZonedTime.MinValue.UtcTotalMilliseconds, "ZonedTime.MinValue.UtcTotalMilliseconds"),
                    Test.ToLog(ZonedTime.MaxValue, "ZonedTime.MaxValue"),
                    Test.ToLog(ZonedTime.MaxValue.UtcTotalMilliseconds, "ZonedTime.MaxValue.UtcTotalMilliseconds"));

            this.TimeZone = timezone_I;
            this.UtcTotalMilliseconds = UtcTotalMilliseconds_I;

            //                                              //Compute a date to request DST info (Delta is not required)
            long longZtimeTotalMilliseconds = this.UtcTotalMilliseconds +
                this.TimeZone.BaseUtcOffsetMinutes * ZonedTime.MillisecondsPerMinute;
            int intTotalDays = (int)(longZtimeTotalMilliseconds / ZonedTime.MillisecondsPerDay);
            Date date = new Date(intTotalDays);

            DaylightSavingTimeInfo dstinfo = this.TimeZone.GetDaylightSavingTimeInfo(date);

            if (
                //                                          //No DST info
                dstinfo.Date.TotalDays == 0
                )
            {
                this.IsDaylightSavingTime = false;
                this.intDeltaDefinedMinutes = 0;
                this.DaylightSavingTimeTypeOfDay = ZonedTimeDstTypeOfDayEnum.NORMAL;
            }
            else
            {
                long longUtcTotalMinutes = this.UtcTotalMilliseconds / ZonedTime.MillisecondsPerMinute;
                this.IsDaylightSavingTime = longUtcTotalMinutes.
                    IsBetween(dstinfo.UtcStartDaylightSavingTimeMinutes, dstinfo.UtcEndDaylightSavingTimeMinutes - 1);

                this.intDeltaDefinedMinutes = dstinfo.DeltaDefinedMinutes;

                this.DaylightSavingTimeTypeOfDay = ZonedTime.ztimetypeGet(date, dstinfo);
            }
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public ZonedTime(
            //                                              //this.*[O], assing values. 

            Date date_I,
            //                                              //This is a UTC time, should >= 00:00:00
            Time time_I
            )
        {
            this.TimeZone = TimeZoneX.Here();

            ZonedTime.subConstructor(out this.UtcTotalMilliseconds, out this.IsDaylightSavingTime,
                out this.intDeltaDefinedMinutes, out this.DaylightSavingTimeTypeOfDay, date_I, time_I, 0,
                this.TimeZone);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public ZonedTime(
            //                                              //this.*[O], assing values. 

            Date date_I,
            Time time_I,
            TimeZoneX timezone_I
            )
        {
            this.TimeZone = timezone_I;

            ZonedTime.subConstructor(out this.UtcTotalMilliseconds, out this.IsDaylightSavingTime,
                out this.intDeltaDefinedMinutes, out this.DaylightSavingTimeTypeOfDay, date_I, time_I, 0,
                this.TimeZone);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public ZonedTime(
            //                                              //this.*[O], assing values. 

            Date date_I,
            Time time_I,
            int intMilliseconds_I
            )
        {
            this.TimeZone = TimeZoneX.Here();

            ZonedTime.subConstructor(out this.UtcTotalMilliseconds, out this.IsDaylightSavingTime,
                out this.intDeltaDefinedMinutes, out this.DaylightSavingTimeTypeOfDay, date_I, time_I,
                intMilliseconds_I, this.TimeZone);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public ZonedTime(
            //                                              //this.*[O], assing values. 

            Date date_I,
            Time time_I,
            //                                              //0-999.
            int intMilliseconds_I,
            TimeZoneX timezone_I
            )
        {
            this.TimeZone = timezone_I;

            ZonedTime.subConstructor(out this.UtcTotalMilliseconds, out this.IsDaylightSavingTime,
                out this.intDeltaDefinedMinutes, out this.DaylightSavingTimeTypeOfDay, date_I, time_I,
                intMilliseconds_I, this.TimeZone);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR CONTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        private static void subConstructor(

            out long longUtctimeMilliseconds_O,
            out bool boolIsDaylightSavingTime_O,
            out int intDeltaDefinedMinutes_O,
            out ZonedTimeDstTypeOfDayEnum dsttypeofday_O,
            Date date_I,
            Time time_I,
            int intMilliseconds_I,
            TimeZoneX timezone_I
            )
        {
            DaylightSavingTimeInfo dstinfo = timezone_I.GetDaylightSavingTimeInfo(date_I);
            ZonedTime.subAbortIfTimeNotValid(date_I, time_I, dstinfo);

            if (
                !intMilliseconds_I.
                    IsBetween(0, 999)
            )
                Test.Abort(Test.ToLog(intMilliseconds_I, "intMilliseconds_I") + " must be in the range 0 - 999");

            //                                              //This value will need to be adjusted by delta in DST
            longUtctimeMilliseconds_O = date_I.TotalDays * ZonedTime.MillisecondsPerDay +
                time_I.TotalSeconds * ZonedTime.MillisecondsPerSecond + intMilliseconds_I -
                timezone_I.BaseUtcOffsetMinutes * ZonedTime.MillisecondsPerMinute;

            if (
                //                                          //No DST defined
                dstinfo.Date.TotalDays == 0
                )
            {
                boolIsDaylightSavingTime_O = false;
                intDeltaDefinedMinutes_O = 0;
                dsttypeofday_O = ZonedTimeDstTypeOfDayEnum.NORMAL;
            }
            else
            {
                //                                              //We need to compute this first
                intDeltaDefinedMinutes_O = dstinfo.DeltaDefinedMinutes;
                dsttypeofday_O = ZonedTime.ztimetypeGet(date_I, dstinfo);

                //                                              //Adjust Utc time.
                //                                              //Remember Towa's Time include DST delta all day
                if (
                    dsttypeofday_O.IsInSet(ZonedTimeDstTypeOfDayEnum.START_DAYLIGHT_SAVING_TIME,
                        ZonedTimeDstTypeOfDayEnum.DAYLIGHT_SAVING_TIME)
                    )
                {
                    longUtctimeMilliseconds_O = longUtctimeMilliseconds_O -
                        intDeltaDefinedMinutes_O * ZonedTime.MillisecondsPerMinute;
                }

                //                                              //This is the real DST as defined in TimeZoneX
                boolIsDaylightSavingTime_O = longUtctimeMilliseconds_O.
                    IsBetween(dstinfo.UtcStartDaylightSavingTimeMinutes * ZonedTime.MillisecondsPerMinute,
                        dstinfo.UtcEndDaylightSavingTimeMinutes * ZonedTime.MillisecondsPerMinute - 1);
            }

            if (
                !longUtctimeMilliseconds_O.
                    IsBetween(ZonedTime.lonMIN_VALUE_TOTAL_MILLISECONDS, ZonedTime.longMAX_VALUE_TOTAL_MILLISECONDS)
            )
                Test.Abort(
                    Test.ToLog(longUtctimeMilliseconds_O, "longUtctimeMilliseconds_O") + " is out of range for a ZonedTime",
                    Test.ToLog(ZonedTime.MinValue, "ZonedTime.MinValue"),
                    Test.ToLog(ZonedTime.MinValue.UtcTotalMilliseconds, "ZonedTime.MinValue.UtcTotalMilliseconds"),
                    Test.ToLog(ZonedTime.MaxValue, "ZonedTime.MaxValue"),
                    Test.ToLog(ZonedTime.MaxValue.UtcTotalMilliseconds, "ZonedTime.MaxValue.UtcTotalMilliseconds"));
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static void subAbortIfTimeNotValid(
            Date date_I,
            //                                              //Should be in range valid for this date/timezone.
            //                                              //Remember Towa's Time include DST delta all day
            Time time_I,
            DaylightSavingTimeInfo dstinfo_I
            )
        {
            //                                              //On DST start clock +Delta (01:00:00-23:59:59).
            //                                              //On DST end clock -Delta (-1:00:00-23:59:59).

            int intMinTotalMinutes = (
                //                                          //No DST defined
                dstinfo_I.Date.TotalDays == 0
                )
                ? 0 : (
                date_I == dstinfo_I.DateStartDaylightSavingTime
                )
                ? dstinfo_I.DeltaDefinedMinutes : (
                date_I == dstinfo_I.DateEndDaylightSavingTime
                )
                ? -dstinfo_I.DeltaDefinedMinutes : 0;

            //                                              //Verify if time is valid for this date
            if (
                !time_I.TotalSeconds.IsBetween(intMinTotalMinutes * 60, Time.MaxValue.TotalSeconds)
                )
                Test.Abort(Test.ToLog(time_I, "time_I") + " is not a valid time in " + Test.ToLog(date_I, "date_I"),
                    Test.ToLog(intMinTotalMinutes, "intMinTotalMinutes"), Test.ToLog(dstinfo_I, "dstinfo_I"));
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static ZonedTimeDstTypeOfDayEnum ztimetypeGet(
            //                                              //ztimetype, corresponding to this time

            Date date_I,
            DaylightSavingTimeInfo dstinfo_I
            )
        {
            return (
                date_I == dstinfo_I.DateStartDaylightSavingTime
                )
                ? ZonedTimeDstTypeOfDayEnum.START_DAYLIGHT_SAVING_TIME : (
                date_I == dstinfo_I.DateEndDaylightSavingTime
                )
                ? ZonedTimeDstTypeOfDayEnum.END_DAYLIGHT_SAVING_TIME : (
                date_I.IsBetween(dstinfo_I.DateStartDaylightSavingTime, dstinfo_I.DateEndDaylightSavingTime)
                )
                ? ZonedTimeDstTypeOfDayEnum.DAYLIGHT_SAVING_TIME : ZonedTimeDstTypeOfDayEnum.NORMAL;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static ZonedTime Now(
            )
        {
            return ZonedTime.Now(TimeZoneX.Here());
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static ZonedTime Now(
            TimeZoneX timezone_I
            )
        {
            DateTimeOffset dto = DateTimeOffset.Now;

            int intOffsetMinutes = (int)Math.Round(dto.Offset.TotalMinutes);
            long longUtcTotalMillisecond = (dto.Ticks / TimeSpan.TicksPerMillisecond) -
                intOffsetMinutes * ZonedTime.MillisecondsPerMinute;

            return new ZonedTime(longUtcTotalMillisecond, timezone_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public ZonedTime To(
            //                                              //Convert zoned time to exactly the same instant in the
            //                                              //      specify time zone.
            //                                              //ztime, same instant in other time zone

            TimeZoneX timezone_I
            )
        {
            return new ZonedTime(this.UtcTotalMilliseconds, timezone_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public ZonedTime ToHere(
            //                                              //Convert zoned time to exactly the same instant Here.
            //                                              //ztime, same instant Here
            )
        {
            return this.To(TimeZoneX.Here());
        }

        //--------------------------------------------------------------------------------------------------------------
        public static ZonedTime operator +(

            //                                              //ztime, new ZonedTime adding seconds

            ZonedTime ztime_I,
            long longMilliseconds_I
            )
        {
            long longTotalMilliseconds = ztime_I.UtcTotalMilliseconds + longMilliseconds_I;

            return new ZonedTime(longTotalMilliseconds, ztime_I.TimeZone);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static ZonedTime operator -(

            //                                              //ztime, new ZonedTime subtracting seconds

            ZonedTime ztime_I,
            long longMilliseconds_I
            )
        {
            long longTotalMilliseconds = ztime_I.UtcTotalMilliseconds - longMilliseconds_I;

            return new ZonedTime(longTotalMilliseconds, ztime_I.TimeZone);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static long operator -(
            //                                              //int, lapse ztime in seconds

            ZonedTime ztimeLeft_I,
            ZonedTime ztimeRight_I
            )
        {
            return ztimeLeft_I.UtcTotalMilliseconds - ztimeRight_I.UtcTotalMilliseconds;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool operator <(
            ZonedTime ztimeLeft_I,
            ZonedTime ztimeRight_I
            )
        {
            return (
                ztimeLeft_I.UtcTotalMilliseconds < ztimeRight_I.UtcTotalMilliseconds
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool operator <=(
            ZonedTime ztimeLeft_I,
            ZonedTime ztimeRight_I
            )
        {
            return (
                ztimeLeft_I.UtcTotalMilliseconds <= ztimeRight_I.UtcTotalMilliseconds
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool operator >(
            ZonedTime ztimeLeft_I,
            ZonedTime ztimeRight_I
            )
        {
            return (
                ztimeLeft_I.UtcTotalMilliseconds > ztimeRight_I.UtcTotalMilliseconds
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool operator >=(
            ZonedTime ztimeLeft_I,
            ZonedTime ztimeRight_I
            )
        {
            return (
                ztimeLeft_I.UtcTotalMilliseconds >= ztimeRight_I.UtcTotalMilliseconds
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public bool IsBetween(
            ZonedTime ZonedTimeStart_I,
            ZonedTime ZonedTimeEnd_I
            )
        {
            return this.UtcTotalMilliseconds.
                IsBetween(ZonedTimeStart_I.UtcTotalMilliseconds, ZonedTimeEnd_I.UtcTotalMilliseconds);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool operator ==(
            ZonedTime ztimeLeft_I,
            ZonedTime ztimeRight_I
            )
        {
            return (
                ztimeLeft_I.UtcTotalMilliseconds == ztimeRight_I.UtcTotalMilliseconds
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool operator !=(
            ZonedTime ztimeLeft_I,
            ZonedTime ztimeRight_I
            )
        {
            return (
                ztimeLeft_I.UtcTotalMilliseconds != ztimeRight_I.UtcTotalMilliseconds
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToString(
            //                                              //Display ztime in UTC
            )
        {
            return this.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public String ToString(
            String Picture_I
            )
        {
            String strToString;
            if (
                //                                          //(Glg, 9Sep2019) PARCHE REQUERIDO CUANDO NO SE INICIALIZO
                (this.TimeZone != null) &&
                Picture_I.EndsWith("ZZZ", StringComparison.Ordinal)
                )
            {
                TimeSpan tspanOffset = new TimeSpan(0, this.OffsetMinutes, 0);
                DateTimeOffset dto =
                    new DateTimeOffset(this.ZtimeTotalMilliseconds * TimeSpan.TicksPerMillisecond, tspanOffset);
                strToString = dto.ToString(Picture_I.TrimEnd('Z')) + "[" + this.TimeZone.DisplayName + "]";
            }
            else
            {
                DateTime dt = new DateTime(this.UtcTotalMilliseconds * TimeSpan.TicksPerMillisecond);
                strToString = dt.ToString(Picture_I);
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
                obj_L is ZonedTime
                ))
                Test.Abort(Test.ToLog(obj_L.GetType(), "obj_L.GetType") + " should be ZonedTime");

            long longTotalMillisecondsToCompare = ((ZonedTime)obj_L).UtcTotalMilliseconds;

            return (this.UtcTotalMilliseconds < longTotalMillisecondsToCompare) ? -1 :
                (this.UtcTotalMilliseconds > longTotalMillisecondsToCompare) ? 1 : 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //NEXT 2 METHODS ARE TO AVOID COMPILE WARNINGS.

        //--------------------------------------------------------------------------------------------------------------
        public override bool Equals(
            Object obj_L
            )
        {
            if (!(
                obj_L is ZonedTime
                ))
                Test.Abort(Test.ToLog(obj_L.GetType(), "obj_L.type") + " should be ZonedTime");

            ZonedTime ztimeRight = (ZonedTime)obj_L;

            return (
                this == ztimeRight
                );
        }

        //--------------------------------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
            return this.GetHashCode();
        }

        //--------------------------------------------------------------------------------------------------------------
        public byte[] Serialize(
            //                                              //Returns a serialized version of the object.

            )
        {
            //(Glg,14Sep2019) En esta Serialización se pierde el TimeZone, CORREGIRE AL DEFINIR COMO MANEJAR ESTO DADO
            //      LAS DIFERENCIA CON CADA SISTEMA OPERATIVO
            Int64 intTimeTotalMilliseconds = this.ZtimeTotalMilliseconds;
            return BitConverter.GetBytes(intTimeTotalMilliseconds);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Deserialize(
            //                                              //Returns a deserialized object.

            //                                              //The object to deserialize.
            out ZonedTime ZonedTime_O,
            //                                              //The serialized bytes.
            ref byte[] Bytes_IO
            )
        {
            byte[] arrbyteTimeTotalMilliseconds;
            Std.SeparateToDeserializeFixSize(out arrbyteTimeTotalMilliseconds, ref Bytes_IO, 8);
            Int64 intTimeTotalMilliseconds = BitConverter.ToInt64(arrbyteTimeTotalMilliseconds, 0);

            ZonedTime_O = new ZonedTime(intTimeTotalMilliseconds);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
