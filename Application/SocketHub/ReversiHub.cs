using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.SocketHub
{
    public class ReversiHub : Hub<IReversiHub>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISpelService _spelService;
        
        public ReversiHub([FromServices] IHttpContextAccessor httpContextAccessor, ISpelService service)
        {
            _httpContextAccessor = httpContextAccessor;
            _spelService = service;
        }

        public Task OnMove(object move)
        {
            throw new NotImplementedException();
        }

        public async Task StartGame()
        {
            // TODO: Add starttime to database.

            var userId = _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            var spel = await _spelService.RetrieveSpelOverSpelerToken(userId);

            await Clients.User(spel.speler1Token).Redirect($"spel/Reversi/{spel.token}");
        }


        public override Task OnConnectedAsync()
        {
            /*Console.WriteLine("ReversiHub: On - Conneced");*/
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            /*Console.WriteLine("ReversiHub: On - Conneced");*/
            // Add your own code here.
            // For example: in a chat application, mark the user as offline, 
            // delete the association between the current connection id and user name.
            return base.OnDisconnectedAsync(exception);
        }
    }
}
