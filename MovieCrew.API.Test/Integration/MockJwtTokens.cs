using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace MovieCrew.API.Test.Integration;

public static class MockJwtTokens
{
    public const string Issuer = "http://localhost:5001";
    public const string Audience = "http://localhost:1234";
    private static readonly JwtSecurityTokenHandler s_tokenHandler = new();
    private static readonly RandomNumberGenerator s_rng = RandomNumberGenerator.Create();
    private static readonly byte[] s_key = new byte[32];

    static MockJwtTokens()
    {
        s_rng.GetBytes(s_key);
        SecurityKey = new SymmetricSecurityKey(s_key) { KeyId = Guid.NewGuid().ToString() };
        SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    }

    public static SecurityKey SecurityKey { get; }
    public static SigningCredentials SigningCredentials { get; }

    public static string GenerateJwtToken()
    {
        return s_tokenHandler.WriteToken(new JwtSecurityToken(Issuer, Audience, new List<Claim>(), null,
            DateTime.UtcNow.AddMinutes(20), SigningCredentials));
    }
}
