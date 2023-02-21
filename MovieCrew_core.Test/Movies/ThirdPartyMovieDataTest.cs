using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Services;

namespace MovieCrew_core.Test.Movies
{
    public class ThirdPartyMovieDataTest
    {
        [Test]
        public void AllRequiredMetaData()
        {
            IThirdPartyMovieData thirdPartyData = new TmdbMovieData();
            MovieMetadataEntity actual = thirdPartyData.SearchMovieData("a movie");

            Assert.Multiple(() =>
            {
                Assert.That(Uri.IsWellFormedUriString(actual.PosterLink, UriKind.Absolute), Is.True);
                Assert.That(actual.Description, Is.Not.Null);
                Assert.That(actual.Revenue, Is.GreaterThanOrEqualTo(0));
                Assert.That(actual.Ratings, Is.GreaterThanOrEqualTo(0));
            });
        }
    }
}
