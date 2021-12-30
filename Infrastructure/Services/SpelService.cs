using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Spellen.Commands.CreateSpel;
using Domain.Entities;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class SpelService : ISpelService
    {
        private IHttpClientFactory HttpClientFactory { get; set; }

        public SpelService(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public async Task<List<Spel>> ReturnListOfSpellen()
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            var response = await httpClient.GetAsync("Spel");

            if (response == null || response.StatusCode == HttpStatusCode.NoContent)
                return null;

            string json;
            using (var content = response.Content)
            {
                json = await content.ReadAsStringAsync();
            }

            return JsonSerializer.Deserialize<List<Spel>>(json);
        }

        public async Task<bool> CreateSpel(CreateSpelCommand spelCommand)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            string json = JsonSerializer.Serialize(spelCommand); 
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("Spel", data);
            return response.IsSuccessStatusCode;
        }

        public Task<Spel> RetrieveSpelOverToken(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<Spel> RetrieveSpelOverSpelerToken(string spelerToken)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            var response = await httpClient.GetAsync($"SpelSpeler/{spelerToken}");

            if (response == null || response.StatusCode == HttpStatusCode.NoContent)
                return null;

            string json;
            using (var content = response.Content)
            {
                json = await content.ReadAsStringAsync();
            }

            return JsonSerializer.Deserialize<Spel>(json);
        }

        public Task JoinSpelReversi(string spelToken, string speler2Token)
        {
            throw new NotImplementedException();
        }
    }
}
