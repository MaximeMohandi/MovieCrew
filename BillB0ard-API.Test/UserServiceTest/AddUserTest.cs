﻿using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.Enums;
using BillB0ard_API.Domain.Exception;
using BillB0ard_API.Domain.Repository;
using BillB0ard_API.Services;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Test.UserServiceTest
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
        public async Task WithIdAndName()
        {
            User expectedUser = new()
            {
                Id = 1234,
                Name = "Ygerne",
                Rates = null,
                Role = 0
            };

            UserRepository userRepository = new(_dbContext);
            UserService userService = new(userRepository);

            await userService.AddUser(new(1234, "Ygerne"));

            Assert.That(_dbContext.Users.Contains(expectedUser), Is.True);
        }

        [Test]
        public async Task WithRole()
        {
            User expectedUser = new()
            {
                Id = 123456,
                Name = "Leodagan",
                Rates = null,
                Role = 1
            };

            UserRepository userRepository = new(_dbContext);
            UserService userService = new(userRepository);

            await userService.AddUser(new(123456, "Leodagan", UserRoles.Admin));

            Assert.That(_dbContext.Users.Contains(expectedUser), Is.True);
        }

        [Test]
        public async Task CantAddUserIfItAlreadyExist()
        {

            UserRepository userRepository = new(_dbContext);
            UserService userService = new(userRepository);

            await userService.AddUser(new(1234567, "Arthur", UserRoles.Bot));

            Assert.ThrowsAsync<UserAlreadyExistException>(async () => await userService.AddUser(new(1234567, "Arthur", UserRoles.Bot)));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
