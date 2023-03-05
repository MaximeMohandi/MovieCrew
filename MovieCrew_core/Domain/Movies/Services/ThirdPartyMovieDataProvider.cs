using System.Net.Http.Headers;

namespace MovieCrew.Core.Domain.Movies.Services
{
    public class ThirdPartyMovieDataProvider
    {
        private readonly HttpClient _client;
        public ThirdPartyMovieDataProvider(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", apiKey);
        }

        public async Task<bool> GetDetails()
        {
            HttpResponseMessage response = await _client.GetAsync("account");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
