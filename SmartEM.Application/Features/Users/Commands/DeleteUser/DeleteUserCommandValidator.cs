using FluentValidation;
using SmartEM.Application.Utils.ErrorTemplates;

namespace SmartEM.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage(ValidationMessages.CollectionCannotBeEmpty)
            .Must(x => x.All(g => g != Guid.Empty)).WithMessage(ValidationMessages.CollectionCannotContainEmptyValues);
    }
}
