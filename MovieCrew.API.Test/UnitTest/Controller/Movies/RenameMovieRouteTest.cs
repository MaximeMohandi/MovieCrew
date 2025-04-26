using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Movies.Exception;

namespace MovieCrew.API.Test.UnitTest.Controller.Movies;

public class RenameMovieRouteTest : MovieTestBase
{
    private readonly ILogger<MovieController> _logger = new FakeLogger<MovieController>();

    [Test]
    public async Task PutShouldRenameMovieAndReturn200()
    {
        // Act
        var response =
            await new MovieController(_service, _logger).PutRenameMovie(1, "test") as StatusCodeResult;

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task PutExistingTitleShouldReturn409()
    {
        // Arrange
        _movieRepositoryMock.Setup(x => x.UpdateTitle(1, "test"))
            .ThrowsAsync(new MovieAlreadyExistException("test"));
        // Act
        var response =
            await new MovieController(_service, _logger).PutRenameMovie(1, "test") as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
            Assert.That(response.Value, Is.EqualTo("test is already in the list."));
        });
    }
}
