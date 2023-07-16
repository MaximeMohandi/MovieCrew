namespace MovieCrew.Core.Domain.Authentication.Model;

public record AuthenticatedClient(string Token, DateTime TokenExpirationDate);
