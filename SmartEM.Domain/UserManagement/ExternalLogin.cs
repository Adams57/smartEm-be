namespace SmartEM.Domain.UserManagement;

public class ExternalLogin : BaseEntity<Guid>
{
    protected ExternalLogin() { }
    public ExternalLogin(LoginProvider loginProvider, string providerId, User user)
    {
        ProviderName = loginProvider;
        ProviderId = providerId;
        User = user;
    }

    public string ProviderId { get; set; } = default!;
    public LoginProvider ProviderName { get; set; } = default!;
    public Guid UserId { get; set; } = default;
    public User User { get; set; } = default!;
}