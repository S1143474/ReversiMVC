using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Spelers.Queries.GetSpeler
{
    public class GetSpelerQuery : IRequest<SpelerDTO>
    {
        public string UserId { get; set; }
        public string Naam { get; set; }

    }

    public class GetSpelerQueryHandler : IRequestHandler<GetSpelerQuery, SpelerDTO>
    {
        private readonly IReversiDbContext _context;

        public GetSpelerQueryHandler(IReversiDbContext context)
        {
            _context = context;
        }

        public async Task<SpelerDTO> Handle(GetSpelerQuery request, CancellationToken cancellationToken)
        {

            var speler =  await _context.Spelers.FirstOrDefaultAsync(s => s.Guid.Equals(request.UserId), cancellationToken);

            return new SpelerDTO()
            {
                Draws = speler?.AantalGelijk ?? 0,
                Wins = speler?.AantalGewonnen ?? 0,
                Losses = speler?.AantalVerloren ?? 0
            };
        }
    }
}
