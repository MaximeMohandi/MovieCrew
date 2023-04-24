using MovieCrew.Core.Domain.Authentication.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

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
        
        var token = CreateToken(_jwtConfiguration.Passphrase, _jwtConfiguration.Issuer, _jwtConfiguration.Audience);
        return new AuthenticatedUser(userId, userName, new JwtSecurityTokenHandler().WriteToken(token),
            token.ValidTo);
    }


    private static JwtSecurityToken CreateToken(string secretKey, string issuer, string audience)
    {
        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        return new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: new List<Claim>(),
            expires: DateTime.UtcNow.AddDays(TokenNbValidationsDays),
            signingCredentials: signingCredentials
        );
    }
}