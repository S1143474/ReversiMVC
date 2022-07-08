using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Spellen.Commands.FinishedSpel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Spellen.Queries.GetSpelFinishedResults
{
    public class GetSpelFinishedResultsQuery : IRequest<FinishedSpelResultsDTO>
    {
        public string SpelerToken { get; set; }
    }

    public class GetSpelFinishedResultsQueryHandle : IRequestHandler<GetSpelFinishedResultsQuery, FinishedSpelResultsDTO>
    {
        private readonly ISpelService _spelService;
        private readonly IReversiDbContext _reversiDbContext;

        public GetSpelFinishedResultsQueryHandle(ISpelService spelService, IReversiDbContext reversiDbContext)
        {
            _spelService = spelService;
            _reversiDbContext = reversiDbContext;
        }

        public async Task<FinishedSpelResultsDTO> Handle(GetSpelFinishedResultsQuery request, CancellationToken cancellationToken)
        {
            var spelToken = await _spelService.GetSpelTokenFromSpelerToken(request.SpelerToken);

            var finishedResults = await _spelService.GetSpelFinishedResults(spelToken);

            if (finishedResults == null) return null;

            finishedResults.IsWinner = finishedResults.WinnerToken == request.SpelerToken;

            var currenSpeler = await _reversiDbContext.Spelers.FindAsync(request.SpelerToken);
            
            if (finishedResults.IsWinner)
                currenSpeler.AantalGewonnen++;

            if (!finishedResults.IsWinner)
                currenSpeler.AantalVerloren++;

            if (finishedResults.IsDraw)
                currenSpeler.AantalGelijk++;

            await _reversiDbContext.SaveChangesAsync(cancellationToken);

            return finishedResults;
        }
    }
}
