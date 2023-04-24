namespace MovieCrew.Core.Domain.Authentication.Model;

public record AuthenticatedUser(long UserId, string UserName, string Token, DateTime TokenExpirationDate);