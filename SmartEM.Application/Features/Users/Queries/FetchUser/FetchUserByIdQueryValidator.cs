using FluentValidation;
using SmartEM.Application.Utils.ErrorTemplates;

namespace SmartEM.Application.Features.Users.Queries.FetchUser;

public class FetchUserByIdQueryValidator : AbstractValidator<FetchUserByIdQuery>
{
    public FetchUserByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty);
    }
}
