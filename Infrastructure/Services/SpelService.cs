using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Spellen.Commands.CreateSpel;
using System.Text.Json;
using Application.Spellen.Commands.FinishedSpel;
using Application.Spellen.Commands.PlaceFiche;
using Application.Spellen.Commands.StartSpel;
using Application.Spellen.Queries.GetSpellen;
using Infrastructure.Common;
using MediatR;

namespace Infrastructure.Services
{
    public class SpelService : ISpelService
    {
        private const string ApiClientName = "SpelRestAPI";
        private HttpClient httpClient { get; }
        private IHttpClientFactory HttpClientFactory { get; set; }

        public SpelService(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;

            httpClient = HttpClientFactory.CreateClient(ApiClientName);
        }

        public async Task<List<SpelDTO>> ReturnListOfSpellen()
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

            var result = JsonSerializer.Deserialize<List<SpelDTO>>(json);
            return result;
        }

        public async Task<bool> CreateSpel(CreateSpelCommand spelCommand)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            string json = JsonSerializer.Serialize(spelCommand); 
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("Spel", data);
            return response.IsSuccessStatusCode;
        }

        public async Task<SpelDTO> RetrieveSpelOverToken(string token)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            var response = await httpClient.GetAsync($"Spel/{token}");

            string json;
            using (var content = response.Content)
            {
                json = await content.ReadAsStringAsync();
            }

            return string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<SpelDTO>(json);
        }

        public async Task<SpelDTO> RetrieveSpelOverSpelerToken(string spelerToken)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            var response = await httpClient.GetAsync($"spel/SpelSpeler/{spelerToken}");

            if (response == null || response.StatusCode == HttpStatusCode.NoContent)
                return null;

            string json;
            using (var content = response.Content)
            {
                json = await content.ReadAsStringAsync();
            }

            return JsonSerializer.Deserialize<SpelDTO>(json);
        }

        public async Task<bool> JoinSpelReversi(StartSpelCommand startSpelCommand)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            string json = JsonSerializer.Serialize(startSpelCommand);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync("spel/join", data);
            return response.IsSuccessStatusCode;
        }

        public async Task<PlacedFichedDTO> PlaceFiche(bool hasPassed, int x, int y, string token, string spelerToken)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            string json = JsonSerializer.Serialize(new { hasPassed, x, y, token, spelerToken });
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync("spel/zet", data);

            if (response == null || response.StatusCode == HttpStatusCode.NoContent || !response.IsSuccessStatusCode)
                return null;

            string jsonResponse;
            using (var content = response.Content)
            {
                jsonResponse = await content.ReadAsStringAsync();
            }

            return JsonSerializer.Deserialize<PlacedFichedDTO>(jsonResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public async Task<string> GetSpelTokenFromSpelerToken(string spelerToken)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            var response = await httpClient.GetAsync($"Spel/SpelToken?spelerToken={spelerToken}");

            if (response == null || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
                return null;

            using var content = response.Content;

            var result = await content.ReadAsStringAsync();

            return result;
        }

        public async Task<FinishedSpelResultsDTO> GetSpelFinishedResults(string spelToken)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            var response = await httpClient.GetAsync($"Spel/SpelFinished?spelToken={spelToken}");

            if (response == null || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
                return null;

            using var content = response.Content;

            var result = await content.ReadAsStringAsync();

            if (result == null || string.IsNullOrWhiteSpace(result))
                return null;

            return JsonSerializer.Deserialize<FinishedSpelResultsDTO>(result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }); ;
        }
    }
}
