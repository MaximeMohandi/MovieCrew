using Microsoft.EntityFrameworkCore;
using Moq;
using MovieCrew.Core.Data;
using MovieCrew.Core.Domain.Movies.Repository;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.Ratings.Repository;
using MovieCrew.Core.Domain.Users.Repository;

namespace MovieCrew.Core.Test;

public abstract class InMemoryMovieTestBase
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase("MovieDbTest")
        .Options;

    protected AppDbContext _dbContext;
    protected Mock<IThirdPartyMovieDataProvider> _fakeDataProvider;
    protected MovieRepository _movieRepository;
    protected RateRepository _rateRepository;
    protected UserRepository _userRepository;

    [OneTimeSetUp]
    public virtual void SetUp()
    {
        _fakeDataProvider = new Mock<IThirdPartyMovieDataProvider>();
        _dbContext = new AppDbContext(_dbContextOptions);
        _dbContext.Database.EnsureCreated();

        SeedInMemoryDatas();

        _dbContext.SaveChanges();

        _movieRepository = new MovieRepository(_dbContext);
        _rateRepository = new RateRepository(_dbContext);
        _userRepository = new UserRepository(_dbContext);
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        _dbContext.Database.EnsureDeleted();
    }

    protected abstract void SeedInMemoryDatas();
}