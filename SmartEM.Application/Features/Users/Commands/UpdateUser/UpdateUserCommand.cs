using MediatR;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<OperationResponse>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string JobTitle { get; set; } = default!;
}
