using Microsoft.EntityFrameworkCore;
using MovieCrew.Core.Data;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Users.Enums;
using MovieCrew.Core.Domain.Users.Exception;
using MovieCrew.Core.Domain.Users.Repository;
using MovieCrew.Core.Domain.Users.Services;

namespace MovieCrew.Core.Test.Users;

public class AddUserTest
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase("MovieDbTest")
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
            Rates = null,
            Role = (int)UserRoles.Bot
        });
        _dbContext.SaveChanges();
    }

    [TestCase(1234, "Ygerne", UserRoles.User)]
    [TestCase(298766, "Tio", UserRoles.Admin)]
    [TestCase(89999955, "Gloria", UserRoles.Bot)]
    public async Task ShouldAddUser(int id, string name, UserRoles role)
    {
        // Arrange
        User expectedUser = new()
        {
            Id = id,
            Name = name,
            Rates = null,
            Role = (int)role
        };
        UserRepository userRepository = new(_dbContext);
        UserService userService = new(userRepository);

        // Act
        await userService.AddUser(id, name, role);

        // Assert
        Assert.That(_dbContext.Users.Contains(expectedUser), Is.True);
    }

    [Test]
    public async Task ShouldThrowExceptionWhenUserAlreadyExist()
    {
        // Arrange
        UserRepository userRepository = new(_dbContext);
        var userService = new UserService(userRepository);

        // Act & Assert
        Assert.ThrowsAsync<UserAlreadyExistException>(() => userService.AddUser(1, "Arthur", UserRoles.Bot),
            "The user Arthur already exist. please verify the name and try again");
    }

    [Test]
    public async Task ShouldThrowExceptionWhenRoleDoNotExist()
    {
        // Arrange
        UserRepository userRepository = new(_dbContext);
        var userService = new UserService(userRepository);

        // Act & Assert
        Assert.ThrowsAsync<UserRoleDoNotExistException>(() => userService.AddUser(787686, "Leodagan", (UserRoles)4),
            "The role 4 do not exist. please verify the role and try again");
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        _dbContext.Database.EnsureDeleted();
    }
}
