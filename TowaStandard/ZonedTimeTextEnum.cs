/*TASK ZonedTimeTextEnum*/
//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: October 30, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public enum ZonedTimeTextEnum : byte
    {
        //                                                  //Do not include offset neither timezone
        //                                                  //Ex. 2018-04-21T15:20:01.123
        UTC,

        //                                                  //date-time include offset
        //                                                  //Ex. 2018-04-21T15:20:01.123-05:00[Central Sta... (Mexico)]
        TIME_ZONE,
    }

    //==================================================================================================================

}
/*END-TASK*/