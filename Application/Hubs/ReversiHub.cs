using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Spellen.Commands.FinishedSpel;
using Application.Spellen.Commands.OpponentsTurn;
using Application.Spellen.Commands.PlaceFiche;
using Application.Spellen.Commands.WrongPlacedFiche;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Hubs
{
    public class ReversiHub : BaseHub
    {
        private readonly ISpelService _spelService;
        
        public ReversiHub([FromServices] IHttpContextAccessor httpContextAccessor, ISpelService service) : base(httpContextAccessor)
        {
            _spelService = service;
        }

        public async Task OnMove(JsonElement dto)
        {
            // TODO: Make function from deserialization.
            var raw = dto.GetRawText();
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var data = JsonSerializer.Deserialize<PlaceFicheDTO>(raw, options);
          
            var result = await Mediator.Send(new PlaceFicheCommand
            {
                //TODO: fix passes.
                HasPassed = data.HasPassed,
                X = data.X,
                Y = data.Y,
                SpelerToken = UserId
            });

            if (result.IsSpelFinished)
            {
                await Mediator.Send(new FinishedSpelCommand { SpelerToken = UserId });
                return;
            }

            if (!result.IsPlaceExecuted)
            {
                await Mediator.Send(new WrongPlacedFicheCommand
                {
                    CurrentSpelerToken = result.PlacedBySpelerToken
                });
                return;
            }

            await Mediator.Send(new OpponentsTurnCommand
            {
                CurrentSpelerToken = result.PlacedBySpelerToken,
                FichesToTurnAround = result.FichesToTurnAround
            });
        }

        public async Task StartGame()
        {
            // TODO: Add starttime to database.

            var spel = await _spelService.RetrieveSpelOverSpelerToken(UserId);

            await Clients.User(spel.speler1Token).Redirect($"spel/Reversi/{spel.token}");
        }

        public async Task SendStartGameAsync(string speler1Token, string speler2Token)
        {
            await Clients.Users(speler1Token, speler2Token).StartGame();
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
