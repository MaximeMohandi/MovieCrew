namespace MovieCrew.API.Test.Controller.Ratings
{
    public record CreateRateDto(int IdMovie, long UserId, decimal Rate);
}