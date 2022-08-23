using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Spellen.Commands.ErrorSpel;
using Application.Spellen.Commands.FinishedSpel;
using Application.Spellen.Commands.OpponentsTurn;
using Application.Spellen.Commands.PlaceFiche;
using Application.Spellen.Commands.WrongPlacedFiche;
using Domain.Enums;
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

            if (result.ApiState == ApiState.Failed)
            {
                await Mediator.Send(new ErrorSpelCommand
                {
                    SpelerToken = UserId,
                    ErrorMessage = "Something went wrong with placing a fiche, please try again."
                });
                return;
                // TODO: Send failed message...
            }

            var baseDto = result.DTO as PlacedFichedDTO;

            if (baseDto.IsSpelFinished)
            {
                await Mediator.Send(new FinishedSpelCommand { SpelerToken = UserId });
                return;
            }

            if (!baseDto.IsPlaceExecuted)
            {
                await Mediator.Send(new WrongPlacedFicheCommand
                {
                    NotExecutedMessage = baseDto.NotExecutedMessage,
                    CurrentSpelerToken = baseDto.PlacedBySpelerToken
                });
                return;
            }

            await Mediator.Send(new OpponentsTurnCommand
            {
                CurrentSpelerToken = baseDto.PlacedBySpelerToken,
                FichesToTurnAround = baseDto.FichesToTurnAround
            });
        }

        public async Task StartGame()
        {
            // TODO: Add starttime to database.

            var spel = await _spelService.RetrieveSpelOverSpelerToken(UserId);

            await Clients.User(spel.Speler1Token.ToString()).Redirect($"spel/Reversi/{spel.Token}");
        }

        public async Task SendStartGameAsync(string speler1Token, string speler2Token)
        {
            await Clients.Users(speler1Token, speler2Token).StartGame();
        }

        public override async Task OnConnectedAsync()
        {
            /*Console.WriteLine("ReversiHub: On - Conneced");*/
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.
            if (!UserId.Equals(default))
            {
                UserHandler.ConnectedIds.Add(UserId);
                await Clients.All.OnPlayerOnline(UserHandler.ConnectedIds.Count);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            /*Console.WriteLine("ReversiHub: On - Conneced");*/
            // Add your own code here.
            // For example: in a chat application, mark the user as offline, 
            // delete the association between the current connection id and user name.
            if (!UserId.Equals(default))
            {
                UserHandler.ConnectedIds.Remove(UserId);
                await Clients.All.OnPlayerOnline(UserHandler.ConnectedIds.Count);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }

    public static class UserHandler
    {
        public static HashSet<Guid> ConnectedIds = new HashSet<Guid>();
    }
}
