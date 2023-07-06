using Moq;
using MovieCrew.Core.Domain.Movies.Repository;
using MovieCrew.Core.Domain.Movies.Services;

namespace MovieCrew.API.Test.UnitTest.Controller.Movies;

public class MovieTestBase
{
    protected Mock<IThirdPartyMovieDataProvider> _movieDataProviderMock;
    protected Mock<IMovieRepository> _movieRepositoryMock;
    protected MovieService _service;

    [SetUp]
    public void Setup()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _movieDataProviderMock = new Mock<IThirdPartyMovieDataProvider>();
        _service = new MovieService(_movieRepositoryMock.Object, _movieDataProviderMock.Object);
    }
}