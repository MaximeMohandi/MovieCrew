using MovieCrew_core.Domain.Movies.Entities;
using MovieCrew_core.Domain.Movies.Exception;
using MovieCrew_core.Domain.Movies.Services;
using MovieCrew_core.Domain.Users.Exception;

namespace MovieCrew_core.Test.Movies
{
    public class AddMovieTest : InMemoryMovieTestBase
    {

        [Test]
        public async Task AddMovie()
        {
            MovieService service = new(_movieRepository);

            MovieEntity addedMovie = await service.AddMovie(new("Pinnochio", "fakelink", 1));

            Assert.Multiple(() =>
            {
                Assert.That(addedMovie.Title, Is.EqualTo("Pinnochio"));
                Assert.That(addedMovie.Poster, Is.EqualTo("fakelink"));
                Assert.That(addedMovie.DateAdded.ToShortDateString(), Is.EqualTo(DateTime.Now.ToShortDateString()));
                Assert.That(_dbContext.Movies.Any(m => m.Name == addedMovie.Title), Is.True);
            });
        }

        [Test]
        public void CantAddMovieProposedByUnknownUser()
        {
            MovieService service = new(_movieRepository);

            Assert.ThrowsAsync<UserNotFoundException>(() => service.AddMovie(new("The Assada Family", "", 2)));
        }

        [Test]
        public void CantAddExistMovie()
        {
            MovieService service = new(_movieRepository);

            Assert.ThrowsAsync<MovieAlreadyExistException>(() => service.AddMovie(new("The Fith element", "", null)));
        }

        protected override void SeedInMemoryDatas()
        {
            _dbContext.Movies.Add(new()
            {
                Id = 1,
                DateAdded = DateTime.Now,
                Name = "The Fith element",
                Poster = ""
            });

            _dbContext.Users.Add(new()
            {
                Id = 1,
                Name = "Geppeto",
                Role = 2
            });
        }
    }
}
