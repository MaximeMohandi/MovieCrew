using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.Users.Exception;

namespace MovieCrew.API.Test.UnitTest.Controller.Movies;

public class AddMovieRouteTest : MovieTestBase
{
    [Test]
    public async Task PostMovieShouldAddMovie()
    {
        // Arrange
        _movieDataProviderMock.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("https://maximemohandi.fr/", "loremp ipsum", 8, 8, 0));
        _movieRepositoryMock.Setup(x => x.Add("test", "https://maximemohandi.fr/", "loremp ipsum", 3))
            .ReturnsAsync(new MovieEntity(1, "test", "https://maximemohandi.fr/", "loremp ipsum", DateTime.Now, null,
                null));
        var movieService = new MovieService(_movieRepositoryMock.Object, _movieDataProviderMock.Object);

        // Act
        var actual = (await _controller.Post(new NewMovieDto("test", 3))).Result as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.Value, Is.EqualTo(1));
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        });
    }

    [Test]
    public async Task PostMovieShouldThrowExceptionWhenMovieAlreadyExist()
    {
        // Arrange
        _movieDataProviderMock.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("", "", 0, 0, 0));
        _movieRepositoryMock.Setup(x => x.Add("test", "", "", 3))
            .ThrowsAsync(new MovieAlreadyExistException("test"));
        var movieService = new MovieService(_movieRepositoryMock.Object, _movieDataProviderMock.Object);

        // Act
        var actual = (await _controller.Post(new NewMovieDto("test", 3))).Result as ObjectResult;

        // Assert
        Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
    }

    [Test]
    public async Task PostMovieShouldThrowExceptionWhenUserNotFound()
    {
        // Arrange
        _movieDataProviderMock.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("", "", 0, 0, 0));
        _movieRepositoryMock.Setup(x => x.Add("test", "", "", 3))
            .ThrowsAsync(new UserNotFoundException(3));
        var movieService = new MovieService(_movieRepositoryMock.Object, _movieDataProviderMock.Object);

        // Act
        var actual = (await _controller.Post(new NewMovieDto("test", 3))).Result as ObjectResult;

        // Assert
        Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }
}
