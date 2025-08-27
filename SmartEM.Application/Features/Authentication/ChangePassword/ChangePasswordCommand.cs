using MediatR;
using SmartEM.Application.Features.Authentication.CustomLogin;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.ChangePassword;

public class ChangePasswordCommand : IRequest<OperationResponse<LoginResponseDto>>
{
    public string OldPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
