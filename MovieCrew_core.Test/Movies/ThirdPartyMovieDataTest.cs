using Microsoft.Extensions.Configuration;
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

            bool actual = await thirdPartyProvider.GetDetails();

            Assert.That(actual, Is.True);
        }
    }
}
