using BillB0ard_API.Data;
using BillB0ard_API.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Test.Movies
{
    public abstract class InMemoryMovieTestBase
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieDbTest")
            .Options;

        protected AppDbContext _dbContext;
        protected MovieRepository _movieRepository;
        protected RateRepository _rateRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dbContext = new AppDbContext(_dbContextOptions);
            _dbContext.Database.EnsureCreated();

            SeedInMemoryDatas();

            _dbContext.SaveChanges();

            _movieRepository = new MovieRepository(_dbContext);
            _rateRepository = new RateRepository(_dbContext);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
        }

        protected abstract void SeedInMemoryDatas();
    }
}
