using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Authentication.Model;
using MovieCrew.Core.Domain.Authentication.Services;

namespace MovieCrew.API.Controller;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly AuthenticationService _authenticationService;

    public AuthenticationController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet("Login")]
    public async Task<ActionResult<AuthenticatedUser>> Get([FromBody] UserLoginDto userLoginDto)
    {
        try
        {
            var authUser = await _authenticationService.Authenticate(userLoginDto.UserId, userLoginDto.UserName);
            return Ok(authUser);
        }
        catch (AuthenticationException e)
        {
            return Problem(e.Message, statusCode: StatusCodes.Status403Forbidden);
        }
    }
}