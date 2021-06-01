/*TASK RP.XJDF*/
using Odyssey2Backend.Utilities;
using System;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 08, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class Perjson1PeriodJson1 : IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strName { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strEndDate { get; set; }
        public String strEndTime { get; set; }
        public bool boolIsByProcess { get; set; }
        public bool boolIsTheFirstPeriod { get; set; }
        public bool boolIsTheLastPeriod { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public Perjson1PeriodJson1(
            String strName_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            bool boolIsByProcess_I,
            bool boolIsTheFirstPeriod_I,
            bool boolIsTheLastPeriod_I
            )
        {
            this.strName = strName_I;
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strEndDate = strEndDate_I;
            this.strEndTime = strEndTime_I;
            this.boolIsByProcess = boolIsByProcess_I;
            this.boolIsTheFirstPeriod = boolIsTheFirstPeriod_I;
            this.boolIsTheLastPeriod = boolIsTheLastPeriod_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            Perjson1PeriodJson1 perjson1 = (Perjson1PeriodJson1)obj_I;

            //                                              //To easy code.
            Date dateStartThis = this.strStartDate.ParseToDate();
            Date dateEndThis = this.strEndDate.ParseToDate();
            Time timeStartThis = this.strStartTime.ParseToTime();
            Time timeEndThis = this.strEndTime.ParseToTime();
            ZonedTime ztimeStartThis = ZonedTimeTools.NewZonedTime(dateStartThis, timeStartThis);
            ZonedTime ztimeEndThis = ZonedTimeTools.NewZonedTime(dateEndThis, timeEndThis);

            Date dateStartToCompare = perjson1.strStartDate.ParseToDate();
            Date dateEndToCompare = perjson1.strEndDate.ParseToDate();
            Time timeStartToCompare = perjson1.strStartTime.ParseToTime();
            Time timeEndToCompare = perjson1.strEndTime.ParseToTime();
            ZonedTime ztimeStartToCompare = ZonedTimeTools.NewZonedTime(dateStartToCompare, timeStartToCompare);
            ZonedTime ztimeEndToCompare = ZonedTimeTools.NewZonedTime(dateEndToCompare, timeEndToCompare);

            int intToReturn = 0;
            /*CASE*/
            if (
                //                                          //This is after to dr.
                (ztimeEndThis - ztimeStartThis) > (ztimeEndToCompare - ztimeStartToCompare)
                )
            {
                intToReturn = -1;
            }
            else if (
                //                                          //This is before to dr.
                (ztimeEndThis - ztimeStartThis) < (ztimeEndToCompare - ztimeStartToCompare)
                )
            {
                intToReturn = 1;
            }
            else
            {
                //                                          //The dates are the same, nothing to do.
            }
            /*END-CASE*/

            return intToReturn;
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
