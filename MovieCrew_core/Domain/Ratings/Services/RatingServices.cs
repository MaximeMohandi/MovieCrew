using MovieCrew.Core.Domain.Ratings.Dtos;
using MovieCrew.Core.Domain.Ratings.Exception;
using MovieCrew.Core.Domain.Ratings.Repository;

namespace MovieCrew.Core.Domain.Ratings.Services;

public class RatingServices
{
    private readonly IRateRepository _rateRepository;

    public RatingServices(IRateRepository rateRepository)
    {
        _rateRepository = rateRepository;
    }

    public async Task RateMovie(int idMovie, long userId, decimal rate)
    {
        if (rate > 10 || rate < 0) throw new RateLimitException(rate);
        await _rateRepository.Add(new RateCreationDto(idMovie, userId, rate));
    }
}