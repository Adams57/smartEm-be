using MediatR;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<OperationResponse>
{
    public IEnumerable<Guid> Ids { get; set; } = default!;
}
