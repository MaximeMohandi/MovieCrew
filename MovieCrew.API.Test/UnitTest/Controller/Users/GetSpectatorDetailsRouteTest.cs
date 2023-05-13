using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Exception;
using MovieCrew.Core.Domain.Users.Repository;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.API.Test.UnitTest.Controller.Users;

public class GetSpectatorDetailsRouteTest
{
    private SpectatorService _service;
    private Mock<IUserRepository> _userRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _service = new SpectatorService(_userRepositoryMock.Object);
    }

    [Test]
    public async Task GetSpectatorDetails()
    {
        var controller = new SpectatorController(_service);
        _userRepositoryMock.Setup(x => x.GetSpectatorDetails(It.IsAny<long>()))
            .ReturnsAsync(new SpectatorDetailsEntity(new UserEntity(1, "Maxime", 2), new List<SpectatorRateEntity>
            {
                new(
                    new MovieEntity(1, "John Wick", "http://Poster.test", "a movie", new DateTime(2023, 3, 2),
                        new DateTime(2023, 4, 2), 4.00M), 3.00M),
                new(
                    new MovieEntity(1, "Troy", "http://Poster.test", "a movie", new DateTime(2022, 1, 25),
                        new DateTime(2023, 2, 2), 6.0M), 9.00M)
            }));

        SpectatorDetailsEntity expected = new(new UserEntity(1, "Maxime", 2), new List<SpectatorRateEntity>
        {
            new(
                new MovieEntity(1, "John Wick", "http://Poster.test", "a movie", new DateTime(2023, 3, 2),
                    new DateTime(2023, 4, 2), 4.00M), 3.00M),
            new(
                new MovieEntity(1, "Troy", "http://Poster.test", "a movie", new DateTime(2022, 1, 25),
                    new DateTime(2023, 2, 2), 6.0M), 9.00M)
        });

        var actual = (await controller.Get()).Result as ObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(actual.Value, Is.EqualTo(expected));
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        });
    }

    [Test]
    public async Task Error404WhenNoSpectatorFound()
    {
        var controller = new SpectatorController(_service);
        _userRepositoryMock.Setup(x => x.GetSpectatorDetails(It.IsAny<long>()))
            .ThrowsAsync(new UserNotFoundException(1));

        var actual = (await controller.Get()).Result as ObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(actual.Value, Is.TypeOf<UserNotFoundException>());
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        });
    }

    [Test]
    public async Task Error400WhenNoSpectatorFound()
    {
        var controller = new SpectatorController(_service);
        _userRepositoryMock.Setup(x => x.GetSpectatorDetails(It.IsAny<long>()))
            .ThrowsAsync(new UserIsNotSpectatorException(1));

        var actual = (await controller.Get()).Result as ObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(actual.Value, Is.TypeOf<UserIsNotSpectatorException>());
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        });
    }
}