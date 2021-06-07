/*TASK RP.SRP*/
using Odyssey2Backend.DB_Odyssey2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //DATE: June 06, 2021.

namespace Odyssey2Backend.Process.SRP
{
    public class ValidateProcessName
    {
        //-------------------------------------------------------------------------------------------------------------
        public static bool isValid(

            //                                              //Name of the process. There can not be more than one 
            //                                              //      process with the same name in the same printshop.
            String strName_I,
            int intPkPrintshop_I,
            ref String strUserMessage_IO
            )
        {
            bool boolValidProcess = false;

            strUserMessage_IO = "Name cannot be empty";
            if (
                (strName_I != null) &&
                (strName_I != "")
                )
            {
                Odyssey2Context context = new Odyssey2Context();
                List<EleentityElementEntityDB> seteleentityProcess = ElementDBQueries.darreleProcessesByName(strName_I,
                    intPkPrintshop_I, context);

                strUserMessage_IO = "Name already exists.";
                if (
                    //                                      //There is not another process with the same name.
                    seteleentityProcess.Count == 0
                    )
                {
                    boolValidProcess = true;
                }
            }
            return boolValidProcess;
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
