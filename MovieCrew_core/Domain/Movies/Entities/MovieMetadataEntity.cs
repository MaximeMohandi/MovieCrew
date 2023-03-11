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
        public string PosterLink { get; set; }

        public string Description { get; }

        public decimal Ratings { get; }

        public decimal Revenue { get; }

    }
}
