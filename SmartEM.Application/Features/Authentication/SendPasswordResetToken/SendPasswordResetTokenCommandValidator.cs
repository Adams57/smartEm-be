using FluentValidation;

namespace SmartEM.Application.Features.Authentication.SendPasswordResetToken;

public class SendPasswordResetTokenCommandValidator : AbstractValidator<SendPasswordResetTokenCommand>
{
    public SendPasswordResetTokenCommandValidator()
    {
        RuleFor(command => command.Email).EmailAddress().WithMessage("Invalid email");
    }
}
