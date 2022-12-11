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
            var newRate = new Rate()
            {
                MovieId = rateCreationDTO.MovieID,
                UserId = rateCreationDTO.UserId,
                Note = rateCreationDTO.Rate
            };

            _dbContext.Add(newRate);
            await _dbContext.SaveChangesAsync();
        }
    }
}
