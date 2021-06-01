/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: August 03, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class TaskjsonTaskJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intnPkTask { get; set; }
        public String strDescription { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strEndDate { get; set; }
        public String strEndTime { get; set; }
        public int intMinutesForNotification { get; set; }
        public bool boolIsNotifiedable { get; set; }
        public bool boolIsCompleted { get; set; }
        public int? intnCustomerId { get; set; }
        public String strCustomerName { get; set; }
        public String strCustomerLastName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public TaskjsonTaskJson(
            //                                              //Cannot be null but frontend prefers this variable name.
            int intnPkTask_I,
            String strDescription_I,
            String strStartDate_I,
            String strStartTime_I,
            String strEndDate_I,
            String strEndTime_I,
            int intMinutesForNotification_I,
            bool boolIsNotifiedable_I, 
            bool boolIsCompleted_I,
            int? intnCustomerId_I,
            String strCustomerName_I,
            String strCustomerLastName_I
            )
        {
            this.intnPkTask = intnPkTask_I;
            this.strDescription = strDescription_I;
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strEndDate = strEndDate_I;
            this.strEndTime = strEndTime_I;
            this.intMinutesForNotification = intMinutesForNotification_I;
            this.boolIsNotifiedable = boolIsNotifiedable_I;
            this.boolIsCompleted = boolIsCompleted_I;
            this.intnCustomerId = intnCustomerId_I;
            this.strCustomerName = strCustomerName_I;
            this.strCustomerLastName = strCustomerLastName_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
