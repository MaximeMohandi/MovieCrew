using MovieCrew.Core.Data;
using MovieCrew.Core.Data.Models;

namespace MovieCrew.Core.Domain.Ratings.Repository;

public class RateRepository : IRateRepository
{
    private readonly AppDbContext _dbContext;

    public RateRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(int movieId, long userId, decimal rate)
    {
        var existingRate = FetchRate(movieId, userId);

        if (existingRate is null)
        {
            _dbContext.Rates.Add(new Rate
            {
                UserId = userId,
                MovieId = movieId,
                Note = rate
            });
            AddViewingDateToMovieRated(movieId);
        }
        else
        {
            existingRate.Note = rate;
            _dbContext.Rates.Update(existingRate);
        }

        await _dbContext.SaveChangesAsync();
    }

    private void AddViewingDateToMovieRated(int movieId)
    {
        var toRateMovie = _dbContext.Movies.First(m => m.Id == movieId);
        toRateMovie.SeenDate = DateTime.Now;
        _dbContext.Movies.Update(toRateMovie);
    }

    private Rate? FetchRate(int movieId, long userId)
    {
        return _dbContext.Rates
            .FirstOrDefault(r => r.UserId == userId && r.MovieId == movieId);
    }
}
