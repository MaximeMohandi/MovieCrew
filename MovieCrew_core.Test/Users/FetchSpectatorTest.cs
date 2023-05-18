using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Exception;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.Core.Test.Users;

public class FetchSpectatorTest : InMemoryMovieTestBase
{
    [Test]
    public async Task ShouldFetchSpectator()
    {
        // Arrange
        SpectatorService service = new(_userRepository);
        var expected = new SpectatorDetailsEntity(new UserEntity(1, "Alyssa"), new List<SpectatorRateEntity>
        {
            new(
                new MovieEntity(1, "Babylon", "", "didn't saw it yet", new DateTime(2023, 2, 8),
                    new DateTime(2023, 2, 9), 3M), 3M)
        });

        // Act
        var actual = await service.FetchSpectator(1);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(actual.GetHashCode(), Is.EqualTo(expected.GetHashCode()));
        });
    }

    [Test]
    public void ShouldThrowExceptionWhenUserIsNotSpectator()
    {
        SpectatorService service = new(_userRepository);
        Assert.ThrowsAsync<UserIsNotSpectatorException>(() => service.FetchSpectator(2),
            "The user 2 did not rate any movie yet.");
    }

    [Test]
    public void ShouldThrowExceptionWhenUserNotFound()
    {
        SpectatorService service = new(_userRepository);
        Assert.ThrowsAsync<UserNotFoundException>(() => service.FetchSpectator(-1),
            "User with id: -1 not found. Please check the conformity and try again");
    }

    protected override void SeedInMemoryDatas()
    {
        Movie[] movies =
        {
            new()
            {
                Id = 1,
                Name = "Babylon",
                Poster = "",
                Description = "didn't saw it yet",
                DateAdded = new DateTime(2023, 2, 8),
                SeenDate = new DateTime(2023, 2, 9)
            }
        };
        _dbContext.Movies.AddRange(movies);

        User[] users =
        {
            new()
            {
                Id = 1,
                Name = "Alyssa",
                Role = 0
            },
            new()
            {
                Id = 2,
                Name = "Tom",
                Role = 1
            }
        };
        _dbContext.Users.AddRange(users);

        Rate[] rates =
        {
            new()
            {
                MovieId = 1,
                UserId = 1,
                Note = 3M
            }
        };
        _dbContext.Rates.AddRange(rates);
    }
}
