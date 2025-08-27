using FluentValidation;
using SmartEM.Application.Utils.ErrorTemplates;

namespace SmartEM.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty);
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty);
        RuleFor(x => x.JobTitle)
           .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty);
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty);
    }
}
