﻿using MovieCrew.Core.Domain.Users.Dtos;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Repository;

namespace MovieCrew.Core.Domain.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task AddUser(UserCreationDto userCreation)
    {
        await _userRepository.Add(userCreation);
    }

    public async Task<UserEntity> GetById(long id)
    {
        return await _userRepository.GetBy(id);
    }
}
