using Microsoft.EntityFrameworkCore;
using MovieCrew_core.Data;
using MovieCrew_core.Domain.Movies.Repository;
using MovieCrew_core.Domain.Ratings.Repository;
using MovieCrew_core.Domain.Users.Repository;

namespace MovieCrew_core.Test
{
    public abstract class InMemoryMovieTestBase
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieDbTest")
            .Options;

        protected AppDbContext _dbContext;
        protected MovieRepository _movieRepository;
        protected RateRepository _rateRepository;
        protected UserRepository _userRepository;

        [OneTimeSetUp]
        public virtual void SetUp()
        {
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
}
