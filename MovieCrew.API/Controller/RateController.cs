using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Ratings.Exception;
using MovieCrew.Core.Domain.Ratings.Services;

namespace MovieCrew.API.Controller;

[Authorize]
[Route("api/rate")]
[ApiController]
public class RateController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RateController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpPost("add")]
    public async Task<ActionResult<string>> Post([FromBody] CreateRateDto createRate)
    {
        try
        {
            await _ratingService.RateMovie(createRate.IdMovie, createRate.UserId, createRate.Rate);
            return CreatedAtAction("Post", createRate.IdMovie);
        }
        catch (RateLimitException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
