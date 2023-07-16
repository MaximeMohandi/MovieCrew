using Microsoft.EntityFrameworkCore;
using MovieCrew.Core.Data;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;
using MovieCrew.Core.Domain.Users.Exception;
using MovieCrew.Core.Domain.Users.Repository;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.Core.Test.Users;

public class FetchUserTest
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase("UserDbTest")
        .Options;

    protected AppDbContext _dbContext;

    [OneTimeSetUp]
    public void SetUp()
    {
        _dbContext = new AppDbContext(_dbContextOptions);
        _dbContext.Database.EnsureCreated();

        _dbContext.Users.Add(new User
        {
            Id = 1,
            Name = "Arthur",
            Role = 1
        });

        _dbContext.SaveChanges();
    }

    [Test]
    public async Task ShouldFetchUser()
    {
        // Arrange
        UserRepository userRepository = new(_dbContext);
        UserEntity expectedUser = new(1, "Arthur", UserRoles.Admin);
        UserService userService = new(userRepository);

        // Act
        var actualUser = await userService.Get(1, "Arthur");

        // Assert
        Assert.That(actualUser, Is.EqualTo(expectedUser));
    }

    [Test]
    public void ShouldThrowExceptionWhenUserWithWrongIdNotFound()
    {
        UserRepository userRepository = new(_dbContext);
        UserService userService = new(userRepository);

        Assert.ThrowsAsync<UserNotFoundException>(() => userService.Get(-1, "Arthur"),
            "The user Arthur already exist. please verify the name and try again");
    }

    [Test]
    public void ShouldThrowExceptionWhenUserWithWrongNameNotFound()
    {
        UserRepository userRepository = new(_dbContext);
        UserService userService = new(userRepository);

        Assert.ThrowsAsync<UserNotFoundException>(() => userService.Get(1, "test"),
            "User test not found. Please check the conformity and try again");
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        _dbContext.Database.EnsureDeleted();
    }
}
