using System.Net.Mime;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;
using MovieCrew.Core.Domain.Users.Exception;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.API.Test.Integration.Users;

public class LoginEndpointTest
{
    private readonly JsonSerializerOptions _jsonOptions;
    private HttpClient _client;
    private Mock<IUserService> _userService;

    public LoginEndpointTest()
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
    public async Task ShouldReturnOkWithUser()
    {
        // Arrange
        _userService.Setup(x => x.GetUser(1234, "test"))
            .ReturnsAsync(new UserEntity(1234, "test", UserRoles.User));
        var expected = JsonSerializer.Serialize(new UserEntity(1234, "test", UserRoles.User), _jsonOptions);

        // Act
        var response = await _client.SendAsync(LoginRequest(new UserLoginDto(1234, "test")));
        var responseContent = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(responseContent.ToLower(), Is.EqualTo(expected.ToLower()));
        });
    }

    [Test]
    public async Task ShouldReturnNotFoundWhenNoUserFound()
    {
        // Arrange
        _userService.Setup(x => x.GetUser(1234, "test"))
            .ThrowsAsync(new UserNotFoundException("test"));

        // Act
        var response = await _client.SendAsync(LoginRequest(new UserLoginDto(1234, "test")));

        //Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }

    private HttpRequestMessage LoginRequest(UserLoginDto loginBody)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(_client.BaseAddress.AbsoluteUri + "api/user/login"),
            Content = new StringContent(JsonSerializer.Serialize(loginBody, _jsonOptions), Encoding.UTF8,
                MediaTypeNames.Application.Json)
        };
        return request;
    }
}
