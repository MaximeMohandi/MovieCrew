using System.Text.Json.Serialization;

namespace MovieCrew.Core.Domain.Movies.Entities
{
    public class MovieMetadataEntity
    {
        private string _posterLink;
        public MovieMetadataEntity(string posterLink, string description, decimal ratings, decimal revenue)
        {
            PosterLink = posterLink;
            Description = description;
            Ratings = ratings;
            Revenue = revenue;
        }
        [JsonPropertyName("poster_path")]
        public string PosterLink { get => _posterLink; set => _posterLink = "https://image.tmdb.org/t/p/original" + value; }

        [JsonPropertyName("overview")]
        public string Description { get; }

        [JsonPropertyName("vote_average")]
        public decimal Ratings { get; }

        [JsonPropertyName("revenue")]
        public decimal Revenue { get; }

    }
}
