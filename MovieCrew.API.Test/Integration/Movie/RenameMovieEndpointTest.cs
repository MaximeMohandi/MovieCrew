using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.Core.Domain.Movies.Exception;

namespace MovieCrew.API.Test.Integration.Movie;

public class RenameMovieEndpointTest : MovieEndpointTestBase
{
    [Test]
    public async Task ShouldReturnOkWhenMovieRenamed()
    {
        // Arrange
        _movieService.Setup(x => x.ChangeTitle(1, "Mario")).Verifiable();

        // Act
        var response = await _client.PutAsync("/api/movie/1/rename?newTitle=Mario", null);

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task ShouldReturnConflictIfMovieAlreadyExist()
    {
        // Arrange
        _movieService.Setup(x => x.ChangeTitle(1, "Mario"))
            .ThrowsAsync(new MovieAlreadyExistException("Mario"));

        // Act
        var response = await _client.PutAsync("/api/movie/1/rename?newTitle=Mario", null);
        var content = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
            Assert.That(content, Is.EqualTo("Mario is already in the list."));
        });
    }
}
