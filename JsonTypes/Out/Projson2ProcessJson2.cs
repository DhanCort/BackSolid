/*TASK RP.XJDF*/
using System;

//                                                          //AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: June 08, 2020. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Projson2ProcessJson2
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkProcessInWorkflow { get; set; }
        public String strName { get; set; }
        public CalperjsonCalculationPeriodJson[] arrcal { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Projson2ProcessJson2(
            int intPkProcessInWorkflow_I,
            String strName_I,
            CalperjsonCalculationPeriodJson[] arrcalperjson_I
            )
        {
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.strName = strName_I;
            this.arrcal = arrcalperjson_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
