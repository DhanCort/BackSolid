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
//                                                          //DATE: June 05, 2021.

namespace Odyssey2Backend.Process.SRP
{
    public class FindValidProcessType
    {
        //------------------------------------------------------------------------------------------------------------- 
        public static int intPkValidProcessType(
            //                                              //Verify if the type for the process is not null 
            //                                              //      and verify if the type is already a clone for the 
            //                                              //      printshop, if it is not then create the clone and
            //                                              //      add it to the printshop and update the type with the
            //                                              //      clone.

            //                                              //Printshop.
            PsPrintShop ps_I,
            EtentityElementTypeEntityDB etentityResourceType_I,
            ProtypProcessType protyp_I,            
            Odyssey2Context context_M,
            ref int intStatus_IO
            )
        {
            int intTypePk = protyp_I.intPk;
            /*CASE*/
            if (
                //                                      //The protyp is not a clone.
                (protyp_I.intPkPrintshop == null) &&
                //                                      //There is not a clone.
                etentityResourceType_I == null
                )
            {
                Odyssey2.subAddTypeToPrintshop(protyp_I.intPk, ps_I, context_M, out intStatus_IO, out intTypePk);
            }
            else if (
                //                                      //The printshop has the clone but the object is about the 
                //                                      //      generic type.
                //                                      //The protyp is not a clone.
                (protyp_I.intPkPrintshop == null) &&
                //                                      //There is a clone.
                (etentityResourceType_I != null)
                )
            {
                intStatus_IO = 0;
                intTypePk = etentityResourceType_I.intPk;
            }
            else if (
                //                                      //The printshop has the clone and the object is that clone.
                (protyp_I.intPkPrintshop != null) &&
                (protyp_I.intPkPrintshop == ps_I.intPk)
                )
            {
                intStatus_IO = 0;
            }
            /*END-CASE*/

            return intTypePk;
        }
    }
    //==================================================================================================================
}
/*END-TASK*/
