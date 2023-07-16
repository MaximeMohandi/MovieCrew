using MovieCrew.Core.Domain.Movies.Entities;

namespace MovieCrew.Core.Domain.Movies.Services;

public interface IMovieService
{
    Task<List<MovieEntity>> FetchAllMovies();
    Task<MovieDetailsEntity> GetMovieDetails(string title);
    Task<MovieDetailsEntity> GetMovieDetails(int movieId);
    Task<MovieEntity> RandomMovie();
    Task<MovieEntity> AddMovie(string title, long? proposedById);
    Task ChangeTitle(int movieId, string currentTitle, string newTitle);
    Task SetSeenDate(int movieId, DateTime seenDate);
}
