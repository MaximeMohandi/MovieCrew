using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Users.Dtos;
using MovieCrew.Core.Domain.Users.Enums;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.API.Test.UnitTest.Controller.Users;

public class RegisterUserRoute
{
    [Test]
    public async Task ShouldReturn201WhenUserIsRegistered()
    {
        // Arrange
        var serviceMock = new Mock<IUserService>();
        var controller = new UserController(serviceMock.Object);

        // Act
        var actual = (await controller.Post(new UserCreationDto("John", UserRoles.Admin))).Result as ObjectResult;

        // Assert
        Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
    }
}
