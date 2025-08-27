using MediatR;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.LogOut;

public class RevokeRefreshTokenCommand : IRequest<OperationResponse>
{
    public string RefreshToken { get; set; } = default!;
}
