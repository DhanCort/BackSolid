/*TASK RP.XJDF*/
using Odyssey2Backend.Utilities;
using System;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 08, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ProjsonProcessJson : IComparable
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strName { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strEndDate { get; set; }
        public String strEndTime { get; set; }
        public Perjson1PeriodJson1[] arrper { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ProjsonProcessJson(
            String strName_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            Perjson1PeriodJson1[] arrperjson1_I
            )
        {
            this.strName = strName_I;
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strEndDate = strEndDate_I;
            this.strEndTime = strEndTime_I;
            this.arrper = arrperjson1_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            ProjsonProcessJson projson = (ProjsonProcessJson)obj_I;

            //                                              //To easy code.
            Date dateStartThis = this.strStartDate.ParseToDate();
            Date dateEndThis = this.strEndDate.ParseToDate();
            Time timeStartThis = this.strStartTime.ParseToTime();
            Time timeEndThis = this.strEndTime.ParseToTime();
            ZonedTime ztimeStartThis = ZonedTimeTools.NewZonedTime(dateStartThis, timeStartThis);
            ZonedTime ztimeEndThis = ZonedTimeTools.NewZonedTime(dateEndThis, timeEndThis);

            Date dateStartToCompare = projson.strStartDate.ParseToDate();
            Date dateEndToCompare = projson.strEndDate.ParseToDate();
            Time timeStartToCompare = projson.strStartTime.ParseToTime();
            Time timeEndToCompare = projson.strEndTime.ParseToTime();
            ZonedTime ztimeStartToCompare = ZonedTimeTools.NewZonedTime(dateStartToCompare, timeStartToCompare);
            ZonedTime ztimeEndToCompare = ZonedTimeTools.NewZonedTime(dateEndToCompare, timeEndToCompare);

            int intToReturn = 0;
            /*CASE*/
            if (
                //                                          //This is after to dr.
                ztimeStartThis >= ztimeEndToCompare
                )
            {
                intToReturn = 1;
            }
            else if (
                //                                          //This is before to dr.
                ztimeEndThis <= ztimeStartToCompare
                )
            {
                intToReturn = -1;
            }
            else
            {
                //                                          //The dates are the same, nothing to do.
            }
            /*END-CASE*/

            return intToReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
