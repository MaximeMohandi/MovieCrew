using Microsoft.AspNetCore.Mvc;

namespace MovieCrew.API.Test.Controller.Ratings
{
    [Route("api/rate")]
    [ApiController]
    public class RateController
    {
        [HttpPost("add")]
        public async Task<ActionResult<string>> RateMovie(int v1, int v2, decimal v3)
        {
            return Ok("");
        }
    }
}