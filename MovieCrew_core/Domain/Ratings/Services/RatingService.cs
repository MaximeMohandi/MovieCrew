using MovieCrew.Core.Domain.Ratings.Dtos;
using MovieCrew.Core.Domain.Ratings.Exception;
using MovieCrew.Core.Domain.Ratings.Repository;

namespace MovieCrew.Core.Domain.Ratings.Services;

public class RatingService : IRatingService
{
    private readonly IRateRepository _rateRepository;

    public RatingService(IRateRepository rateRepository)
    {
        _rateRepository = rateRepository;
    }

    public async Task RateMovie(int idMovie, long userId, decimal rate)
    {
        if (rate is > 10 or < 0) throw new RateLimitException(rate);
        await _rateRepository.Add(new RateCreationDto(idMovie, userId, rate));
    }
}
