using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Repository;

namespace BillB0ard_API.Services
{
    public class MovieService
    {
        private readonly MovieRepository _repository;

        public MovieService(MovieRepository repository)
        {
            this._repository = repository;
        }

        public async Task<List<MovieEntity>> FetchAllMovies()
        {
            return await _repository.GetAll();
        }

        public async Task<MovieEntity> GetByTitle(string title)
        {
            return await _repository.GetMovie(title);
        }

        public async Task<MovieEntity> GetById(int movieId)
        {
            return await _repository.GetMovie(movieId);
        }
    }
}
