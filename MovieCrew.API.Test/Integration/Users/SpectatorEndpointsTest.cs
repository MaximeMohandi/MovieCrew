using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.API.Test.Integration.Users;

public class SpectatorEndpointsTest
{
    private readonly JsonSerializerOptions _jsonOptions;
    private HttpClient _client;
    private Mock<ISpectatorService> _spectatorService;

    public SpectatorEndpointsTest()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    [SetUp]
    public void SetUp()
    {
        _client = new IntegrationTestServer<ISpectatorService>(_spectatorService).CreateDefaultClient();
    }

    [Test]
    public async Task ShouldReturnOkWithSpectatorDetails()
    {
        //Arrange
        var expected = new SpectatorDetailsEntity(new UserEntity(1, "Maxime", UserRoles.Admin),
            new List<SpectatorRateEntity>());
        var expectedJson = JsonSerializer.Serialize(expected, _jsonOptions);
        _spectatorService.Setup(x => x.FetchSpectator(It.IsAny<int>()))
            .ReturnsAsync(expected);

        //Act
        var response = await _client.GetAsync("/api/spectator/1/details");
        var responseContent = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(responseContent.ToLower(), Is.EqualTo(expectedJson.ToLower()));
        });
    }
}
