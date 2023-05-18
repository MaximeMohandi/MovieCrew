using MovieCrew.Core.Domain.Users.Entities;

namespace MovieCrew.Core.Domain.Users.Services;

public interface ISpectatorService
{
    Task<SpectatorDetailsEntity> FetchSpectator(long id);
}