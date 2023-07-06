using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using MovieCrew.Core.Domain.Movies.Entities;

namespace MovieCrew.Core.Domain.Movies.Services;

public class TMDbDataProvider : IThirdPartyMovieDataProvider
{
    private const string TmdbHostUrl = "https://api.themoviedb.org/3/";
    private static readonly HttpClient _client = new();

    public TMDbDataProvider(IConfiguration configuration)
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer",
                configuration.GetSection("ThirdPartyMovieDatabaseProvider:ApiKey").Value);
    }

    public async Task<MovieMetadataEntity> GetDetails(string title)
    {
        throw new NotImplementedException();
    }
}
