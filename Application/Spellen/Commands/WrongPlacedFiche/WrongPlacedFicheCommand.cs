using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Spellen.Commands.WrongPlacedFiche
{
    public class WrongPlacedFicheCommand : IRequest<bool>
    {
        public Guid CurrentSpelerToken { get; set; }
        public string NotExecutedMessage { get; set; }
    }

    public class WrongPlacedFicheCommandHandle : IRequestHandler<WrongPlacedFicheCommand, bool>
    {
        private readonly IHubContext<ReversiHub, IReversiHub> _hub;

        public WrongPlacedFicheCommandHandle(IHubContext<ReversiHub, IReversiHub> hub)
        {
            _hub = hub;
        }

        public async Task<bool> Handle(WrongPlacedFicheCommand request, CancellationToken cancellationToken)
        {
            await _hub.Clients.Users(request.CurrentSpelerToken.ToString()).OnWrongMove(request.NotExecutedMessage);

            // TODO: Make this pretty
            return true;
        }
    }
}
