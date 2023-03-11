using Microsoft.Extensions.Configuration;
using MovieCrew.Core.Domain.Movies.Interfaces;
using MovieCrew.Core.Domain.ThirdPartyMovieProvider.Entities;
using MovieCrew.Core.Domain.ThirdPartyMovieProvider.Exception;
using MovieCrew.Core.Domain.ThirdPartyMovieProvider.Services;

namespace MovieCrew_Core.Test.ThirdPartyMovieProvider
{
    public class ThirdPartyMovieDataTest
    {
        private string _apiKey;
        private string _apiUrl;

        [SetUp]
        public void SetUp()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<ThirdPartyMovieDataTest>()
                .Build();

            _apiKey = configuration["ThirdPartyMovieData:apiKey"];
            _apiUrl = configuration["ThirdPartyMovieData:url"];

        }

        [Test]
        public async Task FetchDetailsFromThirdParty()
        {
            IThirdPartyMovieDataProvider thirdPartyProvider = new TmbdMovieDataProvider(_apiUrl, _apiKey);

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
            IThirdPartyMovieDataProvider thirdPartyProvider = new TmbdMovieDataProvider(_apiUrl, _apiKey);

            Assert.ThrowsAsync<NoMetaDataFound>(async () =>
            {
                //use random hash to be sure a movie will not have this title
                await thirdPartyProvider.GetDetails("hJjK9pLm3tRq");
            }, "No metadata found for the movie hJjK9pLm3tRq.");
        }

        [Test]
        public void DidNotConnectToThirdParty()
        {
            IThirdPartyMovieDataProvider thirdPartyProvider = new TmbdMovieDataProvider("http://test/", "");

            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await thirdPartyProvider.GetDetails("Iron Man");
            });
        }

        [Test]
        public void UnauthorizedAccessToThirdParty()
        {
            IThirdPartyMovieDataProvider thirdPartyProvider = new TmbdMovieDataProvider(_apiUrl, "");

            Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await thirdPartyProvider.GetDetails("Iron Man");
            }, "Response status code does not indicate success: 401 (Unauthorized).");
        }
    }
}
