using BillB0ard_API.Domain.Movies.Entities;

namespace BillB0ard_API.Domain.Users.Entities
{
    public record SpectatorRateEntity(MovieEntity RatedMovie, decimal Rate);
}
