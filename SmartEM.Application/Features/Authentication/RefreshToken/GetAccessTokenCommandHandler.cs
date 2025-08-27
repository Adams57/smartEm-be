using FluentValidation;
using MediatR;
using SmartEM.Application.Contracts.Infrastructure;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Features.Authentication.CustomLogin;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.RefreshToken;

public class GetAccessTokenCommandHandler(
    IUnitOfWork unitOfWork,
    IJWTProvider tokenProvider,
    IValidator<GetAccessTokenCommand> validator) : IRequestHandler<GetAccessTokenCommand, OperationResponse<LeanLoginResponse>>
{
    public async Task<OperationResponse<LeanLoginResponse>> Handle(GetAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
            return OperationResponse<LeanLoginResponse>.FailedResponse(StatusCode.BadRequest)
                .AddErrors(validationResult.Errors.Select(e => e.ErrorMessage).ToList());

        var refreshToken = await unitOfWork.RefreshTokenRepository.GetFirstOrDefaultBySpec(r => r.TokenString == request.RefreshToken);

        if (refreshToken == null)
            return OperationResponse<LeanLoginResponse>.FailedResponse(StatusCode.NotFound)
                .AddError("Refresh token not found");

        if (refreshToken.IsExpired())
        {
            refreshToken.Revoke();
            await unitOfWork.RefreshTokenRepository.UpdateAsync(refreshToken);

            var commitStatus = await unitOfWork.SaveChangesAsync();
            if (!commitStatus.IsSuccessful)
                return OperationResponse<LeanLoginResponse>.FailedResponse(StatusCode.InternalServerError)
                    .AddError("Failed to sign out user");
        }
        if (refreshToken.IsRevoked)
            return OperationResponse<LeanLoginResponse>.FailedResponse(StatusCode.Unauthorized)
                .AddError("Invalid refresh token, log in again");

        var user = await unitOfWork.UserRepository.GetByIdAsync(refreshToken.UserId);
        if (user == null)
            return OperationResponse<LeanLoginResponse>.FailedResponse(StatusCode.NotFound)
                .AddError("User not found");

        var accessToken = tokenProvider.GetAccessToken(user);
        return OperationResponse<LeanLoginResponse>.SuccessfulResponse(new LeanLoginResponse
        {
            AccessToken = accessToken,
            UserId = user.Id,
            AccountType = user.AccountType.ToString(),
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        });
    }
}
