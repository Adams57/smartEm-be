using MediatR;
using SmartEM.Application.Features.Authentication.CustomLogin;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.MicrosoftLogin;

public class MicrosoftLoginCommand : IRequest<OperationResponse<LoginResponseDto>>
{
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string ProviderId { get; set; } = default!;
}
