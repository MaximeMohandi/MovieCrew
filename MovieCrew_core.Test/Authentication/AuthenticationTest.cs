using System.Security.Authentication;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MovieCrew.Core.Data;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Authentication.Model;
using MovieCrew.Core.Domain.Authentication.Services;
namespace MovieCrew.Core.Test.Authentication;

public class AuthenticationTest
{
    //https://code-maze.com/authentication-aspnetcore-jwt-1/
    private const string IsCorrectToken = @"^([a-zA-Z0-9_-]+\.){2}[a-zA-Z0-9_-]+$";

    private readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: "UserDbTest")
        .Options;

    private AppDbContext _dbContext;

    [OneTimeSetUp]
    public void SetUp()
    {
        _dbContext = new AppDbContext(_dbContextOptions);
        _dbContext.Database.EnsureCreated();

        User[] users = new[]
        {
            new User()
            {
                Id = 1,
                Name = "test",
                Role = 1,
            }
        };

        _dbContext.Users.AddRange(users);
        _dbContext.SaveChanges();
    }
 
    [Test]
    public async Task SuccessfulLogin()
    {
        var jwtConfiguration =
            new JwtConfiguration("A passphrase with to be secure @123", "https://test.com", "https://test.com");
        var repository = new AuthenticationRepository(_dbContext);
        var service = new AuthenticationService(repository, jwtConfiguration);
        var expected = new AuthenticatedUser(1, "test", "a.a.a", DateTime.UtcNow.AddDays(1));

        var actual = await service.Authenticate(1, "test");

        Assert.Multiple(() =>
        {
            Assert.That(actual.UserId, Is.EqualTo(expected.UserId));
            Assert.That(actual.UserName, Is.EqualTo(expected.UserName));
            Assert.That(actual.TokenExpirationDate.ToShortTimeString(), Is.EqualTo(expected.TokenExpirationDate.ToShortTimeString()));
            Assert.That(actual.Token, Does.Match(IsCorrectToken));
        });
    }

    [Test]
    public void InvalidUserLogin()
    {
        var repository = new AuthenticationRepository(_dbContext);
        var service = new AuthenticationService(repository, new JwtConfiguration());
        
        Assert.Throws<AuthenticationException>(() =>
        {
            service.Authenticate(1, "test2");
        }, message: "Invalid user.");
    }
}