using MovieCrew.Core.Domain.Ratings.Dtos;
using MovieCrew.Core.Domain.Ratings.Exception;
using MovieCrew.Core.Domain.Ratings.Repository;

namespace MovieCrew.Core.Domain.Ratings.Services
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
