using Microsoft.EntityFrameworkCore;
using MovieCrew.Core.Data;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Users.Dtos;
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
        _dbContext.SaveChanges();
    }

    [TestCase(1, "Ygerne", UserRoles.User)]
    [TestCase(2, "Tio", UserRoles.Admin)]
    [TestCase(3, "Gloria", UserRoles.Bot)]
    public async Task WithIdAndName(long id, string name, UserRoles role)
    {
        User expectedUser = new()
        {
            Id = id,
            Name = name,
            Rates = null,
            Role = (int)role
        };

        UserRepository userRepository = new(_dbContext);
        UserService userService = new(userRepository);

        await userService.AddUser(new UserCreationDto(name, role));

        Assert.That(_dbContext.Users.Contains(expectedUser), Is.True);
    }

    [Test]
    public async Task CantAddUserIfItAlreadyExist()
    {
        UserRepository userRepository = new(_dbContext);
        UserService userService = new(userRepository);

        await userService.AddUser(new UserCreationDto("Arthur", UserRoles.Bot));

        Assert.ThrowsAsync<UserAlreadyExistException>(async () =>
            await userService.AddUser(new UserCreationDto("Arthur", UserRoles.Bot)));
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        _dbContext.Database.EnsureDeleted();
    }
}