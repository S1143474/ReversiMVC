using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Hubs;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Application.Spellen.Commands.PlaceFiche
{
    public class PlaceFicheCommand : IRequest<ARO>
    {
        public bool HasPassed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Token { get; set; }
        public string SpelerToken { get; set; }
    }

    public class PlaceFicheCommandHandle : IRequestHandler<PlaceFicheCommand, ARO>
    {
        private readonly ISpelService _spelService;

        public PlaceFicheCommandHandle(ISpelService spelService)
        {
            _spelService = spelService;
        }

        public async Task<ARO> Handle(PlaceFicheCommand request, CancellationToken cancellationToken)
        {
            var placedFiche = await _spelService.PlaceFiche(request.HasPassed, request.X, request.Y, request.Token,
                request.SpelerToken);

            var response = new ARO
            {
                DTO = placedFiche
            };

            if (placedFiche == null)
            {
                response.ApiState = ApiState.Failed;
                return response;
            }

            placedFiche.PlacedBySpelerToken = request.SpelerToken;
            response.ApiState = ApiState.Success;

            return response;
        }
    }
}
