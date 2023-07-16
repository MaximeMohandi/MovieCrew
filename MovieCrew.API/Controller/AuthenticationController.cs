using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using MovieCrew.Core.Domain.Authentication.Model;
using MovieCrew.Core.Domain.Authentication.Services;

namespace MovieCrew.API.Controller;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet("token")]
    public async Task<ActionResult<AuthenticatedClient>> Get([FromQuery] int clientId)
    {
        try
        {
            var apiKey = HttpContext.Request.Headers["ApiKey"];
            var authUser = await _authenticationService.Authenticate(clientId, apiKey);
            return Ok(authUser);
        }
        catch (AuthenticationException e)
        {
            return Problem(statusCode: StatusCodes.Status403Forbidden);
        }
    }
}
