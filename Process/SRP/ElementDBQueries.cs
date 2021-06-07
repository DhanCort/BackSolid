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
    public class ElementDBQueries
    {
        //------------------------------------------------------------------------------------------------------------- 
        public static List<EleentityElementEntityDB> darreleProcessesByName(

            String strName_I,
            int intPkPrintshop,
            Odyssey2Context context_M
            )
        {
            //                                          //Search a process by name.
            List<EleentityElementEntityDB> seteleentityProcess =
                    (from eleentity in context_M.Element
                     join eleetentity in context_M.ElementType
                     on eleentity.intPkElementType equals eleetentity.intPk
                     where eleetentity.intPrintshopPk == intPkPrintshop && eleentity.strElementName == strName_I &&
                     eleetentity.strResOrPro == EtElementTypeAbstract.strProcess
                     select eleentity).ToList();

            return seteleentityProcess;
        }
    }
    //==================================================================================================================
}
/*END-TASK*/
