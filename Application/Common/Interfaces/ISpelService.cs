using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Spelers.Queries.GetSpellen;
using Application.Spellen.Commands.CreateSpel;
using Application.Spellen.Commands.StartSpel;
using Application.Spellen.Queries.GetSpellen;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    // TODO: Create command for all of the methods.
    public interface ISpelService
    {
        Task<List<SpelDTO>> ReturnListOfSpellen();

        Task<bool> CreateSpel(CreateSpelCommand spelCommand);

        Task<SpelDTO> RetrieveSpelOverToken(string token);

        Task<SpelDTO> RetrieveSpelOverSpelerToken(string spelerToken);

        Task<bool> JoinSpelReversi(StartSpelCommand startSpelCommand);
    }
}
