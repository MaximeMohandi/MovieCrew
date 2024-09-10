using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.Core.Domain.Movies.Exception;

namespace MovieCrew.API.Test.Integration.Movie;

public class RefreshMoviesEndpointTest : MovieEndpointTestBase
{
    [Test]
    public async Task ShouldReturnOk()
    {
        // Arrange
        _movieService.Setup(x => x.RefreshMoviesMetaData()).Verifiable();

        // Act
        var response = await _client.GetAsync("/api/movie/refreshAll");

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task ShouldReturnNotFoundWhenNoMoviesFound()
    {
        // Arrange
        _movieService.Setup(x => x.RefreshMoviesMetaData())
            .ThrowsAsync(new NoMoviesFoundException());

        // Act
        var response = await _client.GetAsync("/api/movie/refreshAll");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(content, Is.EqualTo("It seems that there's no movies in the list. Please try to add new one"));
        });
    }
}
