using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Hubs;
using Application.Spellen.Commands.OpponentsTurn;
using Application.Spellen.Commands.PlaceFiche;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Spellen.Commands.OpponentsTurn
{ 
    public class OpponentsTurnCommand : IRequest<bool>
    {
        public string CurrentSpelerToken { get; set; }
        public List<FicheCoordDTO> FichesToTurnAround { get; set; }
    }

    public class OpponentsTurnCommandHandle : IRequestHandler<OpponentsTurnCommand, bool>
    {
        private readonly ISpelService  _spelService;
        private readonly IHubContext<ReversiHub, IReversiHub> _hub;

        public OpponentsTurnCommandHandle(IHubContext<ReversiHub, IReversiHub> hub, ISpelService spelService)
        {
            _hub = hub;
            _spelService = spelService;
        } 

        public async Task<bool> Handle(OpponentsTurnCommand request, CancellationToken cancellationToken)
        {
            var spel = await _spelService.RetrieveSpelOverSpelerToken(request.CurrentSpelerToken);

            string opponentSpelerToken = (spel.speler1Token == request.CurrentSpelerToken)
                ? spel.speler2Token
                : spel.speler1Token;

            await _hub.Clients.Users(request.CurrentSpelerToken).OnDisableMove(request.FichesToTurnAround, spel.aandeBeurt);
            await _hub.Clients.Users(opponentSpelerToken).OnMove(request.FichesToTurnAround, spel.aandeBeurt);
            return true;
        }
    }
}
