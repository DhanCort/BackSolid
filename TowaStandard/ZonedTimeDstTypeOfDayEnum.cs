/*TASK ZonedTimeDstTypeOfDayEnum*/
//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: October 30, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public enum ZonedTimeDstTypeOfDayEnum : byte
    {
        //                                                  //Any day of "normal" time period that is not the first ___
        //                                                  //      (Ex. in Mexico form Monday 2018-10-29 up to
        //                                                  //      Saturday 2019-04-06)
        NORMAL,

        //                                                  //First day of daylight saving time period (Ex. in Mexico
        //                                                  //      Sunday 2018-04-01)
        START_DAYLIGHT_SAVING_TIME,
        //                                                  //Any day of daylight saving time period that is not the
        //                                                  //      first (Ex. in Mexico form Monday 2018-04-01 up to
        //                                                  //      Saturday 2018-10-27)
        DAYLIGHT_SAVING_TIME,
        //                                                  //First day of "NORMAL" time (Ex. in Mexico Sunday
        //                                                  //      2018-10-28.
        END_DAYLIGHT_SAVING_TIME,

        //                                                  //START_DAYLIGHT_SAVING_TIME and DAYLIGHT_SAVING_TIME are
        //                                                  //      in daylight saving time period.
        //                                                  //END_DAYLIGHT_SAVING_TIME and NORMAL are in "normal" time
        //                                                  //      period.
    }

    //==================================================================================================================

}
/*END-TASK*/