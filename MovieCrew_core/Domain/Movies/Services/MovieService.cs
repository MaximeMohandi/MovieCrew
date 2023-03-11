using MovieCrew.Core.Domain.Movies.Dtos;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Interfaces;
using MovieCrew.Core.Domain.Movies.Repository;

namespace MovieCrew.Core.Domain.Movies.Services
{
    public class MovieService
    {
        private readonly MovieRepository _movieRepository;
        private readonly IThirdPartyMovieDataProvider _thirdPartyMovieProvider;

        public MovieService(MovieRepository movieRepository, IThirdPartyMovieDataProvider thirdPartyMovieProvider)
        {
            _movieRepository = movieRepository;
            _thirdPartyMovieProvider = thirdPartyMovieProvider;
        }

        public async Task<List<MovieEntity>> FetchAllMovies()
        {
            return await _movieRepository.GetAll();
        }

        public async Task<MovieDetailsEntity> GetByTitle(string title)
        {
            var movie = await _movieRepository.GetMovie(title);
            await AddRevenueAndPeopleRatingsToMovie(movie);
            return movie;
        }

        public async Task<MovieDetailsEntity> GetById(int movieId)
        {
            var movie = await _movieRepository.GetMovie(movieId);
            await AddRevenueAndPeopleRatingsToMovie(movie);
            return movie;
        }

        private async Task AddRevenueAndPeopleRatingsToMovie(MovieDetailsEntity movie)
        {
            var metadata = await _thirdPartyMovieProvider.GetDetails(movie.Title.ToLower());
            movie.PeopleAverageRate = metadata.Ratings;
            movie.Revenue = metadata.Revenue;
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

        public async Task<MovieEntity> AddMovie(string title, long? proposedById)
        {
            try
            {
                var metadata = await _thirdPartyMovieProvider.GetDetails(title);
                return await _movieRepository.Add(new(title, metadata.PosterLink, metadata.Description, proposedById));
            }
            catch (NoMetaDataFound)
            {
                throw new MovieNotFoundException(title);
            }
        }

        public async Task ChangeTitle(MovieRenameDto renameDto)
        {
            await _movieRepository.Update(renameDto);
        }

        public async Task SetSeenDate(MovieSetSeenDateDto movieSetSeenDateDTO)
        {
            await _movieRepository.Update(movieSetSeenDateDTO);
        }
    }
}
