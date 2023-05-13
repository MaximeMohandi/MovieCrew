using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;

namespace MovieCrew.API.Test.UnitTest.Controller.Movies;

public class GetAllMoviesTest : MovieTestBase
{
    [Test]
    public async Task GetAllShouldReturnAllMovies()
    {
        // Arrange
        _movieRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(new List<MovieEntity>
            {
                new(1, "Tempête", "http://url", "lorem Ipsum", new DateTime(2023, 3, 11), new DateTime(2023, 3, 18), 2),
                new(1, "John Wick", "http://url", "lorem Ipsum", new DateTime(2023, 3, 11), new DateTime(2023, 3, 18),
                    5.25M)
            });

        // Act
        var actual = (await new MovieController(_service).GetAll()).Result as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.Value, Is.EqualTo(new List<MovieEntity>
            {
                new(1, "Tempête", "http://url", "lorem Ipsum", new DateTime(2023, 3, 11), new DateTime(2023, 3, 18), 2),
                new(1, "John Wick", "http://url", "lorem Ipsum", new DateTime(2023, 3, 11), new DateTime(2023, 3, 18),
                    5.25M)
            }));
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        });
    }

    [Test]
    public async Task GetAllShouldReturn404WhenNoMoviesFound()
    {
        // Arrange
        _movieRepositoryMock.Setup(x => x.GetAll())
            .ThrowsAsync(new NoMoviesFoundException());

        // Act
        var actual = (await new MovieController(_service).GetAll()).Result as ObjectResult;

        // Assert
        Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }
}
