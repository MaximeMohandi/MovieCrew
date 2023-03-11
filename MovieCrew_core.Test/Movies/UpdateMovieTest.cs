using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Dtos;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Services;

namespace MovieCrew.Core.Test.Movies
{
    public class UpdateMovie : InMemoryMovieTestBase
    {
        [Test]
        public async Task Rename()
        {
            MovieService movieService = new(_movieRepository);

            await movieService.ChangeTitle(new(1, "Asterix & Obelix : Mission Cléopatre", "nouveau nom"));

            Assert.That(_dbContext.Movies.First(m => m.Name == "nouveau nom").Name, Is.EqualTo("nouveau nom"));
        }

        [Test]
        public void CantRenameAMovieWithAlreadyUsedTitle()
        {
            MovieService movieService = new(_movieRepository);
            MovieRenameDto renameMovieData = new(1, "Asterix & Obelix : Mission Cléopatre", "Asterix & Obelix : Mission Cléopatre");

            MovieAlreadyExistException ex = Assert.ThrowsAsync<MovieAlreadyExistException>(async () => await movieService.ChangeTitle(renameMovieData));

            Assert.That(ex.Message, Is.EqualTo($"Asterix & Obelix : Mission Cléopatre is already in the list."));
        }


        [Test]
        public async Task SetSeenDate()
        {
            MovieService movieService = new(_movieRepository);
            var updatedMovie = _dbContext.Movies.Single(m => m.Id == 1);

            await movieService.SetSeenDate(new MovieSetSeenDateDto(1, DateTime.Now));

            Assert.That(updatedMovie?.SeenDate?.Date, Is.EqualTo(DateTime.Now.Date));
        }

        protected override void SeedInMemoryDatas()
        {
            _dbContext.Movies.Add(
                new Movie()
                {
                    Id = 1,
                    DateAdded = new DateTime(2022, 12, 12),
                    Name = "Asterix & Obelix : Mission Cléopatre",
                    Poster = "",
                    Description = "Description",
                    Rates = null,
                    SeenDate = null
                });

        }
    }
}
