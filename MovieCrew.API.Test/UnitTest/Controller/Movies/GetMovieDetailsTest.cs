using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;

namespace MovieCrew.API.Test.UnitTest.Controller.Movies;

public class GetMovieDetailsTest : MovieTestBase
{
    [Test]
    public async Task GetMovieDetailsByIdShouldReturnMovieDetails()
    {
        // Arrange
        _movieDataProviderMock
            .Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("https://titanic", "loremp ipsum", 8M, 340000, 0));
        _movieRepositoryMock
            .Setup(x => x.GetMovie(It.IsAny<int>()))
            .ReturnsAsync(new MovieDetailsEntity(1,
                "Titanic",
                "http://titanic",
                "loremp ipsum",
                new DateTime(2023, 3, 12),
                null,
                null,
                null,
                null, null,
                null,
                new UserEntity(1, "Maxime", UserRoles.Admin)));

        // Act
        var actual = (await _controller.GetDetails(1)).Result as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.Value, Is.EqualTo(new MovieDetailsEntity(1,
                "Titanic",
                "http://titanic",
                "loremp ipsum",
                new DateTime(2023, 3, 12),
                null,
                null,
                8M,
                340000, 0,
                null,
                new UserEntity(1, "Maxime", UserRoles.Admin))));
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        });
    }

    [Test]
    public async Task GetMovieDetailByTitleShouldReturnMovieDetails()
    {
        // Arrange
        _movieDataProviderMock
            .Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("https://titanic", "loremp ipsum", 8M, 340000, 0));
        _movieRepositoryMock
            .Setup(x => x.GetMovie(It.IsAny<string>()))
            .ReturnsAsync(new MovieDetailsEntity(1,
                "Titanic",
                "http://titanic",
                "loremp ipsum",
                new DateTime(2023, 3, 12),
                new DateTime(2023, 3, 18),
                2M,
                null,
                null, null,
                new List<MovieRateEntity>
                {
                    new(new UserEntity(1, "user", 2), 2)
                },
                new UserEntity(1, "Maxime", UserRoles.Admin)));

        // Act
        var actual = (await _controller.GetDetails(title: "Titanic")).Result as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.Value, Is.EqualTo(new MovieDetailsEntity(1,
                "Titanic",
                "http://titanic",
                "loremp ipsum",
                new DateTime(2023, 3, 12),
                new DateTime(2023, 3, 18),
                2M,
                8M,
                340000, 0,
                new List<MovieRateEntity>
                {
                    new(new UserEntity(1, "user", 2), 2)
                },
                new UserEntity(1, "Maxime", UserRoles.Admin))));
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        });
    }

    [Test]
    public async Task ShouldReturn404WhenMovieNotFound()
    {
        // Arrange
        _movieRepositoryMock
            .Setup(x => x.GetMovie(-100))
            .ThrowsAsync(new MovieNotFoundException(-100));

        // Act
        var actual = (await _controller.GetDetails(-100)).Result as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(actual.Value,
                Is.EqualTo("There's no movie with the id : -100. Please check the given id and retry."));
        });
    }

    [Test]
    public async Task ShouldReturn400WhenMovieIdAndTitleAreNull()
    {
        // Act
        var actual = (await _controller.GetDetails()).Result as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(actual.Value, Is.EqualTo("Please provide a movie id or title."));
        });
    }

    [Test]
    public async Task ShouldReturn400WhenMovieIdAndTitleAreGiven()
    {
        // Act
        var actual = (await _controller.GetDetails(100, "titanic")).Result as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(actual.Value, Is.EqualTo("Please provide a movie id or title."));
        });
    }
}
