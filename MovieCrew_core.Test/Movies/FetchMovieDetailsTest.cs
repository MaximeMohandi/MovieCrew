using Moq;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;

namespace MovieCrew.Core.Test.Movies;

public class FetchMovieDetailsTest : InMemoryMovieTestBase
{
    [TestCase("Lord of the ring")]
    [TestCase("lord of the ring")]
    [TestCase("Lord Of The Ring")]
    [TestCase("loRd of tHe rIng")]
    public async Task ShouldFetchMovieDetailsByTitle(string title)
    {
        //Arrange
        _fakeDataProvider.Setup(x => x.GetDetails("lord of the ring"))
            .ReturnsAsync(new MovieMetadataEntity("fakeLink", "Greatest movie on earth", 10M, 2555550));
        MovieDetailsEntity expected = new(
            1,
            "Lord of the ring",
            "fakeLink",
            "Greatest movie on earth",
            new DateTime(2022, 5, 10),
            new DateTime(2022, 5, 12),
            5.75M,
            10M,
            2555550,
            new List<MovieRateEntity>
            {
                new(new UserEntity(1, "Jabba", UserRoles.User), 10.0M),
                new(new UserEntity(2, "Dudley", UserRoles.User), 2.0M),
                new(new UserEntity(3, "T-Rex", UserRoles.User), 5.25M)
            },
            new UserEntity(1, "Jabba", UserRoles.User));

        //Act
        var actual = await new MovieService(_movieRepository, _fakeDataProvider.Object).GetMovieDetails(title);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(actual.GetHashCode(), Is.EqualTo(expected.GetHashCode()));
        });
    }

    [Test]
    public void ShouldThrowExceptionWhenMovieWithTitleNotFound()
    {
        //Arrange
        MovieService movieServices = new(_movieRepository, _fakeDataProvider.Object);

        //Assert
        Assert.ThrowsAsync<MovieNotFoundException>(() => movieServices.GetMovieDetails("star wars VIII"),
            "star wars VIII cannot be found. Please check the title and retry.");
    }

    [Test]
    public async Task ShouldFetchMovieDetailsById()
    {
        //Arrange
        _fakeDataProvider.Setup(x => x.GetDetails("lord of the ring"))
            .ReturnsAsync(new MovieMetadataEntity("fakeLink", "Greatest movie on earth", 10M, 2555550));

        MovieDetailsEntity expected = new(
            1,
            "Lord of the ring",
            "fakeLink",
            "Greatest movie on earth",
            new DateTime(2022, 5, 10),
            new DateTime(2022, 5, 12),
            5.75M,
            10M,
            2555550,
            new List<MovieRateEntity>
            {
                new(new UserEntity(1, "Jabba", UserRoles.User), 10.0M),
                new(new UserEntity(2, "Dudley", UserRoles.User), 2.0M),
                new(new UserEntity(3, "T-Rex", UserRoles.User), 5.25M)
            },
            new UserEntity(1, "Jabba", UserRoles.User));

        //Act
        var actual = await new MovieService(_movieRepository, _fakeDataProvider.Object).GetMovieDetails(1);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(actual.GetHashCode(), Is.EqualTo(expected.GetHashCode()));
        });
    }

    [Test]
    public void ShouldThrowExceptionWhenMovieWithIdNotFound()
    {
        //Arrange
        MovieService movieServices = new(_movieRepository, _fakeDataProvider.Object);

        //Act & Assert
        Assert.ThrowsAsync<MovieNotFoundException>(() => movieServices.GetMovieDetails(-1),
            "There's no movie with the id : -1. Please check the given id and retry.");
    }

    [Test]
    public async Task ShouldFetchMovieDetailsEvenIfNoRates()
    {
        //Arrange
        _fakeDataProvider.Setup(x => x.GetDetails("harry potter"))
            .ReturnsAsync(new MovieMetadataEntity("", "", 5M, 2555555M));

        MovieDetailsEntity expected = new(
            2,
            "Harry Potter",
            "",
            "",
            new DateTime(2015, 8, 3),
            null,
            null,
            5M,
            2555555M,
            null,
            null);

        //Act
        var actual = await new MovieService(_movieRepository, _fakeDataProvider.Object).GetMovieDetails(2);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(actual.GetHashCode(), Is.EqualTo(expected.GetHashCode()));
        });
    }

    protected override void SeedInMemoryDatas()
    {
        Movie[] movies =
        {
            new()
            {
                Id = 1,
                DateAdded = new DateTime(2022, 5, 10),
                Name = "Lord of the ring",
                Poster = "fakeLink",
                Description = "Greatest movie on earth",
                SeenDate = new DateTime(2022, 5, 12),
                ProposedById = 1
            },
            new()
            {
                Id = 2,
                DateAdded = new DateTime(2015, 8, 3),
                Name = "Harry Potter",
                Poster = "",
                Description = "",
                SeenDate = null,
                ProposedBy = null
            },
            new()
            {
                Id = 3,
                DateAdded = new DateTime(1996, 9, 21),
                Name = "Jurassic Park",
                Poster = "fakeLink",
                Description = "",
                SeenDate = new DateTime(1996, 9, 23),
                ProposedBy = null
            },
            new()
            {
                Id = 4,
                DateAdded = new DateTime(2022, 10, 15),
                Name = "Lord of the ring II",
                Poster = "fakeLink",
                Description = "Second greatest movie on earth",
                SeenDate = null,
                ProposedBy = null
            }
        };

        _dbContext.Movies.AddRange(movies);

        User[] users =
        {
            new()
            {
                Id = 1,
                Name = "Jabba",
                Role = 2
            },
            new()
            {
                Id = 2,
                Name = "Dudley",
                Role = 2
            },
            new()
            {
                Id = 3,
                Name = "T-Rex",
                Role = 2
            }
        };

        _dbContext.Users.AddRange(users);

        Rate[] rates =
        {
            new()
            {
                MovieId = 1,
                UserId = 1,
                Note = 10.0M
            },
            new()
            {
                MovieId = 1,
                UserId = 2,
                Note = 2.0M
            },
            new()
            {
                MovieId = 1,
                UserId = 3,
                Note = 5.25M
            },

            new()
            {
                MovieId = 3,
                UserId = 1,
                Note = 0
            },
            new()
            {
                MovieId = 3,
                UserId = 2,
                Note = 8.50M
            }
        };

        _dbContext.Rates.AddRange(rates);
    }
}
