using Moq;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Interfaces;

namespace MovieCrew.Core.Test.Movies
{
    public class ThirdPartyMovieDataTest
    {
        private Mock<IThirdPartyMovieDataProvider> _fakeDataProvider;

        [SetUp]
        public void SetUp()
        {
            _fakeDataProvider = new Mock<IThirdPartyMovieDataProvider>();
        }

        [Test]
        public async Task FetchDetailsFromThirdParty()
        {
            _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
                .ReturnsAsync(new MovieMetadataEntity("https://maximemohandi.fr/", "loremp ipsum", 8, 8));
            IThirdPartyMovieDataProvider thirdPartyProvider = _fakeDataProvider.Object;

            //use well known movie (top 10 box office) to be sure to get some data from third party
            MovieMetadataEntity actual = await thirdPartyProvider.GetDetails("Titanic");

            //chack that data are in correct format rather than what they are
            Assert.Multiple(() =>
            {
                Assert.That(Uri.TryCreate(actual.PosterLink, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps), Is.True);
                Assert.That(actual.Description, Has.Length.GreaterThan(0));
                Assert.That(actual.Ratings, Is.GreaterThanOrEqualTo(0).And.LessThanOrEqualTo(10));
                Assert.That(actual.Revenue, Is.GreaterThanOrEqualTo(0));
            });
        }

        [Test]
        public void FetchMovieThatDoNotExist()
        {
            _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
                .ThrowsAsync(new NoMetaDataFound("hJjK9pLm3tRq"));
            IThirdPartyMovieDataProvider thirdPartyProvider = _fakeDataProvider.Object;

            Assert.ThrowsAsync<NoMetaDataFound>(async () =>
            {
                //use random hash to be sure a movie will not have this title
                await thirdPartyProvider.GetDetails("hJjK9pLm3tRq");
            }, "No metadata found for the movie hJjK9pLm3tRq.");
        }
    }
}
