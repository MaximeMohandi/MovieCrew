using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Exception;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.Core.Test.Users
{
    public class FetchSpectatorTest : InMemoryMovieTestBase
    {
        [Test]
        public async Task OneSpectatorWithAllItsMovies()
        {
            SpectatorService service = new(_userRepository);

            SpectatorDetailsEntity actual = await service.FetchSpectator(1);

            var expected = new SpectatorDetailsEntity(new(1, "Alyssa"), new()
            {
                new(new MovieEntity(1, "Babylon", null, new(2023, 2, 8), new(2023, 2, 9), 3M), 3M)
            });

            Assert.Multiple(() =>
            {
                Assert.That(actual, Is.EqualTo(expected));
                Assert.That(actual.GetHashCode(), Is.EqualTo(expected.GetHashCode()));
            });
        }

        [Test]
        public void IsUserButNotSpectator()
        {
            SpectatorService service = new(_userRepository);
            Assert.ThrowsAsync<UserIsNotSpectatorException>(async () =>
            {
                await service.FetchSpectator(2);
            }, "The user 2 did not rate any movie yet.");
        }

        [Test]
        public void SpectatorDoNotExist()
        {
            SpectatorService service = new(_userRepository);
            Assert.ThrowsAsync<UserNotFoundException>(async () =>
            {
                await service.FetchSpectator(-1);
            }, "User with id: -1 not found. Please check the conformity and try again");
        }

        protected override void SeedInMemoryDatas()
        {
            Movie[] movies = new Movie[]
            {
                new Movie()
                {
                    Id = 1,
                    Name = "Babylon",
                    Poster = null,
                    DateAdded = new(2023, 2, 8),
                    SeenDate = new(2023, 2, 9),
                }
            };
            _dbContext.Movies.AddRange(movies);

            User[] users = new User[]
            {
                new User()
                {
                    Id = 1,
                    Name = "Alyssa",
                    Role = 0
                },
                new User()
                {
                    Id = 2,
                    Name = "Tom",
                    Role = 1
                }
            };
            _dbContext.Users.AddRange(users);

            Rate[] rates = new Rate[]
            {
                new Rate()
                {
                    MovieId= 1,
                    UserId= 1,
                    Note=3M
                }
            };
            _dbContext.Rates.AddRange(rates);

        }
    }
}
