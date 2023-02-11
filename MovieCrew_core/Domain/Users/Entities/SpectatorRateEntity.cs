using MovieCrew_core.Domain.Movies.Entities;

namespace MovieCrew_core.Domain.Users.Entities
{
    public record SpectatorRateEntity(MovieEntity RatedMovie, decimal Rate);
}
