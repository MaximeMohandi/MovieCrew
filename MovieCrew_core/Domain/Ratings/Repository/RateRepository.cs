using MovieCrew.Core.Data;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Ratings.Dtos;

namespace MovieCrew.Core.Domain.Ratings.Repository
{
    public class RateRepository
    {
        private readonly AppDbContext _dbContext;
        public RateRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(RateCreationDto rateCreationDTO)
        {
            Rate? existingRate = ExistingRate(rateCreationDTO);

            if (existingRate is null)
            {
                _dbContext.Rates.Add(new Rate()
                {
                    UserId = rateCreationDTO.UserId,
                    MovieId = rateCreationDTO.MovieID,
                    Note = rateCreationDTO.Rate
                });
                AddViewingDateToMovieRated(rateCreationDTO);
            }
            else
            {
                existingRate.Note = rateCreationDTO.Rate;
                _dbContext.Rates.Update(existingRate);
            }

            await _dbContext.SaveChangesAsync();
        }

        private void AddViewingDateToMovieRated(RateCreationDto rateCreationDTO)
        {
            var toRateMovie = _dbContext.Movies.First(m => m.Id == rateCreationDTO.MovieID);
            toRateMovie.SeenDate = DateTime.Now;
            _dbContext.Movies.Update(toRateMovie);
        }

        private Rate? ExistingRate(RateCreationDto rateCreationDTO)
        {
            return _dbContext.Rates
                .FirstOrDefault(r => r.UserId == rateCreationDTO.UserId && r.MovieId == rateCreationDTO.MovieID);
        }
    }
}
