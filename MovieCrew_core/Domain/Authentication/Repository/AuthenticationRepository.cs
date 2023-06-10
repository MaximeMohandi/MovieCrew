using Microsoft.EntityFrameworkCore;
using MovieCrew.Core.Data;

namespace MovieCrew.Core.Domain.Authentication.Repository;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly AppDbContext _dbContext;

    public AuthenticationRepository(AppDbContext databaseContext)
    {
        _dbContext = databaseContext;
    }

    public async Task<bool> IsUserExist(long userId, string userName)
    {
        return await _dbContext.Users.AnyAsync(user => user.Id == userId && user.Name == userName);
    }
}
