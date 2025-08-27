using MediatR;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.SendPasswordResetToken;

public class SendPasswordResetTokenCommand : IRequest<OperationResponse>
{
    public string Email { get; set; } = default!;
}
