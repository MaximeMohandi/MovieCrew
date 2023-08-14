using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Entities;
using MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Exception;

namespace MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Services;

public class TMDbDataProvider : IThirdPartyMovieDataProvider
{
    private static readonly HttpClient _client = new();
    private static string _tmdbHostUrl;
    private static string _tmdbPosterUrl;

    public TMDbDataProvider(IConfiguration configuration)
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer",
                configuration.GetSection("ThirdPartyMovieDatabaseProvider:ApiKey").Value);
        _tmdbHostUrl = configuration.GetSection("ThirdPartyMovieDatabaseProvider:BaseUrl").Value;
        _tmdbPosterUrl = configuration.GetSection("ThirdPartyMovieDatabaseProvider:PosterBaseUrl").Value;
    }

    public async Task<MovieMetadataEntity> GetDetails(string title)
    {
        var foundMovieId = await SearchTMDbMovieId(title);

        var details = await FetchFromTmdb<TMDbMovieEntity>($"/movie/{foundMovieId}");

        return new MovieMetadataEntity(_tmdbPosterUrl + details.PosterPath, details.Overview, details.VoteAverage,
            details.Revenue,
            details.Budget);
    }

    private static async Task<int> SearchTMDbMovieId(string title)
    {
        var searchMovieResults =
            await FetchFromTmdb<TMDbSearchMovieResultsEntity>($"/search/movie?query={title}");

        if (searchMovieResults is null || searchMovieResults.TotalResults == 0)
            throw new NoMetaDataFoundException(title);

        return searchMovieResults.Results.First().id;
    }

    private static async Task<T> FetchFromTmdb<T>(string pathUrl)
    {
        var response = await _client.GetAsync($"{_tmdbHostUrl}{pathUrl}");

        if (!response.IsSuccessStatusCode) throw new CantFetchThirdPartyApiException();

        return await response.Content.ReadFromJsonAsync<T>();
    }
}
