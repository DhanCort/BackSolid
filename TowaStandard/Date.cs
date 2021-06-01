/*TASK Date*/
using System;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa (RGL-Rodrigo García).
//                                                          //DATE: October 25, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public struct Date : BsysInterface, IComparable, SerializableInterface<Date>
    {
        //                                                  //Class to manipulate Date.
        //                                                  //Date is an structure which handles date information
        //                                                  //      independent of geographic location.

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public static readonly Date DummyValue = new Date();

        //                                                  //Include leap-year
        private const int intDAYS_PER_4YEAR = 365 * 4 + 1;
        //                                                  //100s years are not leap-year, except 400s years
        private const int intDAYS_PER_100YEAR = intDAYS_PER_4YEAR * 25 - 1;
        private const int intDAYS_PER_400YEAR = intDAYS_PER_100YEAR * 4 + 1;

        //                                                  //Date range
        private const int intMIN_VALUE_TOTAL_DAYS = 0;
        //                                                  //9999-12-31 (10000 year - 1 leapyear - 1 day)
        private const int intMAX_VALUE_TOTAL_DAYS = Date.intDAYS_PER_400YEAR * 25 - 366 - 1;

        //                                                  //0001-01-01
        public static readonly Date MinValue = new Date(intMIN_VALUE_TOTAL_DAYS);
        //                                                  //9999-12-31
        public static readonly Date MaxValue = new Date(intMAX_VALUE_TOTAL_DAYS);

        //                                                  //0001-01-07 should have been a Sunday (7 day before also
        //                                                  //      should have been a Sunday)
        private static int intBASE_SUNDAY_TOTAL_DAYS = (new Date(6)).TotalDays - 7;

        //                                                  //Tables to compute dates
        private static readonly int[] arrintDAYS_IN_MONTH = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        private static readonly int[] arrintDAY_UP_TO_MONTH;
        private static readonly int[] arrintDAY_UP_TO_MONTH_LEAPYEAR;

        //                                                  //Valid date picture
        //                                                  //Initializer should order (ascending)
        //                                                  //Sorted array is used only to verify standard pictures.
        private static readonly String[] arrstrPICTURES_UNSORTED = 
            { "yyyyMMdd", "yyyy.MM.dd", "yyyy-MM-dd", "yyyy/MM/dd" };

        public static readonly String[] PICTURES;

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static Date(
            )
        {
            //                                              //Prepare day up to month
            Date.arrintDAY_UP_TO_MONTH = new int[12];
            Date.arrintDAY_UP_TO_MONTH[0] = 0;
            Date.arrintDAY_UP_TO_MONTH_LEAPYEAR = new int[12];
            Date.arrintDAY_UP_TO_MONTH_LEAPYEAR[0] = 0;
            for (int intI = 1; intI < 12; intI = intI + 1)
            {
                arrintDAY_UP_TO_MONTH[intI] = arrintDAY_UP_TO_MONTH[intI - 1] + arrintDAYS_IN_MONTH[intI - 1];

                //                                          //February is 29 days
                arrintDAY_UP_TO_MONTH_LEAPYEAR[intI] = arrintDAY_UP_TO_MONTH_LEAPYEAR[intI - 1] +
                    arrintDAYS_IN_MONTH[intI - 1] + ((intI == 2) ? 1 : 0);
            }

            Date.PICTURES = Std.ConcatenateArrays(Date.arrstrPICTURES_UNSORTED);
            Std.Sort(Date.PICTURES);

            subVerifyConstantsDate();
        }

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHODS FOR INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        private static void subVerifyConstantsDate()
        {
            //                                              //Variables requires for test
            Char[] arrcharUSEFUL_IN_DATE_PICTURE = "yMd.-/".ToCharArray();
            Array.Sort(arrcharUSEFUL_IN_DATE_PICTURE);
            DateTime dtTEST = new DateTime(2018, 11, 10);

            for (int intI = 0; intI < PICTURES.Length; intI = intI + 1)
            {
                Test.AbortIfOneOrMoreCharactersAreNotInSortedSet(PICTURES[intI],
                    "DATE_PICTURES[" + intI + "]", arrcharUSEFUL_IN_DATE_PICTURE, "arrcharUSEFUL_IN_DATE_PICTURE");

                //                                          //Verify it works
                try
                {
                    String str = dtTEST.ToString(PICTURES[intI]);
                }
                catch (Exception sysexceptError)
                {
                    Test.Abort(
                        Test.ToLog(PICTURES, "PICTURES[" + intI + "]") + 
                            "can not be a standard picture, it does not work",
                        Test.ToLog(PICTURES, "PICTURES"),
                            "dtTEST(" + dtTEST.ToString("yyyy-MM-dd HH:mm:ss"),
                        Test.ToLog(sysexceptError, "sysexceptError"));
                }
            }

            Test.AbortIfDuplicate(PICTURES, "PICTURES");
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        //                                                  //Date internally keep days before the date.
        //                                                  //00010101==>0, 00010102==>1, etc.
        public readonly int TotalDays;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        public int Year
        {
            get
            {
                //                                          //Current year for this date
                return (new DateTime(this.TotalDays * TimeSpan.TicksPerDay)).Year;
            }
        }

        public int Month
        {
            get
            {
                //                                          //Current month for this date
                return (new DateTime(this.TotalDays * TimeSpan.TicksPerDay)).Month;
            }
        }

        public int Day
        {
            get
            {
                //                                          //Current day for this date
                return (new DateTime(this.TotalDays * TimeSpan.TicksPerDay)).Day;
            }
        }

        //                                                  //SUNDAY = 0, MONDAY = 2, ..., SATURDAY = 6
        public DayOfWeek DayOfWeek
        {
            get
            {
                int intDayOfWeek = (this.TotalDays - Date.intBASE_SUNDAY_TOTAL_DAYS) % 7;

                return (DayOfWeek)intDayOfWeek;
            }
        }

        public int DayOfYear
        {
            get
            {
                return (new DateTime(this.TotalDays * TimeSpan.TicksPerDay)).DayOfYear;
            }
        }

        public bool IsLeapYear
        {
            get
            {
                return Date.boolIsLeapyearGet(this.Year);
            }
        }

        public int LastDayOfMonth
        {
            get
            {
                return (
                    this.IsLeapYear && (this.Month == 2)
                    )
                    ? 29 : Date.arrintDAYS_IN_MONTH[this.Month - 1];
            }
        }

        public int LastDayOfYear
        {
            get
            {
                return (this.IsLeapYear) ? 366 : 365;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        /*SUPPORT METHOD FOR COMPUTED VARIABLES*/

        //--------------------------------------------------------------------------------------------------------------
        private static bool boolIsLeapyearGet(
            //                                              //bool, true = leapyear.

            int intYear_I
            )
        {
            return (
                ((intYear_I % 400) == 0) ||
                ((intYear_I % 4) == 0) && ((intYear_I % 100) != 0)
                );
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
        public Date(
            //                                              //this.*[O], assing values. 

            //                                              //Days before (Ex. 0 => 0001-01-01)
            int TotalDays_I
            )
        {
            if (
                !TotalDays_I.IsBetween(Date.intMIN_VALUE_TOTAL_DAYS, Date.intMAX_VALUE_TOTAL_DAYS)
                )
                Test.Abort(Test.ToLog(TotalDays_I, "TotalDays_I") + " should be in the range " +
                    Date.intMIN_VALUE_TOTAL_DAYS + "-" + Date.intMAX_VALUE_TOTAL_DAYS);

            this.TotalDays = TotalDays_I;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public Date(
            //                                              //Need to compute total day previous this date since
            //                                              //      0001-01-01 (Ex. 0001-01-01 => 0).
            //                                              //this.*[O], assing values. 

            int Year_I,
            int Month_I,
            int Day_I
            )
        {
            if (
                !Year_I.IsBetween(Date.MinValue.Year, Date.MaxValue.Year)
                )
                Test.Abort(
                    Test.ToLog(Year_I, "Year_I") + " should be in the range " + Date.MinValue.Year + "-" +
                        Date.MaxValue.Year,
                   Test.ToLog(Year_I, "Year_I"), Test.ToLog(Month_I, "Month_I"), Test.ToLog(Day_I, "Month_I"));
            if (
                !Month_I.IsBetween(1, 12)
                )
                Test.Abort(
                   Test.ToLog(Month_I, "Month_I") + " should be in the range 1-12",
                   Test.ToLog(Year_I, "Year_I"), Test.ToLog(Month_I, "Month_I"), Test.ToLog(Day_I, "Month_I"));

            int intDaysInMonth = ((Month_I == 2) && ((Year_I % 4) == 0)) ? 29 : Date.arrintDAYS_IN_MONTH[Month_I - 1];
            if (
                !Day_I.IsBetween(1, intDaysInMonth)
                )
                Test.Abort(
                   Test.ToLog(Day_I, "Day_I") + " should be in the range 1-" + intDaysInMonth,
                   Test.ToLog(Year_I, "Year_I"), Test.ToLog(Month_I, "Month_I"), Test.ToLog(Day_I, "Month_I"));

            int intYears = Year_I - 1;

            int int400YearPeriods = intYears / 400;
            int intYearsLast400YearPeriod = intYears % 400;

            int int100YearPeriods = intYearsLast400YearPeriod / 100;
            int intYearsLast100YearPeriod = intYearsLast400YearPeriod % 100;

            int int4YearPeriods = intYearsLast100YearPeriod / 4;
            int intYearsLast4YearPeriod = intYearsLast100YearPeriod % 4;

            int[] arrintDAY_UP_TO_MONTH_THIS_YEAR = (Date.boolIsLeapyearGet(Year_I))
                ? Date.arrintDAY_UP_TO_MONTH_LEAPYEAR : Date.arrintDAY_UP_TO_MONTH;

            //                                              //Compute total days since 0001.01.01 until previous date
            this.TotalDays = int400YearPeriods * Date.intDAYS_PER_400YEAR +
                int100YearPeriods * Date.intDAYS_PER_100YEAR + int4YearPeriods * Date.intDAYS_PER_4YEAR +
                intYearsLast4YearPeriod * 365 + arrintDAY_UP_TO_MONTH_THIS_YEAR[Month_I - 1] + (Day_I - 1);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static Date Now(
            //                                              //Take date from clock and current timezone.
            //                                              //date, today (local timezone)
            )
        {
            return ZonedTime.Now(TimeZoneX.Here()).Date;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Date Now(
            //                                              //Take date from clock in the specify timezone.
            //                                              //date, day now (specify timezone)

            TimeZoneX timezone_I
            )
        {
            return ZonedTime.Now(timezone_I).Date;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Date operator +(
            //                                              //date, date + days

            Date date_I,
            int days_I
            )
        {
            int intTotalDays = date_I.TotalDays + days_I;

            return new Date(intTotalDays);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Date operator -(
            //                                              //date, date - days

            Date date_I,
            int days_I
            )
        {
            int intTotalDays = date_I.TotalDays - days_I;

            return new Date(intTotalDays);
        }

        //-------------------------------------------------------------------------------------------------------------
        public Date AddMonths(
            //                                              //date, date + months (days can be reduce to maximun of
            //                                              //      days in month)

            int months_I
            )
        {
            int intDateTotalMonths = (this.Year - 1) * 12 + (this.Month - 1) + months_I;
            int intYear = intDateTotalMonths / 12 + 1;
            int intMonth = (intDateTotalMonths % 12) + 1;

            int intDay;
            if (
                Date.boolIsLeapyearGet(intYear) && (intMonth == 2)
                )
            {
                intDay = (this.Day > 29) ? 29 : this.Day;
            }
            else
            {
                intDay = (this.Day > Date.arrintDAYS_IN_MONTH[intMonth - 1])
                    ? Date.arrintDAYS_IN_MONTH[intMonth - 1] : this.Day;
            }

            return new Date(intYear, intMonth, intDay);
        }

        //-------------------------------------------------------------------------------------------------------------
        public Date AddYears(
            //                                              //date, date + years (if month is February, days can be
            //                                              //      reduce to maximun of days in month)

            int years_I
            )
        {
            int intYear = this.Year + years_I;

            int intDay = (!Date.boolIsLeapyearGet(intYear) && (this.Month == 2) && this.Day > 28) ? 28 : this.Day;

            return new Date(intYear, this.Month, intDay);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static int operator -(
            //                                              //int, lapse date in days

            Date dateLeft_I,
            Date dateRight_I
            )
        {
            return dateLeft_I.TotalDays - dateRight_I.TotalDays;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator <(
            Date dateLeft_I,
            Date dateRight_I
            )
        {
            return (
                dateLeft_I.TotalDays < dateRight_I.TotalDays
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator <=(
            Date dateLeft_I,
            Date dateRight_I
            )
        {
            return (
                dateLeft_I.TotalDays <= dateRight_I.TotalDays
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator >(
            Date dateLeft_I,
            Date dateRight_I
            )
        {
            return (
                dateLeft_I.TotalDays > dateRight_I.TotalDays
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator >=(
            Date dateLeft_I,
            Date dateRight_I
            )
        {
            return (
                dateLeft_I.TotalDays >= dateRight_I.TotalDays
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public bool IsBetween(
            Date DateStart_I,
            Date DateEnd_I
            )
        {
            return this.TotalDays.IsBetween(DateStart_I.TotalDays, DateEnd_I.TotalDays);
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator ==(
            Date dateLeft_I,
            Date dateRight_I
            )
        {
            return (
                dateLeft_I.TotalDays == dateRight_I.TotalDays
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool operator !=(
            Date dateLeft_I,
            Date dateRight_I
            )
        {
            return (
                dateLeft_I.TotalDays != dateRight_I.TotalDays
                );
        }

        //-------------------------------------------------------------------------------------------------------------
        public String ToText(
            )
        {
            return this.ToText("yyyy-MM-dd");
        }

        //-------------------------------------------------------------------------------------------------------------
        public String ToText(
            String picture_I
            )
        {
            Test.AbortIfNull(picture_I, "Pictures_I");

            if (
                !picture_I.IsInSortedSet(Date.PICTURES)
                )
                Test.Abort(Test.ToLog(picture_I, "picture_I") + " is not a valid picture",
                    Test.ToLog(Date.arrstrPICTURES_UNSORTED, "DATE_PICTURES"));

            return this.ToString(picture_I);
        }

        //-------------------------------------------------------------------------------------------------------------
        public String ToText(
            DateTextEnum datetext_I
            )
        {
            return this.ToText(datetext_I, Language.ENGLISH);
        }

        //-------------------------------------------------------------------------------------------------------------
        public String ToText(
            //                                              //Convert to the specify text option.
            //                                              //str, date in text format

            DateTextEnum datetext_I,
            Language language_I
            )
        {
            int intT2 = (new T2languagePictureTuple(datetext_I, "")).BinarySearch(language_I.PICTURES);
            String strPicture = language_I.PICTURES[intT2].strPicture;

            return this.ToString(strPicture, language_I.culture);
        }

        //-------------------------------------------------------------------------------------------------------------
        public override String ToString(
            )
        {
            return this.ToString("yyyy-MM-dd");
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public String ToString(
            String Picture_I
            )
        {
            //                                              //We will use DateTime formatter
            DateTime dt = new DateTime(this.TotalDays * TimeSpan.TicksPerDay);

            return dt.ToString(Picture_I);
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public String ToString(
            String Picture_I,
            System.Globalization.CultureInfo Culture_I
            )
        {
            //                                              //We will use DateTime formatter
            DateTime dt = new DateTime(this.TotalDays * TimeSpan.TicksPerDay);

            return dt.ToString(Picture_I, Culture_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            //                                              //Required for Sort, BinarySearch and CompareTo.

            //                                              //this[I], object key info.

            //                                              //Date
            Object obj_L
            )
        {
            if (!(
                obj_L is Date
                ))
                Test.Abort(Test.ToLog(obj_L.GetType(), "obj_L.GetType") + " should be Date");

            int intTotalDaysToCompare = ((Date)obj_L).TotalDays;

            return (this.TotalDays < intTotalDaysToCompare) ? -1 :
                (this.TotalDays > intTotalDaysToCompare) ? 1 : 0;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //NEXT 2 METHODS ARE TO AVOID COMPILE WARNINGS.

        //--------------------------------------------------------------------------------------------------------------
        public override bool Equals(
            Object obj_L
            )
        {
            if (!(
                obj_L is Date
                ))
                Test.Abort(Test.ToLog(obj_L.GetType(), "obj_L.GetType") + " should be Date");

            Date dateRight = (Date)obj_L;

            return (
                this == dateRight
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
            return BitConverter.GetBytes(this.TotalDays);
        }

        //--------------------------------------------------------------------------------------------------------------
        public void Deserialize(
            //                                              //Returns a deserialized object.

            //                                              //Object to deserialize.
            out Date Date_O,
            //                                              //The serialized bytes.
            ref byte[] Bytes_IO
            )
        {
            //                                              //Separate in 2 arrays (deserialize & return)
            byte[] arrbyteToDeserialize;
            Std.SeparateToDeserializeFixSize(out arrbyteToDeserialize, ref Bytes_IO, 4);

            int intTotalDays = BitConverter.ToInt32(arrbyteToDeserialize, 0);

            Date_O = new Date(intTotalDays);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
