using Moq;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Exception;
using MovieCrew.Core.Domain.Users.Exception;

namespace MovieCrew.Core.Test.Movies;

public class AddMovieTest : InMemoryMovieTestBase
{
    [Test]
    public async Task ShouldAddMovie()
    {
        //Arrange
        _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("https://maximemohandi.fr/", "loremp ipsum", 8, 8, 0));

        //Act
        var addedMovie = await new MovieService(_movieRepository, _fakeDataProvider.Object).AddMovie("Pinnochio", 1);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(addedMovie.Title, Is.EqualTo("Pinnochio"));
            Assert.That(Uri.TryCreate(addedMovie.Poster, UriKind.Absolute, out var uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps), Is.True);
            Assert.That(addedMovie.DateAdded.ToShortDateString(), Is.EqualTo(DateTime.Now.ToShortDateString()));
            Assert.That(addedMovie.Description, Is.EqualTo("loremp ipsum"));
            Assert.That(_dbContext.Movies.Any(m => m.Name == addedMovie.Title), Is.True);
        });
    }

    [Test]
    public void ShouldThrowExceptionWhenNoMetadataFound()
    {
        //Arrange
        _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ThrowsAsync(new NoMetaDataFoundException("dsfsdfsdaaa"));

        //Act
        MovieService service = new(_movieRepository, _fakeDataProvider.Object);

        //Assert
        Assert.ThrowsAsync<MovieNotFoundException>(() => service.AddMovie("dsfsdfsdaaa", 1));
    }

    [Test]
    public void ShouldThrowExceptionWhenUserDoesNotExist()
    {
        //Arrange
        _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("https://maximemohandi.fr/", "loremp ipsum", 8, 8, 0));

        //Act
        MovieService service = new(_movieRepository, _fakeDataProvider.Object);

        //Assert
        Assert.ThrowsAsync<UserNotFoundException>(() => service.AddMovie("The Asada Family", 2));
    }

    [Test]
    public void ShouldThrowExceptionWhenMovieAlreadyExist()
    {
        //Arrange
        _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("https://maximemohandi.fr/", "loremp ipsum", 8, 8, 0));

        //Act
        MovieService service = new(_movieRepository, _fakeDataProvider.Object);

        //Assert
        Assert.ThrowsAsync<MovieAlreadyExistException>(() => service.AddMovie("The Fifth element", null));
    }

    [TestCase(" with space before", "with space before")]
    [TestCase("with space after ", "with space after")]
    [TestCase(" with space before and after ", "with space before and after")]
    [TestCase("nominal case", "nominal case")]
    [TestCase("with extra  spaces  in between", "with extra spaces in between")]
    public async Task ShouldSanitizeTitleBeforeAddingIt(string title, string expected)
    {
        //Arrange
        _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("https://maximemohandi.fr/", "loremp ipsum", 8, 8, 0));

        //Act
        var addedMovie = await new MovieService(_movieRepository, _fakeDataProvider.Object).AddMovie(title, 1);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(addedMovie.Title, Is.EqualTo(expected));
            Assert.That(Uri.TryCreate(addedMovie.Poster, UriKind.Absolute, out var uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps), Is.True);
            Assert.That(addedMovie.DateAdded.ToShortDateString(), Is.EqualTo(DateTime.Now.ToShortDateString()));
            Assert.That(addedMovie.Description, Is.EqualTo("loremp ipsum"));
            Assert.That(_dbContext.Movies.Any(m => m.Name == addedMovie.Title), Is.True);
        });
    }

    protected override void SeedInMemoryDatas()
    {
        _dbContext.Movies.Add(new Movie
        {
            Id = 1,
            DateAdded = DateTime.Now,
            Name = "The Fifth element",
            Description = "Description",
            Poster = "loremp ipsum"
        });

        _dbContext.Users.Add(new User
        {
            Id = 1,
            Name = "Geppeto",
            Role = 2
        });
    }
}
