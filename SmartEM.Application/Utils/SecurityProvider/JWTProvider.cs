using Microsoft.IdentityModel.Tokens;
using SmartEM.Application.Contracts.Infrastructure;
using SmartEM.Domain.UserManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SmartEM.Application.Utils.SecurityProvider;

class JWTProvider : IJWTProvider
{
    public JWTProvider(JWTConfiguration configuration, TokenExpiry tokenExpiry)
    {
        Configuration = configuration;
        TokenExpiry = tokenExpiry;
    }
    public string GetAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Aud, Configuration.Audience),
            new(JwtRegisteredClaimNames.Iss, Configuration.Issuer),
            new(ClaimTypes.Role, user.AccountType.ToString())
        };
        return MakeToken(claims, Configuration.SecretKey, TokenExpiry.AccessTokenExpiry);
    }

    public string GetPasswordResetToken(string email)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, email)
        };
        return MakeToken(claims, Configuration.SecretKey, TokenExpiry.PasswordResetTokenExpiry);
    }

    public string MakeToken(List<Claim> claims, string secretKey, int tokenExpiry)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(claims: claims,
                                         signingCredentials: signingCredentials,
                                         expires: DateTime.UtcNow.AddMinutes(tokenExpiry));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GetRefreshToken()
    {
        return new RefreshToken(
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            DateTime.UtcNow.AddDays(TokenExpiry.RefreshTokenExpiry));
    }

    public bool CheckTokenExpiry(string token)
    {
        var expiryTime = new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo;
        return DateTime.UtcNow > expiryTime;
    }

    public string? ReadClaimValue(string token, string requiredValue)
    {
        var result = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.FirstOrDefault(c => c.Type == requiredValue)?.Value;
        return result;
    }

    public JWTConfiguration Configuration { get; }
    public TokenExpiry TokenExpiry { get; }
}