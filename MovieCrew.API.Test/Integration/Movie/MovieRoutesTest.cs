using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;

namespace MovieCrew.API.Test.Integration.Movie;

public class MovieRoutesTest
{
    private readonly JsonSerializerOptions _jsonOptions;
    private HttpClient _client;
    private Mock<IMovieService> _movieService;

    public MovieRoutesTest()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    [SetUp]
    public void SetUp()
    {
        _movieService = new Mock<IMovieService>();
        _client = new IntegrationTestServer<IMovieService>(_movieService).CreateDefaultClient();
    }

    [Test]
    public async Task FetchAllMovie()
    {
        var expectedList = new List<MovieEntity>
        {
            new(1, "Tempête", "http://url", "lorem Ipsum", new DateTime(2023, 3, 11), new DateTime(2023, 3, 18),
                2),
            new(1, "John Wick", "http://url", "lorem Ipsum", new DateTime(2023, 3, 11),
                new DateTime(2023, 3, 18),
                5.25M)
        };
        _movieService.Setup(x => x.FetchAllMovies())
            .ReturnsAsync(expectedList);

        var expectedJsonResponse = JsonSerializer.Serialize(expectedList, _jsonOptions);

        var response = await _client.GetAsync("/api/movie/all");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(responseContent.ToLower(), Is.EquivalentTo(expectedJsonResponse.ToLower()));
        });
    }

    [Test]
    public async Task NoMoviesToFetch()
    {
        var expected = "it seems that there's no movies in the list. please try to add new one";
        _movieService.Setup(x => x.FetchAllMovies()).ThrowsAsync(new NoMoviesFoundException());

        var response = await _client.GetAsync("/api/movie/all");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(responseContent.ToLower(), Is.EquivalentTo(expected.ToLower()));
        });
    }

    [Test]
    public async Task FetchMoviesDetailWithId()
    {
        var expectedDetails = new MovieDetailsEntity(1, "Suzume", "", "", new DateTime(2023, 5, 5),
            new DateTime(2023, 5, 6), 3, 9, 300.44M, new List<MovieRateEntity>
            {
                new(new UserEntity(2223, "mant", UserRoles.Admin), 3)
            }, new UserEntity(2222, "cat", UserRoles.User));

        _movieService.Setup(x => x.GetById(1))
            .ReturnsAsync(expectedDetails);

        var expectedJsonResponse = JsonSerializer.Serialize(expectedDetails, _jsonOptions);

        var response = await _client.GetAsync("/api/movie/details?id=1");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(responseContent.ToLower(), Is.EquivalentTo(expectedJsonResponse.ToLower()));
        });
    }

    [Test]
    public async Task FetchMoviesDetailWithTitle()
    {
        var expectedDetails = new MovieDetailsEntity(1, "Suzume", "", "", new DateTime(2023, 5, 5),
            new DateTime(2023, 5, 6), 3, 9, 300.44M, new List<MovieRateEntity>
            {
                new(new UserEntity(2223, "mant", UserRoles.Admin), 3)
            }, new UserEntity(2222, "cat", UserRoles.User));

        _movieService.Setup(x => x.GetByTitle("Suzume"))
            .ReturnsAsync(expectedDetails);

        var expectedJsonResponse = JsonSerializer.Serialize(expectedDetails, _jsonOptions);

        var response = await _client.GetAsync("/api/movie/details?title=Suzume");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(responseContent.ToLower(), Is.EquivalentTo(expectedJsonResponse.ToLower()));
        });
    }

    [Test]
    public async Task NoMovieFound()
    {
        _movieService.Setup(x => x.GetById(1)).ThrowsAsync(new MovieNotFoundException(1));

        var response = await _client.GetAsync("/api/movie/details?id=1");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(responseContent.ToLower(),
                Is.EqualTo("There's no movie with the id : 1. Please check the given id and retry."));
        });
    }
}
