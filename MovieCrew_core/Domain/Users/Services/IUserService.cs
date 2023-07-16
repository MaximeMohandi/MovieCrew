using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;

namespace MovieCrew.Core.Domain.Users.Services;

public interface IUserService
{
    Task AddUser(long id, string name, UserRoles role);
    Task<UserEntity> Get(long id, string name);
}