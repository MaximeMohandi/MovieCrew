using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.DTOs;
using BillB0ard_API.Domain.Entities;
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
            var isUserExist = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userCreation.Id || u.Name == userCreation.Name);
            if (isUserExist is not null) throw new UserAlreadyExistException(userCreation.Name);
            User newUser = new()
            {
                Id = userCreation.Id,
                Name = userCreation.Name,
                Role = (int)userCreation.Role
            };

            _dbContext.Add(newUser);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserEntity> GetBy(long id)
        {
            var dbUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);

            UserEntity user = new(dbUser.Id, dbUser.Name, dbUser.Role);

            return user;
        }
    }
}
