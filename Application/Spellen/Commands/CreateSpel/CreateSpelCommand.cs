using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.Requests;
using Application.Hubs;
using Domain.Entities;
using Domain.Events.Spellen;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;

namespace Application.Spellen.Commands.CreateSpel
{
    public class CreateSpelCommand : IRequest<SpelDto>
    {
        public Guid PlayerToken { get; set; }
        public string Description { get; set; }
    }

    public class CreateSpelCommandHandler : IRequestHandler<CreateSpelCommand, SpelDto>
    {
        public ISpelService _service { get; set; }
        private readonly IHubContext<ReversiHub, IReversiHub> _hub;

        public CreateSpelCommandHandler(ISpelService spelService, IHubContext<ReversiHub, IReversiHub> hub)
        {
            _service = spelService;
            _hub = hub;
        }

        public async Task<SpelDto> Handle(CreateSpelCommand command, CancellationToken cancellationToken)
        {
            // TODO: API CancellationTOken from Nick Chapas YT.
            SpelDto result = null;
            try
            {
                result = await _service.CreateSpel(new SpelCreateDto { Description = command.Description, Speler1Token = command.PlayerToken});
                
                // TODO: Check if there is a better solutions this could probably lead to a lot of reloading.
                await _hub.Clients.AllExcept(command.PlayerToken.ToString()).OnCreateGame();
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
    }
}
