using FluentValidation;
using SmartEM.Application.Utils.ErrorTemplates;

namespace SmartEM.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage(ValidationMessages.MustBeValid);
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty);
        //RuleFor(x => x.LastName)
        //    .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty);
        RuleFor(x => x.AccountType)
            .IsInEnum().WithMessage(ValidationMessages.MustBeValidEnumValue);
    }
}
