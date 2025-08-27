using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Users.Queries.FetchUser;

public class FetchUserDto : IProjectionDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string AccountType { get; set; } = default!;
    public int SequentialId { get; init; }
}
