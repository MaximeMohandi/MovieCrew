using BillB0ard_API.Domain.DTOs;
using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Exception;
using BillB0ard_API.Domain.Repository;

namespace BillB0ard_API.Services
{
    public class MovieService
    {
        private readonly MovieRepository _movieRepository;
        private readonly RateRepository _rateRepository;

        public MovieService(MovieRepository movieRepository, RateRepository rateRepository)
        {
            this._movieRepository = movieRepository;
            this._rateRepository = rateRepository;
        }

        public async Task<List<MovieEntity>> FetchAllMovies()
        {
            return await _movieRepository.GetAll();
        }

        public async Task<MovieEntity> GetByTitle(string title)
        {
            return await _movieRepository.GetMovie(title);
        }

        public async Task<MovieEntity> GetById(int movieId)
        {
            return await _movieRepository.GetMovie(movieId);
        }

        public async Task<MovieEntity> AddMovie(MovieCreationDTO movie)
        {
            try
            {
                var existMovie = await GetByTitle(movie.Title);
                throw new MovieAlreadyExistException(movie.Title);
            }
            catch (MovieNotFoundException)
            {
                return await _movieRepository.Add(movie);
            }

        }

        public async Task Rate(RateCreationDTO rateCreation)
        {
            await _rateRepository.Add(rateCreation);
        }
    }
}
