using Microsoft.AspNetCore.Mvc;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.Users.Exception;

namespace MovieCrew.API.Controller;

[Route("api/movie")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost("add")]
    public async Task<ActionResult<string>> Post([FromBody] NewMovieDto newMovie)
    {
        try
        {
            var addedMovie = await _movieService.AddMovie(newMovie.Title, newMovie.ProposedBy);

            return CreatedAtAction("Post", addedMovie.Id);
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

    [HttpGet("all")]
    public async Task<ActionResult<List<MovieEntity>>> GetAll()
    {
        try
        {
            var list = await _movieService.FetchAllMovies();
            return Ok(list);
        }
        catch (NoMoviesFoundException exception)
        {
            return NotFound(exception);
        }
    }

    [HttpGet("details")]
    public async Task<ActionResult<MovieDetailsEntity>> GetDetails([FromQuery] int? id = null,
        [FromQuery] string? title = null)
    {
        try
        {
            MovieDetailsEntity result = null;

            if (id is null && string.IsNullOrEmpty(title))
                return BadRequest("Either id or title parameter is required.");

            if (id is not null && !string.IsNullOrEmpty(title))
                return BadRequest("Only one of id or title parameters should be provided.");

            if (id != null) result = await _movieService.GetById(id.Value);

            if (!string.IsNullOrEmpty(title)) result = await _movieService.GetByTitle(title);

            return Ok(result);
        }
        catch (MovieNotFoundException exception)
        {
            return NotFound(exception);
        }
    }

    [HttpGet("random")]
    public async Task<ActionResult<MovieEntity>> GetRandomUnseenMovie()
    {
        try
        {
            var movie = await _movieService.RandomMovie();
            return Ok(movie);
        }
        catch (AllMoviesHaveBeenSeenException)
        {
            return NoContent();
        }
    }
}
