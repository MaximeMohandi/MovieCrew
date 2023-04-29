namespace MovieCrew.Core.Domain.Ratings.Dtos;

public record RateCreationDto(int MovieID, long UserId, decimal Rate);