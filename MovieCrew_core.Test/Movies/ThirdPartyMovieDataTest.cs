using Microsoft.Extensions.Configuration;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Services;

namespace BillB0ard_API.Test.Movies
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
        public async Task ICanConnectToThirdParty()
        {
            var thirdPartyProvider = new ThirdPartyMovieDataProvider(_apiUrl, _apiKey);

            MovieMetadataEntity actual = await thirdPartyProvider.GetDetails();

            Assert.Multiple(() =>
            {
                Assert.That(Uri.IsWellFormedUriString(actual.PosterLink, UriKind.RelativeOrAbsolute), Is.True);
                Assert.That(actual.Description, Has.Length.GreaterThan(0));
                Assert.That(actual.Ratings, Is.GreaterThanOrEqualTo(0).And.LessThanOrEqualTo(10));
                Assert.That(actual.Revenue, Is.GreaterThanOrEqualTo(0));
            });
            Assert.That(actual, Is.True);
        }
    }
}
