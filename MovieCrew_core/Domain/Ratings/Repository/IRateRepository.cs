using MovieCrew.Core.Domain.Ratings.Dtos;

namespace MovieCrew.Core.Domain.Ratings.Repository;

public interface IRateRepository
{
    Task Add(RateCreationDto rateCreationDTO);
}