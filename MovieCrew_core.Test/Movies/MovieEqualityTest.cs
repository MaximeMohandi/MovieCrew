using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Entities;

namespace MovieCrew.Core.Test.Movies;

public class MovieEqualityTest
{
    [Test]
    public void SameMovie()
    {
        var ratedMovies = new MovieEntity(1, "Lord of the ring", "fakeLink", "the best movie",
            new DateTime(2022, 5, 10), new DateTime(2022, 5, 12), 5.75M);
        var expextedMovie = new MovieEntity(1, "Lord of the ring", "fakeLink", "the best movie",
            new DateTime(2022, 5, 10), new DateTime(2022, 5, 12), 5.75M);

        Assert.Multiple(() =>
        {
            Assert.That(ratedMovies, Is.EqualTo(expextedMovie));
            Assert.That(ratedMovies.GetHashCode(), Is.EqualTo(expextedMovie.GetHashCode()));
        });
    }

    [Test]
    public void SameMovieModel()
    {
        var ratedMovie = new Movie
        {
            Id = 1,
            Name = "Lord of the ring",
            Poster = "fakelink",
            DateAdded = new DateTime(2022, 5, 10),
            SeenDate = new DateTime(2022, 5, 12)
        };

        var ratedMovie2 = new Movie
        {
            Id = 1,
            Name = "Lord of the ring",
            Poster = "fakelink",
            DateAdded = new DateTime(2022, 5, 10),
            SeenDate = new DateTime(2022, 5, 12)
        };

        Assert.Multiple(() =>
        {
            Assert.That(ratedMovie, Is.EqualTo(ratedMovie2));
            Assert.That(ratedMovie.GetHashCode(), Is.EqualTo(ratedMovie2.GetHashCode()));
        });
    }

    [Test]
    public void SameMovieModelWithRates()
    {
        var ratedMovie = new Movie
        {
            Id = 1,
            Name = "Lord of the ring",
            Poster = "fakelink",
            DateAdded = new DateTime(2022, 5, 10),
            SeenDate = new DateTime(2022, 5, 12)
        };
        ratedMovie.Rates = new List<Rate>
        {
            new()
            {
                UserId = 1,
                MovieId = 1,
                Note = 1L,
                Movie = ratedMovie,
                User = new User()
            },
            new()
            {
                UserId = 2,
                MovieId = 1,
                Note = 1L,
                Movie = ratedMovie,
                User = new User()
            }
        };

        var ratedMovie2 = new Movie
        {
            Id = 1,
            Name = "Lord of the ring",
            Poster = "fakelink",
            DateAdded = new DateTime(2022, 5, 10),
            SeenDate = new DateTime(2022, 5, 12)
        };
        ratedMovie2.Rates = new List<Rate>
        {
            new()
            {
                UserId = 1,
                MovieId = 1,
                Note = 1L,
                Movie = ratedMovie2,
                User = new User()
            },
            new()
            {
                UserId = 2,
                MovieId = 1,
                Note = 1L,
                Movie = ratedMovie2,
                User = new User()
            }
        };
        Assert.Multiple(() =>
        {
            Assert.That(ratedMovie, Is.EqualTo(ratedMovie2));
            Assert.That(ratedMovie.GetHashCode(), Is.EqualTo(ratedMovie2.GetHashCode()));
        });
    }

    [Test]
    public void SameMovieModelWithDifferentRates()
    {
        var ratedMovie = new Movie
        {
            Id = 1,
            Name = "Lord of the ring",
            Poster = "fakelink",
            DateAdded = new DateTime(2022, 5, 10),
            SeenDate = new DateTime(2022, 5, 12)
        };
        ratedMovie.Rates = new List<Rate>
        {
            new()
            {
                UserId = 1,
                MovieId = 1,
                Note = 1.5M,
                Movie = ratedMovie,
                User = new User()
            },
            new()
            {
                UserId = 2,
                MovieId = 1,
                Note = 0L,
                Movie = ratedMovie,
                User = new User()
            }
        };

        var ratedMovie2 = new Movie
        {
            Id = 1,
            Name = "Lord of the ring",
            Poster = "fakelink",
            DateAdded = new DateTime(2022, 5, 10),
            SeenDate = new DateTime(2022, 5, 12)
        };
        ratedMovie2.Rates = new List<Rate>
        {
            new()
            {
                UserId = 1,
                MovieId = 1,
                Note = 1L,
                Movie = ratedMovie2,
                User = new User()
            },
            new()
            {
                UserId = 2,
                MovieId = 1,
                Note = 1L,
                Movie = ratedMovie2,
                User = new User()
            }
        };
        Assert.That(ratedMovie, Is.Not.EqualTo(ratedMovie2));
    }

    [Test]
    public void MovieModelCompareToNull()
    {
        var actualMovie = new Movie
        {
            Id = 1,
            Name = "Chat Potte 2",
            DateAdded = DateTime.Now
        };
        Assert.That(actualMovie, Is.Not.EqualTo(null));
    }
}