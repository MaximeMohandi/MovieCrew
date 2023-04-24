using Microsoft.IdentityModel.Tokens;
using MovieCrew.Core.Domain.Authentication.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace MovieCrew.Core.Domain.Authentication.Services;

public class AuthenticationService
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly IAuthenticationRepository _repository;
    private const int TokenNbValidationsDays = 1;

    public AuthenticationService(IAuthenticationRepository repository, JwtConfiguration jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration;
        _repository = repository;
    }

    public async Task<AuthenticatedUser> Authenticate(long userId, string userName)
    {
        var userExist = await _repository.IsUserExist(userId, userName);
        if (!userExist) throw new AuthenticationException("Invalid user.");

        var token = CreateToken();
        return new AuthenticatedUser(userId, userName, new JwtSecurityTokenHandler().WriteToken(token),
            token.ValidTo);
    }


    private JwtSecurityToken CreateToken()
    {
        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Passphrase));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        return new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: new List<Claim>(),
            expires: DateTime.UtcNow.AddDays(TokenNbValidationsDays),
            signingCredentials: signingCredentials
        );
    }
}