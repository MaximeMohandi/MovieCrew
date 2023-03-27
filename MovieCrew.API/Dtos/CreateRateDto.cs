namespace MovieCrew.API.Dtos
{
    public record CreateRateDto(int IdMovie, long UserId, decimal Rate);
}