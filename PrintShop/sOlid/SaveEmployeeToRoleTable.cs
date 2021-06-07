/*TASK RP.SRP*/
using Odyssey2Backend.DB_Odyssey2;
using Odyssey2Backend.XJDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//                                                          //AUTHOR: Towa (DTC - Daniel Texon).
//                                                          //CO-AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //DATE: June 07, 2021.

namespace Odyssey2Backend.Process.SRP
{
    public class SaveEmployeeToRoleTable
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void subSave(

            //                                              //Save process to DB.
            int intContactId_I,
            int intPkPrintshop_I,
            bool? boolnIsSupervisor_I,
            bool? boolnIsAccountant_I,
            Odyssey2Context context_M
            )
        {
            //                                  //Add employee as supervisor or accountant.
            RolentityRoleEntityDB roleentity = new RolentityRoleEntityDB
            {
                intContactId = intContactId_I,
                intPkPrintshop = intPkPrintshop_I,
                boolSupervisor = (boolnIsSupervisor_I == null ? false : (bool)boolnIsSupervisor_I),
                boolAccountant = (boolnIsAccountant_I == null ? false : (bool)boolnIsAccountant_I)
            };

            context_M.Role.Add(roleentity);
            context_M.SaveChanges();
        }
    }

    //==================================================================================================================
}
/*END-TASK*/
