namespace SmartEM.Domain.UserManagement;

public class User : BaseEntity<Guid>
{
    protected User() { }

    public static User New(string email, string firstName, string lastName, AccountTypes accountType, string jobTitle = default!, Guid? Id = null)
    {
        return new User
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            JobTitle = jobTitle,
            AccountType = accountType,
            IsFirstLogin = true,
            Id = Id ?? Guid.Empty,
        };
    }
    public User SetPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        return this;
    }
    public User ChangeAccountType(AccountTypes accountType)
    {
        AccountType = accountType;
        return this;
    }
    public required string Email { get; set; }
    public string? PasswordHash { get; protected set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public AccountTypes AccountType { get; protected set; }
    public bool IsFirstLogin { get; set; }
    public DateTime LastLoginTimeStamp { get; set; }
    public DateTime LastPasswordResetToken { get; set; }
    public ProfilePicture Photo { get; set; } = default!;
    public ICollection<RefreshToken> RefreshTokens { get; protected set; } = [];
    public ICollection<ExternalLogin> ExternalLogins { get; set; } = [];
    public string? JobTitle { get; set; }
}
