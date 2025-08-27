using SmartEM.Domain.UserManagement;

namespace SmartEM.Application.Features.Authentication.CustomLogin;

public class LoginResponseDto
{
    public LoginResponseDto(User user, string accessToken)
    {
        var refreshToken = user.RefreshTokens.Last();
        UserId = user.Id;
        Email = user.Email;
        AccountType = user.AccountType.ToString();
        FirstName = user.FirstName;
        LastName = user.LastName;
        AccessToken = accessToken;
        RefreshToken = refreshToken.TokenString;
        RefreshTokenExpiry = refreshToken.Expiry;
    }

    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string AccountType { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
}
