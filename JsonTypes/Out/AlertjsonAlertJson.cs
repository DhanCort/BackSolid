/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: July 08, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class AlertjsonAlertJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public String strAlertType { get; set; }
        public String strAlertDescription { get; set; }
        public bool? boolnJob { get; set; }
        public bool boolInEstimating { get; set; }
        public int? intnJobId { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public AlertjsonAlertJson(
            String strAlertTyper_I,
            String strAlertDescription_I,
            bool? boolnJob_I,
            bool boolInEstimating_I,
            int? intnJobId_I
            )
        {
            this.strAlertType = strAlertTyper_I;
            this.strAlertDescription = strAlertDescription_I;
            this.boolnJob = boolnJob_I;
            this.boolInEstimating = boolInEstimating_I;
            this.intnJobId = intnJobId_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
