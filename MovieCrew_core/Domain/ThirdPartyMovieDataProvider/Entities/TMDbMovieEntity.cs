using System.Text.Json.Serialization;

namespace MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Entities;

public class TMDbMovieEntity
{
    public string Overview { get; init; }

    [JsonPropertyName("poster_path")] public string PosterPath { get; init; }

    public decimal Revenue { get; init; }
    public decimal Budget { get; init; }

    [JsonPropertyName("vote_average")] public decimal VoteAverage { get; init; }
}
