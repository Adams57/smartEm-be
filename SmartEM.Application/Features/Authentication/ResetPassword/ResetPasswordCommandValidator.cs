using FluentValidation;
using SmartEM.Application.Utils.ErrorTemplates;

namespace SmartEM.Application.Features.Authentication.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(c => c.Token)
            .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty);
        RuleFor(c => c.Password)
            .MinimumLength(8).WithMessage(ValidationMessages.StringLengthMustBeMoreThan);
    }
}
