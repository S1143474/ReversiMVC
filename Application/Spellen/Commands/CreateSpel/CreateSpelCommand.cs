using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.Spellen;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Application.Spellen.Commands.CreateSpel
{
    public class CreateSpelCommand : IRequest<bool>
    {
        public string PlayerToken { get; set; }
        public string Description { get; set; }
    }

    public class CreateSpelCommandHandler : IRequestHandler<CreateSpelCommand, bool>
    {
        public ISpelService _service { get; set; }
        
        public CreateSpelCommandHandler(ISpelService spelService)
        {
            _service = spelService;
        }

        public async Task<bool> Handle(CreateSpelCommand command, CancellationToken cancellationToken)
        {
            // TODO: API CancellationTOken from Nick Chapas YT.
            var result = await _service.CreateSpel(command);

            return result;
        }
    }
}
