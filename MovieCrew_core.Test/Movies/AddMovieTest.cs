﻿using Moq;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Services;
using MovieCrew.Core.Domain.Users.Exception;

namespace MovieCrew.Core.Test.Movies;

public class AddMovieTest : InMemoryMovieTestBase
{
    [Test]
    public async Task AddMovie()
    {
        _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ReturnsAsync(new MovieMetadataEntity("https://maximemohandi.fr/", "loremp ipsum", 8, 8));
        var thirdPartyProvider = _fakeDataProvider.Object;

        MovieService service = new(_movieRepository, thirdPartyProvider);

        var addedMovie = await service.AddMovie("Pinnochio", 1);

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
    public void CantAddMovieThatDoNotExist()
    {
        _fakeDataProvider.Setup(x => x.GetDetails(It.IsAny<string>()))
            .ThrowsAsync(new NoMetaDataFoundException("dsfsdfsdaaa"));
        var thirdPartyProvider = _fakeDataProvider.Object;

        MovieService service = new(_movieRepository, thirdPartyProvider);

        Assert.ThrowsAsync<MovieNotFoundException>(() => service.AddMovie("dsfsdfsdaaa", 1));
    }

    [Test]
    public void CantAddMovieProposedByUnknownUser()
    {
        var thirdPartyProvider = _fakeDataProvider.Object;
        MovieService service = new(_movieRepository, thirdPartyProvider);

        Assert.ThrowsAsync<UserNotFoundException>(() => service.AddMovie("The Asada Family", 2));
    }

    [Test]
    public void CantAddExistMovie()
    {
        var thirdPartyProvider = _fakeDataProvider.Object;
        MovieService service = new(_movieRepository, thirdPartyProvider);

        Assert.ThrowsAsync<MovieAlreadyExistException>(() => service.AddMovie("The Fifth element", null));
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