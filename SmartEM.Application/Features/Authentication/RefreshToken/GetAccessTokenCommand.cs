using MediatR;
using SmartEM.Application.Features.Authentication.CustomLogin;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.RefreshToken;

public class GetAccessTokenCommand : IRequest<OperationResponse<LeanLoginResponse>>
{
    public string RefreshToken { get; set; } = default!;
}
