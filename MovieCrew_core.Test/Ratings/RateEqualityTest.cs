using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Ratings.Entities;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;

namespace MovieCrew.Core.Test.Ratings;

public class RateEqualityTest
{
    [Test]
    public void SameRateModelShouldBeEqual()
    {
        //Arrange
        Rate expectedRate = new()
        {
            Movie = new Movie(),
            MovieId = 1,
            Note = 2.0M,
            User = new User(),
            UserId = 1
        };

        var actualRate = new Rate
        {
            Movie = new Movie(),
            MovieId = 1,
            Note = 2.0M,
            User = new User(),
            UserId = 1
        };

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(actualRate, Is.EqualTo(expectedRate));
            Assert.That(actualRate.GetHashCode(), Is.EqualTo(expectedRate.GetHashCode()));
        });
    }

    [Test]
    public void RateModelShouldNotBeEqualToNull()
    {
        //Arrange
        var actualRate = new Rate
        {
            Movie = new Movie(),
            MovieId = 1,
            Note = 2.0M,
            User = new User(),
            UserId = 1
        };

        //Assert
        Assert.That(actualRate, Is.Not.EqualTo(null));
    }

    [Test]
    public void SameRateEntityShouldBeEqual()
    {
        //Arrange
        RateEntity firstRate = new(new UserEntity(1, "test", UserRoles.Admin), 2M);
        RateEntity secondRate = new(new UserEntity(1, "test", UserRoles.Admin), 2M);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(firstRate, Is.EqualTo(secondRate));
            Assert.That(firstRate.GetHashCode(), Is.EqualTo(secondRate.GetHashCode()));
        });
    }

    [Test]
    public void ListOfRatesEntityShouldBeEqual()
    {
        //Arrange
        List<RateEntity> firstList = new()
        {
            new RateEntity(new UserEntity(1, "test", UserRoles.Admin), 2M)
        };
        List<RateEntity> secondList = new()
        {
            new RateEntity(new UserEntity(1, "test", UserRoles.Admin), 2M)
        };

        //Assert
        CollectionAssert.AreEqual(firstList, secondList);
    }
}
