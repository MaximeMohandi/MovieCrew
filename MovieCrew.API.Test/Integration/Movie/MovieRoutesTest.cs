using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Services;

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

        var response = await (await _client.GetAsync("/api/movie/all")).Content.ReadAsStringAsync();

        Assert.That(response.ToLower(), Is.EquivalentTo(expectedJsonResponse.ToLower()));
    }

    [Test]
    public async Task NoMoviesFound()
    {
        _movieService.Setup(x => x.FetchAllMovies())
            .ThrowsAsync(new NoMoviesFoundException());

        var response = await _client.GetAsync("/api/movie/all");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(responseContent,
                Is.EqualTo("It seem that there's no movies in the list. Please try to add new one"));
        });
    }
}
