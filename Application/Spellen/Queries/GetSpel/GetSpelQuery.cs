using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Mapping;
using Application.Spellen.Queries.GetSpellen;
using Domain.Entities;
using MediatR;

namespace Application.Spellen.Queries.GetSpel
{
    public class GetSpelQuery : IRequest<GetSpelDTO>
    {
        public string Id { get; set; }
        public string UserId { get; set; }
    }

    public class GetSpelQueryHandle : IRequestHandler<GetSpelQuery, GetSpelDTO>
    {
        private readonly IReversiDbContext _context;
        public ISpelService _spelService { get; set; } 

        public GetSpelQueryHandle(ISpelService spelService, IReversiDbContext context)
        {
            _context = context;
            _spelService = spelService;
        }

        public async Task<GetSpelDTO> Handle(GetSpelQuery request, CancellationToken cancellationToken)
        {
            var spel = await _spelService.RetrieveSpelOverToken(request.Id);

            if (spel == null)
                return null;

            var speler1 = 
                await _context.Spelers.FindAsync(request.UserId == spel.speler1Token ? spel.speler1Token : spel.speler2Token);
            var speler2 =
                await _context.Spelers.FindAsync(request.UserId == spel.speler1Token ? spel.speler2Token : spel.speler1Token);

            GetSpelDTO spelDTO = spel.MapToGetSpelDto();

            spelDTO.Speler1Naam = speler1?.Naam ?? "UnKnown";
            spelDTO.Speler2Naam = speler2?.Naam ?? "UnKnown";

            return spelDTO;
        }
    }
}
 