using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Models.Requests;
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
        Task<List<SpelDto>> ReturnListOfSpellen();

        Task<SpelDto> CreateSpel(SpelCreateDto spelCreateDto);

        Task<SpelDto> RetrieveSpelOverToken(Guid token);

        Task<SpelDto> RetrieveSpelOverSpelerToken(Guid spelerToken);
        Task<SpelDto> RetrieveFinishedSpelOverSpelerToken(Guid spelerToken);

        Task<bool> JoinSpelReversi(StartSpelCommand startSpelCommand);

        Task<PlacedFichedDTO> PlaceFiche(bool hasPassed, int x, int y, string token, string spelerToken);

        Task<bool> SurrenderSpel(Guid spelerToken, Guid token);

        Task<string> GetSpelTokenFromSpelerToken(Guid spelerToken);

        Task<FinishedSpelResultsDTO> GetSpelFinishedResults(string spelToken);

        Task<List<FinishedSpelResultsDTO>> GetSpellenFinishedBySpelerTokenDescAsync(Guid spelerToken);
        Task<bool> DeleteSpelUnFinished(Guid spelToken);

    }
}
