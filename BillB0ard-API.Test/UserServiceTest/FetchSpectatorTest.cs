using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.Movies.Entities;
using BillB0ard_API.Domain.Users.Entities;
using BillB0ard_API.Domain.Users.Services;

namespace BillB0ard_API.Test.UserServiceTest
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
