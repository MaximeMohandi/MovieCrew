using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using MovieCrew.Core.Data;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Authentication.Model;
using MovieCrew.Core.Domain.Authentication.Repository;
using MovieCrew.Core.Domain.Authentication.Services;

namespace MovieCrew.Core.Test.Authentication;

public class AuthenticationTest
{
    private const string IsCorrectToken = @"^([a-zA-Z0-9_-]+\.){2}[a-zA-Z0-9_-]+$";

    private readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase("UserDbTest")
        .Options;

    private readonly JwtConfiguration _jwtConfiguration =
        new("A passphrase with to be secure @123", "https://test.com", "https://test.com");

    private AppDbContext _dbContext;
    private AuthenticationRepository _repository;

    [OneTimeSetUp]
    public void SetUp()
    {
        _dbContext = new AppDbContext(_dbContextOptions);
        _dbContext.Database.EnsureCreated();
        Client[] clients =
        {
            new()
            {
                Id = 1,
                ApiKey = "test"
            }
        };

        _dbContext.Clients.AddRange(clients);
        _dbContext.SaveChanges();

        _repository = new AuthenticationRepository(_dbContext);
    }

    [Test]
    public async Task ShouldAuthenticateClient()
    {
        // Arrange
        var service = new AuthenticationService(_repository, _jwtConfiguration);

        // Act
        var actual = await service.Authenticate(1, "test");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual.TokenExpirationDate.ToShortTimeString(),
                Is.EqualTo(DateTime.UtcNow.AddDays(1).ToShortTimeString()));
            Assert.That(actual.Token, Does.Match(IsCorrectToken));
        });
    }

    [Test]
    public void ShouldThrowsExceptionWhenClientInvalid()
    {
        var service = new AuthenticationService(_repository, _jwtConfiguration);

        Assert.ThrowsAsync<AuthenticationException>(async () => { await service.Authenticate(1, "test2"); },
            "Invalid client.");
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        _dbContext.Database.EnsureDeleted();
    }
}
