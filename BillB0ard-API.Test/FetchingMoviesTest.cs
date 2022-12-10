using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Test
{
    public class Tests
    {
        private static DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieDbTest")
            .Options;

        AppDbContext _dbContext;

        [OneTimeSetUp]
        public void Setup()
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

        [OneTimeTearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task ByTitle()
        {
            MovieRepository movieRepository = new MovieRepository(_dbContext);
            MovieEntity fetchedMovies = await movieRepository.GetMovie("Lord of the ring");
            Assert.That(fetchedMovies.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task ByTitleIgnoreCase()
        {
            MovieRepository movieRepository = new MovieRepository(_dbContext);
            MovieEntity fetchedMovies = await movieRepository.GetMovie("lord of The Ring");
            Assert.That(fetchedMovies.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task ById()
        {
            MovieRepository movieRepository = new MovieRepository(_dbContext);
            MovieEntity fetchedMovies = await movieRepository.GetMovie(1);
            Assert.That(fetchedMovies.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task All()
        {
            MovieRepository movieRepository = new MovieRepository(_dbContext);
            List<MovieEntity> expected = new()
            {
                new MovieEntity(1,"Lord of the ring", "fakeLink",new DateTime(2022, 5, 10), new DateTime(2022, 5, 12)),
                new MovieEntity(2,"Harry Potter", null,new DateTime(2015, 8, 3),null),
                new MovieEntity(3,"Jurassic Park", "fakeLink",new DateTime(1996, 9, 21), new DateTime(1996, 9, 23)),
                new MovieEntity(4,"Lord of the ring II", "fakeLink",new DateTime(2022, 10, 15), null)
            };
            var fetchedMovies = await movieRepository.GetAll();

            CollectionAssert.AreEqual(expected, fetchedMovies);
        }

        [Test]
        public async Task MovieWithRates()
        {
            MovieRepository movieRepository = new MovieRepository(_dbContext);
            var ratedMovie = new MovieEntity(1, "Lord of the ring", "fakeLink", new DateTime(2022, 5, 10), new DateTime(2022, 5, 12));
            ratedMovie.Rates = new List<RateEntity>()
            {
                new(ratedMovie, new(1, "Jabba"), 10.0M),
                new(ratedMovie, new(2, "Dudley"), 2.0M),
                new(ratedMovie, new(3, "T-Rex"), 5.25M),
            };

            MovieEntity fetchedMovies = await movieRepository.GetMovie(1);


            Assert.That(ratedMovie.Rates[0].Rate, Is.EqualTo(fetchedMovies.Rates[0].Rate));
        }
    }
}