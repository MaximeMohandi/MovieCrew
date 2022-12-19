using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.DTOs;

namespace BillB0ard_API.Domain.Repository
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
            }
            else
            {
                existingRate.Note = rateCreationDTO.Rate;
                _dbContext.Rates.Update(existingRate);

            }

            await _dbContext.SaveChangesAsync();
        }

        private Rate? ExistingRate(RateCreationDto rateCreationDTO)
        {
            return _dbContext.Rates
                .FirstOrDefault(r => r.UserId == rateCreationDTO.UserId && r.MovieId == rateCreationDTO.MovieID);
        }
    }
}
