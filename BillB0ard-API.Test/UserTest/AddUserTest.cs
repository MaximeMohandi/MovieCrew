using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.Repository;
using BillB0ard_API.Services;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Test.UserTest
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

        [Test]
        public async Task FromName()
        {
            User expectedUser = new()
            {
                Id = 1,
                Name = "Leodagan",
                Rates = null,
                Role = 0
            };

            UserRepository userRepository = new(_dbContext);
            UserService userService = new(userRepository);

            await userService.AddUser("Leodagan");

            Assert.That(_dbContext.Users.Contains(expectedUser), Is.True);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
