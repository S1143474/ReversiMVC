using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.Requests;
using MediatR;

namespace Application.Spellen.Queries.GetSpellen
{
    public class GetSpellenHistoryQuery : IRequest<List<SpelFinishedDto>>
    {
        public Guid SpelerToken { get; set; }
    }

    public class GetSpellenHistoryQueryHandle : IRequestHandler<GetSpellenHistoryQuery, List<SpelFinishedDto>>
    {
        private readonly ISpelService _spelService;

        public GetSpellenHistoryQueryHandle(ISpelService spelService)
        {
            _spelService = spelService;
        }

        public async Task<List<SpelFinishedDto>> Handle(GetSpellenHistoryQuery request, CancellationToken cancellationToken)
        {
            var spellenDto = await _spelService.GetSpellenFinishedBySpelerTokenDescAsync(request.SpelerToken);

            return spellenDto;
        }
    }
}
