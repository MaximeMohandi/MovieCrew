using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.API.Dtos;
using MovieCrew.Core.Domain.Ratings.Repository;
using MovieCrew.Core.Domain.Ratings.Services;

namespace MovieCrew.API.Test.UnitTest.Controller.Ratings;

public class RateMovieRouteTest
{
    private Mock<IRateRepository> _rateRepoMock;
    private RatingService _service;

    [SetUp]
    public void SetUp()
    {
        _rateRepoMock = new Mock<IRateRepository>();
        _service = new RatingService(_rateRepoMock.Object);
    }

    [Test]
    public async Task ShouldReturn201WhenRateIsCreated()
    {
        // Arrange
        var controller = new RateController(_service);

        // Act
        var actual = (await controller.Post(new CreateRateDto(1, 2, 3.0M))).Result as ObjectResult;

        // Assert
        Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
    }

    [TestCase(-2)]
    [TestCase(200)]
    public async Task ShouldReturn400WhenRateIsNotBetween0And10(decimal rate)
    {
        // Arrange
        var controller = new RateController(_service);

        // Act
        var actual = (await controller.Post(new CreateRateDto(1, 1, rate))).Result as ObjectResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(actual.Value, Is.EqualTo($"The rate must be between 0 and 10. Actual : {rate}"));
        });
    }
}