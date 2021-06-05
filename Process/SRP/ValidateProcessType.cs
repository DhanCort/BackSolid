/*TASK RP.SRP*/
using Odyssey2Backend.App;
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.PrintShop;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //DATE: June 04, 2021.

namespace Odyssey2Backend.Process.SRP
{
    public class ValidateProcessType
    {
        //------------------------------------------------------------------------------------------------------------- 
        public static bool boolIsValidType(
            //                                              //Verify if the type for the process is not null 
            //                                              //      and verify if the type is already a clone for the 
            //                                              //      printshop, if it is not then create the clone and
            //                                              //      add it to the printshop and update the type with the
            //                                              //      clone.

            //                                              //Printshop.
            PsPrintShop ps_I,
            Odyssey2Context context_M,
            //                                              //Type.
            ref ProtypProcessType protyp_M
            )
        {
            bool boolValidType = false;

            if (
                protyp_M != null
                )
            {
                //                                          //To easy code.
                ProtypProcessType protyp = protyp_M;
                //                                          //Search for a type with the same data.
                EtentityElementTypeEntityDB etentityResourceType =
                    ElementTypeDBQueries.etentityGetType(ps_I, context_M, protyp);

                int intStatus = -1;
                int intTypePk = FindValidProcessType.intPkValidProcessType(ps_I, etentityResourceType, protyp,
                    context_M, ref intStatus);

                if (
                    intStatus == 0
                    )
                {
                    boolValidType = true;
                    protyp_M = (ProtypProcessType)EtElementTypeAbstract.etFromDB(context_M, intTypePk);
                }
            }
            return boolValidType;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
