﻿using MovieCrew.Core.Domain.Movies.Interfaces;
using MovieCrew.Core.Domain.ThirdPartyMovieProvider.Exception;
using MovieCrew_core.Domain.Movies.Dtos;
using MovieCrew_core.Domain.Movies.Entities;
using MovieCrew_core.Domain.Movies.Exception;
using MovieCrew_core.Domain.Movies.Repository;

namespace MovieCrew_core.Domain.Movies.Services
{
    public class MovieService
    {
        private readonly MovieRepository _movieRepository;
        private readonly IThirdPartyMovieDataProvider? _thirdPartyMovieProvider;

        public MovieService(MovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

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
            try
            {
                var metadata = await _thirdPartyMovieProvider.GetDetails(movie.Title);
                return await _movieRepository.Add(new(movie.Title, metadata.PosterLink, movie.proposedById));
            }
            catch (NoMetaDataFound)
            {
                throw new MovieNotFoundException(movie.Title);
            }
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
