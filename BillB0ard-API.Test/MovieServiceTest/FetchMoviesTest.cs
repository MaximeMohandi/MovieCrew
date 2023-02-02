using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Enums;
using BillB0ard_API.Domain.Exception;
using BillB0ard_API.Services;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Test.MovieServiceTest
{
    public class FetchMovieTest : InMemoryMovieTestBase
    {
        [Test]
        public async Task All()
        {
            MovieService movieServices = new(_movieRepository, _rateRepository);
            List<MovieEntity> expected = new()
            {
                new MovieEntity(1,"Lord of the ring", "fakeLink",new DateTime(2022, 5, 10), new DateTime(2022, 5, 12), 5.75M),
                new MovieEntity(2,"Harry Potter", null,new DateTime(2015, 8, 3),null, null),
                new MovieEntity(3,"Jurassic Park", "fakeLink",new DateTime(1996, 9, 21), new DateTime(1996, 9, 23),4.25M),
                new MovieEntity(4,"Lord of the ring II", "fakeLink",new DateTime(2022, 10, 15), null, null)
            };

            var actual = await movieServices.FetchAllMovies();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void NoMoviesAtAll()
        {
            var _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieDbTest2")
            .Options;

            using AppDbContext db = new(_options);
            MovieService movieServices = new(new(db), new(db));

            Assert.ThrowsAsync<NoMoviesFoundException>(
                async () => await movieServices.FetchAllMovies(),
                "It seem that there's no movies in the list. Please try to add new one"
            );
        }

        [TestCase("Lord of the ring")]
        [TestCase("lord of the ring")]
        [TestCase("Lord Of The Ring")]
        [TestCase("loRd of tHe rIng")]
        public async Task MovieDetailByTitle(string title)
        {
            MovieService movieService = new(_movieRepository, _rateRepository);

            List<MovieRateEntity> expectedRates = new()
            {
                new(new(1, "Jabba", UserRoles.User), 10.0M),
                new(new(2, "Dudley", UserRoles.User), 2.0M),
                new(new(3, "T-Rex", UserRoles.User), 5.25M),
            };
            MovieDetailsEntity expected = new(1, "Lord of the ring", "fakeLink", new DateTime(2022, 5, 10), new DateTime(2022, 5, 12), 5.75M, expectedRates);


            MovieDetailsEntity actual = await movieService.GetByTitle(title);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void TitleNotFound()
        {
            MovieService movieServices = new(_movieRepository, _rateRepository);

            MovieNotFoundException ex = Assert.ThrowsAsync<MovieNotFoundException>(async () => await movieServices.GetByTitle("star wars VIII"));

            Assert.That(ex.Message, Is.EqualTo("star wars VIII cannot be found. Please check the title and retry."));
        }

        [Test]
        public async Task MovieDetailById()
        {
            MovieService movieService = new(_movieRepository, _rateRepository);

            List<MovieRateEntity> expectedRates = new()
            {
                new(new(1, "Jabba", UserRoles.User), 10.0M),
                new(new(2, "Dudley", UserRoles.User), 2.0M),
                new(new(3, "T-Rex", UserRoles.User), 5.25M),
            };
            MovieDetailsEntity expected = new(1, "Lord of the ring", "fakeLink", new DateTime(2022, 5, 10), new DateTime(2022, 5, 12), 5.75M, expectedRates);


            MovieDetailsEntity actual = await movieService.GetById(1);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void IdNotFound()
        {
            MovieService movieServices = new(_movieRepository, _rateRepository);

            MovieNotFoundException ex = Assert.ThrowsAsync<MovieNotFoundException>(async () => await movieServices.GetById(-1));

            Assert.That(ex.Message, Is.EqualTo("There's no movie with the id : -1. Please check the given id and retry."));
        }

        [Test]
        public async Task MovieDetailButWithNoRates()
        {
            MovieService movieService = new(_movieRepository, _rateRepository);

            MovieDetailsEntity expected = new(2, "Harry Potter", null, new DateTime(2015, 8, 3), null, null, null);

            MovieDetailsEntity actual = await movieService.GetById(2);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public async Task RandomMovieFromUnseenList()
        {
            MovieService movieServices = new(_movieRepository, _rateRepository);

            MovieEntity randomMovie = await movieServices.RandomMovie();

            Assert.That(randomMovie.Id, Is.EqualTo(2).Or.EqualTo(4));
        }

        [Test]
        public void NoMovieLeftToWatch()
        {
            var _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieDbTest2")
            .Options;

            using AppDbContext db = new(_options);
            MovieService movieServices = new(new(db), new(db));

            AllMoviesHaveBeenSeenException ex = Assert.ThrowsAsync<AllMoviesHaveBeenSeenException>(async () => await movieServices.RandomMovie());

            Assert.That(ex.Message, Is.EqualTo("It seems that you have seen all the movies in the list. Please try to add new one"));

        }

        protected override void SeedInMemoryDatas()
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
                    Role = 2,
                },
                new User()
                {
                    Id = 2,
                    Name = "Dudley",
                    Role = 2,
                },
                new User()
                {
                    Id = 3,
                    Name = "T-Rex",
                    Role = 2,
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
        }

    }
}