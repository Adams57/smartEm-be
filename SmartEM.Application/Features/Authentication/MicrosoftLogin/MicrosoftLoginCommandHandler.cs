using FluentValidation;
using MediatR;
using SmartEM.Application.Contracts.Infrastructure;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Features.Authentication.CustomLogin;
using SmartEM.Application.Utils;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Application.Features.Authentication.MicrosoftLogin;

public class MicrosoftLoginCommandHandler(
    IValidator<MicrosoftLoginCommand> validator, 
    IAccessManager accessManager,
    IJWTProvider tokenProvider, 
    IUnitOfWork unitOfWork) : IRequestHandler<MicrosoftLoginCommand, OperationResponse<LoginResponseDto>>
{
    public async Task<OperationResponse<LoginResponseDto>> Handle(MicrosoftLoginCommand request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return OperationResponse<LoginResponseDto>.FailedResponse(StatusCode.BadRequest)
                .AddErrors(errors);
        }

        var user = await unitOfWork.UserRepository.GetFirstOrDefaultByPredicateIncluding(u => u.Email.Equals(request.Email),
                                    asNoTracking: true, u => u.RefreshTokens, u => u.ExternalLogins);
        if (user is not null && !user.ExternalLogins.Any(e => e.ProviderName == LoginProvider.Microsoft & e.ProviderId == request.ProviderId))
            return OperationResponse<LoginResponseDto>.FailedResponse(StatusCode.BadRequest)
                                                      .AddError("Email isn't linked to an external login");
        if (user is null)
        {
            var names = request.Name.Split(' ');
            user = User.New(request.Email, names[0], names[1], AccountTypes.User);
            await unitOfWork.UserRepository.AddAsync(user, cancellationToken);

            var externalLogin = new ExternalLogin(LoginProvider.Microsoft, request.ProviderId, user);
            await unitOfWork.ExternalLoginRepository.AddAsync(externalLogin, cancellationToken);

            var commitUser = await unitOfWork.SaveChangesAsync();
            if (!commitUser.IsSuccessful) return OperationResponse<LoginResponseDto>
                                                        .FailedResponse(StatusCode.InternalServerError)
                                                        .AddError("Failed to save user");
        }
        var accessToken = tokenProvider.GetAccessToken(user);
        var refreshToken = tokenProvider.GetRefreshToken().SetUser(user);
        user.LastLoginTimeStamp = DateTime.UtcNow;

        await accessManager.DeleteInvalidRefreshTokens(user.RefreshTokens);
        await unitOfWork.RefreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

        var commitStatus = await unitOfWork.SaveChangesAsync();
        if (!commitStatus.IsSuccessful) return OperationResponse<LoginResponseDto>
                                                    .FailedResponse(StatusCode.InternalServerError)
                                                    .AddError("Failed to log in user");

        var response = new LoginResponseDto(user, accessToken);
        return OperationResponse<LoginResponseDto>.SuccessfulResponse(response);

    }
}
