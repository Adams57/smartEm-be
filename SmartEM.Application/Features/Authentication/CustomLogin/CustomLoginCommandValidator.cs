using FluentValidation;
using SmartEM.Application.Utils.ErrorTemplates;

namespace SmartEM.Application.Features.Authentication.CustomLogin;

public class CustomLoginCommandValidator : AbstractValidator<CustomLoginCommand>
{
    public CustomLoginCommandValidator()
    {
        RuleFor(command => command.Email)
            .EmailAddress().WithMessage(ValidationMessages.MustBeValid);
        RuleFor(command => command.Password)
            .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty);
    }
}
