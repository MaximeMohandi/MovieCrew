using BillB0ard_API.Domain.Movies.Dtos;
using BillB0ard_API.Domain.Movies.Entities;
using BillB0ard_API.Domain.Movies.Exception;
using BillB0ard_API.Domain.Movies.Repository;

namespace BillB0ard_API.Domain.Movies.Services
{
    public class MovieService
    {
        private readonly MovieRepository _movieRepository;

        public MovieService(MovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<List<MovieEntity>> FetchAllMovies()
        {
            return await _movieRepository.GetAll();
        }

        public async Task<MovieDetailsEntity> GetByTitle(string title)
        {
            return await _movieRepository.GetMovie(title);
        }

        public async Task<MovieDetailsEntity> GetById(int movieId)
        {
            return await _movieRepository.GetMovie(movieId);
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

        public async Task<MovieEntity> AddMovie(MovieCreationDto movie)
        {
            return await _movieRepository.Add(movie);
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

    }
}
