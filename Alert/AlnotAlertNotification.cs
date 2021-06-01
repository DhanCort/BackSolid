/*TASK RP.ALERTS*/
using System;
using Microsoft.AspNetCore.SignalR;

//                                                          //AUTHOR: Towa (IUGS - Ivan Guzman).
//                                                          //CO-AUTHOR: Towa (LGF - Liliana Gutierrez).
//                                                          //DATE: August 18, 2020.

namespace Odyssey2Backend.Alert
{
    //==================================================================================================================
    public class AlnotAlertNotification
    {
        //--------------------------------------------------------------------------------------------------------------
        public static void subSendToAll(
            String strUserId_I,
            int intPrintshopId_I,
            String strMessage_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            iHubContext_I.Clients.GroupExcept(intPrintshopId_I + "", strUserId_I).SendAsync("AlertForAll", 
                strMessage_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subReduceToAll(
            int intPrintshopId_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            iHubContext_I.Clients.Group(intPrintshopId_I + "").SendAsync("ReduceForAll");
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSendToAFew(
            //                                              //Send the notification for certains contacts.

            String strPrintshopId_I,
            int[] arrintContactId_I,
            String strMessage_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            String strCxContactId = "";
            foreach (int intContactId in arrintContactId_I)
            {
                strCxContactId = strCxContactId + "|" + intContactId;
            }

            iHubContext_I.Clients.Group(strPrintshopId_I).SendAsync("AlertForAFew", strCxContactId, strMessage_I);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subReduceToAFew(
            //                                              //Reduce the notification for certains contacts.

            String strPrintshopId_I, 
            int[] arrintContactId_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            String strCxContactId = "";
            foreach (int intContactId in arrintContactId_I)
            {
                strCxContactId = strCxContactId + "|" + intContactId;
            }

            iHubContext_I.Clients.Group(strPrintshopId_I).SendAsync("ReduceForAFew", strCxContactId);
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subSendTaskToAFew(
            int[] arrintContactId_I,
            String strMessage_I,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            String strCxContactId = "";
            foreach (int intContactId in arrintContactId_I)
            {
                strCxContactId = strCxContactId + "|" + intContactId;
                iHubContext_I.Clients.All.SendAsync("AlertForAFew", strCxContactId, strMessage_I);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        public static void subReduceToOne(
            int intContactId,
            IHubContext<ConnectionHub> iHubContext_I
            )
        {
            String strCxContactId = "";
            strCxContactId = strCxContactId + "|" + intContactId;

            iHubContext_I.Clients.All.SendAsync("ReduceForAFew", strCxContactId);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
