﻿using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Exception;
using BillB0ard_API.Services;

namespace BillB0ard_API.Test.Movies
{
    public class AddMovieTest : InMemoryMovieTestBase
    {

        [Test]
        public async Task AddMovie()
        {
            MovieService service = new(_movieRepository, _rateRepository);

            MovieEntity addedMovie = await service.AddMovie(new("Pinnochio", "fakelink"));

            Assert.Multiple(() =>
            {
                Assert.That(addedMovie.Title, Is.EqualTo("Pinnochio"));
                Assert.That(addedMovie.Poster, Is.EqualTo("fakelink"));
                Assert.That(addedMovie.DateAdded.ToShortDateString(), Is.EqualTo(DateTime.Now.ToShortDateString()));
            });
        }

        [Test]
        public void CantAddExistMovie()
        {
            MovieService service = new(_movieRepository, _rateRepository);

            Assert.ThrowsAsync<MovieAlreadyExistException>(() => service.AddMovie(new("The Fith element", "")));
        }

        protected override void SeedInMemoryDatas()
        {
            _dbContext.Movies.Add(new()
            {
                Id = 1,
                DateAdded = DateTime.Now,
                Name = "The Fith element",
                Poster = ""
            });
        }
    }
}
