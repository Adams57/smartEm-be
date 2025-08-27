namespace SmartEM.Application.Features.Authentication.RefreshToken;

public class AccessTokenDto
{
    public string AccessToken { get; set; } = default!;
    public Guid UserId { get; set; }
}
