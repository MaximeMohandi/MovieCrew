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
    private Mock<IAuthenticationRepository> _authRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _authRepositoryMock = new Mock<IAuthenticationRepository>();
    }

    [Test]
    public async Task Login()
    {
        _authRepositoryMock.Setup(x => x.IsUserExist(1, "test"))
            .ReturnsAsync(true);
        JwtConfiguration jwtConfiguration =
            new("A.ComplexP4ss3@Phrase", "http://test.issuer", "http://test.audience");
        AuthenticationService authenticationService = new(_authRepositoryMock.Object, jwtConfiguration);
        AuthenticationController authenticationController = new(authenticationService);

        var actual = (await authenticationController.Get(new UserLoginDto(1, "test"))).Result as ObjectResult;
        var actualAuthenticatedUser = actual.Value as AuthenticatedUser;

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
    public async Task LoginFailInvalidUserReturn403ForbiddenError()
    {
        _authRepositoryMock.Setup(x => x.IsUserExist(1, "test"))
            .ReturnsAsync(true);
        JwtConfiguration jwtConfiguration =
            new("A.ComplexP4ss3@Phrase", "http://test.issuer", "http://test.audience");
        AuthenticationService authenticationService = new(_authRepositoryMock.Object, jwtConfiguration);
        AuthenticationController authenticationController = new(authenticationService);

        var actual = (await authenticationController.Get(new UserLoginDto(2, "test"))).Result as ObjectResult;

        Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status403Forbidden));
    }
}
