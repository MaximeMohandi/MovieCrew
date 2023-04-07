using Microsoft.AspNetCore.Mvc;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Exception;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.API.Controller
{
    [Route("api/spectator")]
    [ApiController]
    public class SpectatorController : ControllerBase
    {
        private readonly SpectatorService _spectatorService;

        public SpectatorController(SpectatorService spectatorService)
        {
            _spectatorService = spectatorService;
        }

        [HttpGet("/{userId}/details")]
        public async Task<ActionResult<SpectatorDetailsEntity>> Get()
        {
            try
            {

                var spectatorDetails = await _spectatorService.FetchSpectator(1);
                return Ok(spectatorDetails);
            }
            catch (UserException ex)
            {
                if (ex.GetType() == typeof(UserNotFoundException))
                {
                    return NotFound(ex);
                }

                return BadRequest(ex);
            }
        }
    }
}