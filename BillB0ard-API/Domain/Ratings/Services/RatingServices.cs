using BillB0ard_API.Domain.Ratings.Dtos;
using BillB0ard_API.Domain.Ratings.Exception;
using BillB0ard_API.Domain.Ratings.Repository;

namespace BillB0ard_API.Domain.Ratings.Services
{
    public class RatingServices
    {
        private readonly RateRepository _rateRepository;

        public RatingServices(RateRepository rateRepository)
        {
            _rateRepository = rateRepository;
        }

        public async Task RateMovie(RateCreationDto rateCreation)
        {
            if (rateCreation.Rate > 10 || rateCreation.Rate < 0)
            {
                throw new RateLimitException(rateCreation.Rate);
            }
            await _rateRepository.Add(rateCreation);
        }
    }
}
