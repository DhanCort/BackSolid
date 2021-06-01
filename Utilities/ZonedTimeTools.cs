/*TASK RP. ZonedTimeTools*/
using System;
using TowaStandard;

//                                                          //AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //CO-AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //DATE: May 18, 2020.

namespace Odyssey2Backend.Utilities
{
    //==================================================================================================================
    public static class ZonedTimeTools
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTANT VARIABLES.

        public static readonly TimeZoneX timezone = TimeZoneX.US_CENTRAL;

        public const String strUS_CENTRAL = "US_CENTRAL";
        public const String strUS_ALASKAN = "US_ALASKAN";
        public const String strUS_EASTERN = "US_EASTERN";
        public const String strUS_HAWAIIAN = "US_HAWAIIAN";
        public const String strUS_MOUNTAIN = "US_MOUNTAIN";
        public const String strUS_PACIFIC = "US_PACIFIC";
        public const String strEU_WESTERN_BERLIN = "EU_WESTERN_BERLIN";
        public const String strGREENWICH = "GREENWICH";
        public const String strAZORES = "AZORES";
        public const String strMID_ATLANTIC = "MID_ATLANTIC";
        public const String strGREENLAND = "GREENLAND";
        public const String strATLANTIC = "ATLANTIC";

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //DINAMIC VARIABLES.

        private static ZonedTime ztimeNow_Z;
        public static ZonedTime ztimeNow
        {
            get
            {
                ZonedTimeTools.ztimeNow_Z = ZonedTimeTools.NewZonedTime(Date.Now(ZonedTimeTools.timezone),
                    Time.Now(ZonedTimeTools.timezone));
                return ZonedTimeTools.ztimeNow_Z;
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //TRANSFORMATION METHODS.

        //--------------------------------------------------------------------------------------------------------------

        public static ZonedTime NewZonedTime(
            Date date_I,
            Time time_I
            )
        {
            ZonedTime ztime;
            ZonedTime ztimeTestDay = new ZonedTime(date_I, "01:00:00".ParseToTime(), ZonedTimeTools.timezone);
            if (
                !(ztimeTestDay.DaylightSavingTimeTypeOfDay == ZonedTimeDstTypeOfDayEnum.START_DAYLIGHT_SAVING_TIME) ||
                time_I >= "01:00:00".ParseToTime()
                )
            {
                ztime = new ZonedTime(date_I, time_I);
            }
            else
            {
                ztime = ztimeTestDay;
            }
            return ztime;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolIsValidStartDateTimeAndEndDateTime(
            //                                              //Validate the Dates and Time if this is valid, therefore
            //                                              //      return the ZtimeStart and ZtimeEnd.

            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            String strPrintshopTimeZone_I,
            out ZonedTime ztimeStart_O,
            out ZonedTime ztimeEnd_O,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsValid = false;

            //                                              //Assign ZoneTime.
            ztimeStart_O = new ZonedTime();
            ztimeEnd_O = new ZonedTime();

            strUserMessage_IO = "Date or time cannot be empty.";
            strDevMessage_IO = "";
            if (
                !String.IsNullOrEmpty(strStartDate_I) && !String.IsNullOrEmpty(strStartTime_I) &&
                !String.IsNullOrEmpty(strEndDate_I) && !String.IsNullOrEmpty(strEndTime_I)
                )
            {
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Date or time format is not allow.";
                if (
                    //                                      //Validate date and time strings are parseable.
                    strStartDate_I.IsParsableToDate() &&
                    strStartTime_I.IsParsableToTime() &&
                    strEndDate_I.IsParsableToDate() &&
                    strEndTime_I.IsParsableToTime()
                    )
                {
                    //                                      //To easy code.
                    ztimeStart_O = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(strStartDate_I.ParseToDate(),
                        strStartTime_I.ParseToTime(), strPrintshopTimeZone_I);
                    ztimeEnd_O = ZonedTimeTools.NewZonedTimeConsideringPrintshopTimeZone(strEndDate_I.ParseToDate(),
                        strEndTime_I.ParseToTime(), strPrintshopTimeZone_I);

                    strUserMessage_IO = "Select valid date or time.";
                    strDevMessage_IO = "";
                    if (
                        //                                  //Date and time are in the future.
                        ztimeStart_O >= ZonedTimeTools.ztimeNow &&
                        ztimeEnd_O >= ZonedTimeTools.ztimeNow
                        )
                    {
                        strUserMessage_IO = "Select valid date or time.";
                        strDevMessage_IO = "";
                        if (
                            //                              //End is after the start.
                            ztimeStart_O < ztimeEnd_O
                            )
                        {
                            boolIsValid = true;
                        }
                        else
                        {
                            if (
                                ztimeStart_O == ztimeEnd_O
                                )
                            {
                                strUserMessage_IO = "Something is wrong.";
                                strDevMessage_IO = "Empty DateTime.";
                                if (
                                    ztimeStart_O.DaylightSavingTimeTypeOfDay ==
                                    ZonedTimeDstTypeOfDayEnum.START_DAYLIGHT_SAVING_TIME &&
                                    ztimeEnd_O.Time == "01:00:00".ParseToTime()
                                    )
                                {
                                    strUserMessage_IO = "In daylight saving time.";
                                    strDevMessage_IO = "Starts in hour that does not exists becuase of the dayligth " +
                                        "saving time.";
                                }
                            }
                        }
                    }
                }
            }

            return boolIsValid;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static ZonedTime NewZonedTimeConsideringPrintshopTimeZone(
            //                                              //For a given date and time, return a US_CENTRAL zonedTime
            //                                              //      considering printshop's timezone.

            Date date_I,
            Time time_I,
            String strPrintshopTimeZone_I
            )
        {
            //                                              //Base zonedTime created from US_CENTRAL.
            ZonedTime ztime = new ZonedTime(date_I, time_I, TimeZoneX.US_CENTRAL);
            if (
                //                                          //Printshop has not set zonedTime, we will take US_CENTRAL.
                strPrintshopTimeZone_I == null
                )
            {
                ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, TimeZoneX.US_CENTRAL, TimeZoneX.US_CENTRAL);
                //ZonedTime ztimeTestDay = new ZonedTime(date_I, "01:00:00".ParseToTime(), ZonedTimeTools.timezone);
                //if (
                //    !(ztimeTestDay.DaylightSavingTimeTypeOfDay == 
                //    ZonedTimeDstTypeOfDayEnum.START_DAYLIGHT_SAVING_TIME) || time_I >= "01:00:00".ParseToTime()
                //    )
                //{
                //    //                                      //NOTHING TO DO.
                //}
                //else
                //{
                //    ztime = ztimeTestDay;
                //}
            }
            else
            {
                /*CASE*/
                if (
                    strPrintshopTimeZone_I == ZonedTimeTools.strUS_CENTRAL 
                    )
                {
                    //NOTHING TO DO.
                }
                else if (
                   strPrintshopTimeZone_I == ZonedTimeTools.strUS_ALASKAN 
                   )
                {
                    ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, TimeZoneX.US_ALASKAN, TimeZoneX.US_CENTRAL);
                }
                else if (
                    strPrintshopTimeZone_I == ZonedTimeTools.strUS_EASTERN 
                   )
                {
                    ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, TimeZoneX.US_EASTERN, TimeZoneX.US_CENTRAL);
                }
                else if (
                    strPrintshopTimeZone_I == ZonedTimeTools.strUS_HAWAIIAN 
                   )
                {
                    ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, TimeZoneX.US_HAWAIIAN, TimeZoneX.US_CENTRAL);
                }
                else if (
                    strPrintshopTimeZone_I == ZonedTimeTools.strUS_MOUNTAIN 
                   )
                {
                    ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, TimeZoneX.US_MOUNTAIN, TimeZoneX.US_CENTRAL);
                }
                else if (
                    strPrintshopTimeZone_I == ZonedTimeTools.strUS_PACIFIC 
                   )
                {
                    ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, TimeZoneX.US_PACIFIC, TimeZoneX.US_CENTRAL);
                }
                else if (
                    strPrintshopTimeZone_I == ZonedTimeTools.strEU_WESTERN_BERLIN
                    )
                {
                    ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, TimeZoneX.EU_WESTERN, TimeZoneX.US_CENTRAL);
                }
                else if (
                    strPrintshopTimeZone_I == ZonedTimeTools.strGREENWICH
                    )
                {
                    ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, TimeZoneX.GREENWICH, TimeZoneX.US_CENTRAL);
                }
                else if (
                    strPrintshopTimeZone_I == ZonedTimeTools.strAZORES
                    )
                {
                    ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, TimeZoneX.AZORES, TimeZoneX.US_CENTRAL);
                }
                else if (
                    strPrintshopTimeZone_I == ZonedTimeTools.strMID_ATLANTIC
                    )
                {
                    ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, TimeZoneX.MID_ATLANTIC, TimeZoneX.US_CENTRAL);
                }
                else if (
                    strPrintshopTimeZone_I == ZonedTimeTools.strGREENLAND
                    )
                {
                    ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, TimeZoneX.GREENLAND, TimeZoneX.US_CENTRAL);
                }
                else if (
                    strPrintshopTimeZone_I == ZonedTimeTools.strATLANTIC
                    )
                {
                    ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, TimeZoneX.ATLANTIC, TimeZoneX.US_CENTRAL);
                }
                /*END-CASE*/
            }

            return ztime;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolIsValidDateTime(
            //                                              //Validate the Date and Time if this are valid and are not
            //                                              //      in future, therefore return the Ztime.

            String strDate_I,
            String strTime_I,
            out ZonedTime ztime_O,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsValid = false;

            //                                              //Assign ZoneTime.
            ztime_O = new ZonedTime();

            strUserMessage_IO = "Date or time cannot be empty.";
            strDevMessage_IO = "";
            if (
                !String.IsNullOrEmpty(strDate_I) && !String.IsNullOrEmpty(strTime_I)
                )
            {
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Date or time format is not allow.";
                if (
                    //                                      //Validate date and time strings are parseable.
                    strDate_I.IsParsableToDate() &&
                    strTime_I.IsParsableToTime()
                    )
                {
                    //                                      //To easy code.
                    ztime_O = ZonedTimeTools.NewZonedTime(strDate_I.ParseToDate(), strTime_I.ParseToTime());

                    strUserMessage_IO = "Date and time cannot be in the future.";
                    strDevMessage_IO = "";
                    if (
                        //                                  //Date and time are not in the future.
                        ztime_O <= ZonedTimeTools.ztimeNow
                        )
                    {
                        boolIsValid = true;
                    }
                }
            }

            return boolIsValid;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolDateIsNotInTheFuture(
            //                                              //Validate the Date if this is valid and it is not
            //                                              //      in the future.

            String strDate_I,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsValid = false;

            strUserMessage_IO = "Date cannot be empty.";
            strDevMessage_IO = "";
            if (
                !String.IsNullOrEmpty(strDate_I)
                )
            {
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Date format is not allow.";
                if (
                    //                                      //Validate date and time strings are parseable.
                    strDate_I.IsParsableToDate()
                    )
                {
                    Date dateDate = strDate_I.ParseToDate();
                    Date dateNow = Date.Now(ZonedTimeTools.timezone);

                    strUserMessage_IO = "Date cannot be in the future.";
                    strDevMessage_IO = "";
                    if (
                        //                                  //Date and time are not in the future.
                        dateDate <= dateNow
                        )
                    {
                        boolIsValid = true;
                    }
                }
            }

            return boolIsValid;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static bool boolIsValidStartDateTimeAndEndDateTimeAccount(
            //                                              //Validate the Dates and Time if this is valid, therefore
            //                                              //      return the ZtimeStart and ZtimeEnd.
            //                                              //This method allows to use dates in the past.

            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            out ZonedTime ztimeStart_O,
            out ZonedTime ztimeEnd_O,
            ref String strUserMessage_IO,
            ref String strDevMessage_IO
            )
        {
            bool boolIsValid = false;

            //                                              //Assign ZoneTime.
            ztimeStart_O = new ZonedTime();
            ztimeEnd_O = new ZonedTime();

            strUserMessage_IO = "Date or time cannot be empty.";
            strDevMessage_IO = "";
            if (
                !String.IsNullOrEmpty(strStartDate_I) && !String.IsNullOrEmpty(strStartTime_I) &&
                !String.IsNullOrEmpty(strEndDate_I) && !String.IsNullOrEmpty(strEndTime_I)
                )
            {
                strUserMessage_IO = "Something is wrong.";
                strDevMessage_IO = "Date or time format is not allow.";
                if (
                    //                                      //Validate date and time strings are parseable.
                    strStartDate_I.IsParsableToDate() &&
                    strStartTime_I.IsParsableToTime() &&
                    strEndDate_I.IsParsableToDate() &&
                    strEndTime_I.IsParsableToTime()
                    )
                {
                    //                                      //To easy code.
                    ztimeStart_O = ZonedTimeTools.NewZonedTime(strStartDate_I.ParseToDate(),
                        strStartTime_I.ParseToTime());
                    ztimeEnd_O = ZonedTimeTools.NewZonedTime(strEndDate_I.ParseToDate(), strEndTime_I.ParseToTime());

                    strUserMessage_IO = "Select valid date or time.";
                    strDevMessage_IO = "";

                    bool boolEndIsAfterStart = ztimeStart_O < ztimeEnd_O;
                    if (
                        //                                  //End is after the start.
                        boolEndIsAfterStart
                        )
                    {
                        boolIsValid = true;
                    }
                    else
                    {
                        if (
                            ztimeStart_O == ztimeEnd_O
                            )
                        {
                            strUserMessage_IO = "Something is wrong.";
                            strDevMessage_IO = "Empty DateTime.";
                            if (
                                ztimeStart_O.DaylightSavingTimeTypeOfDay ==
                                ZonedTimeDstTypeOfDayEnum.START_DAYLIGHT_SAVING_TIME &&
                                ztimeEnd_O.Time == "01:00:00".ParseToTime()
                                )
                            {
                                strUserMessage_IO = "In daylight saving time.";
                                strDevMessage_IO = "Starts in hour that does not exists because of the dayligth " +
                                    "saving time.";
                            }
                        }
                    }
                }
            }

            return boolIsValid;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        public static ZonedTime ztimeCSTToASpecificTimeZone(
            //                                              //Convert 'Central Standard Time' time zone based on
            //                                              //      given timezone id

            Date date_I,
            Time time_I,
            String strTimeZoneId_I
            )
        {
            //                                              //Timezone to return
            ZonedTime ztime;
            TimeZoneX timezoneFrom = TimeZoneX.US_CENTRAL;

            /*CASE*/
            if (
                //                                          //Convert time to US_HAWAIIAN
                strTimeZoneId_I == ZonedTimeTools.strUS_HAWAIIAN
                )
            {
                TimeZoneX timezoneTo = TimeZoneX.US_HAWAIIAN;
                ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, timezoneFrom, timezoneTo);
            }
            else if (
                //                                          //Convert time to US_ALASKAN
                strTimeZoneId_I == ZonedTimeTools.strUS_ALASKAN
                )
            {
                TimeZoneX timezoneTo = TimeZoneX.US_ALASKAN;
                ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, timezoneFrom, timezoneTo);
            }
            else if (
                //                                          //Convert time to US_PACIFIC
                strTimeZoneId_I == ZonedTimeTools.strUS_PACIFIC
                )
            {
                TimeZoneX timezoneTo = TimeZoneX.US_PACIFIC;
                ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, timezoneFrom, timezoneTo);
            }
            else if (
                //                                          //Convert time to US_MOUNTAIN
                strTimeZoneId_I == ZonedTimeTools.strUS_MOUNTAIN
                )
            {
                TimeZoneX timezoneTo = TimeZoneX.US_MOUNTAIN;
                ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, timezoneFrom, timezoneTo);
            }
            else if (
               //                                           //Convert time to US_EASTERN
               strTimeZoneId_I == ZonedTimeTools.strUS_EASTERN
                )
            {
                TimeZoneX timezoneTo = TimeZoneX.US_EASTERN;
                ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, timezoneFrom, timezoneTo);
            }
            else if (
                strTimeZoneId_I == ZonedTimeTools.strEU_WESTERN_BERLIN
                )
            {
                TimeZoneX timezoneTo = TimeZoneX.EU_WESTERN;
                ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, timezoneFrom, timezoneTo);
            }
            else if (
                strTimeZoneId_I == ZonedTimeTools.strGREENWICH
                )
            {
                TimeZoneX timezoneTo = TimeZoneX.GREENWICH;
                ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, timezoneFrom, timezoneTo);
            }
            else if (
                strTimeZoneId_I == ZonedTimeTools.strAZORES
                )
            {
                TimeZoneX timezoneTo = TimeZoneX.AZORES;
                ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, timezoneFrom, timezoneTo);
            }
            else if (
                strTimeZoneId_I == ZonedTimeTools.strMID_ATLANTIC
                )
            {
                TimeZoneX timezoneTo = TimeZoneX.MID_ATLANTIC;
                ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, timezoneFrom, timezoneTo);
            }
            else if (
                strTimeZoneId_I == ZonedTimeTools.strGREENLAND
                )
            {
                TimeZoneX timezoneTo = TimeZoneX.GREENLAND;
                ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, timezoneFrom, timezoneTo);
            }
            else if (
                strTimeZoneId_I == ZonedTimeTools.strATLANTIC
                )
            {
                TimeZoneX timezoneTo = TimeZoneX.ATLANTIC;
                ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, timezoneFrom, timezoneTo);
            }
            else
            {
                //                                          //(To) Default is US_CENTRAL
                TimeZoneX timezoneTo = timezoneFrom;
                ztime = ZonedTimeTools.ztimeConvertTime(date_I, time_I, timezoneFrom, timezoneTo);
            }
            /*END-CASE*/

            return ztime;
        }

        //--------------------------------------------------------------------------------------------------------------
        public static ZonedTime ztimeConvertTime(
            //                                              //Convert time to a given time zone

            Date date_I,
            Time time_I,
            //                                              //Timezone to convert
            TimeZoneX timezoneFrom_I,
            //                                              //Destination timezone
            TimeZoneX timezoneTo_I
            )
        {
            ZonedTime ztimeFinal;
            ZonedTime ztimeTestDay = new ZonedTime(date_I, "01:00:00".ParseToTime(), timezoneFrom_I);
            if (
                !(ztimeTestDay.DaylightSavingTimeTypeOfDay == ZonedTimeDstTypeOfDayEnum.START_DAYLIGHT_SAVING_TIME) ||
                time_I >= "01:00:00".ParseToTime()
                )
            {
                //                                          //Create Printshop's zonedtime.
                ZonedTime ztimeFrom = new ZonedTime(date_I, time_I, timezoneFrom_I);
                //                                          //Convert timezone.
                ZonedTime ztimeConverted = ztimeFrom.To(timezoneTo_I);
                //                                          //Create new zonedtime with the correct values.
                ztimeFinal = new ZonedTime(ztimeConverted.Date, ztimeConverted.Time);
            }
            else
            {
                ztimeFinal = ztimeTestDay;
            }

            return ztimeFinal;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
