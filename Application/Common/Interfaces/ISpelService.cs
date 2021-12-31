using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Spelers.Queries.GetSpellen;
using Application.Spellen.Commands.CreateSpel;
using Application.Spellen.Queries.GetSpellen;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    // TODO: Create command for all of the methods.
    public interface ISpelService
    {
        Task<List<SpelDTO>> ReturnListOfSpellen();

        Task<bool> CreateSpel(CreateSpelCommand spelCommand);

        Task<Spel> RetrieveSpelOverToken(string token);

        Task<Spel> RetrieveSpelOverSpelerToken(string spelerToken);

        Task JoinSpelReversi(string spelToken, string speler2Token);
    }
}
