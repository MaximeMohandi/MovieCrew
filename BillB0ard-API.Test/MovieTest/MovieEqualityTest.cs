using BillB0ard_API.Domain.Entities;

namespace BillB0ard_API.Test.Movies
{
    public class MovieEqualityTest
    {
        [Test]
        public void SameMovie()
        {
            var ratedMovie = new MovieEntity(1, "Lord of the ring", "fakeLink", new DateTime(2022, 5, 10), new DateTime(2022, 5, 12));
            ratedMovie.Rates = new List<RateEntity>()
            {
                new(ratedMovie, new(1, "Jabba"), 10.0M),
                new(ratedMovie, new(2, "Dudley"), 2.0M),
                new(ratedMovie, new(3, "T-Rex"), 5.25M),
            };

            Assert.That(ratedMovie, Is.EqualTo(ratedMovie));
        }
    }
}