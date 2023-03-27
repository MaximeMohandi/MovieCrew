using Microsoft.AspNetCore.Mvc;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Ratings.Exception;
using MovieCrew.Core.Domain.Ratings.Services;

namespace MovieCrew.API.Controller
{
    [Route("api/rate")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private RatingServices _ratingService;

        public RateController(RatingServices ratingServices)
        {
            _ratingService = ratingServices;
        }

        [HttpPost("add")]
        public async Task<ActionResult<string>> Post([FromBody] CreateRateDto createRate)
        {
            try
            {
                await _ratingService.RateMovie(createRate.IdMovie, createRate.UserId, createRate.Rate);
                return CreatedAtAction("add", createRate.IdMovie);
            }
            catch (RateLimitException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}