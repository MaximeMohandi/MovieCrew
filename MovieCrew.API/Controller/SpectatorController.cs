using Microsoft.AspNetCore.Mvc;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Exception;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.API.Controller;

[Route("api/spectator")]
[ApiController]
public class SpectatorController : ControllerBase
{
    private readonly ISpectatorService _spectatorService;

    public SpectatorController(ISpectatorService spectatorService)
    {
        _spectatorService = spectatorService;
    }

    [HttpGet("{userId}/details")]
    public async Task<ActionResult<SpectatorDetailsEntity>> Get([FromRoute] long userId)
    {
        try
        {
            var spectatorDetails = await _spectatorService.FetchSpectator(userId);
            return Ok(spectatorDetails);
        }
        catch (UserException ex)
        {
            if (ex.GetType() == typeof(UserNotFoundException)) return NotFound(ex.Message);

            return BadRequest(ex.Message);
        }
    }
}
