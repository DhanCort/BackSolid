/*TASK DaylightSavingTimeInfo*/
using System;

//                                                          //AUTHOR: Towa (GLG-Gerardo Lopez).
//                                                          //CO-AUTHOR: Towa (RGL-Rodrigo García).
//                                                          //DATE: November 5, 2018.

namespace TowaStandard
{
    //==================================================================================================================
    public struct DaylightSavingTimeInfo
    {
        //                                                  //Collet all useful info of the adjustment rule that
        //                                                  //      correspond to some DST period

        //--------------------------------------------------------------------------------------------------------------

        //                                                  //Year and Month for witch this info is collected.
        //                                                  //Start(month) <= Date(month).
        //                                                  //Start(month) < End(month)
        public readonly Date Date;

        //                                                  //DST adds this period to offset (Delta = 0, if there is no
        //                                                  //      DST defined)
        public readonly int DeltaDefinedMinutes;

        //                                                  //North hemisfere DST start before middle of the year
        //                                                  //South hemisfere DST start after midlle of the year

        //                                                  //Should include UtcOffset
        public readonly Date DateStartDaylightSavingTime;
        public readonly long UtcStartDaylightSavingTimeMinutes;

        //                                                  //Should include UtcOffset + Delta
        public readonly Date DateEndDaylightSavingTime;
        public readonly long UtcEndDaylightSavingTimeMinutes;

        //--------------------------------------------------------------------------------------------------------------
        public DaylightSavingTimeInfo(Date dateMonth_I, int intDeltaMinutes_I, Date dateStartDst_I,
            long longUtcStartDstInMinutes_I, Date dateEndDst_I, long longUtcEndDstInMinutes_I)
        {
            this.Date = dateMonth_I;
            this.DeltaDefinedMinutes = intDeltaMinutes_I;
            this.DateStartDaylightSavingTime = dateStartDst_I;
            this.UtcStartDaylightSavingTimeMinutes = longUtcStartDstInMinutes_I;
            this.DateEndDaylightSavingTime = dateEndDst_I;
            this.UtcEndDaylightSavingTimeMinutes = longUtcEndDstInMinutes_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        /*ACCESS-METHODS*/

        //--------------------------------------------------------------------------------------------------------------
        public String ToText()
        {
            return "<" + Test.ToLog(this.Date, "Date") + ", " + 
                   Test.ToLog(this.DeltaDefinedMinutes, "DeltaDefinedMinutes") + ", " +
                   Test.ToLog(this.DateStartDaylightSavingTime, "DateStartDaylightSavingTime") + ", " +
                   Test.ToLog(this.UtcStartDaylightSavingTimeMinutes, "UtcStartDaylightSavingTimeMinutes") + ", " +
                   Test.ToLog(this.DateEndDaylightSavingTime, "DateEndDaylightSavingTime") + ", " +
                   Test.ToLog(this.UtcEndDaylightSavingTimeMinutes, "UtcEndDaylightSavingTimeMinutes") + ">";
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
