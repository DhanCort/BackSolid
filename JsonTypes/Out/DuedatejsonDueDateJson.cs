/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 28, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class DuedatejsonDueDateJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strDescription { get; set; }
        public String strDate { get; set; }
        public String strTime { get; set; }
        public String strStartDate { get; set; }
        public String strStartTime { get; set; }
        public String strFirstName { get; set; }
        public String strLastName { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public DuedatejsonDueDateJson(
            String strDescription_I,
            String strDate_I,
            String strTime_I,
            String strStartDate_I,
            String strStartTime_I,
            String strFirstName_I,
            String strLastName_I
            )
        {
            this.strDescription = strDescription_I;
            this.strDate = strDate_I;
            this.strTime = strTime_I;
            this.strStartDate = strStartDate_I;
            this.strStartTime = strStartTime_I;
            this.strFirstName = strFirstName_I;
            this.strLastName = strLastName_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
