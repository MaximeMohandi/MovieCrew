using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.Entities;

namespace BillB0ard_API.Test.DataEqualityTest
{
    public class MovieEqualityTest
    {
        [Test]
        public void SameMovie()
        {
            var ratedMovies = new MovieEntity(1, "Lord of the ring", "fakeLink", new DateTime(2022, 5, 10), new DateTime(2022, 5, 12));
            ratedMovies.Rates = new List<RateEntity>()
            {
                new(new(1, "Jabba"), 10.0M),
                new(new(2, "Dudley"), 2.0M),
                new(new(3, "T-Rex"), 5.25M),
            };
            var expextedMovie = new MovieEntity(1, "Lord of the ring", "fakeLink", new DateTime(2022, 5, 10), new DateTime(2022, 5, 12));
            expextedMovie.Rates = new List<RateEntity>()
            {
                new(new(1, "Jabba"), 10.0M),
                new(new(2, "Dudley"), 2.0M),
                new(new(3, "T-Rex"), 5.25M),
            };

            Assert.That(ratedMovies, Is.EqualTo(expextedMovie));
        }

        [Test]
        public void SameMovieModel()
        {
            var ratedMovie = new Movie()
            {
                Id = 1,
                Name = "Lord of the ring",
                Poster = "fakelink",
                DateAdded = new DateTime(2022, 5, 10),
                SeenDate = new DateTime(2022, 5, 12)
            };

            var ratedMovie2 = new Movie()
            {
                Id = 1,
                Name = "Lord of the ring",
                Poster = "fakelink",
                DateAdded = new DateTime(2022, 5, 10),
                SeenDate = new DateTime(2022, 5, 12)
            };

            Assert.That(ratedMovie, Is.EqualTo(ratedMovie2));
        }

        [Test]
        public void SameMovieModelWithRates()
        {
            var ratedMovie = new Movie()
            {
                Id = 1,
                Name = "Lord of the ring",
                Poster = "fakelink",
                DateAdded = new DateTime(2022, 5, 10),
                SeenDate = new DateTime(2022, 5, 12)
            };
            ratedMovie.Rates = new List<Rate>
            {
                new Rate()
                {
                    UserId= 1,
                    MovieId= 1,
                    Note=1L,
                    Movie = ratedMovie,
                    User=new User()
                },
                new Rate()
                {
                    UserId= 2,
                    MovieId= 1,
                    Note=1L,
                    Movie = ratedMovie,
                    User=new User()
                },
            };

            var ratedMovie2 = new Movie()
            {
                Id = 1,
                Name = "Lord of the ring",
                Poster = "fakelink",
                DateAdded = new DateTime(2022, 5, 10),
                SeenDate = new DateTime(2022, 5, 12),
            };
            ratedMovie2.Rates = new List<Rate>
            {
                new Rate()
                {
                    UserId= 1,
                    MovieId= 1,
                    Note=1L,
                    Movie = ratedMovie2,
                    User=new User()
                },
                new Rate()
                {
                    UserId= 2,
                    MovieId= 1,
                    Note=1L,
                    Movie = ratedMovie2,
                    User=new User()
                },
            };
            Assert.That(ratedMovie, Is.EqualTo(ratedMovie2));
        }
    }
}