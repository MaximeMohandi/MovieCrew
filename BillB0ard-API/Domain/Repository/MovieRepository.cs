using BillB0ard_API.Data;
using BillB0ard_API.Domain.Entities;

namespace BillB0ard_API.Domain.Repository
{
    public class MovieRepository
    {
        private AppDbContext _dbContext;
        public MovieRepository(AppDbContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task<Movie> GetMovie(string title)
        {
            return _dbContext.Movies.Where(m => m.Name.ToLower() == title.ToLower()).Select(m => new Movie(m.Id, m.Name, m.Poster, m.DateAdded, m.SeenDate)).First();
        }
    }
}
