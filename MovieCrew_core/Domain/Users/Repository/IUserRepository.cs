﻿using MovieCrew.Core.Domain.Users.Entities;

namespace MovieCrew.Core.Domain.Users.Repository;

public interface IUserRepository
{
    Task Add(long id, string name, int role);
    Task<UserEntity> GetBy(long id, string name);
    Task<SpectatorDetailsEntity> GetSpectatorDetails(long idSpectator);
}