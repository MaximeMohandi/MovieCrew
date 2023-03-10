using MovieCrew.Core.Domain.ThirdPartyMovieProvider.Entities;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MovieCrew.Core.Domain.ThirdPartyMovieProvider.Services
{
    public class ThirdPartyMovieDataProvider
    {
        private readonly HttpClient _client;
        private readonly string _searchQuery = "search/movie?query=";
        public ThirdPartyMovieDataProvider(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", apiKey);
        }

        public async Task<MovieMetadataEntity> GetDetails(string title)
        {
            HttpResponseMessage response = await _client.GetAsync(_searchQuery + title);
            var content = await response.Content.ReadAsStringAsync();
            JsonNode results = JsonNode.Parse(content)["results"];

            if (results.AsArray().Count == 0)
            {
                throw new Exception("movie not existing");
            }
            return JsonSerializer.Deserialize<MovieMetadataEntity>(results[0]);
        }
    }
}
