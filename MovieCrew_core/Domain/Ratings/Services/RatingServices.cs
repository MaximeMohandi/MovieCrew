using MovieCrew_core.Domain.Ratings.Dtos;
using MovieCrew_core.Domain.Ratings.Exception;
using MovieCrew_core.Domain.Ratings.Repository;

namespace MovieCrew_core.Domain.Ratings.Services
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
