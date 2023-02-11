namespace MovieCrew_core.Domain.Ratings.Dtos
{
    public record RateCreationDto(int MovieID, long UserId, decimal Rate);
}
