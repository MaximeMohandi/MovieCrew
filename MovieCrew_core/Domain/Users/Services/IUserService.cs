using MovieCrew.Core.Domain.Users.Dtos;
using MovieCrew.Core.Domain.Users.Entities;

namespace MovieCrew.Core.Domain.Users.Services;

public interface IUserService
{
    Task AddUser(UserCreationDto userCreation);
    Task<UserEntity> GetById(long id);
}