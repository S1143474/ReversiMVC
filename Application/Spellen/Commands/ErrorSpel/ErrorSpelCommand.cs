using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Spellen.Commands.ErrorSpel
{
    public class ErrorSpelCommand : IRequest<bool>
    {
        public string SpelerToken { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ErrorSpelCommandHandle : IRequestHandler<ErrorSpelCommand, bool>
    {
        private readonly IHubContext<ReversiHub, IReversiHub> _hub;

        public ErrorSpelCommandHandle(IHubContext<ReversiHub, IReversiHub> hub)
        {
            _hub = hub;
        }

        public async Task<bool> Handle(ErrorSpelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _hub.Clients.Users(request.SpelerToken).OnError(request.ErrorMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
