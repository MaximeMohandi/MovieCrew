using MovieCrew.Core.Domain.Movies.Entities;
using System.Net.Http.Headers;
using System.Text.Json;

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

        public async Task<MovieMetadataEntity> GetDetails()
        {
            HttpResponseMessage response = await _client.GetAsync("movie/100");
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<MovieMetadataEntity>(content);
        }
    }
}
