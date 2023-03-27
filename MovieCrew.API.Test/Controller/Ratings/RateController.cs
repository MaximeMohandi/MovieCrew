using Microsoft.AspNetCore.Mvc;
using MovieCrew.Core.Domain.Ratings.Services;

namespace MovieCrew.API.Test.Controller.Ratings
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
        public async Task<ActionResult<string>> RateMovie([FromBody] CreateRateDto createRate)
        {
            await _ratingService.RateMovie(createRate.IdMovie, createRate.UserId, createRate.Rate);
            return CreatedAtAction("add", createRate.IdMovie);
        }
    }
}