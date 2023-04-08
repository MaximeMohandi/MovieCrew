﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Interfaces;
using MovieCrew.Core.Domain.Movies.Repository;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.Users.Enums;

namespace MovieCrew.API.Test.Controller.Movies
{
    public class GetMovieDetailsTest
    {
        private Mock<IMovieRepository> _movieRepositoryMock;
        private Mock<IThirdPartyMovieDataProvider> _movieDataProviderMock;
        private MovieService _service;

        [SetUp]
        public void Setup()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _movieDataProviderMock = new Mock<IThirdPartyMovieDataProvider>();
            _service = new(_movieRepositoryMock.Object, _movieDataProviderMock.Object);
        }

        [Test]
        public async Task GetMovieDetailsById()
        {
            _movieDataProviderMock
                .Setup(x => x.GetDetails(It.IsAny<string>()))
                .ReturnsAsync(new MovieMetadataEntity("https://titanic", "loremp ipsum", 8M, 340000));
            _movieRepositoryMock
                .Setup(x => x.GetMovie(It.IsAny<int>()))
                .ReturnsAsync(new MovieDetailsEntity(1,
                                                     "Titanic",
                                                     "http://titanic",
                                                     "loremp ipsum",
                                                     new(2023, 3, 12),
                                                     null,
                                                     null,
                                                     null,
                                                     null,
                                                     null,
                                                     new(1, "Maxime", UserRoles.Admin)));

            MovieController controller = new(_service);

            var actual = (await controller.GetDetails(id: 1)).Result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(actual.Value, Is.EqualTo(new MovieDetailsEntity(1,
                                              "Titanic",
                                              "http://titanic",
                                              "loremp ipsum",
                                              new(2023, 3, 12),
                                              null,
                                              null,
                                              8M,
                                              340000,
                                              null,
                                              new(1, "Maxime", UserRoles.Admin))));
                Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            });
        }

        [Test]
        public async Task GetMovieDetailsByTitle()
        {
            _movieDataProviderMock
                .Setup(x => x.GetDetails(It.IsAny<string>()))
                .ReturnsAsync(new MovieMetadataEntity("https://titanic", "loremp ipsum", 8M, 340000));
            _movieRepositoryMock
                .Setup(x => x.GetMovie(It.IsAny<string>()))
                .ReturnsAsync(new MovieDetailsEntity(1,
                                                     "Titanic",
                                                     "http://titanic",
                                                     "loremp ipsum",
                                                     new(2023, 3, 12),
                                                     new(2023, 3, 18),
                                                     2M,
                                                     null,
                                                     null,
                                                     new List<MovieRateEntity>
                                                     {
                                                         new(new(1,"user", 2), 2)
                                                     },
                                                     new(1, "Maxime", UserRoles.Admin)));

            MovieController controller = new(_service);

            var actual = (await controller.GetDetails(title: "Titanic")).Result as ObjectResult;

            Assert.Multiple(() =>
            {
                Assert.That(actual.Value, Is.EqualTo(new MovieDetailsEntity(1,
                                                  "Titanic",
                                                  "http://titanic",
                                                  "loremp ipsum",
                                                  new(2023, 3, 12),
                                                  new(2023, 3, 18),
                                                  2M,
                                                  8M,
                                                  340000,
                                                  new List<MovieRateEntity>
                                                  {
                                                    new(new(1,"user", 2), 2)
                                                  },
                                                  new(1, "Maxime", UserRoles.Admin))));
                Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            });
        }

        [Test]
        public async Task MovieNotFound()
        {
            _movieRepositoryMock
                .Setup(x => x.GetMovie(-100))
                .ThrowsAsync(new MovieNotFoundException(-100));

            MovieController controller = new(_service);

            var actual = (await controller.GetDetails(-100)).Result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
                Assert.That(actual.Value, Is.TypeOf<MovieNotFoundException>());
            });
        }

        [Test]
        public async Task CantFetchMovieWithoutDefiningWichOne()
        {
            MovieController controller = new(_service);

            var actual = (await controller.GetDetails()).Result as ObjectResult;

            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(actual.Value, Is.EqualTo("Either id or title parameter is required."));
        }

        [Test]
        public async Task CantFetchMovieByGivingAnIdAndATitle()
        {
            MovieController controller = new(_service);

            var actual = (await controller.GetDetails(100, "titanic")).Result as ObjectResult;

            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(actual.Value, Is.EqualTo("Only one of id or title parameters should be provided."));
        }
    }
}