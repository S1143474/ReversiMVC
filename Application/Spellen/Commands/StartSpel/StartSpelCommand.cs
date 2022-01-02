using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interfaces;
using Infrastructure.SocketHub;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Spellen.Commands.StartSpel
{
    public class StartSpelCommand : IRequest
    {
        public string Speler2Token { get; set; }
        public string SpelToken { get; set; }
    }

    public class StartSpelCommandHandle : RequestHandler<StartSpelCommand>
    {
        public IHubContext<ReversiHub, IReversiHub> _hub;
        private readonly ISpelService _spelService;
        
        public StartSpelCommandHandle(IHubContext<ReversiHub, IReversiHub> hub, ISpelService service)
        {
            _hub = hub;
            _spelService = service;
        }

        protected override async void Handle(StartSpelCommand request)
        {

            var result = await _spelService.JoinSpelReversi(request);

            var spel = await _spelService.RetrieveSpelOverToken(request.SpelToken);


            if (spel.speler2Token is not null)
            {
                await _hub.Clients.Users(spel.speler1Token, spel.speler2Token).StartGame(spel.speler1Token, spel.speler2Token);
            }
        }
    }
}
