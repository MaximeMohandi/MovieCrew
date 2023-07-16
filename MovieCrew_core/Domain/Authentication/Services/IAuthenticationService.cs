using MovieCrew.Core.Domain.Authentication.Model;

namespace MovieCrew.Core.Domain.Authentication.Services;

public interface IAuthenticationService
{
    Task<AuthenticatedClient> Authenticate(int clientId, string apiKey);
}
