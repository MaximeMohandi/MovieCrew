using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Ratings.Exception;
using MovieCrew.Core.Domain.Ratings.Services;

namespace MovieCrew.Core.Test.Ratings;

public class RateMovieTest : InMemoryMovieTestBase
{
    [Test]
    public async Task MovieWithoutRate()
    {
        RatingServices rateService = new(_rateRepository);
        var expectedRate = new Rate
        {
            MovieId = 2,
            UserId = 1,
            Note = 2.0M
        };

        await rateService.RateMovie(2, 1, 2.0M);
        Assert.Multiple(() =>
        {
            Assert.That(_dbContext.Rates.Any(r => r.Equals(expectedRate)), Is.True);
            Assert.That(_dbContext.Movies.First(m => m.Id == 2).SeenDate?.ToShortDateString(),
                Is.EqualTo(DateTime.Now.ToShortDateString()));
        });
    }

    [Test]
    public async Task ExistingRate()
    {
        RatingServices rateService = new(_rateRepository);

        await rateService.RateMovie(1, 1, 0.0M);

        var updatedRate = _dbContext.Rates
            .First(r => r.UserId == 1 && r.MovieId == 1);

        Assert.That(updatedRate.Note, Is.EqualTo(0.0M));
    }

    [TestCase(11.0)]
    [TestCase(-1.0)]
    [TestCase(10.1)]
    public void RateMustBeBetween0To10(decimal rate)
    {
        RatingServices rateService = new(_rateRepository);

        var ex = Assert.ThrowsAsync<RateLimitException>(async () => await rateService.RateMovie(1, 1, rate));

        Assert.That(ex.Message, Is.EqualTo($"The rate must be between 0 and 10. Actual : {rate}"));
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
                Description = "Best movie ever",
                SeenDate = new DateTime(2022, 5, 12)
            },
            new Movie
            {
                Id = 2,
                DateAdded = new DateTime(2015, 8, 3),
                Name = "Harry Potter",
                Description = "good movie",
                Poster = "alink",
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
                Role = 0
            },
            new User
            {
                Id = 2,
                Name = "Dudley",
                Role = 0
            },
            new User
            {
                Id = 3,
                Name = "T-Rex",
                Role = 0
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
            }
        };
        _dbContext.Rates.AddRange(rates);
    }
}