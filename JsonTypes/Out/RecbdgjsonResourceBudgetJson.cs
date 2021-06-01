/*TASK RP.XJDF*/
using System;
using System.Collections.Generic;

//                                                          //AUTHOR: Towa (CLGA - Cesar garcia).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: July 3, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class RecbdgjsonResourceBudgetJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int? intnPkResource { get; set; }
        public String strName { get; set; }
        public int intPkEleetOrEleele { get; set; }
        public bool boolIsEleet { get; set; }
        public int? intnGroupResourceId { get; set; }
        public bool boolHasOption { get; set; }
        public double numQuantity { get; set; }
        public String strUnit { get; set; }
        public double numCost { get; set; }
        public bool? boolnIsAvailable { get; set; }
        public bool? boolnIsCalendar { get; set; }
        public String[] arrstrInfo { get; set; }
        public bool boolIsCompleted { get; set; }
        public bool boolAllowDecimal { get; set; }
        public bool boolIsPaper { get; set; }
        public String strLink { get; set; }
        public int intHours { get; set; }
        public int intMinutes { get; set; }
        public int intSeconds { get; set; }
        public bool? boolnIsDeviceOrMiscConsumable { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public RecbdgjsonResourceBudgetJson(
            int? intnPk_I,
            String strName_I,
            int intPkEleetOrEleele_I,
            bool boolIsEleet_I,
            int? intnGroupResourceId_I,
            bool boolHasOption_I,
            double numQuantity_I,
            String strUnit_I,
            double numCostByResource_I,
            bool? boolnIsAvailable_I,
            bool? boolnIsCalendar_I,
            String[] arrstrInfo_I,
            bool boolIsCompleted_I,
            bool boolAllowDecimal_I,
            bool boolIsPaper_I,
            String strLink_I,
            bool? boolnIsDeviceOrMiscConsumable_I
            )
        {
            this.intnPkResource = intnPk_I;
            this.strName = strName_I;
            this.intPkEleetOrEleele = intPkEleetOrEleele_I;
            this.boolIsEleet = boolIsEleet_I;
            this.intnGroupResourceId = intnGroupResourceId_I;
            this.boolHasOption = boolHasOption_I;
            this.numQuantity = numQuantity_I;
            this.strUnit = strUnit_I;
            this.numCost = numCostByResource_I;
            this.boolnIsAvailable = boolnIsAvailable_I;
            this.boolnIsCalendar = boolnIsCalendar_I;
            this.arrstrInfo = arrstrInfo_I;
            this.boolIsCompleted = boolIsCompleted_I;
            this.boolAllowDecimal = boolAllowDecimal_I;
            this.boolIsPaper = boolIsPaper_I;
            this.strLink = strLink_I;
            this.boolnIsDeviceOrMiscConsumable = boolnIsDeviceOrMiscConsumable_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
