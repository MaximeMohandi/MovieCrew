using Microsoft.EntityFrameworkCore;
using MovieCrew.Core.Data;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Dtos;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Users.Exception;

namespace MovieCrew.Core.Domain.Movies.Repository
{
    public class MovieRepository
    {
        private readonly AppDbContext _dbContext;
        public MovieRepository(AppDbContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task<MovieDetailsEntity> GetMovie(string title)
        {

            var movie = await _dbContext.Movies.Where(m => m.Name.ToLower() == title.ToLower()).FirstOrDefaultAsync();
            return movie is null
                ? throw new MovieNotFoundException(title)
                : new(movie.Id,
                       movie.Name,
                       movie.Poster,
                       movie.Description,
                       movie.DateAdded,
                       movie.SeenDate,
                       movie.Rates?.Average(r => r.Note),
                       movie.Rates?
                       .Select(r => new MovieRateEntity(new(r.User.Id, r.User.Name, r.User.Role), r.Note))
                       .ToList(),
                       movie.ProposedBy == null ? null : new(movie.ProposedBy.Id, movie.ProposedBy.Name, movie.ProposedBy.Role)
                    );
        }

        public async Task<MovieDetailsEntity> GetMovie(int id)
        {
            var movie = await _dbContext.Movies.Where(m => m.Id == id).FirstOrDefaultAsync();
            return movie is null
                ? throw new MovieNotFoundException(id)
                : new(movie.Id,
                       movie.Name,
                       movie.Poster,
                       movie.Description,
                       movie.DateAdded,
                       movie.SeenDate,
                       movie.Rates?.Average(r => r.Note),
                       movie.Rates?
                       .Select(r => new MovieRateEntity(new(r.User.Id, r.User.Name, r.User.Role), r.Note))
                       .ToList(),
                       movie.ProposedBy == null ? null : new(movie.ProposedBy.Id, movie.ProposedBy.Name, movie.ProposedBy.Role)
                    );
        }

        private static MovieEntity MappedMovie(Movie movie)
        {
            return new(
                movie.Id,
                movie.Name,
                movie.Poster,
                movie.Description,
                movie.DateAdded,
                movie.SeenDate,
                movie.Rates?.Average(r => r.Note)
            );
        }

        public async Task<List<MovieEntity>> GetAll()
        {
            var movies = await _dbContext.Movies
                .Select(m => MappedMovie(m))
                .ToListAsync();

            if (movies.Count == 0) throw new NoMoviesFoundException();

            return movies;
        }

        public async Task<MovieEntity> Add(MovieCreationDto creationMovie)
        {
            if (TitleExist(creationMovie.Title))
                throw new MovieAlreadyExistException(creationMovie.Title);

            if (creationMovie.proposedById != null && !_dbContext.Users.Any(u => u.Id == creationMovie.proposedById))
            {
                throw new UserNotFoundException(creationMovie.proposedById.Value);
            }

            var movie = new Movie()
            {
                Name = creationMovie.Title,
                Poster = creationMovie.Poster,
                Description = creationMovie.Description,
                DateAdded = DateTime.Now,
                ProposedById = creationMovie.proposedById
            };

            _dbContext.Movies.Add(movie);

            await _dbContext.SaveChangesAsync();

            return MappedMovie(movie);
        }

        private bool TitleExist(string title)
        {
            return _dbContext.Movies.Any(m => m.Name.ToLower() == title.ToLower());
        }

        public async Task Update(MovieRenameDto renameDto)
        {
            if (TitleExist(renameDto.NewTitle))
            {
                throw new MovieAlreadyExistException(renameDto.NewTitle);
            }


            Movie? movieToRename = ExistingMovie(renameDto.MovieID);

            movieToRename.Name = renameDto.NewTitle;

            _dbContext.Movies.Update(movieToRename);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(MovieChangePosterDto changePoster)
        {
            var movieToChange = ExistingMovie(changePoster.MovieId) ?? throw new MovieNotFoundException(changePoster.MovieId);
            movieToChange.Poster = changePoster.NewPosterLink;
            _dbContext.Movies.Update(movieToChange);


            await _dbContext.SaveChangesAsync();

        }

        public async Task Update(MovieSetSeenDateDto movieSetSeenDateDTO)
        {
            var existingMovie = ExistingMovie(movieSetSeenDateDTO.MovieID);
            existingMovie.SeenDate = new DateTime(movieSetSeenDateDTO.SeenDate.Year, movieSetSeenDateDTO.SeenDate.Month, movieSetSeenDateDTO.SeenDate.Day);
            await _dbContext.SaveChangesAsync();
        }

        private Movie ExistingMovie(int id)
        {
            return _dbContext.Movies.FirstOrDefault(m => m.Id == id) ?? throw new MovieNotFoundException(id);
        }

        public async Task<List<MovieEntity>> GetAllUnSeen()
        {
            return await _dbContext.Movies
                .Where(m => !m.SeenDate.HasValue)
                .Select(m => MappedMovie(m))
                .ToListAsync();
        }
    }
}
