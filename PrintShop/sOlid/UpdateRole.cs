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
    public class UpdateRole
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void subUpdate(

            //                                              //Update employee role.
            bool? boolnIsSupervisor_I,
            bool? boolnIsAccountant_I,
            RolentityRoleEntityDB roleentity_M,
            Odyssey2Context context_M
            )
        {
            if (
                (boolnIsAccountant_I == false &&
                roleentity_M.boolSupervisor == false) ||
                (boolnIsSupervisor_I == false &&
                roleentity_M.boolAccountant == false)
                )
            {
                //                              //Remove employee.
                context_M.Role.Remove(roleentity_M);
            }
            else
            {
                if (
                //                                  //Set or unset Accountant.
                boolnIsSupervisor_I == null
                )
                {
                    roleentity_M.boolAccountant = (bool)boolnIsAccountant_I;
                }
                else
                //                                  //Set or unset supervisor.
                {
                    roleentity_M.boolSupervisor = (bool)boolnIsSupervisor_I;
                }
            }
            context_M.SaveChanges();
        }
    }

    //==================================================================================================================
}
/*END-TASK*/

