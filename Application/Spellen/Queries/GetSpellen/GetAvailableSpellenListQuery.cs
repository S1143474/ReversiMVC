using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.Requests;
using Application.Spellen.Queries.GetSpellen;
using MediatR;

namespace Application.Spelers.Queries.GetSpellen
{
    public class GetAvailableSpellenListQuery : IRequest<List<SpelDto>>
    {

    }

    public class GetAvailableSpellenListQueryHandler : IRequestHandler<GetAvailableSpellenListQuery, List<SpelDto>>
    {
        public ISpelService _spelService { get; set; }

        public GetAvailableSpellenListQueryHandler(ISpelService service)
        {
            _spelService = service;
        }

        public async Task<List<SpelDto>> Handle(GetAvailableSpellenListQuery request, CancellationToken cancellationToken)
        {
            var spellen = await _spelService.ReturnListOfSpellen();

            return spellen;
        }
    }
}
