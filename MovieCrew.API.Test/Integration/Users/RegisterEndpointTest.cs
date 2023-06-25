using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Users.Enums;
using MovieCrew.Core.Domain.Users.Exception;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.API.Test.Integration.Users;

public class RegisterEndpointTest
{
    private readonly JsonSerializerOptions _jsonOptions;
    private HttpClient _client;
    private Mock<IUserService> _userService;

    public RegisterEndpointTest()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    [SetUp]
    public void SetUp()
    {
        _userService = new Mock<IUserService>();
        _client = new IntegrationTestServer<IUserService>(_userService).CreateDefaultClient();
    }

    [Test]
    public async Task ShouldReturnCreated()
    {
        // Arrange
        _userService.Setup(x => x.AddUser(It.IsAny<string>(), It.IsAny<UserRoles>()))
            .Verifiable();
        var createUser = new UserCreationDto("test", UserRoles.Admin);

        // Act
        var response = await _client.PostAsync("/api/user/register",
            new StringContent(JsonSerializer.Serialize(createUser, _jsonOptions), Encoding.UTF8, "application/json"));

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
    }

    [Test]
    public async Task ShouldReturnConflictWhenUserExist()
    {
        // Arrange
        _userService.Setup(x => x.AddUser(It.IsAny<string>(), It.IsAny<UserRoles>()))
            .ThrowsAsync(new UserAlreadyExistException("test"));
        var createUser = new UserCreationDto("test", UserRoles.Admin);

        // Act
        var response = await _client.PostAsync("/api/user/register",
            new StringContent(JsonSerializer.Serialize(createUser, _jsonOptions), Encoding.UTF8, "application/json"));
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
            Assert.That(responseContent.ToLower(),
                Is.EqualTo("the user test already exist. please verify the name and try again"));
        });
    }

    [Test]
    public async Task ShouldReturnBadRequestWhenInvalidRole()
    {
        // Arrange
        _userService.Setup(x => x.AddUser(It.IsAny<string>(), It.IsAny<UserRoles>()))
            .ThrowsAsync(new UserRoleDoNotExistException("10"));
        var createUser = new UserCreationDto("test", (UserRoles)10);

        // Act
        var response = await _client.PostAsync("/api/user/register",
            new StringContent(JsonSerializer.Serialize(createUser, _jsonOptions), Encoding.UTF8, "application/json"));
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(responseContent.ToLower(),
                Is.EqualTo("the user role 10 do not exist. please verify the role and try again"));
        });
    }
}
