using System.Text.Json.Serialization;

namespace MovieCrew.Core.Domain.ThirdPartyMovieProvider.Entities
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
        public string PosterLink { get; set; }

        [JsonPropertyName("overview")]
        public string Description { get; }

        [JsonPropertyName("vote_average")]
        public decimal Ratings { get; }

        [JsonPropertyName("revenue")]
        public decimal Revenue { get; }

    }
}
