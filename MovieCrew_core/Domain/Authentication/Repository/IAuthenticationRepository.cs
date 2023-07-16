namespace MovieCrew.Core.Domain.Authentication.Repository;

public interface IAuthenticationRepository
{
    Task<bool> IsClientValid(long clientId, string apiKey);
}
