using MovieCrew_core.Data;
using MovieCrew_core.Data.Models;
using MovieCrew_core.Domain.Users.Enums;
using MovieCrew_core.Domain.Users.Exception;
using MovieCrew_core.Domain.Users.Repository;
using MovieCrew_core.Domain.Users.Services;
using Microsoft.EntityFrameworkCore;

namespace MovieCrew_core.Test.Users
{
    public class AddUserTest
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: "MovieDbTest")
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

            await userService.AddUser(new(name, role));

            Assert.That(_dbContext.Users.Contains(expectedUser), Is.True);
        }

        [Test]
        public async Task CantAddUserIfItAlreadyExist()
        {

            UserRepository userRepository = new(_dbContext);
            UserService userService = new(userRepository);

            await userService.AddUser(new("Arthur", UserRoles.Bot));

            Assert.ThrowsAsync<UserAlreadyExistException>(async () => await userService.AddUser(new("Arthur", UserRoles.Bot)));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
