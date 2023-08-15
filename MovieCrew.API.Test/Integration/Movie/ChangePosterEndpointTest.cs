using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.Core.Domain.Movies.Exception;

namespace MovieCrew.API.Test.Integration.Movie;

public class ChangePosterEndpointTest : MovieEndpointTestBase
{
    [Test]
    public async Task ShouldReturnOkWhenPosterChanged()
    {
        // Arrange
        _movieService.Setup(x => x.ChangePoster(1, "pathToPoster")).Verifiable();

        // Act
        var response = await _client.PutAsync("/api/movie/1/poster?newPoster=pathToPoster", null);

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task ShouldReturnNotFoundWhenPosterChangedDoNotExist()
    {
        // Arrange
        _movieService.Setup(x => x.ChangePoster(It.IsAny<int>(), It.IsAny<string>()))
            .ThrowsAsync(new MovieNotFoundException(1));
        // Act
        var response = await _client.PutAsync("/api/movie/1/poster?newPoster=pathToPoster", null);

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }
}
