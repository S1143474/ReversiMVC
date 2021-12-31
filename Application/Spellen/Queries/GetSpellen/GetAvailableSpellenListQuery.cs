using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Spellen.Queries.GetSpellen;
using MediatR;

namespace Application.Spelers.Queries.GetSpellen
{
    public class GetAvailableSpellenListQuery : IRequest<List<SpelDTO>>
    {

    }

    public class GetAvailableSpellenListQueryHandler : IRequestHandler<GetAvailableSpellenListQuery, List<SpelDTO>>
    {
        public ISpelService _spelService { get; set; }

        public GetAvailableSpellenListQueryHandler(ISpelService service)
        {
            _spelService = service;
        }

        public async Task<List<SpelDTO>> Handle(GetAvailableSpellenListQuery request, CancellationToken cancellationToken)
        {
            var spellen = await _spelService.ReturnListOfSpellen();

            return spellen;
        }
    }
}
