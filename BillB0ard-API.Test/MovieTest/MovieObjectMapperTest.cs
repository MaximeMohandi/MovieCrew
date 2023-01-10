using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Mappers;

namespace BillB0ard_API.Test.MovieTest
{
    public class MovieObjectMapperTest
    {
        [Test]
        public void MapModelWithoutRateToEntity()
        {
            Movie movieModel = new Movie
            {
                Id = 8,
                Name = "Lamar Jackson",
                DateAdded = new(2023, 1, 8),
                Poster = "http://MVP2019.NFL",
                SeenDate = new(2023, 1, 8)
            };
            MovieEntity expectedEntity = new(8, "Lamar Jackson", "http://MVP2019.NFL", new(2023, 1, 8), new(2023, 1, 8));

            MovieEntity entity = movieModel.ToEntity();

            Assert.That(entity, Is.EqualTo(expectedEntity));
        }

        [Test]
        public void MapEntityToModel()
        {
            MovieEntity movieEntity = new(8, "Lamar Jackson", "http://MVP2019.NFL", new(2023, 1, 8), null);
            Movie expectedModel = new Movie
            {
                Id = 8,
                Name = "Lamar Jackson",
                DateAdded = new(2023, 1, 8),
                Poster = "http://MVP2019.NFL"
            };

            Movie movie = movieEntity.ToModel();

            Assert.That(movie, Is.EqualTo(expectedModel));
        }
    }
}
