using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Application.SocketHub
{
    public class ReversiHub : Hub<IReversiHub>
    {

    }
}
