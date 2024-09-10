using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.Users.Exception;

namespace MovieCrew.API.Controller;

[Authorize]
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
            return Conflict(exception.Message);
        }
        catch (UserNotFoundException exception)
        {
            return NotFound(exception.Message);
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
            return NotFound(exception.Message);
        }
    }

    [HttpGet("details")]
    public async Task<ActionResult<MovieDetailsEntity>> GetDetails([FromQuery] int? id = null,
        [FromQuery] string? title = null)
    {
        try
        {
            MovieDetailsEntity result = null;

            if ((id is null && string.IsNullOrEmpty(title)) || (id is not null && !string.IsNullOrEmpty(title)))
                return BadRequest("Please provide a movie id or title.");

            if (id != null) result = await _movieService.GetMovieDetails(id.Value);

            if (!string.IsNullOrEmpty(title)) result = await _movieService.GetMovieDetails(title);

            return Ok(result);
        }
        catch (MovieNotFoundException exception)
        {
            return NotFound(exception.Message);
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

    [HttpPut("{idMovie}/rename")]
    public async Task<ActionResult> PutRenameMovie([FromRoute] int idMovie, [FromQuery] string newTitle)
    {
        try
        {
            await _movieService.ChangeTitle(idMovie, newTitle);
            return Ok();
        }
        catch (Exception e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPut("{idMovie}/poster")]
    public async Task<ActionResult> PutChangePosterMovie([FromRoute] int idMovie, [FromQuery] string newPoster)
    {
        try
        {
            await _movieService.ChangePoster(idMovie, newPoster);
            return Ok();
        }
        catch (MovieNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("refreshAll")]
    public async Task<ActionResult> GetRefreshMovies()
    {
        try
        {
            await _movieService.RefreshMoviesMetaData();
            return Ok();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}
