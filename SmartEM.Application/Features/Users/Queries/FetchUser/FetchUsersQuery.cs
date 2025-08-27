using MediatR;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Users.Queries.FetchUser;

public class FetchUsersQuery : IRequest<OperationResponse<PaginatedResponse<FetchUserDto>>>
{
    public PaginatedRequest? PaginatedRequest { get; set; } = default!;
}
