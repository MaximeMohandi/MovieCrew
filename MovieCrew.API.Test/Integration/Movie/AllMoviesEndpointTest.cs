using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;

namespace MovieCrew.API.Test.Integration.Movie;

public class AllMoviesEndpointTest : MovieEndpointTestBase
{
    [Test]
    public async Task ShouldReturnAllMovies()
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
            Assert.That(responseContent.ToLower(), Is.EqualTo(expectedJsonResponse.ToLower()));
        });
    }

    [Test]
    public async Task ShouldReturnNotFoundWhenNoMoviesFound()
    {
        var expected = "it seems that there's no movies in the list. please try to add new one";
        _movieService.Setup(x => x.FetchAllMovies()).ThrowsAsync(new NoMoviesFoundException());

        var response = await _client.GetAsync("/api/movie/all");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(responseContent.ToLower(), Is.EqualTo(expected.ToLower()));
        });
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
                Is.EqualTo("It seems that there's no movies in the list. Please try to add new one"));
        });
    }
}
