using MovieCrew.Core.Domain.Movies.Entities;

namespace MovieCrew.Core.Domain.Users.Entities;

public record SpectatorRateEntity(MovieEntity RatedMovie, decimal Rate);