namespace SmartEM.Domain.UserManagement;

public class RefreshToken : BaseEntity<Guid>
{
    protected RefreshToken() { }
    public RefreshToken(string token, DateTime expiry)
    {
        TokenString = token;
        Expiry = expiry;
        IsRevoked = false;
    }
    public RefreshToken SetUser(User user)
    {
        User = user;
        return this;
    }
    public void Revoke()
    {
        IsRevoked = true;
    }

    public bool IsExpired()
    {
        if (DateTime.UtcNow > Expiry)
            return true;
        return false;
    }
    public string TokenString { get; set; } = default!;
    public DateTime Expiry { get; set; }
    public bool IsRevoked { get; protected set; }
    public Guid UserId { get; set; } = default;
    public User User { get; protected set; } = default!;
}