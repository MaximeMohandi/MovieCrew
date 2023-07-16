using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;

namespace MovieCrew.API.Test.Integration.Movie;

public class RandomMovieEndpointTest : MovieEndpointTestBase
{
    [Test]
    public async Task ShouldReturnRandomMovie()
    {
        var expectedDetails = new MovieDetailsEntity(
            1,
            "Suzume",
            "",
            "",
            new DateTime(2023, 5, 5),
            new DateTime(2023, 5, 6),
            3,
            9,
            300.44M, null,
            new List<MovieRateEntity>
            {
                new(new UserEntity(2223, "mant", UserRoles.Admin), 3)
            },
            new UserEntity(2222, "cat", UserRoles.User));

        _movieService.Setup(x => x.RandomMovie())
            .ReturnsAsync(expectedDetails);

        var expectedJsonResponse = JsonSerializer.Serialize(expectedDetails, _jsonOptions);

        var response = await _client.GetAsync("/api/movie/random");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(responseContent.ToLower(), Is.EqualTo(expectedJsonResponse.ToLower()));
        });
    }

    [Test]
    public async Task ShouldReturnNoContentWhenNoMovies()
    {
        _movieService.Setup(x => x.RandomMovie())
            .ThrowsAsync(new AllMoviesHaveBeenSeenException());

        var response = await _client.GetAsync("/api/movie/random");

        Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
    }
}