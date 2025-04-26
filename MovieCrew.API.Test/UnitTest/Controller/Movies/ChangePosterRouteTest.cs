using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.Core.Domain.Movies.Exception;

namespace MovieCrew.API.Test.UnitTest.Controller.Movies;

public class ChangePosterRouteTest : MovieTestBase
{
    [Test]
    public async Task PutShouldReturn200()
    {
        // Act
        var response = await _controller.PutChangePosterMovie(1, "test") as StatusCodeResult;

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task PutShouldReturn404IfMovieDoNotExist()
    {
        // Arrange
        _movieRepositoryMock.Setup(x => x.UpdatePoster(1, "test"))
            .ThrowsAsync(new MovieNotFoundException(1));
        // Act
        var response = await _controller.PutChangePosterMovie(1, "test") as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(response.Value,
                Is.EqualTo("There's no movie with the id : 1. Please check the given id and retry."));
        });
    }
}
