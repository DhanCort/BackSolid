/*TASK RP.JSON*/
using System;

//                                                          //AUTHOR: Towa (CCC - Cesar Cigarroa).
//                                                          //CO-AUTHOR: Towa (LGF -Liliana Gutierrez).
//                                                          //DATE: November 13, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class JobmovsjsonJobMovementsJson
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public AccmovjsonAccountMovementJson[] arrAccountMovements { get; set; }
        public double numTotal { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public JobmovsjsonJobMovementsJson(
            AccmovjsonAccountMovementJson[] arrAccountMovements_I,
            double numTotal_I
            )
        {
            this.arrAccountMovements = arrAccountMovements_I;
            this.numTotal = numTotal_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
