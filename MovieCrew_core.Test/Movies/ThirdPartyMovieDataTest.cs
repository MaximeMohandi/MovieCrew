using Moq;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Exception;
using MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Services;

namespace MovieCrew.Core.Test.Movies;

public class ThirdPartyMovieDataTest
{
    private Mock<IThirdPartyMovieDataProvider> _fakeDataProvider;

    [SetUp]
    public void SetUp()
    {
        _fakeDataProvider = new Mock<IThirdPartyMovieDataProvider>();
    }

    [Test]
    public async Task ThirdPartyProviderShouldReturnValidData()
    {
        // Arrange
        _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("https://maximemohandi.fr/", "loremp ipsum", 8, 8));

        // Act
        var actual = await _fakeDataProvider.Object.GetDetails("Titanic");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(Uri.TryCreate(actual.PosterLink, UriKind.Absolute, out var uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps), Is.True);
            Assert.That(actual.Description, Has.Length.GreaterThan(0));
            Assert.That(actual.Ratings, Is.GreaterThanOrEqualTo(0).And.LessThanOrEqualTo(10));
            Assert.That(actual.Revenue, Is.GreaterThanOrEqualTo(0));
        });
    }

    [Test]
    public void ShouldThrowExceptionWhenNoMetadataFoundForMovieTitle()
    {
        _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ThrowsAsync(new NoMetaDataFoundException("hJjK9pLm3tRq"));
        var thirdPartyProvider = _fakeDataProvider.Object;

        Assert.ThrowsAsync<NoMetaDataFoundException>(async () =>
        {
            //use random hash to be sure a movie will not have this title
            await thirdPartyProvider.GetDetails("hJjK9pLm3tRq");
        }, "No metadata found for the movie hJjK9pLm3tRq.");
    }

    [Test]
    public void ShouldThrowExceptionWhenCantFetchThirdPartyApi()
    {
        _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ThrowsAsync(new CantFetchThirdPartyApiException());
        var thirdPartyProvider = _fakeDataProvider.Object;

        Assert.ThrowsAsync<CantFetchThirdPartyApiException>(
            async () => { await thirdPartyProvider.GetDetails("Titanic"); },
            "An error occurred while fetching the third party API.");
    }
}
