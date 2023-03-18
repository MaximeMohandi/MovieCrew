using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Interfaces;
using MovieCrew.Core.Domain.Movies.Repository;
using MovieCrew.Core.Domain.Movies.Services;

namespace MovieCrew.API.Test.Controller
{
    public class GetMoviesRouteTest
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
        public async Task GetAllMovies()
        {
            _movieRepositoryMock.Setup(x => x.GetAll())
                .ReturnsAsync(new List<MovieEntity>()
            {
                new(1, "Tempête","http://url" ,"lorem Ipsum", new(2023,3,11), new(2023,3,18),2),
                new(1, "John Wick","http://url" ,"lorem Ipsum", new(2023,3,11), new(2023,3,18),5.25M),
            });

            MovieService service = new MovieService(_movieRepositoryMock.Object, _movieDataProviderMock.Object);
            MovieController controller = new MovieController(service);

            var expected = new List<MovieEntity>()
            {
                new(1, "Tempête","http://url" ,"lorem Ipsum", new(2023,3,11), new(2023,3,18),2),
                new(1, "John Wick","http://url" ,"lorem Ipsum", new(2023,3,11), new(2023,3,18),5.25M),
            };

            var actual = (await controller.GetAll()).Result as ObjectResult;

            Assert.That(actual.Value, Is.EqualTo(expected));
            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        public async Task GetAllMoviesButNoMovieFount()
        {
            _movieRepositoryMock.Setup(x => x.GetAll())
                .ThrowsAsync(new NoMoviesFoundException());

            MovieService service = new MovieService(_movieRepositoryMock.Object, _movieDataProviderMock.Object);
            MovieController controller = new MovieController(service);

            var actual = (await controller.GetAll()).Result as ObjectResult;

            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }
    }
}
