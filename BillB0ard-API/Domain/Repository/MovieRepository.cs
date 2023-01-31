using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.DTOs;
using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Exception;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Domain.Repository
{
    public class MovieRepository
    {
        private readonly AppDbContext _dbContext;
        public MovieRepository(AppDbContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task<MovieEntity> GetMovie(string title)
        {

            var movie = await _dbContext.Movies.Where(m => m.Name.ToLower() == title.ToLower()).FirstOrDefaultAsync();
            if (movie is null) throw new MovieNotFoundException(title);

            return MappedMovie(movie);
        }

        public async Task<MovieEntity> GetMovie(int id)
        {
            var movie = await _dbContext.Movies.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (movie is null) throw new MovieNotFoundException(id);

            return MappedMovie(movie);
        }

        private static MovieEntity MappedMovie(Movie movie)
        {
            return new(
                movie.Id,
                movie.Name,
                movie.Poster,
                movie.DateAdded,
                movie.SeenDate,
                movie.Rates?.Average(r => r.Note)
            );
        }

        public async Task<List<MovieEntity>> GetAll()
        {
            var movies = await _dbContext.Movies
                .Select(m => new MovieEntity(m.Id, m.Name, m.Poster, m.DateAdded, m.SeenDate))
                .ToListAsync();

            if (movies.Count == 0) throw new NoMoviesFoundException();

            return movies;
        }

        public async Task<MovieEntity> Add(MovieCreationDto creationMovie)
        {
            if (TitleExist(creationMovie.Title))
                throw new MovieAlreadyExistException(creationMovie.Title);

            var movie = new Movie()
            {
                Name = creationMovie.Title,
                Poster = creationMovie.Poster,
                DateAdded = DateTime.Now,
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
            var movieToChange = ExistingMovie(changePoster.MovieId);

            if (movieToChange is null) throw new MovieNotFoundException(changePoster.MovieId);

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
