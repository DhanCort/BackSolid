/*TASK RP.SRP*/
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //DATE: June 06, 2021.

namespace Odyssey2Backend.Process.SRP
{
    public class SaveElementType
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void subSave(

            //                                              //Save elementType to DB.           
            String strPrintshopId_I,
            int intPkPrintshop_I,
            Odyssey2Context context_M
            )
        {
            EtentityElementTypeEntityDB etentity = new EtentityElementTypeEntityDB
            {
                strXJDFTypeId = EtElementTypeAbstract.strNotXJDF,
                strResOrPro = EtElementTypeAbstract.strProcess,
                intPrintshopPk = intPkPrintshop_I,
                strAddedBy = strPrintshopId_I,
                strCustomTypeId = ProtypProcessType.strProCustomType
            };

            context_M.ElementType.Add(etentity);
            context_M.SaveChanges();
        }
    }

    //==================================================================================================================
}
/*END-TASK*/

