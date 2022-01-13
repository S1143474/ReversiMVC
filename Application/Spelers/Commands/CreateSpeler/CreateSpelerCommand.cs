using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Spelers.Queries.GetSpeler;
using Domain.Entities;
using MediatR;

namespace Application.Spelers.Commands.CreateSpeler
{
    public class CreateSpelerCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

    }

    public class CreateSpelerCommandHandle : IRequestHandler<CreateSpelerCommand, bool>
    {
        private readonly IReversiDbContext _reversiDbContext;

        public CreateSpelerCommandHandle(IReversiDbContext reversiDbContext)
        {
            _reversiDbContext = reversiDbContext;
        }

        public async Task<bool> Handle(CreateSpelerCommand request, CancellationToken cancellationToken)
        {
            var spelerModel = _reversiDbContext.Spelers.FirstOrDefault(s => s.Guid.Equals(request.UserId));

            if (spelerModel != null)
                return true;

            spelerModel = new Speler(request.UserId, request.UserName);

            await _reversiDbContext.Spelers.AddAsync(spelerModel, cancellationToken);

            var savedEntities = await _reversiDbContext.SaveChangesAsync(cancellationToken);

            return savedEntities == 1;

        }
    }
}
