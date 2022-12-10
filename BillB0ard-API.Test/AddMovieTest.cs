using BillB0ard_API.Data;
using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Exception;
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
            _dbContext.SaveChanges();
        }

        [Test]
        public async Task OnlyTitle()
        {
            MovieRepository movieRepository = new(_dbContext);

            MovieEntity addedMovie = await movieRepository.Add("Dragon");

            Assert.That(addedMovie.Id, Is.EqualTo(1));
            Assert.That(addedMovie.Title, Is.EqualTo("Dragon"));
        }

        [Test]
        public async Task OneWithPoster()
        {
            MovieRepository movieRepository = new(_dbContext);

            MovieEntity addedMovie = await movieRepository.Add("Pinnochio", "fakelink");

            Assert.Multiple(() =>
            {
                Assert.That(addedMovie.Id, Is.EqualTo(1));
                Assert.That(addedMovie.Title, Is.EqualTo("Pinnochio"));
                Assert.That(addedMovie.Poster, Is.EqualTo("fakelink"));
            });
        }

        [Test]
        public async Task CantAddExistMovie()
        {
            MovieRepository movieRepository = new(_dbContext);

            movieRepository.Add("The Fith element");

            Assert.ThrowsAsync<MovieException>(() => movieRepository.Add("The Fith element"));
        }
    }
}
