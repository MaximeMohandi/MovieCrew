using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Exception;
using BillB0ard_API.Services;

namespace BillB0ard_API.Test.Movies
{
    public class AddMovieTest : InMemoryMovieTestBase
    {

        [Test]
        public async Task OnlyTitle()
        {
            MovieService service = new(_movieRepository, _rateRepository);

            MovieEntity addedMovie = await service.AddMovie(new("Dragon"));

            Assert.Multiple(() =>
            {
                Assert.That(addedMovie.Title, Is.EqualTo("Dragon"));
            });
        }

        [Test]
        public async Task OneWithPoster()
        {
            MovieService service = new(_movieRepository, _rateRepository);

            MovieEntity addedMovie = await service.AddMovie(new("Pinnochio", "fakelink"));

            Assert.Multiple(() =>
            {
                Assert.That(addedMovie.Title, Is.EqualTo("Pinnochio"));
                Assert.That(addedMovie.Poster, Is.EqualTo("fakelink"));
            });
        }

        [Test]
        public void CantAddExistMovie()
        {
            MovieService service = new(_movieRepository, _rateRepository);

            Assert.ThrowsAsync<MovieAlreadyExistException>(() => service.AddMovie(new("The Fith element")));
        }

        protected override void SeedInMemoryDatas()
        {
            _dbContext.Movies.Add(new()
            {
                Id = 1,
                DateAdded = DateTime.Now,
                Name = "The Fith element"
            });
        }
    }
}
