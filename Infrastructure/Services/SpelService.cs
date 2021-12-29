using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Spellen.Commands.CreateSpel;
using Domain.Entities;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class SpelService : ISpelService
    {
        private IHttpClientFactory HttpClientFactory { get; set; }

        public SpelService(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public Task<List<Spel>> ReturnListOfSpellen()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateSpel(CreateSpelCommand spelCommand)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            string json = JsonConvert.SerializeObject(spelCommand);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync("Spel", data);
            return result.IsSuccessStatusCode;
        }

        public Task<Spel> RetrieveSpelOverToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task<Spel> RetrieveSpelOverSpelerToken(string spelerToken)
        {
            throw new NotImplementedException();
        }

        public Task JoinSpelReversi(string spelToken, string speler2Token)
        {
            throw new NotImplementedException();
        }
    }
}
