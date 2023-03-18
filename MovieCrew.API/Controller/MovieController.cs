﻿using Microsoft.AspNetCore.Mvc;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.Users.Exception;

namespace MovieCrew.API.Controller
{
    [Route("api/movie")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _movieService;
        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<string>> Post([FromBody] NewMovieDto newMovie)
        {
            try
            {
                var addedMovie = await _movieService.AddMovie(newMovie.Title, newMovie.ProposedBy);

                return CreatedAtAction("add", addedMovie.Id);

            }
            catch (MovieAlreadyExistException exception)
            {
                return Conflict(exception);
            }
            catch (UserNotFoundException exception)
            {
                return NotFound(exception);
            }
        }
    }
}