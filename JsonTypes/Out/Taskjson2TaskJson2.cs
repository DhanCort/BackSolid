/*TASK RP.XJDF*/
using System;
using TowaStandard;

//                                                          //AUTHOR: Towa (JLBD - José Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: September 01, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Taskjson2TaskJson2 : IComparable
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkTask { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strEndDate { get; set; }
        public String strEndTime { get; set; }
        public String strDescription { get; set; }
        public bool boolIsCompleted { get; set; }
        public String strCustomerName { get; set; }
        public String strCustomerLastName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Taskjson2TaskJson2(
            int intPkTask_I,
            String strDescription_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            bool boolIsCompleted_I,
            String strCustomerName_I,
            String strCustomerLastName_I
            )
        {
            this.intPkTask = intPkTask_I;
            this.strDescription = strDescription_I;
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strEndDate = strEndDate_I;
            this.strEndTime = strEndTime_I;
            this.boolIsCompleted = boolIsCompleted_I;
            this.strCustomerName = strCustomerName_I;
            this.strCustomerLastName = strCustomerLastName_I;
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public int CompareTo(
            Object obj_I
            )
        {
            Taskjson2TaskJson2 taskjson2 = (Taskjson2TaskJson2)obj_I;

            Time time1 = this.strEndTime.ParseToTime();
            Time time2 = taskjson2.strEndTime.ParseToTime();

            return time1.CompareTo(time2);
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
