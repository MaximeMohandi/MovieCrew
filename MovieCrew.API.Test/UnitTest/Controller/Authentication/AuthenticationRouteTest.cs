using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Authentication.Model;
using MovieCrew.Core.Domain.Authentication.Repository;
using MovieCrew.Core.Domain.Authentication.Services;

namespace MovieCrew.API.Test.UnitTest.Controller.Authentication;

public class AuthenticationRouteTest
{
    private const string IsCorrectToken = @"^([a-zA-Z0-9_-]+\.){2}[a-zA-Z0-9_-]+$";
    private AuthenticationController _authenticationController;
    private Mock<IAuthenticationRepository> _authRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _authRepositoryMock = new Mock<IAuthenticationRepository>();
        var jwtConfiguration =
            new JwtConfiguration("A.ComplexP4ss3@Phrase", "http://test.issuer", "http://test.audience");
        var authenticationService = new AuthenticationService(_authRepositoryMock.Object, jwtConfiguration);
        _authenticationController = new AuthenticationController(authenticationService);
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(x => x.Request.Headers["ApiKey"]).Returns("test");
        _authenticationController.ControllerContext.HttpContext = httpContextMock.Object;
    }

    [Test]
    public async Task SuccessAuthenticationShouldReturnToken()
    {
        // Arrange
        _authRepositoryMock.Setup(x => x.IsClientValid(1, "test"))
            .ReturnsAsync(true);

        // Act
        var actual = (await _authenticationController.Get(1)).Result as ObjectResult;
        var authenticatedClient = actual?.Value as AuthenticatedClient;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual?.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(authenticatedClient?.Token, Does.Match(IsCorrectToken));
            Assert.That(authenticatedClient?.TokenExpirationDate.ToShortTimeString(),
                Is.EqualTo(DateTime.UtcNow.AddDays(1).ToShortTimeString()));
        });
    }

    [Test]
    public async Task ShouldReturnForbiddenWhenInvalidClientButCorrectKey()
    {
        _authRepositoryMock.Setup(x => x.IsClientValid(2, "test"))
            .ReturnsAsync(false);

        var actual = (await _authenticationController.Get(2)).Result as ObjectResult;

        Assert.That(actual?.StatusCode, Is.EqualTo(StatusCodes.Status403Forbidden));
    }

    [Test]
    public async Task ShouldReturnForbiddenWhenValidClientButInvalidKey()
    {
        _authRepositoryMock.Setup(x => x.IsClientValid(2, "test"))
            .ReturnsAsync(false);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(x => x.Request.Headers["ApiKey"]).Returns("false key");
        _authenticationController.ControllerContext.HttpContext = httpContextMock.Object;

        var actual = (await _authenticationController.Get(1)).Result as ObjectResult;

        Assert.That(actual?.StatusCode, Is.EqualTo(StatusCodes.Status403Forbidden));
    }
}