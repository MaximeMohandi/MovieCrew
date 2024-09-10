using Moq;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.ThirdPartyMovieDataProvider.Exception;

namespace MovieCrew.Core.Test.Movies;

public class RefreshMovieMetaDataTests : InMemoryMovieTestBase
{
    [Test]
    public async Task DescriptionShouldChangeWhenRefreshingData()
    {
        //Arrange
        _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("https://maximemohandi.fr/", "loremp ipsum", 8, 8, 0));
        MovieService movieService = new(_movieRepository, _fakeDataProvider.Object);

        //Act
        await movieService.RefreshMoviesMetaData();

        //Assert
        Assert.That(_dbContext.Movies.First(m => m.Id == 1).Description, Is.EqualTo("loremp ipsum"));
    }

    [Test]
    public async Task PosterShouldChangeWhenRefreshingData()
    {
        //Arrange
        _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("https://maximemohandi.fr/", "loremp ipsum", 8, 8, 0));
        MovieService movieService = new(_movieRepository, _fakeDataProvider.Object);

        //Act
        await movieService.RefreshMoviesMetaData();

        //Assert
        Assert.That(_dbContext.Movies.First(m => m.Id == 2).Poster, Is.EqualTo("https://maximemohandi.fr/"));
    }

    [Test]
    public async Task ShouldContinueIfNoMetadataFound()
    {
        // Arrange
        _fakeDataProvider.Setup(x => x.GetDetails(It.Is<string>(x => x == "Asterix & Obelix : Mission Cléopatre")))
            .ThrowsAsync(new NoMetaDataFoundException(""));
        _fakeDataProvider.Setup(x =>
                x.GetDetails(It.Is<string>(x => x == "The Lord of the Rings: The Fellowship of the Ring")))
            .ReturnsAsync(new MovieMetadataEntity("https://maximemohandi.fr/", "loremp ipsum", 8, 8, 0));
        MovieService movieService = new(_movieRepository, _fakeDataProvider.Object);

        // Act
        await movieService.RefreshMoviesMetaData();

        // Assert
        Assert.That(_dbContext.Movies.First(m => m.Id == 1).Description, Is.EqualTo("loremp ipsum"));
    }


    protected override void SeedInMemoryDatas()
    {
        _dbContext.Movies.AddRange(new Movie
        {
            Id = 1,
            DateAdded = new DateTime(2022, 12, 12),
            Name = "Asterix & Obelix : Mission Cléopatre",
            Poster = "",
            Description = "",
            Rates = null,
            SeenDate = null
        }, new Movie
        {
            Id = 2,
            DateAdded = new DateTime(2022, 12, 18),
            Name = "The Lord of the Rings: The Fellowship of the Ring",
            Poster = "",
            Description = "Description",
            Rates = null,
            SeenDate = null
        });
    }
}
