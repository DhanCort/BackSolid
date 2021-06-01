/*TASK RP.XJDF*/
using Odyssey2Backend.JsonTemplates.Out;
using System;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa ().
//                                                          //DATE: Febraury 18, 2021. 

namespace Odyssey2Backend.JsonTypes
{
    //=================================================================================================================  
    public class Projson3ProcessJson3
    {
        //-------------------------------------------------------------------------------------------------------------
        //                                                  //INSTANCE VARIABLES.

        public int intPkProcessInWorkflow { get; set; }
        public String strName { get; set; }
        public CalperjsonCalculationPeriodJson[] arrcal { get; set; }
        public Resperjson2ResourcePeriodJson2[] arrresper { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //-------------------------------------------------------------------------------------------------------------
        public Projson3ProcessJson3(
            int intPkProcessInWorkflow_I,
            String strName_I,
            CalperjsonCalculationPeriodJson[] arrcalperjson_I,
            Resperjson2ResourcePeriodJson2[] arrresper_I
            )
        {
            this.intPkProcessInWorkflow = intPkProcessInWorkflow_I;
            this.strName = strName_I;
            this.arrcal = arrcalperjson_I;
            this.arrresper = arrresper_I;
        }

        //-------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
