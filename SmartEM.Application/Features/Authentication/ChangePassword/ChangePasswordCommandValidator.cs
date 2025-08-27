using FluentValidation;
using SmartEM.Application.Utils.ErrorTemplates;

namespace SmartEM.Application.Features.Authentication.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(c => c.OldPassword)
            .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty);
        RuleFor(c => c.NewPassword)
            .MinimumLength(8).WithMessage(ValidationMessages.StringLengthMustBeMoreThan);
    }
}
