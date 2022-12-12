using BillB0ard_API.Data.Models;
using BillB0ard_API.Services;

namespace BillB0ard_API.Test.MovieTest
{
    public class UpdateMovie : InMemoryMovieTestBase
    {
        [Test]
        public async Task Rename()
        {
            MovieService movieService = new MovieService(_movieRepository, _rateRepository);

            await movieService.ChangeTitle(new(1, "Asterix & Obelix : Mission Cléopatre", "nouveau nom"));

            Assert.That(_dbContext.Movies.First(m => m.Name == "nouveau nom").Name, Is.EqualTo("nouveau nom"));
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
