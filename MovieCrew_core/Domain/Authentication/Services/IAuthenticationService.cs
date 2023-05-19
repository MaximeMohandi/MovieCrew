using MovieCrew.Core.Domain.Authentication.Model;

namespace MovieCrew.Core.Domain.Authentication.Services;

public interface IAuthenticationService
{
    Task<AuthenticatedUser> Authenticate(long userId, string userName);
}