using System.Net.Http.Headers;

namespace MovieCrew.Core.Domain.Movies.Services
{
    public class ThirdPartyMovieDataProvider
    {
        private string _apiKey;
        private readonly string _baseUrl = "https://api.themoviedb.org/3/";
        private HttpClient _client;
        public ThirdPartyMovieDataProvider()
        {
            _apiKey = ""
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _apiKey);
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
