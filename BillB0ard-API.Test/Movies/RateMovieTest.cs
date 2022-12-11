using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.DTOs;
using BillB0ard_API.Domain.Repository;
using BillB0ard_API.Services;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Test.Movies
{
    public class RateMovieTest
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieDbTest")
            .Options;

        private AppDbContext _dbContext;
        private MovieRepository _movieRepository;
        private RateRepository _rateRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dbContext = new AppDbContext(_dbContextOptions);
            _dbContext.Database.EnsureCreated();


            Movie[] movies = new[]
                        {
                new Movie()
                {
                    Id = 1,
                    DateAdded = new DateTime(2022, 5, 10),
                    Name = "Lord of the ring",
                    Poster = "fakeLink",
                    SeenDate = new DateTime(2022, 5, 12)
                },
                new Movie()
                {
                    Id = 2,
                    DateAdded = new DateTime(2015, 8, 3),
                    Name = "Harry Potter",
                    Poster = null,
                    SeenDate = null
                }

            };
            _dbContext.Movies.AddRange(movies);

            User[] users = new[]
            {
                new User()
                {
                    Id = 1,
                    Name = "Jabba",
                    Role = 0,
                },
                new User()
                {
                    Id = 2,
                    Name = "Dudley",
                    Role = 0,
                },
                new User()
                {
                    Id = 3,
                    Name = "T-Rex",
                    Role = 0,
                },

            };

            _dbContext.Users.AddRange(users);

            Rate[] rates = new[]
            {
                new Rate()
                {
                    MovieId= 1,
                    UserId= 1,
                    Note = 10.0M
                },
                new Rate()
                {
                    MovieId= 1,
                    UserId= 2,
                    Note = 2.0M
                },
                new Rate()
                {
                    MovieId= 1,
                    UserId= 3,
                    Note = 5.25M
                }
            };
            _dbContext.Rates.AddRange(rates);

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
        public async Task MovieWithoutRate()
        {
            MovieService movieService = new(_movieRepository, _rateRepository);
            var rateCreation = new RateCreationDTO(2, 1, 2.0M);
            var expectedRate = new Rate()
            {
                MovieId = 2,
                UserId = 1,
                Note = 2.0M
            };

            await movieService.Rate(rateCreation);

            Assert.That(_dbContext.Rates.Any(r => r.Note == expectedRate.Note && r.MovieId == expectedRate.MovieId && r.UserId == expectedRate.UserId), Is.True);
        }

    }
}
