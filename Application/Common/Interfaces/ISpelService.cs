using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Spellen.Commands.CreateSpel;
using Application.Spellen.Commands.FinishedSpel;
using Application.Spellen.Commands.PlaceFiche;
using Application.Spellen.Commands.StartSpel;
using Application.Spellen.Queries.GetSpellen;

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

        Task<PlacedFichedDTO> PlaceFiche(bool hasPassed, int x, int y, string token, string spelerToken);

        Task<string> GetSpelTokenFromSpelerToken(string spelerToken);

        Task<FinishedSpelResultsDTO> GetSpelFinishedResults(string spelToken);
    }
}
