using FluentValidation;
using SmartEM.Application.Utils.ErrorTemplates;

namespace SmartEM.Application.Features.Authentication.MicrosoftLogin;

public class MicrosoftLoginCommandValidator : AbstractValidator<MicrosoftLoginCommand>
{
    public MicrosoftLoginCommandValidator()
    {
        RuleFor(command => command.Email)
            .EmailAddress().WithMessage(ValidationMessages.MustBeValid);
        RuleFor(command => command.Name)
            .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty);
        RuleFor(command => command.ProviderId)
            .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty)
            .Must(r => { return Guid.TryParse(r, out _); })
            .WithMessage(ValidationMessages.MustBeValid);
    }
}
