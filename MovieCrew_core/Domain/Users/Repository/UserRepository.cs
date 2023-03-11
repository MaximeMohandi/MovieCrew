using Microsoft.EntityFrameworkCore;
using MovieCrew.Core.Data;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Users.Dtos;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;
using MovieCrew.Core.Domain.Users.Exception;

namespace MovieCrew.Core.Domain.Users.Repository
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

        public async Task<SpectatorDetailsEntity> GetSpectatorDetails(long idSpectator)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == idSpectator)
                ?? throw new UserNotFoundException(idSpectator);

            if (user.Rates is null) throw new UserIsNotSpectatorException(idSpectator);

            List<SpectatorRateEntity> spectatorRates = user.Rates
                .Select(r =>
                    new SpectatorRateEntity(
                        new(r.MovieId, r.Movie.Name, r.Movie.Poster, r.Movie.DateAdded, r.Movie.SeenDate, r.Movie.Rates?.Average(r => r.Note)),
                        r.Note)
                ).ToList();
            return new SpectatorDetailsEntity(new(user.Id, user.Name, user.Role), spectatorRates);
        }
    }
}
