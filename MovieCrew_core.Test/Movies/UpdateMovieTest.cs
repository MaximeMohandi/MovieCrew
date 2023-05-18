using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Dtos;
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
        await movieService.ChangeTitle(new MovieRenameDto(1, "Asterix & Obelix : Mission Cléopatre", "nouveau nom"));

        //Assert
        Assert.That(_dbContext.Movies.First(m => m.Name == "nouveau nom").Name, Is.EqualTo("nouveau nom"));
    }

    [Test]
    public void ShouldThrowExceptionWhenNewTitleAlreadyExist()
    {
        //Arrange
        MovieService movieService = new(_movieRepository, _fakeDataProvider.Object);
        MovieRenameDto renameMovieData =
            new(1, "Asterix & Obelix : Mission Cléopatre", "Asterix & Obelix : Mission Cléopatre");

        //Act & Assert
        Assert.ThrowsAsync<MovieAlreadyExistException>(() =>
            movieService.ChangeTitle(renameMovieData), "Asterix & Obelix : Mission Cléopatre is already in the list.");
    }


    [Test]
    public async Task MovieSeenDateShouldChangeWhenSetSeenDate()
    {
        //Arrange
        MovieService movieService = new(_movieRepository, _fakeDataProvider.Object);
        var updatedMovie = _dbContext.Movies.Single(m => m.Id == 1);

        //Act
        await movieService.SetSeenDate(new MovieSetSeenDateDto(1, DateTime.Now));

        //Assert
        Assert.That(updatedMovie?.SeenDate?.Date, Is.EqualTo(DateTime.Now.Date));
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
