using MovieCrew.Core.Domain.Movies.Interfaces;
using MovieCrew.Core.Domain.ThirdPartyMovieProvider.Entities;
using MovieCrew.Core.Domain.ThirdPartyMovieProvider.Exception;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MovieCrew.Core.Domain.ThirdPartyMovieProvider.Services
{
    public class TmbdMovieDataProvider : IThirdPartyMovieDataProvider
    {
        private readonly HttpClient _client;
        private readonly string _searchQuery = "search/movie?query=";

        public TmbdMovieDataProvider(string baseUrl, string apiKey)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", apiKey);
        }

        public async Task<MovieMetadataEntity> GetDetails(string title)
        {
            HttpResponseMessage response = await _client.GetAsync(_searchQuery + title);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            JsonNode? results = JsonNode.Parse(content)?["results"];

            if (results is null || results.AsArray().Count == 0)
            {
                throw new NoMetaDataFound(title);
            }

            var metadata = JsonSerializer.Deserialize<MovieMetadataEntity>(results[0]);
            metadata.PosterLink = "https://image.tmdb.org/t/p/original" + metadata.PosterLink;

            return metadata;
        }
    }
}
