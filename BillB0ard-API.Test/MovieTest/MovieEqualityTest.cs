using BillB0ard_API.Domain.Entities;

namespace BillB0ard_API.Test.Movies
{
    public class MovieEqualityTest
    {
        [Test]
        public void SameMovie()
        {
            var ratedMovies = new MovieEntity(1, "Lord of the ring", "fakeLink", new DateTime(2022, 5, 10), new DateTime(2022, 5, 12));
            ratedMovies.Rates = new List<RateEntity>()
            {
                new(ratedMovies, new(1, "Jabba"), 10.0M),
                new(ratedMovies, new(2, "Dudley"), 2.0M),
                new(ratedMovies, new(3, "T-Rex"), 5.25M),
            };
            var expextedMovie = ratedMovies;
            expextedMovie.Rates = new List<RateEntity>()
            {
                new(expextedMovie, new(1, "Jabba"), 10.0M),
                new(expextedMovie, new(2, "Dudley"), 2.0M),
                new(expextedMovie, new(3, "T-Rex"), 5.25M),
            };

            Assert.That(ratedMovies, Is.EqualTo(expextedMovie));
        }
    }
}