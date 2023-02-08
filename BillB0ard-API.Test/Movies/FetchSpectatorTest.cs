using BillB0ard_API.Domain.Movies.Entities;

namespace BillB0ard_API.Test.Movies
{
    public class FetchSpectatorTest
    {
        [Test]
        public void OneSpectatorWithAllItsMovies()
        {
            var actual = new SpectatorEntity(new(1, "Manon"), new()
            {
                new(new MovieEntity(1, "Babylon", null, new(2023, 2, 8), new(2023, 2, 9), 3M), 3M)
            });
            var expected = new SpectatorEntity(new(1, "Manon"), new()
            {
                new(new MovieEntity(1, "Babylon", null, new(2023, 2, 8), new(2023, 2, 9), 3M), 3M)
            });
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
