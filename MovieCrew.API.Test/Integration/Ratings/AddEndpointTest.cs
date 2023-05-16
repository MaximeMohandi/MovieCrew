using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Ratings.Exception;
using MovieCrew.Core.Domain.Ratings.Services;

namespace MovieCrew.API.Test.Integration.Ratings;

public class AddEndpointTest
{
    private readonly JsonSerializerOptions _jsonOptions;
    private HttpClient _client;
    private Mock<IRatingService> _ratingService;

    public AddEndpointTest()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    [SetUp]
    public void SetUp()
    {
        _ratingService = new Mock<IRatingService>();
        _client = new IntegrationTestServer<IRatingService>(_ratingService).CreateDefaultClient();
    }

    [Test]
    public async Task ShouldReturnCreated()
    {
        // Arrange
        _ratingService.Setup(x => x.RateMovie(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<decimal>()))
            .Verifiable();
        var createRate = new CreateRateDto(1, 1, 10);

        // Act
        var response = await _client.PostAsync("/api/rate/add",
            new StringContent(JsonSerializer.Serialize(createRate, _jsonOptions), Encoding.UTF8, "application/json"));

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
    }

    [TestCase(11)]
    [TestCase(-1)]
    public async Task ShouldReturnBadRequestWhenRateIsOutOfLimit(decimal rate)
    {
        // Arrange
        _ratingService.Setup(x => x.RateMovie(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<decimal>()))
            .ThrowsAsync(new RateLimitException(rate));
        var createRate = new CreateRateDto(1, 1, 11);

        // Act
        var response = await _client.PostAsync("/api/rate/add",
            new StringContent(JsonSerializer.Serialize(createRate, _jsonOptions), Encoding.UTF8, "application/json"));
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(responseContent.ToLower(), Is.EqualTo($"the rate must be between 0 and 10. actual : {rate}"));
        });
    }
}
