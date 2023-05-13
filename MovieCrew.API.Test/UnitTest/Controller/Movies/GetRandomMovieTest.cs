using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;

namespace MovieCrew.API.Test.UnitTest.Controller.Movies;

public class GetRandomMovieTest : MovieTestBase
{
    [Test]
    public async Task GetRandomUnseenMovieShouldReturnRandomMovieWithNoSeenDate()
    {
        // Arrange
        var unseenMovie = new List<MovieEntity>
        {
            new(1, "movie1", "http:Link", "description", new DateTime(2012, 12, 12), null, null),
            new(2, "movie2", "http:Lin2k", "lorem description", new DateTime(2023, 12, 12), null, null),
            new(3, "movie3", "http:Lin2k", "lorem description", new DateTime(2023, 12, 12), null, null),
            new(4, "movie4", "http:Lin2k", "lorem description", new DateTime(2023, 12, 12), null, null)
        };
        _movieRepositoryMock.Setup(x => x.GetAllUnSeen())
            .ReturnsAsync(unseenMovie);

        // Act
        var actual = (await new MovieController(_service).GetRandomUnseenMovie()).Result as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(unseenMovie, Has.Member(actual.Value));
        });
    }

    [Test]
    public async Task ShouldReturn404WhenNoUnseenMoviesFound()
    {
        _movieRepositoryMock.Setup(x => x.GetAllUnSeen())
            .ThrowsAsync(new AllMoviesHaveBeenSeenException());
        MovieController movieController = new(_service);

        var actual = (await movieController.GetRandomUnseenMovie()).Result as StatusCodeResult;

        Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
    }
}
