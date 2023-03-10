using System.Text.Json.Serialization;

namespace MovieCrew.Core.Domain.Movies.Entities
{
    public class MovieMetadataEntity
    {
        public MovieMetadataEntity(string posterLink, string description, decimal ratings, decimal revenue)
        {
            PosterLink = posterLink;
            Description = description;
            Ratings = ratings;
            Revenue = revenue;
        }
        [JsonPropertyName("poster_path")]
        public string PosterLink { get; }

        [JsonPropertyName("overview")]
        public string Description { get; }

        [JsonPropertyName("vote_average")]
        public decimal Ratings { get; }

        [JsonPropertyName("revenue")]
        public decimal Revenue { get; }

    }
}
