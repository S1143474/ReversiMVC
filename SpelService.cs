using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReversiMvcApp.Models.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReversiMvcApp
{
    public class SpelService : ISpelService
    {

        //public const string AVAILABLE = "Available";
        //public const string WAITING = "Waiting";
        //public const string PLAYING = "Playing";
        /*private readonly ILogger<SpelService> _logger;

        public SpelService(ILogger<SpelService> logger)
        {
            _logger = logger;
        }*/

        
        /// <summary>
        /// Task retrieves a List of json spel objects
        /// </summary>
        /// <returns></returns>
        public async Task<List<SpelJsonModel>> ReturnListOfSpellen()
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri("https://localhost:44339/api/Spel");

                HttpResponseMessage response = null;

                try {
                    response = await client.GetAsync(url);
                } catch (HttpRequestException httpre) {
                    Console.WriteLine(httpre);
                }


                if (response == null) return null;
                string json;

                using (var content = response.Content)
                {
                    json = await content.ReadAsStringAsync();
                }

                return JsonConvert.DeserializeObject<List<SpelJsonModel>>(json);
            }
        }

        /// <summary>
        /// Task creates a spel object and sends it to the api.
        /// </summary>
        /// <param name="playerToken"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task CreateSpel(PlaceGameJsonObj placeGameJsonObj)
        {
            using (HttpClient client = new HttpClient())
            {   
                Uri url = new Uri($"https://localhost:44339/api/Spel");

                string json = JsonConvert.SerializeObject(placeGameJsonObj);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                await client.PostAsync(url, data);
            }
        }

        /// <summary>
        /// Task retrieves a spel reversi via a unique token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<SpelJsonModel> RetrieveSpelViaToken(string token)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri($"https://localhost:44339/api/Spel/{token}");

                HttpResponseMessage response = await client.GetAsync(url);

                string json;

                using (HttpContent content = response.Content)
                {
                    json = await content.ReadAsStringAsync();
                }

                return JsonConvert.DeserializeObject<SpelJsonModel>(json);
            }
        }

        /// <summary>
        /// Task retrieves a spel reversi via a unique speler token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<SpelJsonModel> RetrieveSpelViaSpelerToken(string token)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri($"https://localhost:44339/api/SpelSpeler/{token}");
                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync(url);
                }
                catch (HttpRequestException hre)
                {
                    // TODO: LOGGG
                    return null;
                }

                if (response == null)
                    return null;

                string json;
                using (HttpContent content = response.Content)
                {
                    json = await content.ReadAsStringAsync();
                }

                return JsonConvert.DeserializeObject<SpelJsonModel>(json);
            }
        }

        public async Task JoinGameReversi(string spelToken, string speler2Token)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri($"https://localhost:44339/api/Spel/join");

                string json = JsonConvert.SerializeObject(new { SpelToken = spelToken, Speler2Token = speler2Token });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    await client.PutAsync(url, data);
                } catch (HttpRequestException hre)
                {
                    //_logger.LogError(hre, $" - Something went wrong with player: { speler2Token } when trying to join the game: { spelToken }");
                }
            }
        }
    }
}
