using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Users.Entities;

namespace MovieCrew.Core.Domain.Movies.Extension;

public static class MovieMapExtension
{
    public static MovieEntity ToEntity(this Movie movie)
    {
        return new MovieEntity(
            movie.Id,
            movie.Name,
            movie.Poster,
            movie.Description,
            movie.DateAdded,
            movie.SeenDate,
            movie.Rates?.Average(r => r.Note)
        );
    }

    public static MovieDetailsEntity ToDetailledEntity(this Movie movie)
    {
        return new MovieDetailsEntity(movie.Id,
            movie.Name,
            movie.Poster,
            movie.Description,
            movie.DateAdded,
            movie.SeenDate,
            movie.Rates?.Average(r => r.Note),
            null,
            null,
            movie.Rates?
                .Select(r => new MovieRateEntity(new UserEntity(r.User.Id, r.User.Name, r.User.Role), r.Note))
                .ToList(),
            movie.ProposedBy == null
                ? null
                : new UserEntity(movie.ProposedBy.Id, movie.ProposedBy.Name, movie.ProposedBy.Role));
    }
}