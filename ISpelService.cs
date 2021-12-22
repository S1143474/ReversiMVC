using ReversiMvcApp.Models.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp
{
    public interface ISpelService
    {
        Task<List<SpelJsonModel>> ReturnListOfSpellen();
        Task CreateSpel(PlaceGameJsonObj placeGameJsonObj);
        Task<SpelJsonModel> RetrieveSpelViaToken(string token);
        Task<SpelJsonModel> RetrieveSpelViaSpelerToken(string token);
        Task JoinGameReversi(string spelToken, string speler2Token);
    }
}
