﻿using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.DTOs;
using BillB0ard_API.Domain.Exception;
using BillB0ard_API.Services;

namespace BillB0ard_API.Test.MovieServiceTest
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
                Note = 2.0M,
            };

            await movieService.Rate(rateCreation);
            Assert.Multiple(() =>
            {
                Assert.That(_dbContext.Rates.Any(r => r.Equals(expectedRate)), Is.True);
                Assert.That(_dbContext.Movies.FirstOrDefault(m => m.Id == 2).SeenDate?.ToShortDateString(),
                    Is.EqualTo(DateTime.Now.ToShortDateString()));
            });
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

        [TestCase(11.0)]
        [TestCase(-1.0)]
        [TestCase(10.1)]
        public void RateMustBeBetween0To10(decimal rate)
        {
            MovieService movieServices = new(_movieRepository, _rateRepository);
            RateCreationDto rateCreation = new(1, 1, rate);

            var ex = Assert.ThrowsAsync<RateLimitException>(async () => await movieServices.Rate(rateCreation));

            Assert.That(ex.Message, Is.EqualTo($"The rate must be between 0 and 10. Actual : {rate}"));
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
