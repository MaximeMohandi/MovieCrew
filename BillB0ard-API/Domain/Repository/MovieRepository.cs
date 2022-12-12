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
        private AppDbContext _dbContext;
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

        private MovieEntity MappedMovie(Movie movie)
        {
            var movieEntity = new MovieEntity(movie.Id, movie.Name, movie.Poster, movie.DateAdded, movie.SeenDate);

            return new(movie.Id, movie.Name, movie.Poster, movie.DateAdded, movie.SeenDate)
            {
                Rates = movie.Rates?.Select(r => new RateEntity(movieEntity, new(r.UserId, r.User.Name), r.Note)).ToList()
            };
        }

        public async Task<List<MovieEntity>> GetAll()
        {
            return await _dbContext.Movies
                .Select(m => new MovieEntity(m.Id, m.Name, m.Poster, m.DateAdded, m.SeenDate))
                .ToListAsync();
        }

        public async Task<MovieEntity> Add(MovieCreationDTO creationMovie)
        {
            if (MovieAlreadyExist(creationMovie.Title))
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

        private bool MovieAlreadyExist(string title)
        {
            return _dbContext.Movies.Any(m => m.Name.ToLower() == title.ToLower());
        }

        public async Task Update(MovieRenameDTO renameDto)
        {
            var movieToRename = _dbContext.Movies.FirstOrDefault(m => m.Id == renameDto.MovieID);

            if (movieToRename is null) throw new MovieNotFoundException(renameDto.MovieTitle);

            if (MovieAlreadyExist(renameDto.NewTitle))
            {
                throw new MovieAlreadyExistException(renameDto.NewTitle);
            }

            movieToRename.Name = renameDto.NewTitle;

            _dbContext.Movies.Update(movieToRename);

            await _dbContext.SaveChangesAsync();
        }
    }
}
