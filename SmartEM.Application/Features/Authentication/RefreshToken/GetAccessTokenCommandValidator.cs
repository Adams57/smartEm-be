using FluentValidation;
using SmartEM.Application.Utils.ErrorTemplates;

namespace SmartEM.Application.Features.Authentication.RefreshToken;

public class GetAccessTokenCommandValidator : AbstractValidator<GetAccessTokenCommand>
{
    public GetAccessTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage(ValidationMessages.NotNullOrEmpty);
    }
}
