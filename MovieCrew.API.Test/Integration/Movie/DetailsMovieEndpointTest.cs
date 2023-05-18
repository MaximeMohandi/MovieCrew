using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;

namespace MovieCrew.API.Test.Integration.Movie;

public class DetailsMovieEndpointTest : MovieEndpointTestBase
{
    [Test]
    public async Task ShouldReturnMovieDetailsWhenCorrectId()
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
            Assert.That(responseContent.ToLower(), Is.EqualTo(expectedJsonResponse.ToLower()));
        });
    }

    [Test]
    public async Task ShouldReturnMovieDetailsWhenCorrectTitle()
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
            Assert.That(responseContent.ToLower(), Is.EqualTo(expectedJsonResponse.ToLower()));
        });
    }

    [Test]
    public async Task ShouldReturnNotFoundWhenNoMovieFoundWithId()
    {
        _movieService.Setup(x => x.GetById(1)).ThrowsAsync(new MovieNotFoundException(1));

        var response = await _client.GetAsync("/api/movie/details?id=1");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(responseContent,
                Is.EqualTo("There's no movie with the id : 1. Please check the given id and retry."));
        });
    }

    [Test]
    public async Task ShouldReturnNotFoundWhenNoMovieFoundWithTitle()
    {
        _movieService.Setup(x => x.GetByTitle("Suzume")).ThrowsAsync(new MovieNotFoundException("Suzume"));

        var response = await _client.GetAsync("/api/movie/details?title=Suzume");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(responseContent,
                Is.EqualTo("Suzume cannot be found. Please check the title and retry."));
        });
    }

    [Test]
    public async Task ShouldReturnBadRequestWhenNoIdOrTitleGiven()
    {
        var response = await _client.GetAsync("/api/movie/details");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(responseContent,
                Is.EqualTo("Please provide a movie id or title."));
        });
    }

    [Test]
    public async Task ShouldReturnBadRequestWhenIdAndTitleGiven()
    {
        var response = await _client.GetAsync("/api/movie/details?id=1&title=Suzume");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(responseContent,
                Is.EqualTo("Please provide a movie id or title."));
        });
    }
}
