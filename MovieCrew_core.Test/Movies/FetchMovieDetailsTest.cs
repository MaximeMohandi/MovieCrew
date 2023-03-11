using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;

namespace MovieCrew.Core.Test.Movies
{
    public class FetchMovieDetailsTest : InMemoryMovieTestBase
    {

        [TestCase("Lord of the ring")]
        [TestCase("lord of the ring")]
        [TestCase("Lord Of The Ring")]
        [TestCase("loRd of tHe rIng")]
        public async Task MovieDetailByTitle(string title)
        {
            MovieService movieService = new(_movieRepository);

            List<MovieRateEntity> expectedRates = new()
            {
                new(new(1, "Jabba", UserRoles.User), 10.0M),
                new(new(2, "Dudley", UserRoles.User), 2.0M),
                new(new(3, "T-Rex", UserRoles.User), 5.25M),
            };
            MovieDetailsEntity expected = new(1, "Lord of the ring", "fakeLink", "Greatest movie on earth", new DateTime(2022, 5, 10), new DateTime(2022, 5, 12), 5.75M, expectedRates, new UserEntity(1, "Jabba", UserRoles.User));


            MovieDetailsEntity actual = await movieService.GetByTitle(title);

            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.EqualTo(expected));
                Assert.That(actual.GetHashCode(), Is.EqualTo(expected.GetHashCode()));
            });
        }

        [Test]
        public void TitleNotFound()
        {
            MovieService movieServices = new(_movieRepository);

            MovieNotFoundException ex = Assert.ThrowsAsync<MovieNotFoundException>(async () => await movieServices.GetByTitle("star wars VIII"));

            Assert.That(ex.Message, Is.EqualTo("star wars VIII cannot be found. Please check the title and retry."));
        }

        [Test]
        public async Task MovieDetailById()
        {
            MovieService movieService = new(_movieRepository);

            List<MovieRateEntity> expectedRates = new()
            {
                new(new(1, "Jabba", UserRoles.User), 10.0M),
                new(new(2, "Dudley", UserRoles.User), 2.0M),
                new(new(3, "T-Rex", UserRoles.User), 5.25M),
            };
            MovieDetailsEntity expected = new(1, "Lord of the ring", "fakeLink", "Greatest movie on earth", new DateTime(2022, 5, 10), new DateTime(2022, 5, 12), 5.75M, expectedRates, new UserEntity(1, "Jabba", UserRoles.User));


            MovieDetailsEntity actual = await movieService.GetById(1);

            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.EqualTo(expected));
                Assert.That(actual.GetHashCode(), Is.EqualTo(expected.GetHashCode()));
            });
        }

        [Test]
        public void IdNotFound()
        {
            MovieService movieServices = new(_movieRepository);

            MovieNotFoundException ex = Assert.ThrowsAsync<MovieNotFoundException>(async () => await movieServices.GetById(-1));

            Assert.That(ex.Message, Is.EqualTo("There's no movie with the id : -1. Please check the given id and retry."));
        }

        [Test]
        public async Task MovieDetailButWithNoRates()
        {
            MovieService movieService = new(_movieRepository);

            MovieDetailsEntity expected = new(2, "Harry Potter", "", "", new DateTime(2015, 8, 3), null, null, null, null);

            MovieDetailsEntity actual = await movieService.GetById(2);

            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.EqualTo(expected));
                Assert.That(actual.GetHashCode(), Is.EqualTo(expected.GetHashCode()));
            });
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
                    Description = "Greatest movie on earth",
                    SeenDate = new DateTime(2022, 5, 12),
                    ProposedById = 1
                },
                new Movie()
                {
                    Id = 2,
                    DateAdded = new DateTime(2015, 8, 3),
                    Name = "Harry Potter",
                    Poster = "",
                    Description = "",
                    SeenDate = null,
                    ProposedBy = null
                },
                new Movie()
                {
                    Id = 3,
                    DateAdded = new DateTime(1996, 9, 21),
                    Name = "Jurassic Park",
                    Poster = "fakeLink",
                    Description = "",
                    SeenDate = new DateTime(1996, 9, 23),
                    ProposedBy = null
                },
                new Movie()
                {
                    Id = 4,
                    DateAdded = new DateTime(2022, 10, 15),
                    Name = "Lord of the ring II",
                    Poster = "fakeLink",
                    Description = "Second greatest movie on earth",
                    SeenDate = null,
                    ProposedBy = null
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
