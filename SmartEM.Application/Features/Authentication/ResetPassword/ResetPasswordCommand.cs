using MediatR;
using SmartEM.Application.Features.Authentication.CustomLogin;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.ResetPassword;

public class ResetPasswordCommand : IRequest<OperationResponse<LoginResponseDto>>
{
    public string Token { get; set; } = default!;
    public string Password { get; set; } = default!;
}
