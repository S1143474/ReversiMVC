using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.Requests;
using Domain.Entities;
using Domain.Events.Spellen;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
        
        public CreateSpelCommandHandler(ISpelService spelService)
        {
            _service = spelService;
        }

        public async Task<SpelDto> Handle(CreateSpelCommand command, CancellationToken cancellationToken)
        {
            // TODO: API CancellationTOken from Nick Chapas YT.
            SpelDto result = null;
            try
            {
                result = await _service.CreateSpel(new SpelCreateDto { Description = command.Description, Speler1Token = command.PlayerToken});
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
    }
}
