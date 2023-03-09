using MovieCrew.Core.Domain.Movies.Entities;
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

        public async Task<MovieMetadataEntity> GetDetails()
        {
            HttpResponseMessage response = await _client.GetAsync("movie/100");
            return new("http://tes.", "2", 0, 2);
        }
    }
}
