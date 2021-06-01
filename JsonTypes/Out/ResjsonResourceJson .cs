/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: November 28, 2019. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class ResjsonResourceJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPk { get; set; }
        public String strTypeId{ get; set; }
        public String strUnit { get; set; }
        public double? numQuantity { get; set; }
        public double? numCost { get; set; }
        public double? numnMin { get; set; }
        public double? numnBlock { get; set; }
        public bool? boolnIsCalendar { get; set; }
        public bool? boolnIsAvailable { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public ResjsonResourceJson(
            int intPk_I,
            String strTypeId_I,
            String strUnit_I,
            double? numQuantity_I,
            double? numCost_I,
            double? numnMin_I,
            double? numnBlock_I,
            bool? boolnIsCalendar_I,
            bool? boolnIsAvailable_I
            )
        {
            this.intPk = intPk_I;
            this.strTypeId = strTypeId_I;
            this.strUnit = strUnit_I;
            this.numQuantity = numQuantity_I;
            this.numCost = numCost_I;
            this.numnMin = numnMin_I;
            this.numnBlock = numnBlock_I;
            this.boolnIsCalendar = boolnIsCalendar_I;
            this.boolnIsAvailable = boolnIsAvailable_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
