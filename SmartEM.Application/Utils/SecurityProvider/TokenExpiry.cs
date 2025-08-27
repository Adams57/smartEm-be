namespace SmartEM.Application.Utils.SecurityProvider;

public class TokenExpiry
{
    public int AccessTokenExpiry { get; set; }
    public int RefreshTokenExpiry { get; set; }
    public int PasswordResetTokenExpiry { get; set; }
}
