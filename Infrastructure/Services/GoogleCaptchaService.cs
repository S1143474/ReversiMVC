using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application;
using Application.Common.Interfaces;
using Application.Common.Models.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class GoogleCaptchaService : IGoogleCaptchaService
    {
        private const string ApiClientName = "GoogleCaptcha";
        private HttpClient httpClient { get; }
        private IHttpClientFactory HttpClientFactory { get; }

        private readonly JsonSerializerOptions _options;

        private readonly IOptionsMonitor<GoogleCaptchaConfig> _config;

        private readonly ILogger<GoogleCaptchaService> _logger;

        public GoogleCaptchaService(IHttpClientFactory httpClientFactory, IOptionsMonitor<GoogleCaptchaConfig> config, ILogger<GoogleCaptchaService> logger)
        {
            HttpClientFactory = httpClientFactory;

            httpClient = HttpClientFactory.CreateClient(ApiClientName);

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            _config = config;

            _logger = logger;
        }

        public async Task<bool> VerifyCaptchaToken(string token)
        {
            try
            {
                var response = await httpClient.GetAsync($"?secret={_config.CurrentValue.SecretKey}&response={token}");

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError($"Failed to retrieve a captcha token.");
                    return false;
                }

                var responseStream = await response.Content.ReadAsStreamAsync();

                var googleResult = await JsonSerializer.DeserializeAsync<GoogleCaptchaResponse>(responseStream, _options);

                var isSucces = googleResult.Success && googleResult.Score > 0.5;
                
                if (isSucces)
                    _logger.LogInformation($"Succesfully retrieved captcha with a good score: {googleResult.Score}");
                else 
                    _logger.LogWarning($"Retrieved captcha with a score less than 0.5: {googleResult.Score}");

                return isSucces;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong with retrieving/deserializing a captcha token: {ex}");
                
                return false;
            }
        }
    }
}
