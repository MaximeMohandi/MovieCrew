using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;

namespace MovieCrew.API.Test.UnitTest.Controller.Movies;

public class RefreshMoviesTest : MovieTestBase
{
    [Test]
    public async Task GetRefreshMoviesShouldReturn200()
    {
        // Act
        var toUpdateMovie = new List<MovieEntity>
        {
            new(1, "movie1", "", "description", new DateTime(2012, 12, 12), null, null),
            new(2, "movie2", "http:link", "", new DateTime(2023, 12, 12), null, null)
        };
        _movieDataProviderMock.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("https://titanic", "loremp ipsum", 8M, 340000, 0));
        _movieRepositoryMock.Setup(x => x.UpdateDescription(It.Is<int>(i => i.Equals(2)), It.IsAny<string>()))
            .Verifiable();
        _movieRepositoryMock.Setup(x => x.UpdatePoster(It.Is<int>(i => i.Equals(1)), It.IsAny<string>()))
            .Verifiable();
        _movieRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(toUpdateMovie);
        var response =
            await new MovieController(_service).GetRefreshMovies() as StatusCodeResult;

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task GetRefreshMoviesShouldReturn404WhenNoMoviesFound()
    {
        // Arrange
        _movieRepositoryMock.Setup(x => x.GetAll())
            .ThrowsAsync(new NoMoviesFoundException());

        // Act
        var actual = await new MovieController(_service).GetRefreshMovies() as ObjectResult;

        // Assert
        Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }
}
