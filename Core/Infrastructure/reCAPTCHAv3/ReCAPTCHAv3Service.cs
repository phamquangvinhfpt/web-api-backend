using Microsoft.Extensions.Options;
using System.Text.Json;
namespace Core.Infrastructure.reCAPTCHAv3
{
    public class ReCAPTCHAv3Service : IReCAPTCHAv3Service
    {
        private readonly reCAPTCHAv3Settings _settings;
        private readonly HttpClient _httpClient;
        public ReCAPTCHAv3Service(IOptions<reCAPTCHAv3Settings> settings, HttpClient httpClient)
        {
            _settings = settings.Value;
            _httpClient = httpClient;
        }

        public async Task<ReCAPTCHAv3Response> Verify(string token)
        {
            if (token.Equals(_settings.SecretDeveloperToken))
                return new ReCAPTCHAv3Response { success = true, score = 1.0 };
            var response = await _httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_settings.SecretKey}&response={token}", null!);
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ReCAPTCHAv3Response>(responseString);
            if (result is null)
                throw new Exception("Failed to deserialize reCAPTCHAv3 response");
            return result;
        }
    }
}