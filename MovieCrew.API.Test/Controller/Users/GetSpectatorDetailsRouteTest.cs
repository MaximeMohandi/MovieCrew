using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Repository;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.API.Test.Controller.Users
{
    public class GetSpectatorDetailsRouteTest
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private SpectatorService _service;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _service = new SpectatorService(_userRepositoryMock.Object);
        }

        [Test]
        public async Task GetSpectatorDetails()
        {
            SpectatorController controller = new SpectatorController(_service);
            _userRepositoryMock.Setup(x => x.GetSpectatorDetails(It.IsAny<long>()))
                .ReturnsAsync(new SpectatorDetailsEntity(new(1, "Maxime", 2), new List<SpectatorRateEntity>
            {
                new SpectatorRateEntity(new MovieEntity(1, "John Wick", "http://Poster.test", "a movie", new(2023,3,2), new(2023,4,2), 4.00M), 3.00M),
                new SpectatorRateEntity(new MovieEntity(1, "Troy", "http://Poster.test", "a movie", new(2022,1,25), new(2023,2,2), 6.0M), 9.00M),
            }));

            SpectatorDetailsEntity expected = new(new(1, "Maxime", 2), new List<SpectatorRateEntity>
            {
                new SpectatorRateEntity(new MovieEntity(1, "John Wick", "http://Poster.test", "a movie", new(2023,3,2), new(2023,4,2), 4.00M), 3.00M),
                new SpectatorRateEntity(new MovieEntity(1, "Troy", "http://Poster.test", "a movie", new(2022,1,25), new(2023,2,2), 6.0M), 9.00M),
            });

            var actual = (await controller.Get()).Result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(actual.Value, Is.EqualTo(expected));
                Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            });
        }
    }
}
