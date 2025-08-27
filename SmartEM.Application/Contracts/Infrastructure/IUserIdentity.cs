namespace SmartEM.Application.Contracts.Infrastructure;

public interface IUserIdentity
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
}
