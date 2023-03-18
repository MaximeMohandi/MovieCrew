using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Interfaces;
using MovieCrew.Core.Domain.Movies.Repository;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.Users.Enums;

namespace MovieCrew.API.Test.Controller
{
    public class GetMovieDetailsTest
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
        public async Task GetMovieDetailsAsync()
        {
            _movieDataProviderMock.Setup(x => x.GetDetails(It.IsAny<string>()))
                .ReturnsAsync(new MovieMetadataEntity("https://titanic", "loremp ipsum", 8M, 340000));
            _movieRepositoryMock.Setup(x => x.GetMovie(It.IsAny<int>()))
                .ReturnsAsync(new MovieDetailsEntity(1, "Titanic", "http://titanic", "loremp ipsum", new(2023, 3, 12), null, null, null, null, null, new(1, "Maxime", UserRoles.Admin)));
            MovieService service = new MovieService(_movieRepositoryMock.Object, _movieDataProviderMock.Object);
            MovieController controller = new MovieController(service);

            var expected = new MovieDetailsEntity(1, "Titanic", "http://titanic", "loremp ipsum", new(2023, 3, 12), null, null, 8M, 340000, null, new(1, "Maxime", UserRoles.Admin));

            var actual = (await controller.GetDetails(1)).Result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(actual.Value, Is.EqualTo(expected));
                Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            });
        }
    }
}
