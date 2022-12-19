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

        public async Task<MovieEntity> AddMovie(MovieCreationDto movie)
        {
            return await _movieRepository.Add(movie);
        }

        public async Task Rate(RateCreationDto rateCreation)
        {
            if (rateCreation.Rate > 10 || rateCreation.Rate < 0)
            {
                throw new RateLimitException(rateCreation.Rate);
            }
            await _rateRepository.Add(rateCreation);
            await this.SetSeenDate(new(rateCreation.MovieID, DateTime.Now));
        }

        public async Task ChangeTitle(MovieRenameDto renameDto)
        {
            await _movieRepository.Update(renameDto);
        }

        public async Task AddPoster(MovieChangePosterDto changePoster)
        {
            if (!IsValidUrl(changePoster))
            {
                throw new MoviePosterFormatException();
            }
            await _movieRepository.Update(changePoster);

        }

        private static bool IsValidUrl(MovieChangePosterDto changePoster)
        {
            return Uri.IsWellFormedUriString(changePoster.NewPosterLink, UriKind.Absolute) && changePoster.NewPosterLink.StartsWith("http");
        }

        public async Task SetSeenDate(MovieSetSeenDateDto movieSetSeenDateDTO)
        {
            await _movieRepository.Update(movieSetSeenDateDTO);
        }

        public async Task<MovieEntity> RandomMovie()
        {
            Random rand = new();

            var unseenMovies = await _movieRepository.GetAllUnSeen();

            if (unseenMovies is null || unseenMovies.Count == 0)
            {
                throw new AllMoviesHaveBeenSeenException();
            }

            int randomIndex = rand.Next(unseenMovies.Count);

            return unseenMovies[randomIndex];


        }
    }
}
