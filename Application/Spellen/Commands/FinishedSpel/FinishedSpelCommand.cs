using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Spellen.Commands.FinishedSpel
{
    public class FinishedSpelCommand : IRequest<bool>
    {
        public Guid SpelerToken { get; set; }
    }

    public class FinishedSpelCommandHandle : IRequestHandler<FinishedSpelCommand, bool>
    {
        private readonly ISpelService _spelService;
        private readonly IHubContext<ReversiHub, IReversiHub> _hub;

        public FinishedSpelCommandHandle(ISpelService service, IHubContext<ReversiHub, IReversiHub> hub)
        {
            _spelService = service;
            _hub = hub;
        }

        public async Task<bool> Handle(FinishedSpelCommand request, CancellationToken cancellationToken)
        {
            var result = await _spelService.RetrieveFinishedSpelOverSpelerToken(request.SpelerToken);

                await _hub.Clients.Users(result.Speler1Token.ToString(), result.Speler2Token.ToString()).Redirect($"spel/Result");
            /*await _hub.Clients.Users(result.speler1Token, result.speler2Token).OnFinish(new FinishedSpelDTO
            {
                WinnerName = finishedResults.WinnerToken,
                LoserName = finishedResults.LoserToken,
                IsDraw = finishedResults.IsDraw,
                AmountOfGeenFichesTurned = finishedResults.AmountOfGeenFichesTurned,
                AmountOfWitFichesTurned = finishedResults.AmountOfWitFichesTurned,
                AmountOfZwartFichesTurned = finishedResults.AmountOfZwartFichesTurned
            });*/
            return true;
        }
    }
}
