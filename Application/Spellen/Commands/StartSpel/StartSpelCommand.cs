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
        public Guid Speler2Token { get; set; }
        public Guid SpelToken { get; set; }
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
            var spel = await _spelService.RetrieveSpelOverToken(request.SpelToken);
            
            if (spel.Speler1Token.Equals(request.Speler2Token))
                return SpelState.Waiting;
            
            var isSpelJoined = await _spelService.JoinSpelReversi(request);

            if (isSpelJoined == false)
                return SpelState.Error;


            if (spel.Speler2Token is not null)
            {
                await _hub.Clients.Users(spel.Speler1Token.ToString()).Redirect($"spel/Reversi/{spel.Token}");

                return SpelState.Playing;
            }

            return SpelState.Waiting;
        }
    }
}
