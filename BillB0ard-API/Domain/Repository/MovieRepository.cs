﻿using BillB0ard_API.Data;
using BillB0ard_API.Domain.Entities;
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
            var movieEntity = new MovieEntity(movie.Id, movie.Name, movie.Poster, movie.DateAdded, movie.SeenDate);
            if (movie.Rates != null)
                movieEntity.Rates = movie.Rates.Select(r => new RateEntity(movieEntity, new(r.UserId, r.User.Name), r.Note)).ToList();

            return movieEntity;
        }

        public async Task<MovieEntity> GetMovie(int id)
        {
            var movie = await _dbContext.Movies.Where(m => m.Id == id).FirstOrDefaultAsync();
            var movieEntity = new MovieEntity(movie.Id, movie.Name, movie.Poster, movie.DateAdded, movie.SeenDate);
            if (movie.Rates != null)
                movieEntity.Rates = movie.Rates.Select(r => new RateEntity(movieEntity, new(r.UserId, r.User.Name), r.Note)).ToList();

            return movieEntity;
        }

        public async Task<List<MovieEntity>> GetAll()
        {
            return await _dbContext.Movies
                .Select(m => new MovieEntity(m.Id, m.Name, m.Poster, m.DateAdded, m.SeenDate))
                .ToListAsync();
        }
    }
}
