using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Enums;
using MovieCrew.Core.Domain.Users.Exception;
using MovieCrew.Core.Domain.Users.Repository;

namespace MovieCrew.Core.Domain.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserEntity> GetById(long id)
    {
        return await _userRepository.GetBy(id);
    }

    public async Task AddUser(string name, UserRoles role)
    {
        if (!Enum.IsDefined(typeof(UserRoles), role))
            throw new UserRoleDoNotExistException(role.ToString());

        await _userRepository.Add(name, (int)role);
    }
}
