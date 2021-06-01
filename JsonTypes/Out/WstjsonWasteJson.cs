/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (JLBD - José Basurto).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: September 14, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class WstjsonWasteJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.
        public double numInitial { get; set; }
        public double numWaste { get; set; }
        public double numTotal { get; set; }
        public double? numnWasteToPropagate { get; set; }
        public String strUnitPropagate { get; set; }
        public String strTarget { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public WstjsonWasteJson(
            double numInitial_I,
            double numWaste_I,
            double numTotal_I,
            double? numnWasteToPropagate_I,
            String strUnitPropagate_I,
            String strTarget_I
            )
        {
            this.numInitial = numInitial_I;
            this.numWaste = numWaste_I;
            this.numTotal = numTotal_I;
            this.numnWasteToPropagate = numnWasteToPropagate_I;
            this.strUnitPropagate = strUnitPropagate_I;
            this.strTarget = strTarget_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
