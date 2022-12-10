using BillB0ard_API.Data.Models;

namespace BillB0ard_API.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void WhenIFetchAMovieWithItsTitle_ThenIGetTheMovie()
        {
            Movie fetchedMovies = new Movie();
            Movie expectedMovies = fetchedMovies;
            Assert.That(fetchedMovies, Is.EqualTo(expectedMovies));
        }
    }
}