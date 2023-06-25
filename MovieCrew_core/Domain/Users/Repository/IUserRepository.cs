using MovieCrew.Core.Domain.Users.Entities;

namespace MovieCrew.Core.Domain.Users.Repository;

public interface IUserRepository
{
    Task Add(string name, int role);
    Task<UserEntity> GetBy(long id);
    Task<SpectatorDetailsEntity> GetSpectatorDetails(long idSpectator);
}
