using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Hubs
{
    public class ReversiHub : Hub, IReversiHub
    {

        /*public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task GameStarted(string guid)
        {
            await Clients.User(guid).SendAsync("GameStarted", guid);
        }*/

        public async Task RedirectPlayer()
        {

        }
    }
}
