using BillB0ard_API.Data;
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
            return await _dbContext.Movies
                    .Where(m => m.Name.ToLower() == title.ToLower())
                    .Select(m => new MovieEntity(m.Id, m.Name, m.Poster, m.DateAdded, m.SeenDate, null))
                    .FirstOrDefaultAsync();
        }

        public async Task<MovieEntity> GetMovie(int id)
        {
            return await _dbContext.Movies
                .Where(m => m.Id == id)
                .Select(m => new MovieEntity(m.Id, m.Name, m.Poster, m.DateAdded, m.SeenDate, null))
                .FirstOrDefaultAsync();
        }

        public async Task<List<MovieEntity>> GetAll()
        {
            return await _dbContext.Movies
                .Select(m => new MovieEntity(m.Id, m.Name, m.Poster, m.DateAdded, m.SeenDate, null))
                .ToListAsync();
        }
    }
}
