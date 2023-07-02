using Microsoft.EntityFrameworkCore;
using MovieCrew.Core.Data;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Extension;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;
using MovieCrew.Core.Domain.Users.Exception;

namespace MovieCrew.Core.Domain.Users.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<UserEntity> GetBy(long id)
    {
        var dbUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);

        return dbUser is null
            ? throw new UserNotFoundException(id)
            : new UserEntity(dbUser.Id, dbUser.Name, (UserRoles)dbUser.Role);
    }

    public async Task<SpectatorDetailsEntity> GetSpectatorDetails(long idSpectator)
    {
        var user = await _dbContext.Users
                       .Include(u => u.Rates)
                       .ThenInclude(r => r.Movie)
                       .ThenInclude(m => m.Rates)
                       .FirstOrDefaultAsync(u => u.Id == idSpectator)
                   ?? throw new UserNotFoundException(idSpectator);

        if (user.Rates is null || user.Rates.Count == 0) throw new UserIsNotSpectatorException(idSpectator);

        var spectatorRates = user.Rates
            .Select(r =>
                new SpectatorRateEntity(
                    r.Movie.ToEntity(),
                    r.Note)
            ).ToList();
        return new SpectatorDetailsEntity(new UserEntity(user.Id, user.Name, user.Role), spectatorRates);
    }

    public async Task Add(string name, int role)
    {
        var isUserExist = await _dbContext.Users.SingleOrDefaultAsync(u => u.Name == name);
        if (isUserExist is not null) throw new UserAlreadyExistException(name);
        User newUser = new()
        {
            Name = name,
            Role = role
        };

        _dbContext.Add(newUser);

        await _dbContext.SaveChangesAsync();
    }
}
