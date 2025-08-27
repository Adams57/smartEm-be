using MediatR;
using SmartEM.Application.Utils;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<OperationResponse<Guid>>
{
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? JobTitle { get; set; } = default!;
    public AccountTypes AccountType { get; set; }
}
