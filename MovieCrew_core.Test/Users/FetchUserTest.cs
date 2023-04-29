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


        User[] users =
        {
            new User
            {
                Id = 1,
                Name = "Arthur",
                Role = 1
            }
        };

        _dbContext.Users.AddRange(users);

        _dbContext.SaveChanges();
    }

    [Test]
    public async Task ById()
    {
        UserRepository userRepository = new(_dbContext);
        UserEntity expectedUser = new(1, "Arthur", UserRoles.Admin);
        UserService userService = new(userRepository);

        var actualUser = await userService.GetById(1);

        Assert.That(actualUser, Is.EqualTo(expectedUser));
    }

    [Test]
    public void UnknownId()
    {
        UserRepository userRepository = new(_dbContext);
        UserService userService = new(userRepository);

        var exception = Assert.ThrowsAsync<UserNotFoundException>(async () => await userService.GetById(-1));
        Assert.That(exception.Message,
            Is.EqualTo("User with id: -1 not found. Please check the conformity and try again"));
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        _dbContext.Database.EnsureDeleted();
    }
}