using MovieCrew.Core.Domain.Movies.Dtos;
using MovieCrew.Core.Domain.Movies.Entities;

namespace MovieCrew.Core.Domain.Movies.Repository;

public interface IMovieRepository
{
    Task<MovieEntity> Add(MovieCreationDto creationMovie);
    Task<List<MovieEntity>> GetAll();
    Task<List<MovieEntity>> GetAllUnSeen();
    Task<MovieDetailsEntity> GetMovie(int id);
    Task<MovieDetailsEntity> GetMovie(string title);
    Task Update(MovieRenameDto renameDto);
    Task Update(MovieSetSeenDateDto movieSetSeenDateDTO);
}