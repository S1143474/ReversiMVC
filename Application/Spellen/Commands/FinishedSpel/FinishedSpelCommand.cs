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
        public string SpelerToken { get; set; }
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
            var result = await _spelService.RetrieveSpelOverSpelerToken(request.SpelerToken);

            await _hub.Clients.Users(result.speler1Token, result.speler2Token).OnFinish(new FinishedSpelDTO
            {
                
            });

            return true && result != null;
        }
    }
}
