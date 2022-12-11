using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Exception;
using BillB0ard_API.Domain.Repository;
using BillB0ard_API.Services;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Test.Movies
{
    public class FetchMovieTest
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieDbTest")
            .Options;

        private AppDbContext _dbContext;
        private MovieRepository _movieRepository;
        private RateRepository _rateRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            _dbContext = new AppDbContext(_dbContextOptions);
            _dbContext.Database.EnsureCreated();

            SeedInMemoryDatas();

            _movieRepository = new MovieRepository(_dbContext);

        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task ByTitle()
        {
            MovieService movieService = new(_movieRepository, _rateRepository);

            MovieEntity fetchedMovies = await movieService.GetByTitle("Lord of the ring");

            Assert.That(fetchedMovies.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task ByTitleIgnoreCase()
        {
            MovieService movieRepository = new(_movieRepository, _rateRepository);

            MovieEntity fetchedMovies = await movieRepository.GetByTitle("lord of The Ring");

            Assert.That(fetchedMovies.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task ById()
        {
            MovieService movieService = new(_movieRepository, _rateRepository);

            MovieEntity fetchedMovies = await movieService.GetById(1);

            Assert.That(fetchedMovies.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task All()
        {
            MovieService movieServices = new(_movieRepository, _rateRepository);
            List<MovieEntity> expected = new()
            {
                new MovieEntity(1,"Lord of the ring", "fakeLink",new DateTime(2022, 5, 10), new DateTime(2022, 5, 12)),
                new MovieEntity(2,"Harry Potter", null,new DateTime(2015, 8, 3),null),
                new MovieEntity(3,"Jurassic Park", "fakeLink",new DateTime(1996, 9, 21), new DateTime(1996, 9, 23)),
                new MovieEntity(4,"Lord of the ring II", "fakeLink",new DateTime(2022, 10, 15), null)
            };

            var fetchedMovies = await movieServices.FetchAllMovies();

            CollectionAssert.AreEqual(expected, fetchedMovies);
        }

        [Test]
        public async Task MovieWithRates()
        {
            MovieService movieServices = new(_movieRepository, _rateRepository);
            var baseMovie = new MovieEntity(1, "Lord of the ring", "fakeLink", new DateTime(2022, 5, 10), new DateTime(2022, 5, 12)) { Rates = null };
            var expectedRates = new List<RateEntity>()
            {
                new(baseMovie, new(1, "Jabba"), 10.0M),
                new(baseMovie, new(2, "Dudley"), 2.0M),
                new(baseMovie, new(3, "T-Rex"), 5.25M),
            };

            MovieEntity fetchedMovies = await movieServices.GetById(1);

            Assert.Multiple(() =>
            {
                CollectionAssert.AreEqual(expectedRates, fetchedMovies.Rates);
                Assert.That(fetchedMovies.AverageRate, Is.EqualTo(5.75m));
                Assert.That(fetchedMovies.LowestRates, Is.EqualTo(2M));
                Assert.That(fetchedMovies.TopRate, Is.EqualTo(10M));
            });

        }

        [Test]
        public void TitleNotFound()
        {
            MovieService movieServices = new(_movieRepository, _rateRepository);

            MovieNotFoundException ex = Assert.ThrowsAsync<MovieNotFoundException>(async () => await movieServices.GetByTitle("star wars VIII"));

            Assert.That(ex.Message, Is.EqualTo("star wars VIII cannot be found. Please check the title and retry."));
        }

        [Test]
        public void IdNotFound()
        {
            MovieService movieServices = new(_movieRepository, _rateRepository);

            MovieNotFoundException ex = Assert.ThrowsAsync<MovieNotFoundException>(async () => await movieServices.GetById(-1));

            Assert.That(ex.Message, Is.EqualTo("There's no movie with the id : -1. Please check the given id and retry."));
        }




        private void SeedInMemoryDatas()
        {
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
                },
                new Movie()
                {
                    Id = 3,
                    DateAdded = new DateTime(1996, 9, 21),
                    Name = "Jurassic Park",
                    Poster = "fakeLink",
                    SeenDate = new DateTime(1996, 9, 23),
                },
                new Movie()
                {
                    Id = 4,
                    DateAdded = new DateTime(2022, 10, 15),
                    Name = "Lord of the ring II",
                    Poster = "fakeLink",
                    SeenDate = null
                },

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
                },

                new Rate()
                {
                    MovieId= 3,
                    UserId= 1,
                    Note = 0
                },
                new Rate()
                {
                    MovieId= 3,
                    UserId= 2,
                    Note = 8.50M
                },

            };

            _dbContext.Rates.AddRange(rates);

            _dbContext.SaveChanges();
        }
    }
}