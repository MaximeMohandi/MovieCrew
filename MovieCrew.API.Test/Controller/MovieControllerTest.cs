using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Movies.Dtos;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Interfaces;
using MovieCrew.Core.Domain.Movies.Repository;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.Users.Exception;

namespace MovieCrew.API.Test.Controller
{
    public class MovieControllerTest
    {
        private Mock<IMovieRepository> _movieRepositoryMock;
        private Mock<IThirdPartyMovieDataProvider> _movieDataProviderMock;

        [SetUp]
        public void Setup()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _movieDataProviderMock = new Mock<IThirdPartyMovieDataProvider>();
        }


        [Test]
        public async Task AddMovie()
        {
            _movieDataProviderMock.Setup(x => x.GetDetails(It.IsAny<string>()))
                .ReturnsAsync(new MovieMetadataEntity("https://maximemohandi.fr/", "loremp ipsum", 8, 8));

            _movieRepositoryMock.Setup(x => x.Add(It.IsAny<MovieCreationDto>()))
                .ReturnsAsync(new MovieEntity(1, "test", "https://maximemohandi.fr/", "loremp ipsum", DateTime.Now, null, null));

            var movieService = new MovieService(_movieRepositoryMock.Object, _movieDataProviderMock.Object);

            var controller = new MovieController(movieService);

            var actual = (await controller.Post(new("test", 3))).Result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(actual.Value, Is.EqualTo(1));
                Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
            });
        }

        [Test]
        public async Task AddExistingMovie()
        {
            _movieDataProviderMock.Setup(x => x.GetDetails(It.IsAny<string>()))
                .ReturnsAsync(new MovieMetadataEntity("", "", 0, 0));
            _movieRepositoryMock.Setup(x => x.Add(It.IsAny<MovieCreationDto>()))
                .ThrowsAsync(new MovieAlreadyExistException("test"));

            var movieService = new MovieService(_movieRepositoryMock.Object, _movieDataProviderMock.Object);

            var controller = new MovieController(movieService);

            var actual = (await controller.Post(new("test", 3))).Result as ObjectResult;

            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
        }

        [Test]
        public async Task AddFromUnknowUser()
        {
            _movieDataProviderMock.Setup(x => x.GetDetails(It.IsAny<string>()))
                .ReturnsAsync(new MovieMetadataEntity("", "", 0, 0));
            _movieRepositoryMock.Setup(x => x.Add(It.IsAny<MovieCreationDto>()))
                .ThrowsAsync(new UserNotFoundException(3));

            var movieService = new MovieService(_movieRepositoryMock.Object, _movieDataProviderMock.Object);

            var controller = new MovieController(movieService);

            var actual = (await controller.Post(new("test", 3))).Result as ObjectResult;

            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }
    }
}
