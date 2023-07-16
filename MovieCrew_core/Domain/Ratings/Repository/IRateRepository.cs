namespace MovieCrew.Core.Domain.Ratings.Repository;

public interface IRateRepository
{
    Task Add(int movieId, long userId, decimal rate);
}
