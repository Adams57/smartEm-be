using MediatR;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Users.Queries.FetchUser;

public class FetchUserByIdQuery : IRequest<OperationResponse<FetchUserDto>>
{
    public Guid Id { get; set; }
}
