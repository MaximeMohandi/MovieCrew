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
            return await _movieRepository.Add(movie);
        }

        public async Task Rate(RateCreationDTO rateCreation)
        {
            if (rateCreation.Rate > 10 || rateCreation.Rate < 0)
            {
                throw new RateLimitException(rateCreation.Rate);
            }
            await _rateRepository.Add(rateCreation);
        }

        public async Task ChangeTitle(MovieRenameDTO renameDto)
        {
            await _movieRepository.Update(renameDto);
        }
    }
}
