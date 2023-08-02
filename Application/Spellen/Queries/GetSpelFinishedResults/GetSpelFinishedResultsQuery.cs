using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Spellen.Commands.FinishedSpel;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Spellen.Queries.GetSpelFinishedResults
{
    public class GetSpelFinishedResultsQuery : IRequest<FinishedSpelDTO>
    {
        public Guid SpelerToken { get; set; }
    }

    public class GetSpelFinishedResultsQueryHandle : IRequestHandler<GetSpelFinishedResultsQuery, FinishedSpelDTO>
    {
        private readonly ISpelService _spelService;
        private readonly IReversiDbContext _reversiDbContext;
        private readonly IMapper _mapper;

        public GetSpelFinishedResultsQueryHandle(ISpelService spelService, IReversiDbContext reversiDbContext, IMapper mapper)
        {
            _spelService = spelService;
            _reversiDbContext = reversiDbContext;
            _mapper = mapper;
        }

        public async Task<FinishedSpelDTO> Handle(GetSpelFinishedResultsQuery request, CancellationToken cancellationToken)
        {
            var spelResults = await _spelService.RetrieveFinishedSpelOverSpelerToken(request.SpelerToken);

            var finishedResults = await _spelService.GetSpelFinishedResults(spelResults.Token.ToString());

            if (finishedResults == null) return null;

            
            var currenSpeler = await _reversiDbContext.Spelers.FindAsync(request.SpelerToken);

            var speler1 = await _reversiDbContext.Spelers.FindAsync(spelResults.Speler1Token);
            var speler2 = await _reversiDbContext.Spelers.FindAsync(spelResults.Speler2Token);

            if (finishedResults.GameWonBy.Equals(currenSpeler.Guid))
            {
                currenSpeler.AantalGewonnen++;
                finishedResults.WinnerToken = currenSpeler.Guid.ToString();
                finishedResults.LoserToken = (finishedResults.WinnerToken.Equals(speler1.Guid.ToString()) ? speler2.Guid : speler1.Guid).ToString();
            }

            if (!finishedResults.GameWonBy.Equals(currenSpeler.Guid))
            {
                currenSpeler.AantalVerloren++;
                finishedResults.LoserToken = currenSpeler.Guid.ToString();
                finishedResults.WinnerToken = (finishedResults.LoserToken.Equals(speler1.Guid.ToString()) ? speler2.Guid : speler1.Guid).ToString();
            }

            if (finishedResults.IsDraw)
                currenSpeler.AantalGelijk++;

            var mappedSpelResults = _mapper.Map<FinishedSpelDTO>(finishedResults);
            mappedSpelResults.IsWinner = finishedResults.GameWonBy.Equals(request.SpelerToken);

            mappedSpelResults.WinnerName = finishedResults.GameWonBy.Equals(speler1.Guid) ? speler1.Naam : speler2.Naam;
            mappedSpelResults.LoserName = finishedResults.GameLostBy.Equals(speler1.Guid) ? speler1.Naam : speler2.Naam;

            await _reversiDbContext.SaveChangesAsync(cancellationToken);

            return mappedSpelResults;
        }
    }
}
