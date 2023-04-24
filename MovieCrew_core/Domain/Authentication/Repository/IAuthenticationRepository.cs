namespace MovieCrew.Core.Domain.Authentication.Services;

public interface IAuthenticationRepository
{
    Task<bool> IsUserExist(long userId, string userName);
}