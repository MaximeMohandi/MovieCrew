using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Authentication.Model;
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
    }

    [Test]
    public async Task SuccessLoginShouldReturnAuthenticatedUser()
    {
        // Arrange
        _authRepositoryMock.Setup(x => x.IsUserExist(1, "test"))
            .ReturnsAsync(true);

        // Act
        var actual = (await _authenticationController.Get(new UserLoginDto(1, "test"))).Result as ObjectResult;
        var actualAuthenticatedUser = actual.Value as AuthenticatedUser;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actualAuthenticatedUser.UserName, Is.EqualTo("test"));
            Assert.That(actualAuthenticatedUser.UserId, Is.EqualTo(1));
            Assert.That(actualAuthenticatedUser.Token, Does.Match(IsCorrectToken));
            Assert.That(actualAuthenticatedUser.TokenExpirationDate.ToShortTimeString(),
                Is.EqualTo(DateTime.UtcNow.AddDays(1).ToShortTimeString()));
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        });
    }

    [Test]
    public async Task ShouldReturnForbiddenWhenInvalidUser()
    {
        _authRepositoryMock.Setup(x => x.IsUserExist(2, "test"))
            .ReturnsAsync(false);

        var actual = (await _authenticationController.Get(new UserLoginDto(2, "test"))).Result as ObjectResult;

        Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status403Forbidden));
    }
}
