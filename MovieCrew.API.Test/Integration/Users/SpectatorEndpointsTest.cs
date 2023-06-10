using System.Dynamic;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Moq;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;
using MovieCrew.Core.Domain.Users.Exception;
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
        _spectatorService = new Mock<ISpectatorService>();
        _client = new IntegrationTestServer<ISpectatorService>(_spectatorService).CreateDefaultClient();
        dynamic data = new ExpandoObject();
        _client.SetFakeBearerToken((object)data);
    }

    [Test]
    public async Task ShouldReturnOkWithSpectatorDetails()
    {
        //Arrange
        var expected = new SpectatorDetailsEntity(new UserEntity(1, "Maxime", UserRoles.Admin),
            new List<SpectatorRateEntity>());
        var expectedJson = JsonSerializer.Serialize(expected, _jsonOptions);
        _spectatorService.Setup(x => x.FetchSpectator(It.IsAny<long>()))
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

    [Test]
    public async Task ShouldReturnNotFoundWhenNoSpectatorFound()
    {
        //Arrange
        _spectatorService.Setup(x => x.FetchSpectator(It.IsAny<long>()))
            .ThrowsAsync(new UserNotFoundException(1));

        //Act
        var response = await _client.GetAsync("/api/spectator/1/details");
        var responseContent = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(responseContent,
                Is.EqualTo("User with id: 1 not found. Please check the conformity and try again"));
        });
    }

    // test for 400
    [Test]
    public void ShouldReturnBadRequestWhenIdIsNotValid()
    {
        //Arrange
        _spectatorService.Setup(x => x.FetchSpectator(It.IsAny<long>()))
            .ThrowsAsync(new UserIsNotSpectatorException(0));

        //Act
        var response = _client.GetAsync("/api/spectator/0/details").Result;
        var responseContent = response.Content.ReadAsStringAsync().Result;

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(responseContent, Is.EqualTo("The user 0 did not rate any movie yet."));
        });
    }
}
