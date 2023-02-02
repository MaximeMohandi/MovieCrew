﻿using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Enums;
using BillB0ard_API.Domain.Exception;
using BillB0ard_API.Domain.Repository;
using BillB0ard_API.Services;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Test.UserServiceTest
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
        public async Task FetchUserById()
        {
            UserRepository userRepository = new(_dbContext);
            UserEntity expectedUser = new(1, "Arthur", UserRoles.Admin);
            UserService userService = new(userRepository);

            UserEntity actualUser = await userService.GetById(1);

            Assert.That(actualUser, Is.EqualTo(expectedUser));
        }

        [Test]
        public async Task FetchUserByName()
        {
            UserRepository userRepository = new(_dbContext);
            UserEntity expectedUser = new(1, "Arthur", UserRoles.Admin);
            UserService userService = new(userRepository);

            UserEntity actualUser = await userService.GetByName("Arthur");

            Assert.That(actualUser, Is.EqualTo(expectedUser));
        }

        [Test]
        public void CantFetchUserWithUnknownUserName()
        {
            UserRepository userRepository = new(_dbContext);
            UserService userService = new(userRepository);

            UserNotFoundException exception = Assert.ThrowsAsync<UserNotFoundException>(async () => await userService.GetByName("Perceval"));
            Assert.That(exception.Message, Is.EqualTo("User 'Perceval' not found. Please check the username and try again"));
        }

        [Test]
        public void CantFetchUserWithUnknownUserId()
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