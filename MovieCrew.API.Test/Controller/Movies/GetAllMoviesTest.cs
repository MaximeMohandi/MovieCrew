using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Interfaces;
using MovieCrew.Core.Domain.Movies.Repository;
using MovieCrew.Core.Domain.Movies.Services;

namespace MovieCrew.API.Test.Controller.Movies;

public class GetAllMoviesTest
{
    private Mock<IThirdPartyMovieDataProvider> _movieDataProviderMock;
    private Mock<IMovieRepository> _movieRepositoryMock;
    private MovieService _service;

    [SetUp]
    public void Setup()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _movieDataProviderMock = new Mock<IThirdPartyMovieDataProvider>();
        _service = new MovieService(_movieRepositoryMock.Object, _movieDataProviderMock.Object);
    }

    [Test]
    public async Task GetAllMovies()
    {
        _movieRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(new List<MovieEntity>
            {
                new(1, "Tempête", "http://url", "lorem Ipsum", new DateTime(2023, 3, 11), new DateTime(2023, 3, 18), 2),
                new(1, "John Wick", "http://url", "lorem Ipsum", new DateTime(2023, 3, 11), new DateTime(2023, 3, 18),
                    5.25M)
            });
        MovieController controller = new(_service);

        var expected = new List<MovieEntity>
        {
            new(1, "Tempête", "http://url", "lorem Ipsum", new DateTime(2023, 3, 11), new DateTime(2023, 3, 18), 2),
            new(1, "John Wick", "http://url", "lorem Ipsum", new DateTime(2023, 3, 11), new DateTime(2023, 3, 18),
                5.25M)
        };

        var actual = (await controller.GetAll()).Result as ObjectResult;
        Assert.Multiple(() =>
        {
            Assert.That(actual.Value, Is.EqualTo(expected));
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        });
    }

    [Test]
    public async Task TryGettingMoviesReturn404()
    {
        _movieRepositoryMock.Setup(x => x.GetAll())
            .ThrowsAsync(new NoMoviesFoundException());
        MovieController controller = new(_service);

        var actual = (await controller.GetAll()).Result as ObjectResult;

        Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }

    [Test]
    public async Task GetRandomUnseenMovie()
    {
        var unseenMovie = new List<MovieEntity>
        {
            new(1, "movie1", "http:Link", "description", new DateTime(2012, 12, 12), null, null),
            new(2, "movie2", "http:Lin2k", "lorem description", new DateTime(2023, 12, 12), null, null),
            new(3, "movie3", "http:Lin2k", "lorem description", new DateTime(2023, 12, 12), null, null),
            new(4, "movie4", "http:Lin2k", "lorem description", new DateTime(2023, 12, 12), null, null)
        };
        _movieRepositoryMock.Setup(x => x.GetAllUnSeen())
            .ReturnsAsync(unseenMovie);
        MovieController movieController = new(_service);


        var actual = (await movieController.GetRandomUnseenMovie()).Result as ObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(unseenMovie, Has.Member(actual.Value));
        });
    }

    [Test]
    public async Task GetRandomMovieWhenNoMoreMoviesReturn204()
    {
        _movieRepositoryMock.Setup(x => x.GetAllUnSeen())
            .ThrowsAsync(new AllMoviesHaveBeenSeenException());
        MovieController movieController = new(_service);

        var actual = (await movieController.GetRandomUnseenMovie()).Result as StatusCodeResult;

        Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
    }
}