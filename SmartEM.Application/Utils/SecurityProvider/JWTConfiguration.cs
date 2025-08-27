namespace SmartEM.Application.Utils.SecurityProvider;

public class JWTConfiguration
{
    public string SecretKey { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Issuer { get; set; } = default!;

}
