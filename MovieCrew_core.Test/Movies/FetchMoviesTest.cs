using Microsoft.EntityFrameworkCore;
using MovieCrew.Core.Data;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Repository;
using MovieCrew.Core.Domain.Movies.Services;

namespace MovieCrew.Core.Test.Movies;

public class FetchMovieTest : InMemoryMovieTestBase
{
    [Test]
    public async Task All()
    {
        MovieService movieServices = new(_movieRepository, _fakeDataProvider.Object);
        List<MovieEntity> expected = new()
        {
            new MovieEntity(1, "Lord of the ring", "fakeLink", "the best movie ever", new DateTime(2022, 5, 10),
                new DateTime(2022, 5, 12), 5.75M),
            new MovieEntity(3, "Jurassic Park", "fakeLink", "not the best movie ever", new DateTime(1996, 9, 21),
                new DateTime(1996, 9, 23), 4.25M),
            new MovieEntity(4, "Lord of the ring II", "fakeLink", "the second best movie ever",
                new DateTime(2022, 10, 15), null, null)
        };

        var actual = await movieServices.FetchAllMovies();

        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void NoMoviesAtAll()
    {
        var _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("MovieDbTest2")
            .Options;

        using AppDbContext db = new(_options);
        MovieService movieServices = new(new MovieRepository(db), _fakeDataProvider.Object);

        Assert.ThrowsAsync<NoMoviesFoundException>(
            async () => await movieServices.FetchAllMovies(),
            "It seem that there's no movies in the list. Please try to add new one"
        );
    }

    [Test]
    public async Task RandomMovieFromUnseenList()
    {
        MovieService movieServices = new(_movieRepository, _fakeDataProvider.Object);

        var randomMovie = await movieServices.RandomMovie();

        Assert.That(randomMovie.Id, Is.EqualTo(2).Or.EqualTo(4));
    }

    [Test]
    public void NoMovieLeftToWatch()
    {
        var _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("MovieDbTest2")
            .Options;

        using AppDbContext db = new(_options);
        MovieService movieServices = new(new MovieRepository(db), _fakeDataProvider.Object);

        var ex = Assert.ThrowsAsync<AllMoviesHaveBeenSeenException>(async () => await movieServices.RandomMovie());

        Assert.That(ex.Message,
            Is.EqualTo("It seems that you have seen all the movies in the list. Please try to add new one"));
    }

    protected override void SeedInMemoryDatas()
    {
        Movie[] movies =
        {
            new Movie
            {
                Id = 1,
                DateAdded = new DateTime(2022, 5, 10),
                Name = "Lord of the ring",
                Poster = "fakeLink",
                Description = "the best movie ever",
                SeenDate = new DateTime(2022, 5, 12)
            },
            new Movie
            {
                Id = 3,
                DateAdded = new DateTime(1996, 9, 21),
                Name = "Jurassic Park",
                Poster = "fakeLink",
                Description = "not the best movie ever",
                SeenDate = new DateTime(1996, 9, 23)
            },
            new Movie
            {
                Id = 4,
                DateAdded = new DateTime(2022, 10, 15),
                Name = "Lord of the ring II",
                Poster = "fakeLink",
                Description = "the second best movie ever",
                SeenDate = null
            }
        };

        _dbContext.Movies.AddRange(movies);

        User[] users =
        {
            new User
            {
                Id = 1,
                Name = "Jabba",
                Role = 2
            },
            new User
            {
                Id = 2,
                Name = "Dudley",
                Role = 2
            },
            new User
            {
                Id = 3,
                Name = "T-Rex",
                Role = 2
            }
        };

        _dbContext.Users.AddRange(users);

        Rate[] rates =
        {
            new Rate
            {
                MovieId = 1,
                UserId = 1,
                Note = 10.0M
            },
            new Rate
            {
                MovieId = 1,
                UserId = 2,
                Note = 2.0M
            },
            new Rate
            {
                MovieId = 1,
                UserId = 3,
                Note = 5.25M
            },

            new Rate
            {
                MovieId = 3,
                UserId = 1,
                Note = 0
            },
            new Rate
            {
                MovieId = 3,
                UserId = 2,
                Note = 8.50M
            }
        };

        _dbContext.Rates.AddRange(rates);
    }
}