using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Users.Exception;

namespace MovieCrew.API.Test.Integration.Movie;

public class AddMovieEndpointTest : MovieEndpointTestBase
{
    [Test]
    public async Task ShouldReturnCreatedWithInsertedMovieId()
    {
        // Arrange
        var newMovie = new NewMovieDto("Matrix", 29844);
        _movieService.Setup(x => x.AddMovie(newMovie.Title, newMovie.ProposedBy))
            .ReturnsAsync(new MovieEntity(1, "Matrix", "http://url", "lorem Ipsum", new DateTime(2023, 3, 11),
                null, null));

        // Act
        var response = await _client.PostAsJsonAsync("/api/movie/add", newMovie);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
            Assert.That(responseContent, Is.EqualTo("1"));
        });
    }

    [Test]
    public async Task ShouldReturnConflictWhenMovieAlreadyExist()
    {
        // Arrange
        var newMovie = new NewMovieDto("Matrix", 29844);
        _movieService.Setup(x => x.AddMovie(newMovie.Title, newMovie.ProposedBy))
            .ThrowsAsync(new MovieAlreadyExistException(newMovie.Title));

        // Act
        var response = await _client.PostAsJsonAsync("/api/movie/add", newMovie);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
            Assert.That(responseContent, Is.EqualTo("Matrix is already in the list."));
        });
    }

    [Test]
    public async Task ShouldReturnNotFoundWhenUserNotFound()
    {
        // Arrange
        var newMovie = new NewMovieDto("Matrix", 29844);
        _movieService.Setup(x => x.AddMovie(newMovie.Title, newMovie.ProposedBy))
            .ThrowsAsync(new UserNotFoundException(newMovie.ProposedBy));

        // Act
        var response = await _client.PostAsJsonAsync("/api/movie/add", newMovie);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(responseContent,
                Is.EqualTo("User with id: 29844 not found. Please check the conformity and try again"));
        });
    }
}
