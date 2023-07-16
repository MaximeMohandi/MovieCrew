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

    public async Task<bool> IsClientValid(int clientId, string apiKey)
    {
        return await _dbContext.Clients.AnyAsync(client => client.Id == clientId && client.ApiKey == apiKey);
    }
}
