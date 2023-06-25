using Microsoft.AspNetCore.Mvc;
using MovieCrew.Core.Domain.Users.Dtos;
using MovieCrew.Core.Domain.Users.Exception;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.API.Controller;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<string>> Post([FromBody] UserCreationDto userCreationDto)
    {
        try
        {
            await _userService.AddUser(userCreationDto.Name, userCreationDto.Role);
            return CreatedAtAction("Post", userCreationDto.Name);
        }
        catch (UserAlreadyExistException e)
        {
            return Conflict(e.Message);
        }
        catch (UserRoleDoNotExistException e)
        {
            return BadRequest(e.Message);
        }
    }
}
