﻿using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Services;

namespace MovieCrew.Core.Test.Movies;

public class UpdateMovie : InMemoryMovieTestBase
{
    [Test]
    public async Task TitleShouldChangeWhenRename()
    {
        //Arrange
        MovieService movieService = new(_movieRepository, _fakeDataProvider.Object);

        //Act
        await movieService.ChangeTitle(1, "nouveau nom");

        //Assert
        Assert.That(_dbContext.Movies.First(m => m.Name == "nouveau nom").Name, Is.EqualTo("nouveau nom"));
    }

    [Test]
    public void ShouldThrowExceptionWhenNewTitleAlreadyExist()
    {
        //Arrange
        MovieService movieService = new(_movieRepository, _fakeDataProvider.Object);

        //Act & Assert
        Assert.ThrowsAsync<MovieAlreadyExistException>(() =>
                movieService.ChangeTitle(1,
                    "Asterix & Obelix : Mission Cléopatre"),
            "Asterix & Obelix : Mission Cléopatre is already in the list.");
    }

    [Test]
    public async Task PosterShouldChangeWhenUpdate()
    {
        //Arrange
        MovieService movieService = new(_movieRepository, _fakeDataProvider.Object);

        //Act
        await movieService.ChangePoster(1, "path/to/poster");

        //Assert
        Assert.That(_dbContext.Movies.First(m => m.Id == 1).Poster,
            Is.EqualTo("path/to/poster"));
    }

    [Test]
    public void ShouldThrowExceptionWhenMovieNotFound()
    {
        //Arrange
        MovieService movieService = new(_movieRepository, _fakeDataProvider.Object);

        //Act & Assert
        Assert.ThrowsAsync<MovieNotFoundException>(() =>
                movieService.ChangePoster(3, "path/to/poster"),
            "Movie with id 3 not found.");
    }

    protected override void SeedInMemoryDatas()
    {
        _dbContext.Movies.Add(
            new Movie
            {
                Id = 1,
                DateAdded = new DateTime(2022, 12, 12),
                Name = "Asterix & Obelix : Mission Cléopatre",
                Poster = "",
                Description = "Description",
                Rates = null,
                SeenDate = null
            });
    }
}
