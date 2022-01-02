using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.SocketHub
{
    public class ReversiHub : Hub<IReversiHub>
    {
        public async Task StartGame(string speler1Token, string speler2Token)
        {
            await Clients.Users(speler1Token, speler2Token).StartGame("bie0", "daba");
        }
    }
}
