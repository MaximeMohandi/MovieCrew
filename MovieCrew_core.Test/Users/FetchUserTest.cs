using MovieCrew_core.Data;
using MovieCrew_core.Data.Models;
using MovieCrew_core.Domain.Users.Entities;
using MovieCrew_core.Domain.Users.Enums;
using MovieCrew_core.Domain.Users.Exception;
using MovieCrew_core.Domain.Users.Repository;
using MovieCrew_core.Domain.Users.Services;
using Microsoft.EntityFrameworkCore;

namespace MovieCrew_core.Test.Users
{
    public class FetchUserTest
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: "UserDbTest")
           .Options;

        protected AppDbContext _dbContext;

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
                    Name = "Arthur",
                    Role = 1,
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

            UserEntity actualUser = await userService.GetById(1);

            Assert.That(actualUser, Is.EqualTo(expectedUser));
        }

        [Test]
        public void UnknownId()
        {
            UserRepository userRepository = new(_dbContext);
            UserService userService = new(userRepository);

            UserNotFoundException exception = Assert.ThrowsAsync<UserNotFoundException>(async () => await userService.GetById(-1));
            Assert.That(exception.Message, Is.EqualTo("User with id: -1 not found. Please check the conformity and try again"));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
