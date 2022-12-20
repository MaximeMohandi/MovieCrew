using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.DTOs;

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
            User newUser = new()
            {
                Name = userCreation.Name,
            };

            _dbContext.Add(newUser);

            await _dbContext.SaveChangesAsync();
        }
    }
}
