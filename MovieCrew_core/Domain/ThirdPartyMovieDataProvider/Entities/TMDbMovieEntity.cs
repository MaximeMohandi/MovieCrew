using System.Text.Json.Serialization;

namespace MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Entities;

public class TMDbMovieEntity
{
    public string Overview { get; init; }

    [JsonPropertyName("poster_path")] public string PosterPath { get; init; }

    public int Revenue { get; init; }
    public int Budget { get; init; }

    [JsonPropertyName("vote_average")] public decimal VoteAverage { get; init; }
}
