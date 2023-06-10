using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Moq;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Authentication.Model;
using MovieCrew.Core.Domain.Authentication.Services;

namespace MovieCrew.API.Test.Integration.Authentication;

public class LoginEndpointsTest
{
    private readonly JsonSerializerOptions _jsonOptions;
    private Mock<IAuthenticationService> _authenticationService;
    private HttpClient _client;

    public LoginEndpointsTest()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    [SetUp]
    public void SetUp()
    {
        _authenticationService = new Mock<IAuthenticationService>();
        _client = new IntegrationTestServer<IAuthenticationService>(_authenticationService).CreateDefaultClient();
    }

    [Test]
    public async Task LoginEndpointShouldReturnConnectedUser()
    {
        // Arrange
        var expected = new AuthenticatedUser(1, "Maxime", FakeToken(), DateTime.Now.AddDays(1));
        var expectedJson = JsonSerializer.Serialize(expected, _jsonOptions);
        _authenticationService.Setup(x => x.Authenticate(It.IsAny<long>(), It.IsAny<string>()))
            .ReturnsAsync(expected);

        // Act
        var response = await _client.PostAsJsonAsync("/api/authentication/login",
            new UserLoginDto(1, "Maxime"));
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(responseContent.ToLower(), Is.EqualTo(expectedJson.ToLower()));
        });
    }

    private static string FakeToken()
    {
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken("", "", new List<Claim>(),
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fake-key-sdqfsdklfsqdlfjkdskmfjl")),
                SecurityAlgorithms.HmacSha256)));
    }

    [Test]
    public async Task LoginEndpointShouldReturnForbiddenWhenUserIsNotConnected()
    {
        // Arrange
        _authenticationService.Setup(x => x.Authenticate(It.IsAny<long>(), It.IsAny<string>()))
            .ThrowsAsync(new AuthenticationException("Invalid User"));

        // Act
        var response = await _client.PostAsJsonAsync("/api/authentication/login",
            new UserLoginDto(1, "Maxime"));

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status403Forbidden));
    }
}
