using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MovieCrew.Core.Domain.Authentication.Model;
using MovieCrew.Core.Domain.Authentication.Repository;

namespace MovieCrew.Core.Domain.Authentication.Services;

public class AuthenticationService : IAuthenticationService
{
    private const int TokenNbValidationsDays = 1;
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly IAuthenticationRepository _repository;

    public AuthenticationService(IAuthenticationRepository repository, JwtConfiguration jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration;
        _repository = repository;
    }

    public async Task<AuthenticatedClient> Authenticate(long clientId, string apiKey)
    {
        var isClientValid = await _repository.IsClientValid(clientId, apiKey);
        if (!isClientValid) throw new AuthenticationException("Invalid client");

        var token = CreateToken(apiKey);
        return new AuthenticatedClient(new JwtSecurityTokenHandler().WriteToken(token),
            token.ValidTo);
    }

    private JwtSecurityToken CreateToken(string apiKey)
    {
        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        return new JwtSecurityToken(
            _jwtConfiguration.Issuer,
            _jwtConfiguration.Audience,
            new List<Claim>(),
            expires: DateTime.UtcNow.AddDays(TokenNbValidationsDays),
            signingCredentials: signingCredentials
        );
    }
}
