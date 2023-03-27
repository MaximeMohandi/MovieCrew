using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Ratings.Repository;
using MovieCrew.Core.Domain.Ratings.Services;

namespace MovieCrew.API.Test.Controller.Ratings
{
    public class RateMovieRouteTest
    {
        private Mock<IRateRepository> _rateRepoMock;
        private RatingServices _service;

        [SetUp]
        public void SetUp()
        {
            _rateRepoMock = new Mock<IRateRepository>();
            _service = new RatingServices(_rateRepoMock.Object);
        }

        [Test]
        public async Task RateMovie()
        {
            RateController controller = new RateController(_service);
            var actual = (await controller.Post(new(1, 2, 3.0M))).Result as ObjectResult;

            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        }

        [TestCase(-2)]
        [TestCase(200)]
        public async Task ErrorWhenRateOutOfBound(decimal rate)
        {
            var controller = new RateController(_service);

            var actual = (await controller.Post(new(1, 1, rate))).Result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
                Assert.That(actual.Value, Is.EqualTo($"The rate must be between 0 and 10. Actual : {rate}"));
            });
        }
    }
}
