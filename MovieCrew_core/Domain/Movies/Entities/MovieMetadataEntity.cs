namespace MovieCrew.Core.Domain.Movies.Entities;

public class MovieMetadataEntity
{
    public MovieMetadataEntity(string posterLink, string description, decimal ratings, decimal revenue, decimal budget)
    {
        PosterLink = posterLink;
        Description = description;
        Ratings = ratings;
        Revenue = revenue;
        Budget = budget;
    }

    public string PosterLink { get; }
    public string Description { get; }
    public decimal Ratings { get; }
    public decimal Revenue { get; }
    public decimal Budget { get; }
}
