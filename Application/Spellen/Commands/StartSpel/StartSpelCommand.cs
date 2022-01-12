using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Hubs;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Spellen.Commands.StartSpel
{
    public class StartSpelCommand : IRequest<SpelState>
    {
        public string Speler2Token { get; set; }
        public string SpelToken { get; set; }
    }

    public class StartSpelCommandHandle : IRequestHandler<StartSpelCommand, SpelState>
    {
        public IHubContext<ReversiHub, IReversiHub> _hub;
       /* public IReversiHub _hub;*/
        private readonly ISpelService _spelService;
        
        public StartSpelCommandHandle(IHubContext<ReversiHub, IReversiHub> hub/*IReversiHub hub*/, ISpelService service)
        {
            _hub = hub;
            _spelService = service;
        }

        public async Task<SpelState> Handle(StartSpelCommand request, CancellationToken cancellationToken)
        {
            var result = await _spelService.JoinSpelReversi(request);

            var spel = await _spelService.RetrieveSpelOverToken(request.SpelToken);


            if (spel.speler2Token is not null)
            {
                /* await _hub.SendStartGameAsync(spel.speler1Token, spel.speler2Token);
                 await _hub.SendRedirectAsync(spel.speler1Token, spel.speler2Token,
                     $"spel/Reversi/{spel.token}");*/

                await _hub.Clients.Users(spel.speler1Token, spel.speler2Token).StartGame();
                await _hub.Clients.Users(spel.speler1Token).Redirect($"spel/Reversi/{spel.token}");

                return SpelState.Playing;
            }

            return SpelState.Waiting;
        }
    }
}
