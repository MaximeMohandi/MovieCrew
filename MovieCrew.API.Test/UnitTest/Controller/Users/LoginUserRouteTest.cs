using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;
using MovieCrew.Core.Domain.Users.Exception;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.API.Test.UnitTest.Controller.Users;

public class LoginUserRouteTest
{
    [Test]
    public async Task ShouldReturn200WithLoggedUser()
    {
        // Arrange
        var serviceMock = new Mock<IUserService>();
        serviceMock.Setup(x => x.GetUser(1234579, "a_personne"))
            .ReturnsAsync(new UserEntity(1234579, "a_personne", UserRoles.User));
        var controller = new UserController(serviceMock.Object);

        // Act
        var actual = (await controller.Get(new UserLoginDto(1234579, "a_personne"))).Result as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(actual.Value, Is.EqualTo(new UserEntity(1234579, "a_personne", UserRoles.User)));
        });
    }

    [Test]
    public async Task ShouldReturn404WhenUserIsNotFound()
    {
        // Arrange
        var serviceMock = new Mock<IUserService>();
        serviceMock.Setup(x => x.GetUser(It.IsAny<long>(), It.IsAny<string>()))
            .ThrowsAsync(new UserNotFoundException("a_personne"));
        var controller = new UserController(serviceMock.Object);

        // Act
        var actual = (await controller.Get(new UserLoginDto(1234579, "a_personne"))).Result as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(actual.Value,
                Is.EqualTo("User a_personne not found. Please check the conformity and try again"));
        });
    }
}
