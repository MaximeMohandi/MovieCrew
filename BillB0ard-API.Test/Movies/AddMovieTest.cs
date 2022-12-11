using BillB0ard_API.Data;
using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Exception;
using BillB0ard_API.Domain.Repository;
using BillB0ard_API.Services;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Test.Movies
{
    public class AddMovieTest
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieDbTest")
            .Options;

        AppDbContext _dbContext;
        private MovieRepository _movieRepository;
        private RateRepository _rateRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dbContext = new AppDbContext(_dbContextOptions);
            _dbContext.Database.EnsureCreated();
            _dbContext.SaveChanges();

            _movieRepository = new MovieRepository(_dbContext);
            _rateRepository = new RateRepository(_dbContext);
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
            MovieService service = new(_movieRepository, _rateRepository);

            MovieEntity addedMovie = await service.AddMovie(new("Dragon"));

            Assert.Multiple(() =>
            {
                Assert.That(addedMovie.Id, Is.EqualTo(1));
                Assert.That(addedMovie.Title, Is.EqualTo("Dragon"));
            });
        }

        [Test]
        public async Task OneWithPoster()
        {
            MovieService service = new(_movieRepository, _rateRepository);

            MovieEntity addedMovie = await service.AddMovie(new("Pinnochio", "fakelink"));

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
            MovieService service = new(_movieRepository, _rateRepository);

            await service.AddMovie(new("The Fith element"));

            Assert.ThrowsAsync<MovieAlreadyExistException>(() => service.AddMovie(new("The Fith element")));
        }
    }
}
