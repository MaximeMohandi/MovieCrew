using System.Text.Json.Serialization;

namespace MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Entities;

public class TMDbSearchMovieResultsEntity
{
    [JsonPropertyName("total_results")] public int TotalResults { get; init; }
    public TMdbMovieSearchResultEntity[] Results { get; init; }
}
