using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.DTOs;
using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Enums;
using BillB0ard_API.Domain.Exception;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Domain.Repository
{
    public class UserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(UserCreationDto userCreation)
        {
            var isUserExist = await _dbContext.Users.SingleOrDefaultAsync(u => u.Name == userCreation.Name);
            if (isUserExist is not null) throw new UserAlreadyExistException(userCreation.Name);
            User newUser = new()
            {
                Name = userCreation.Name,
                Role = (int)userCreation.Role
            };

            _dbContext.Add(newUser);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserEntity> GetBy(long id)
        {
            var dbUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (dbUser is null) throw new UserNotFoundException(id);

            return new(dbUser.Id, dbUser.Name, (UserRoles)dbUser.Role);
        }

        public async Task<UserEntity> GetBy(string userName)
        {
            var dbUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Name == userName);

            if (dbUser is null) throw new UserNotFoundException(userName);

            return new(dbUser.Id, dbUser.Name, (UserRoles)dbUser.Role);
        }
    }
}
