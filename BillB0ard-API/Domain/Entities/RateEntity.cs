namespace BillB0ard_API.Domain.Entities
{
    public record RateEntity(MovieEntity MovieRated, UserEntity RatedBy, decimal Rate);
}
