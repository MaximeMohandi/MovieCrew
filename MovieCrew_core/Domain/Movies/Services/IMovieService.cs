using MovieCrew.Core.Domain.Movies.Dtos;
using MovieCrew.Core.Domain.Movies.Entities;

namespace MovieCrew.Core.Domain.Movies.Services;

public interface IMovieService
{
    Task<List<MovieEntity>> FetchAllMovies();
    Task<MovieDetailsEntity> GetByTitle(string title);
    Task<MovieDetailsEntity> GetById(int movieId);
    Task<MovieEntity> RandomMovie();
    Task<MovieEntity> AddMovie(string title, long? proposedById);
    Task ChangeTitle(MovieRenameDto renameDto);
    Task SetSeenDate(MovieSetSeenDateDto movieSetSeenDateDTO);
}