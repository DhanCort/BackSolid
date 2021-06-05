/*TASK RP.SRP*/
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
    public class ElementTypeDBQueries
    {
        //------------------------------------------------------------------------------------------------------------- 
        public static EtentityElementTypeEntityDB etentityGetType(

            PsPrintShop ps_I,
            Odyssey2Context context_M,
            //                                              //Type.
            ProtypProcessType protyp_I
            )
        {
            //                                          //Search for a type with the same data.
            EtentityElementTypeEntityDB etentityResourceType = context_M.ElementType.FirstOrDefault(
                            et => et.strAddedBy == protyp_I.strAddedBy &&
                            et.strCustomTypeId == protyp_I.strCustomTypeId &&
                            et.intPrintshopPk == ps_I.intPk);

            return etentityResourceType;
        }
    }
    //==================================================================================================================
}
/*END-TASK*/
