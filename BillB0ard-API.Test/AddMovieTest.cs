using BillB0ard_API.Data;
using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Test
{
    public class AddMovieTest
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieDbTest")
            .Options;

        AppDbContext _dbContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dbContext = new AppDbContext(_dbContextOptions);
            _dbContext.Database.EnsureCreated();
            _dbContext.SaveChanges();
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task AddOne()
        {
            MovieRepository movieService = new(_dbContext);

            MovieEntity addedMovie = await movieService.Add("Pinnochio");

            Assert.That(addedMovie.Id, Is.EqualTo(1));
            Assert.That(addedMovie.Title, Is.EqualTo("Pinnochio"));
        }

        [Test]
        public async Task AddOne1()
        {
            MovieRepository movieService = new(_dbContext);

            MovieEntity addedMovie = await movieService.Add("Pinnochio", "fakelink");

            Assert.Multiple(() =>
            {
                Assert.That(addedMovie.Id, Is.EqualTo(1));
                Assert.That(addedMovie.Title, Is.EqualTo("Pinnochio"));
                Assert.That(addedMovie.Poster, Is.EqualTo("fakelink"));
            });
        }


    }
}
