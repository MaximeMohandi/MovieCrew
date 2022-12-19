using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.DTOs;
using BillB0ard_API.Domain.Exception;
using BillB0ard_API.Services;
using BillB0ard_API.Test.MovieTest;

namespace BillB0ard_API.Test.Movies
{
    public class RateMovieTest : InMemoryMovieTestBase
    {

        [Test]
        public async Task MovieWithoutRate()
        {
            MovieService movieService = new(_movieRepository, _rateRepository);
            var rateCreation = new RateCreationDto(2, 1, 2.0M);
            var expectedRate = new Rate()
            {
                MovieId = 2,
                UserId = 1,
                Note = 2.0M
            };

            await movieService.Rate(rateCreation);

            Assert.That(_dbContext.Rates.Any(r => r.Equals(expectedRate)), Is.True);
        }

        [Test]
        public async Task ExistingRate()
        {
            MovieService movieService = new(_movieRepository, _rateRepository);
            RateCreationDto rateCreation = new(1, 1, 0.0M);

            await movieService.Rate(rateCreation);

            var updatedRate = _dbContext.Rates
                .First(r => r.UserId == rateCreation.UserId && r.MovieId == rateCreation.MovieID);

            Assert.That(updatedRate.Note, Is.EqualTo(0.0M));
        }

        [Test]
        public void CannotRateAbove10()
        {
            MovieService movieServices = new(_movieRepository, _rateRepository);
            RateCreationDto rateCreation = new(1, 1, 11.0M);

            var ex = Assert.ThrowsAsync<RateLimitException>(async () => await movieServices.Rate(rateCreation));

            Assert.That(ex.Message, Is.EqualTo("The rate must be between 0 and 10. Actual : 11,0"));
        }

        [Test]
        public void CannotGiveRateBelowZero()
        {
            MovieService movieServices = new(_movieRepository, _rateRepository);
            RateCreationDto rateCreation = new(1, 1, -1);

            var ex = Assert.ThrowsAsync<RateLimitException>(async () => await movieServices.Rate(rateCreation));

            Assert.That(ex.Message, Is.EqualTo("The rate must be between 0 and 10. Actual : -1"));
        }

        [Test]
        public async Task SetSeenDateToTodayWhenRatingAMovieForTheFirstTieme()
        {
            MovieService movieService = new(_movieRepository, _rateRepository);

            await movieService.Rate(new(2, 1, 10M));

            Movie actual = _dbContext.Movies.Single(x => x.Id == 2);

            Assert.That(actual?.SeenDate.Value.Date, Is.EqualTo(DateTime.Now.Date.Date));
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
                }

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
                }
            };
            _dbContext.Rates.AddRange(rates);
        }

    }
}
