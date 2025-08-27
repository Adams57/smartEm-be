using SmartEM.Domain.UserManagement;

namespace SmartEM.Application.Contracts.Infrastructure;

public interface IJWTProvider
{
    string GetAccessToken(User user);
    string GetPasswordResetToken(string email);
    RefreshToken GetRefreshToken();
    bool CheckTokenExpiry(string token);
    string? ReadClaimValue(string token, string requiredValue);
}
