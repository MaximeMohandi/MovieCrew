using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Moq;
using MovieCrew.API.Controller;
using MovieCrew.Core.Domain.Movies.Repository;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Services;

namespace MovieCrew.API.Test.UnitTest.Controller.Movies;

public class MovieTestBase
{
    protected MovieController _controller;
    protected ILogger<MovieController> _logger;
    protected Mock<IThirdPartyMovieDataProvider> _movieDataProviderMock;
    protected Mock<IMovieRepository> _movieRepositoryMock;
    protected MovieService _service;


    [SetUp]
    public void Setup()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _movieDataProviderMock = new Mock<IThirdPartyMovieDataProvider>();
        _logger = new FakeLogger<MovieController>();
        _service = new MovieService(_movieRepositoryMock.Object, _movieDataProviderMock.Object);
        _controller = new MovieController(_service, _logger);
    }
}
