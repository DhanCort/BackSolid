/*TASK RP.JSON*/
using System;
using TowaStandard;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: May 28, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //==================================================================================================================  
    public class Porjson2PeriodOrRuleJson2 : IComparable
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public ZonedTime ztimeStart { get; set; }
        public ZonedTime ztimeEnd { get; set; }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public Porjson2PeriodOrRuleJson2(
            ZonedTime ztimeStart_I,
            ZonedTime ztimeEnd_I
            )
        {
            this.ztimeStart = ztimeStart_I;
            this.ztimeEnd = ztimeEnd_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            Porjson2PeriodOrRuleJson2 porjson2 = (Porjson2PeriodOrRuleJson2)obj_I;

            int intToReturn = 0;
            /*CASE*/
            if (
                //                                          //This is after to dr.
                this.ztimeStart > porjson2.ztimeStart
                )
            {
                intToReturn = 1;
            }
            else if (
                //                                          //This is before to dr.
                this.ztimeStart < porjson2.ztimeStart
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

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
