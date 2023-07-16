namespace MovieCrew.Core.Domain.Authentication.Repository;

public interface IAuthenticationRepository
{
    Task<bool> IsClientValid(int clientId, string apiKey);
}
