using MovieCrew.Core.Domain.Movies.Services;

namespace BillB0ard_API.Test.Movies
{
    public class ThirdPartyMovieDataTest
    {

        [SetUp]
        public void SetUp()
        {
            //todo get user secret 
        }

        [Test]
        public async Task ICanConnectToThirdParty()
        {
            var thirdPartyProvider = new ThirdPartyMovieDataProvider("", "");

            bool actual = await thirdPartyProvider.GetDetails();

            Assert.That(actual, Is.True);
        }
    }
}
