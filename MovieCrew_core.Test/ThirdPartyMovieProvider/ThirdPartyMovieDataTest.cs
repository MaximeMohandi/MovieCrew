using Microsoft.Extensions.Configuration;
using MovieCrew.Core.Domain.ThirdPartyMovieProvider.Entities;
using MovieCrew.Core.Domain.ThirdPartyMovieProvider.Services;

namespace BillB0ard_API.Test.ThirdPartyMovieProvider
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
            var thirdPartyProvider = new ThirdPartyMovieDataProvider(_apiUrl, _apiKey);

            //use well known movie (top 10 box office) to be sure to get some data from third party
            MovieMetadataEntity actual = await thirdPartyProvider.GetDetails("Titanic");

            Assert.Multiple(() =>
            {
                Uri uriResult;
                Assert.That(Uri.TryCreate(actual.PosterLink, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps), Is.True);
                Assert.That(actual.Description, Has.Length.GreaterThan(0));
                Assert.That(actual.Ratings, Is.GreaterThanOrEqualTo(0).And.LessThanOrEqualTo(10));
                Assert.That(actual.Revenue, Is.GreaterThanOrEqualTo(0));
            });
        }
    }
}
