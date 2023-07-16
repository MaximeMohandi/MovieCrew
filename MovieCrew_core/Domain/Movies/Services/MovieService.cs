using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Repository;
using MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Exception;
using MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Services;

namespace MovieCrew.Core.Domain.Movies.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IThirdPartyMovieDataProvider _thirdPartyMovieProvider;

    public MovieService(IMovieRepository movieRepository, IThirdPartyMovieDataProvider thirdPartyMovieProvider)
    {
        _movieRepository = movieRepository;
        _thirdPartyMovieProvider = thirdPartyMovieProvider;
    }

    public async Task<List<MovieEntity>> FetchAllMovies()
    {
        return await _movieRepository.GetAll();
    }

    public async Task<MovieDetailsEntity> GetMovieDetails(string title)
    {
        var movie = await _movieRepository.GetMovie(title);
        await AddRevenueAndPeopleRatingsToMovie(movie);
        return movie;
    }

    public async Task<MovieDetailsEntity> GetMovieDetails(int movieId)
    {
        var movie = await _movieRepository.GetMovie(movieId);
        await AddRevenueAndPeopleRatingsToMovie(movie);
        return movie;
    }

    public async Task<MovieEntity> RandomMovie()
    {
        Random rand = new();

        var unseenMovies = await _movieRepository.GetAllUnSeen();

        if (unseenMovies is null || unseenMovies.Count == 0) throw new AllMoviesHaveBeenSeenException();

        var randomIndex = rand.Next(unseenMovies.Count);

        return unseenMovies[randomIndex];
    }

    public async Task<MovieEntity> AddMovie(string title, long? proposedById)
    {
        try
        {
            var metadata = await _thirdPartyMovieProvider.GetDetails(title);
            return await _movieRepository.Add(title, metadata.PosterLink, metadata.Description,
                proposedById);
        }
        catch (NoMetaDataFoundException)
        {
            throw new MovieNotFoundException(title);
        }
    }

    public async Task ChangeTitle(int movieId, string currentTitle, string newTitle)
    {
        await _movieRepository.Update(movieId, currentTitle, newTitle);
    }

    public async Task SetSeenDate(int movieId, DateTime seenDate)
    {
        await _movieRepository.Update(movieId, seenDate);
    }

    private async Task AddRevenueAndPeopleRatingsToMovie(MovieDetailsEntity movie)
    {
        var metadata = await _thirdPartyMovieProvider.GetDetails(movie.Title.ToLower());
        movie.PeopleAverageRate = metadata.Ratings;
        movie.Revenue = metadata.Revenue;
    }
}
