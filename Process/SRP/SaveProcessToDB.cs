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
    public class SaveProcessToDB
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void subSave(

            //                                              //Save process to DB.
            String strName_I,
            Odyssey2Context context_M,
            int intPkElementType_I,
            out int intPkProcessAdded_O
            )
        {
            //                                      //Add the process.
            EleentityElementEntityDB elentity = new EleentityElementEntityDB
            {
                strElementName = strName_I,
                intPkElementType = intPkElementType_I,
            };
            context_M.Element.Add(elentity);
            context_M.SaveChanges();

            intPkProcessAdded_O = elentity.intPk;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
