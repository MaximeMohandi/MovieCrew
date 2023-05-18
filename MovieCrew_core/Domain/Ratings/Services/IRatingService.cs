namespace MovieCrew.Core.Domain.Ratings.Services;

public interface IRatingService
{
    Task RateMovie(int idMovie, long userId, decimal rate);
}
