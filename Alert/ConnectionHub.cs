/*TASK RP.ALERTS*/
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

//                                                          //AUTHOR: Towa (DPG - Daniel Pena).
//                                                          //CO-AUTHOR: Towa (AQG - Andrea Quiroz).
//                                                          //DATE: July 03, 2020.

namespace Odyssey2Backend.Alert
{
    //==================================================================================================================
    public class ConnectionHub : Hub
    {
        //--------------------------------------------------------------------------------------------------------------
        //                                                  //CONSTRUCTORS.

        //--------------------------------------------------------------------------------------------------------------
        public ConnectionHub()
        {
        }

        //--------------------------------------------------------------------------------------------------------------
        //                                                  //ACCESS METHODS.

        //--------------------------------------------------------------------------------------------------------------
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        //--------------------------------------------------------------------------------------------------------------
        public async Task CreateGroup(string userId, String strGroupName)
        {
            await Groups.AddToGroupAsync(userId, strGroupName);
        }

        //--------------------------------------------------------------------------------------------------------------
    }

    //==================================================================================================================
}
/*END-TASK*/
