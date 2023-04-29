using MovieCrew.Core.Domain.Users.Dtos;
using MovieCrew.Core.Domain.Users.Entities;

namespace MovieCrew.Core.Domain.Users.Repository;

public interface IUserRepository
{
    Task Add(UserCreationDto userCreation);
    Task<UserEntity> GetBy(long id);
    Task<SpectatorDetailsEntity> GetSpectatorDetails(long idSpectator);
}