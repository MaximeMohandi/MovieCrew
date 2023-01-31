using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Exception;
using BillB0ard_API.Services;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Test.MovieServiceTest
{
    public class FetchMovieTest : InMemoryMovieTestBase
    {
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

        [Test]
        public async Task FetchARandomMovieInTheUnseenMovie()
        {
            MovieService movieServices = new(_movieRepository, _rateRepository);

            MovieEntity randomMovie = await movieServices.RandomMovie();

            Assert.That(randomMovie.Id, Is.EqualTo(2).Or.EqualTo(4));
        }

        [Test]
        public void IfNoUnseenMovieThrowExceptionWhenGettingRandomMovie()
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
        }

    }
}