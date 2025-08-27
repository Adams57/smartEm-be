using MediatR;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.CustomLogin;

public class CustomLoginCommand : IRequest<OperationResponse<LoginResponseDto>>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
