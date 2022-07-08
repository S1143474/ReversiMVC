using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Spelers.Queries.GetSpelers
{
    public class GetSpelersQuery : IRequest<List<Speler>>
    {

    }

    public class GetSpelersQueryHandle : IRequestHandler<GetSpelersQuery, List<Speler>>
    {
        private readonly IReversiDbContext _reversiDbContext;

        public GetSpelersQueryHandle(IReversiDbContext reversiDbContext)
        {
            _reversiDbContext = reversiDbContext;
        }

        public async Task<List<Speler>> Handle(GetSpelersQuery request, CancellationToken cancellationToken)
        {
            return await _reversiDbContext.Spelers.ToListAsync(cancellationToken);
        }
    }
}
