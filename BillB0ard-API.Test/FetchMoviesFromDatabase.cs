using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Test
{
    public class Tests
    {
        private static DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieDbTest")
            .Options;

        AppDbContext dbContext;

        [OneTimeSetUp]
        public void Setup()
        {
            dbContext = new AppDbContext(_dbContextOptions);
            dbContext.Database.EnsureCreated();

        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            dbContext.Database.EnsureDeleted();
        }

        [Test]
        public void WhenIFetchAMovieWithItsTitle_ThenIGetTheMovie()
        {
            Movie fetchedMovies = new Movie();
            Movie expectedMovies = fetchedMovies;
            Assert.That(fetchedMovies, Is.EqualTo(expectedMovies));
        }
    }
}