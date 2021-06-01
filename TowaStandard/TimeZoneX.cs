/*TASK TimeZoneX*/
using System;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa (RGL-Rodrigo García).
//                                                          //DATE: October 19, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public class TimeZoneX : BsysAbstract, IComparable
    {
        //                                                  //This class overrides System.TimeZoneX.

        //                                                  //Notice that constructor is PRIVATE and all possible object
        //                                                  //      are constructed as PUBLIC constants during
        //                                                  //      initialization.

        //                                                  //This class will include any info requires to process date
        //                                                  //      & time that depends on time zone.

        //--------------------------------------------------------------------------------------------------------------
        /*CONSTANTS*/

        public static readonly TimeZoneX UTC = new TimeZoneX("", "", TimeZoneInfo.FindSystemTimeZoneById("UTC"));

        
        //                                                  //Mexico
        public static readonly TimeZoneX MX_PACIFIC = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time (Mexico)"));
        public static readonly TimeZoneX MX_MOUNTAIN = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time (Mexico)"));
        public static readonly TimeZoneX MX_CENTRAL = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)"));
        public static readonly TimeZoneX MX_EASTERN = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time (Mexico)"));

        //                                                  //United States of America
        public static readonly TimeZoneX US_HAWAIIAN = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"));
        public static readonly TimeZoneX US_ALASKAN = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time"));
        public static readonly TimeZoneX US_PACIFIC = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
        public static readonly TimeZoneX US_MOUNTAIN = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("US Mountain Standard Time"));
        public static readonly TimeZoneX US_CENTRAL = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
        public static readonly TimeZoneX US_EASTERN = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time"));

        //                                                  //South hemisphere and West UTC
        public static readonly TimeZoneX PA = new TimeZoneX("", "",
        TimeZoneInfo.FindSystemTimeZoneById("Paraguay Standard Time"));
        public static readonly TimeZoneX BR = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        //                                                  //North hemisphere and East UTC
        public static readonly TimeZoneX EU_CENTRAL = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"));
        public static readonly TimeZoneX FI = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time"));

        //                                                  //South hemisphere and East UTC
        public static readonly TimeZoneX NZ = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time"));
        public static readonly TimeZoneX AU_EASTERN = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time"));

        //                                                  //Nepal (offset +5:45)
        public static readonly TimeZoneX NE = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Nepal Standard Time"));

        //                                                  //Dateline Islands (offset -12:00 - min offset) 
        public static readonly TimeZoneX DL = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Dateline Standard Time"));

        //                                                  //Line Islands (offset +14:00 - max offset)
        public static readonly TimeZoneX LI = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Line Islands Standard Time"));

        //                                                  //Greenwich
        public static readonly TimeZoneX GREENWICH = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Greenwich Standard Time"));

        //                                                  //Azores
        public static readonly TimeZoneX AZORES = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Azores Standard Time"));

        //                                                  //Mid-Atlantic
        public static readonly TimeZoneX MID_ATLANTIC = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Mid-Atlantic Standard Time"));

        //                                                  //Greenland
        public static readonly TimeZoneX GREENLAND = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Greenland Standard Time"));

        //                                                  //Atlantic
        public static readonly TimeZoneX ATLANTIC = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time"));

        //                                                  //Western Europe
        public static readonly TimeZoneX EU_WESTERN = new TimeZoneX("", "",
            TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"));

        //                                                  //This array is required to find local timezone
        public static readonly TimeZoneX[] arrtimezoneSORTED =
        {
            //UTC,
            
            MX_PACIFIC, MX_MOUNTAIN, MX_CENTRAL,
            MX_EASTERN, US_HAWAIIAN, US_ALASKAN, US_PACIFIC,
            US_MOUNTAIN, US_CENTRAL, US_EASTERN, PA,
            BR, EU_CENTRAL, FI, NZ, AU_EASTERN,
            NE, DL, LI, GREENWICH, AZORES, MID_ATLANTIC, GREENLAND, ATLANTIC, EU_WESTERN,

        };

        //--------------------------------------------------------------------------------------------------------------
        /*INITIALIZER*/

        //--------------------------------------------------------------------------------------------------------------
        static TimeZoneX(
            )
        {
            Array.Sort(arrtimezoneSORTED);
        }

        //--------------------------------------------------------------------------------------------------------------
        /*INSTANCE VARIABLES*/

        public /*KEY*/ String DisplayName;
        public String IdWindows;
        public String IdJava;

        //                                                  //Required in C# to access timezone information.
        //                                                  //To implement this class in other Technology Instance (Ex.
        //                                                  //      Java) may require other non-standard content.
        public TimeZoneInfo tzinfo;

        //--------------------------------------------------------------------------------------------------------------
        /*COMPUTED VARIABLES*/

        public int BaseUtcOffsetMinutes
        {
            get
            {
                TimeSpan tspan = this.tzinfo.BaseUtcOffset;

                return tspan.Hours * 60 + tspan.Minutes;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogShort()
        {
            return "<" + Test.ToLog(this.DisplayName) + ", " + Test.ToLog(this.IdWindows) + ", " +
                   Test.ToLog(this.IdJava) + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToLogFull()
        {
            return "<" + Test.ToLog(this.DisplayName, "DisplayName") + ", " + Test.ToLog(this.IdWindows, "IdWindows") +
                ", " + Test.ToLog(this.IdJava, "IdJava") + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        /*OBJECT CONSTRUCTORS*/

        //--------------------------------------------------------------------------------------------------------------
        private TimeZoneX(
            //                                              //this.*[O], assing values. 

            String strIdWindows_I,
            String strIdJava_I,
            TimeZoneInfo tzinfo_I)
        {
            Test.AbortIfNull(strIdWindows_I, "strIdWindows_I");
            Test.AbortIfNull(strIdJava_I, "strIdJava_I");
            Test.AbortIfNull(tzinfo_I, "tzinfo_I");

            this.DisplayName = tzinfo_I.DisplayName;
            this.IdWindows = strIdWindows_I;
            this.IdJava = strIdJava_I;
            this.tzinfo = tzinfo_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public static TimeZoneX Here(
            //                                              //Find local timezone (where software is running)
            )
        {
            TimeZoneInfo tzinfoLocal = TimeZoneInfo.Local;

            int intTz = tzinfoLocal.DisplayName.BinarySearch(arrtimezoneSORTED);

            TimeZoneX timezoneHere = TimeZoneX.UTC;
            /*
            TimeZoneX timezoneHere;
            if (
                intTz < 0
                )
            {
                Test.Warning("tzinfoLocal.DisplayName(" + tzinfoLocal.DisplayName +
                    ") is not included in standard TimeZoneX, will be replaced by TimeZoneX.UTC");

                timezoneHere = TimeZoneX.UTC;
            }
            else
            {
                timezoneHere = TimeZoneX.arrtimezoneSORTED[intTz];
            }
            */

            return timezoneHere;
        }

        //--------------------------------------------------------------------------------------------------------------
        public DaylightSavingTimeInfo GetDaylightSavingTimeInfo(
            //                                              //dstinfo, info of DST period.
            //                                              //(see description of DaylightSavingTimeInfo)

            //                                              //Date of actual DST period or previous to next period
            Date Date_I
            )
        {
            TimeZoneInfo.AdjustmentRule tzadjrule = this.tzadjruleGet(Date_I);

            DaylightSavingTimeInfo dstinfoGet;
            if (
                //                                          //NO adjustment rule found
                tzadjrule == default(TimeZoneInfo.AdjustmentRule)
                )
            {
                //                                          //No info
                dstinfoGet = default(DaylightSavingTimeInfo);
            }
            else
            {
                //                                          //Compute DST dates

                int intStartYear = Date_I.Year;
                //                                          //Start(year-month) should be < End(year-month)
                int intEndYear = intStartYear;

                if (
                    //                                      //Usually true for south hemisphere countries where
                    //                                      //      daylight saving time starts on fall and finishes
                    //                                      //      on spring
                    tzadjrule.DaylightTransitionStart.Month > tzadjrule.DaylightTransitionEnd.Month &&
                    intStartYear > 1
                    )
                {
                    if (
                        Date_I.Month <= tzadjrule.DaylightTransitionEnd.Month
                        )
                    {
                        intStartYear = intStartYear - 1;
                    }
                    else
                    {
                        intEndYear = intEndYear + 1;
                    }
                }

                Date dateStart = TimeZoneX.dateTransition(tzadjrule.DaylightTransitionStart, intStartYear);
                Date dateEnd = TimeZoneX.dateTransition(tzadjrule.DaylightTransitionEnd, intEndYear);

                //                                          //Construct DST Info

                //                                          //Delta in minutes
                int intDeltaInMinutes = tzadjrule.DaylightDelta.Hours * 60 + tzadjrule.DaylightDelta.Minutes;

                //                                          //Start in minutes
                DateTime dtimeTimeOfDayStart = tzadjrule.DaylightTransitionStart.TimeOfDay;
                long longStartInMinutes = dateStart.TotalDays * 24 * 60 +
                    (dtimeTimeOfDayStart.Hour * 60 + dtimeTimeOfDayStart.Minute);

                //                                          //End in minutes
                DateTime dtimeTimeOfDayEnd = tzadjrule.DaylightTransitionEnd.TimeOfDay;
                long longEndInMinutes = dateEnd.TotalDays * 24 * 60 +
                    (dtimeTimeOfDayEnd.Hour * 60 + dtimeTimeOfDayEnd.Minute);

                //                                          //Adjust Start and End DST period to Utc.
                //                                          //Notice: End DST (TimeOfDay) should include Delta

                dstinfoGet = new DaylightSavingTimeInfo(Date_I, intDeltaInMinutes, dateStart,
                    longStartInMinutes - this.BaseUtcOffsetMinutes, dateEnd,
                    longEndInMinutes - this.BaseUtcOffsetMinutes - intDeltaInMinutes);
            }

            return dstinfoGet;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private TimeZoneInfo.AdjustmentRule tzadjruleGet(
            Date Date_I
            )
        {
            TimeZoneInfo.AdjustmentRule[] arrtzadjrule = this.tzinfo.GetAdjustmentRules();

            //                                          //Move date back to day 1 of month, and compute tick
            Date dateDay1 = new Date(Date_I.Year, Date_I.Month, 1);
            long longDateDay1InTick = dateDay1.TotalDays * TimeSpan.TicksPerDay;

            int intI = 0;
            /*WHILE-UNTIL*/
            while (!(
                (intI >= arrtzadjrule.Length) ||
                //                                          //This ztime.Date takes this adjusments rules
                longDateDay1InTick.IsBetween(arrtzadjrule[intI].DateStart.Ticks, arrtzadjrule[intI].DateEnd.Ticks)
                ))
            {
                intI = intI + 1;
            }

            TimeZoneInfo.AdjustmentRule tzadjrule;
            if (
                //                                          //Did NOT Found a rule
                (intI >= arrtzadjrule.Length) ||
                //                                          //We are at the limit (year 9998 or 9999), depending on
                //                                          //      month/day may try to look after year 9999 (this
                //                                          //      will be manage has NO DST)
                (Date_I.Year >= 9998)
                )
            {
                //                                          //No info, mean NO DST
                tzadjrule = default(TimeZoneInfo.AdjustmentRule);
            }
            else
            {
                tzadjrule = arrtzadjrule[intI];

                //                                          //There are some tzadjrules in C# that incorreclty define
                //                                          //      that DST starts at 23:59:59.999 PM when it really
                //                                          //      starts at 12:00:00.000 AM of the next day.
                if (
                    tzadjrule.DaylightTransitionStart.TimeOfDay.Hour == 23 &&
                    tzadjrule.DaylightTransitionStart.TimeOfDay.Minute == 59 &&
                    tzadjrule.DaylightTransitionStart.TimeOfDay.Second == 59
                )
                {
                    //                                      //The new transition rule will start at 12:00:00.000 AM
                    //                                      //      one day after the previous transitions rule
                    TimeZoneInfo.TransitionTime tzttimeNewDstStart = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(
                        new DateTime(1, 1, 1, 0, 0, 0),
                        tzadjrule.DaylightTransitionStart.Month,
                        tzadjrule.DaylightTransitionStart.Week,
                        (DayOfWeek)(((int)tzadjrule.DaylightTransitionStart.DayOfWeek + 1) % 7));

                    tzadjrule = TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(tzadjrule.DateStart, tzadjrule.DateEnd,
                        tzadjrule.DaylightDelta, tzttimeNewDstStart, tzadjrule.DaylightTransitionEnd);
                }

                //                                          //The same issue explained before happens when DST ends.
                if (
                    tzadjrule.DaylightTransitionEnd.TimeOfDay.Hour == 23 &&
                    tzadjrule.DaylightTransitionEnd.TimeOfDay.Minute == 59 &&
                    tzadjrule.DaylightTransitionEnd.TimeOfDay.Second == 59
                )
                {
                    //                                      //The new transition rule will start at 12:00:00.000 AM
                    //                                      //      one day after the previous transitions rule
                    TimeZoneInfo.TransitionTime tzttimeNewDstEnd = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(
                        new DateTime(1, 1, 1, 0, 0, 0),
                        tzadjrule.DaylightTransitionEnd.Month,
                        tzadjrule.DaylightTransitionEnd.Week,
                        (DayOfWeek)(((int)tzadjrule.DaylightTransitionEnd.DayOfWeek + 1) % 7));

                    tzadjrule = TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(tzadjrule.DateStart, tzadjrule.DateEnd,
                        tzadjrule.DaylightDelta, tzadjrule.DaylightTransitionStart, tzttimeNewDstEnd);
                }

            }

            return tzadjrule;
        }

        //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        private static Date dateTransition(
            //                                              //date, transition date

            TimeZoneInfo.TransitionTime tzttime_I,
            int intYear_I
            )
        {
            //                                              //Compute day from rule
            int intDay;
            if (
                tzttime_I.IsFixedDateRule
                )
            {
                //                                          //Fix rule
                intDay = tzttime_I.Day;
            }
            else
            {
                //                                          //Need to compute the day

                //                                          //Compute day in first week
                Date dateDay1 = new Date(intYear_I, tzttime_I.Month, 1);
                int intDaysToAdd = (int)tzttime_I.DayOfWeek - (int)dateDay1.DayOfWeek;
                if (
                    intDaysToAdd < 0
                    )
                {
                    intDaysToAdd = intDaysToAdd + 7;
                }
                int intDayWeek1 = 1 + intDaysToAdd;
                //                                          //Compute day in required week.
                //                                          //First = 1, ..., Four = 4, Last = 5.
                //                                          //Since last week is coded as week 5, may need to adjust to
                //                                          //      week 4

                intDay = intDayWeek1 + ((int)tzttime_I.Week - 1) * 7;

                if (
                    intDay > dateDay1.LastDayOfMonth
                    )
                {
                    intDay = intDay - 7;
                }
            }

            return new Date(intYear_I, tzttime_I.Month, intDay);
        }

        //--------------------------------------------------------------------------------------------------------------
        public override String ToString()
        {
            return "<" + this.DisplayName + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(Object objArgument_I)
        {
            int intCompareTo;
            /*CASE*/
            if (
                objArgument_I is TimeZoneX
                )
            {
                intCompareTo = String.CompareOrdinal(this.DisplayName, ((TimeZoneX)objArgument_I).DisplayName);
            }
            else if (
                objArgument_I is String
                )
            {
                intCompareTo = String.CompareOrdinal(this.DisplayName, (String)objArgument_I);
            }
            else
            {
                Test.Abort(
                    Test.ToLog(objArgument_I.GetType(), "objArgument_I.type") +
                        " is not a compatible CompareTo argument, the options are: TimeZoneX & String",
                    Test.ToLog(this.GetType(), "this.type"));

                intCompareTo = -1;
            }
            /*CASE*/

            return intCompareTo;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
