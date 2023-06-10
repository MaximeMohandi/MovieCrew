namespace MovieCrew.Core.Domain.Authentication.Repository;

public interface IAuthenticationRepository
{
    Task<bool> IsUserExist(long userId, string userName);
}
