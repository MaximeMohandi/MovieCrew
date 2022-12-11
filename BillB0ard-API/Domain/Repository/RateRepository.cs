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

        public async Task Add(RateCreationDTO rateCreationDTO)
        {
            var rate = _dbContext.Rates
                .FirstOrDefault(r => r.UserId == rateCreationDTO.UserId && r.MovieId == rateCreationDTO.MovieID);

            if (rate is null or default(Rate))
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
                rate.Note = rateCreationDTO.Rate;
                _dbContext.Rates.Update(rate);

            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
