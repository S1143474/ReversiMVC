using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Spellen.Commands.PlaceFiche
{
    public class PlaceFicheCommand : IRequest<PlaceFicheDTO>
    {
        public bool HasPassed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Token { get; set; }
        public string SpelerToken { get; set; }
    }

    public class PlaceFicheCommandHandle : IRequestHandler<PlaceFicheCommand, PlaceFicheDTO>
    {
        private readonly ISpelService _spelService;

        public PlaceFicheCommandHandle(ISpelService spelService)
        {
            _spelService = spelService;
        }

        public async Task<PlaceFicheDTO> Handle(PlaceFicheCommand request, CancellationToken cancellationToken)
        {
            PlaceFicheDTO result = await _spelService.PlaceFiche(request.HasPassed, request.X, request.Y, request.Token,
                request.SpelerToken);


            return result;
        }
    }
}
