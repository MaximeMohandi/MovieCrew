using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.DTOs;
using BillB0ard_API.Domain.Exception;
using BillB0ard_API.Services;

namespace BillB0ard_API.Test.MovieTest
{
    public class UpdateMovie : InMemoryMovieTestBase
    {
        [Test]
        public async Task Rename()
        {
            MovieService movieService = new(_movieRepository, _rateRepository);

            await movieService.ChangeTitle(new(1, "Asterix & Obelix : Mission Cléopatre", "nouveau nom"));

            Assert.That(_dbContext.Movies.First(m => m.Name == "nouveau nom").Name, Is.EqualTo("nouveau nom"));
        }

        [Test]
        public void CantRenameAMovieWithAlreadyUsedTitle()
        {
            MovieService movieService = new(_movieRepository, _rateRepository);
            MovieRenameDto renameMovieData = new(1, "Asterix & Obelix : Mission Cléopatre", "Asterix & Obelix : Mission Cléopatre");

            MovieAlreadyExistException ex = Assert.ThrowsAsync<MovieAlreadyExistException>(async () => await movieService.ChangeTitle(renameMovieData));

            Assert.That(ex.Message, Is.EqualTo($"Asterix & Obelix : Mission Cléopatre is already in the list."));
        }

        [Test]
        public async Task AddPoster()
        {
            MovieService movieService = new(_movieRepository, _rateRepository);

            await movieService.AddPoster(new(1, "http://newPoster.com"));

            var moviePoster = _dbContext.Movies.First(m => m.Id == 1);

            Assert.That(moviePoster.Poster, Is.EqualTo("http://newPoster.com"));
        }

        [TestCase("htt://www.test.com")]
        [TestCase("alink")]
        [TestCase("www.test.fr")]
        [TestCase("www.test.")]
        [TestCase("poster.com")]
        public void CantAddPosterWithIncorrectFormat(string url)
        {
            MovieService movieService = new(_movieRepository, _rateRepository);

            Assert.ThrowsAsync<MoviePosterFormatException>(async () => await movieService.AddPoster(new(1, url)));
        }

        [Test]
        public async Task SetSeenDate()
        {
            MovieService movieService = new(_movieRepository, _rateRepository);
            var updatedMovie = _dbContext.Movies.Single(m => m.Id == 1);

            await movieService.SetSeenDate(new MovieSetSeenDateDto(1, DateTime.Now));

            Assert.That(updatedMovie?.SeenDate.Value.Date, Is.EqualTo(DateTime.Now.Date));
        }

        protected override void SeedInMemoryDatas()
        {
            _dbContext.Movies.Add(
                new Movie()
                {
                    Id = 1,
                    DateAdded = new DateTime(2022, 12, 12),
                    Name = "Asterix & Obelix : Mission Cléopatre",
                    Poster = null,
                    Rates = null,
                    SeenDate = null
                });

        }
    }
}
