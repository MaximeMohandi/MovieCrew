using Microsoft.Extensions.Configuration;
using MovieCrew.Core.Domain.ThirdPartyMovieProvider.Services;
using MovieCrew_core.Domain.Movies.Entities;
using MovieCrew_core.Domain.Movies.Exception;
using MovieCrew_core.Domain.Movies.Services;
using MovieCrew_core.Domain.Users.Exception;
using MovieCrew_Core.Test.ThirdPartyMovieProvider;

namespace MovieCrew_core.Test.Movies
{
    public class AddMovieTest : InMemoryMovieTestBase
    {
        private string _apiKey;
        private string _apiUrl;
        public override void SetUp()
        {
            base.SetUp();
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<ThirdPartyMovieDataTest>()
                .Build();

            _apiKey = configuration["ThirdPartyMovieData:apiKey"];
            _apiUrl = configuration["ThirdPartyMovieData:url"];
        }

        [Test]
        public async Task AddMovie()
        {
            MovieService service = new(_movieRepository, new TmbdMovieDataProvider(_apiUrl, _apiKey));

            MovieEntity addedMovie = await service.AddMovie("Pinnochio", 1);

            Assert.Multiple(() =>
            {
                Assert.That(addedMovie.Title, Is.EqualTo("Pinnochio"));
                Assert.That(Uri.TryCreate(addedMovie.Poster, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps), Is.True);
                Assert.That(addedMovie.DateAdded.ToShortDateString(), Is.EqualTo(DateTime.Now.ToShortDateString()));
                Assert.That(_dbContext.Movies.Any(m => m.Name == addedMovie.Title), Is.True);
            });
        }

        [Test]
        public void CantAddMovieThatDoNotExist()
        {
            MovieService service = new(_movieRepository, new TmbdMovieDataProvider(_apiUrl, _apiKey));

            Assert.ThrowsAsync<MovieNotFoundException>(() => service.AddMovie("dsfsdfsdaaa", 1));
        }

        [Test]
        public void CantAddMovieProposedByUnknownUser()
        {
            MovieService service = new(_movieRepository, new TmbdMovieDataProvider(_apiUrl, _apiKey));

            Assert.ThrowsAsync<UserNotFoundException>(() => service.AddMovie("The Asada Family", 2));
        }

        [Test]
        public void CantAddExistMovie()
        {
            MovieService service = new(_movieRepository, new TmbdMovieDataProvider(_apiUrl, _apiKey));

            Assert.ThrowsAsync<MovieAlreadyExistException>(() => service.AddMovie("The Fifth element", null));
        }

        protected override void SeedInMemoryDatas()
        {
            _dbContext.Movies.Add(new()
            {
                Id = 1,
                DateAdded = DateTime.Now,
                Name = "The Fifth element",
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
