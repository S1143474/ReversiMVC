using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.Requests;
using Application.Spellen.Commands.FinishedSpel;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Spellen.Queries.GetSpellen
{
    public class GetSpellenHistoryQuery : IRequest<List<FinishedSpelResultsDTO>>
    {
        public Guid SpelerToken { get; set; }
    }

    public class GetSpellenHistoryQueryHandle : IRequestHandler<GetSpellenHistoryQuery, List<FinishedSpelResultsDTO>>
    {
        private readonly ISpelService _spelService;
        private readonly UserManager<IdentityUser> _userManager;

        public GetSpellenHistoryQueryHandle(ISpelService spelService, UserManager<IdentityUser> userManager)
        {
            _spelService = spelService;
            _userManager = userManager;
        }

        public async Task<List<FinishedSpelResultsDTO>> Handle(GetSpellenHistoryQuery request, CancellationToken cancellationToken)
        {
            var spellenDto = await _spelService.GetSpellenFinishedBySpelerTokenDescAsync(request.SpelerToken);

            foreach(var spel in spellenDto)
            {
                var winner = await _userManager.FindByIdAsync(spel.GameWonBy.ToString());
                var loser = await _userManager.FindByIdAsync(spel.GameLostBy.ToString());
                spel.WinnerToken = winner.UserName ?? "Unkown";
                spel.LoserToken = loser.UserName ?? "Unkown";
            }

            return spellenDto;
        }
    }
}
