using MovieCrew.Core.Domain.Movies.Entities;

namespace MovieCrew.Core.Domain.Movies.Repository;

public interface IMovieRepository
{
    Task<MovieEntity> Add(string title, string poster, string description, long? proposedById);
    Task<List<MovieEntity>> GetAll();
    Task<List<MovieEntity>> GetAllUnSeen();
    Task<MovieDetailsEntity> GetMovie(int id);
    Task<MovieDetailsEntity> GetMovie(string title);
    Task Update(int movieId, string newTitle);
    Task Update(int movieId, DateTime seenDate);
}
