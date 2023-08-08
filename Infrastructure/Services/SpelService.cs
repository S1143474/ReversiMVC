using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Spellen.Commands.CreateSpel;
using System.Text.Json;
using Application.Common.Models.Requests;
using Application.Spellen.Commands.FinishedSpel;
using Application.Spellen.Commands.PlaceFiche;
using Application.Spellen.Commands.StartSpel;
using Application.Spellen.Queries.GetSpellen;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Linq;

namespace Infrastructure.Services
{
    public class SpelService : ISpelService
    {
        private const string ApiClientName = "SpelRestAPI";

        private HttpClient httpClient { get; }
        private IHttpClientFactory HttpClientFactory { get; }

        private readonly JsonSerializerOptions _options;

        public SpelService(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;

            httpClient = HttpClientFactory.CreateClient(ApiClientName);

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<SpelDto>> ReturnListOfSpellen()
        {
            var response = await httpClient.GetAsync("spel/queue");

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();

            var spellenDto = await JsonSerializer.DeserializeAsync<List<SpelDto>>(stream, _options);

            return spellenDto;
        }

        /*public async Task<List<SpelDto>> ReturnListOfSpellen()
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

            var result = JsonSerializer.Deserialize<List<SpelDto>>(json);
            return result;
        }*/

        public async Task<SpelDto> CreateSpel(SpelCreateDto spelCreateDto)
        {
            string json = JsonSerializer.Serialize(spelCreateDto);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("spel/create", data);

            var stream = await response.Content.ReadAsStreamAsync();

            var inQueueSpel = await JsonSerializer.DeserializeAsync<SpelDto>(stream, _options);

            return inQueueSpel;
        }

        /*public async Task<bool> CreateSpel(CreateSpelCommand spelCommand)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            string json = JsonSerializer.Serialize(spelCommand); 
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("Spel", data);
            return response.IsSuccessStatusCode;
        }*/

        public async Task<SpelDto> RetrieveSpelOverToken(Guid token)
        {
            var response = await httpClient.GetAsync($"spel/{token}", HttpCompletionOption.ResponseHeadersRead);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException hre)
            {
                return null;
            }

            var stream = await response.Content.ReadAsStreamAsync();

            var inProcesSpel = await JsonSerializer.DeserializeAsync<SpelDto>(stream, _options);

            return inProcesSpel;

        }
        /*public async Task<SpelDTO> RetrieveSpelOverToken(string token)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            var response = await httpClient.GetAsync($"Spel/{token}");

            string json;
            using (var content = response.Content)
            {
                json = await content.ReadAsStringAsync();
            }

            return string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<SpelDTO>(json);
        }*/

        public async Task<SpelDto> RetrieveSpelOverSpelerToken(Guid spelerToken)
        {
            var response = await httpClient.GetAsync($"spel/unfinished?spelerToken={spelerToken}", HttpCompletionOption.ResponseHeadersRead);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException hre)
            {
                return null;
            }

            var stream = await response.Content.ReadAsStreamAsync();

            var spelDto = await JsonSerializer.DeserializeAsync<SpelDto>(stream, _options);

            return spelDto;
        }

        public async Task<SpelDto> RetrieveFinishedSpelOverSpelerToken(Guid spelerToken)
        {
            var response = await httpClient.GetAsync($"spel/finished?spelerToken={spelerToken}", HttpCompletionOption.ResponseHeadersRead);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException hre)
            {
                return null;
            }

            var stream = await response.Content.ReadAsStreamAsync();

            var spelDto = await JsonSerializer.DeserializeAsync<List<SpelDto>>(stream, _options);

            return spelDto.FirstOrDefault();
        }
        /*public async Task<SpelDTO> RetrieveSpelOverSpelerToken(string spelerToken)
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
        }*/

        public async Task<bool> JoinSpelReversi(StartSpelCommand startSpelCommand)
        {
            string json = JsonSerializer.Serialize(startSpelCommand);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"spel/participate", data);

            return response.IsSuccessStatusCode;
        }
        /*public async Task<bool> JoinSpelReversi(StartSpelCommand startSpelCommand)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            string json = JsonSerializer.Serialize(startSpelCommand);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync("spel/join", data);
            return response.IsSuccessStatusCode;
        }*/

        public async Task<PlacedFichedDTO> PlaceFiche(bool hasPassed, int x, int y, string token, string spelerToken)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            string json = JsonSerializer.Serialize(new { hasPassed, x, y, token, spelerToken });
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"spel/inprocess/{token}/move", data);
/*            var response = await httpClient.PutAsync("spel/zet", data);
*/
            if (response == null || response.StatusCode == HttpStatusCode.NoContent || !response.IsSuccessStatusCode)
                return null;

            string jsonResponse;
            using (var content = response.Content)
            {
                jsonResponse = await content.ReadAsStringAsync();
            }

            return JsonSerializer.Deserialize<PlacedFichedDTO>(jsonResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public async Task<bool> SurrenderSpel(Guid spelerToken, Guid token)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");
            string json = JsonSerializer.Serialize(new { spelerToken });
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"spel/inprocess/{token}/surrender", data);
            
            string jsonResponse;
            using (var content = response.Content)
            {
                jsonResponse = await content.ReadAsStringAsync();
            }

            return JsonSerializer.Deserialize<bool>(jsonResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public async Task<string> GetSpelTokenFromSpelerToken(Guid spelerToken)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            /*var response = await httpClient.GetAsync($"Spel/SpelToken?spelerToken={spelerToken}");*/
            var response = await httpClient.GetAsync($"spel/unfinished?spelerToken={spelerToken}");
            if (response == null || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
                return null;

            string jsonResponse;
            using (var content = response.Content)
            {
                jsonResponse = await content.ReadAsStringAsync();
            }
            var result = JsonSerializer.Deserialize<SpelDto>(jsonResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return result.Token.ToString();
        }

        public async Task<FinishedSpelResultsDTO> GetSpelFinishedResults(string spelToken)
        {
            var httpClient = HttpClientFactory.CreateClient("SpelRestAPI");

            var response = await httpClient.GetAsync($"spel/finished/{spelToken}");

            if (response == null || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
                return null;

            using var content = response.Content;

            var result = await content.ReadAsStringAsync();

            if (result == null || string.IsNullOrWhiteSpace(result))
                return null;

            return JsonSerializer.Deserialize<FinishedSpelResultsDTO>(result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }); ;
        }

        public async Task<List<SpelFinishedDto>> GetSpellenFinishedBySpelerTokenDescAsync(Guid spelerToken)
        {
            var response = await httpClient.GetAsync($"spel/finished?spelerToken={spelerToken}&OrderBy=finishedAt desc", HttpCompletionOption.ResponseHeadersRead);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException hre)
            {
                return null;
            }

            var stream = await response.Content.ReadAsStreamAsync();

            var spellenDto = await JsonSerializer.DeserializeAsync<List<SpelFinishedDto>>(stream, _options);

            return spellenDto;
        }

        public async Task<bool> DeleteSpelUnFinished(Guid spelToken)
        {
            var response = await httpClient.DeleteAsync($"spel/unfinished/delete/{spelToken}");

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException hre)
            {
                return false;
            }

            return true;
        }
    }
}
