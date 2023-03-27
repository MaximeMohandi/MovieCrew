using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
            var actual = (await controller.RateMovie(new(1, 2, 3.0M))).Result as ObjectResult;

            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        }
    }
}
