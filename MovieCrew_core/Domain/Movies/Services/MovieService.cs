﻿using MovieCrew.Core.Domain.Movies.Entities;
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
        await AddExtraDataToMovie(movie);
        return movie;
    }

    public async Task<MovieDetailsEntity> GetMovieDetails(int movieId)
    {
        var movie = await _movieRepository.GetMovie(movieId);
        await AddExtraDataToMovie(movie);
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

    public async Task ChangeTitle(int movieId, string newTitle)
    {
        await _movieRepository.UpdateTitle(movieId, newTitle);
    }

    public async Task ChangePoster(int movieId, string newPoster)
    {
        await _movieRepository.UpdatePoster(movieId, newPoster);
    }

    public async Task RefreshMoviesMetaData()
    {
        foreach (var movie in await FetchAllMovies())
        {
            MovieMetadataEntity? metadata = null;
            if (movie.Description == string.Empty)
            {
                metadata = await _thirdPartyMovieProvider.GetDetails(movie.Title.ToLower());
                await _movieRepository.UpdateDescription(movie.Id, metadata.Description);
            }

            if (movie.Poster == string.Empty)
            {
                metadata ??= await _thirdPartyMovieProvider.GetDetails(movie.Title.ToLower());
                await _movieRepository.UpdatePoster(movie.Id, metadata.PosterLink);
            }
        }
    }

    private async Task AddExtraDataToMovie(MovieDetailsEntity movie)
    {
        var metadata = await _thirdPartyMovieProvider.GetDetails(movie.Title.ToLower());
        movie.PeopleAverageRate = metadata.Ratings;
        movie.Revenue = metadata.Revenue;
        movie.Budget = metadata.Budget;
    }
}
