using FluentValidation;
using MediatR;
using SmartEM.Application.Contracts.Infrastructure;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Features.Authentication.CustomLogin;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.ResetPassword;

public class ResetPasswordCommandHandler(
    IValidator<ResetPasswordCommand> validation, 
    IUnitOfWork unitOfWork,
    IJWTProvider tokenProvider, 
    IAccessManager accessManager) : IRequestHandler<ResetPasswordCommand, OperationResponse<LoginResponseDto>>
{
    public async Task<OperationResponse<LoginResponseDto>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = validation.Validate(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return OperationResponse<LoginResponseDto>.FailedResponse(StatusCode.BadRequest)
                .AddErrors(errors);
        }
        var isValidToken = ValidateToken(request.Token, out string email);

        if (!isValidToken)
            return OperationResponse<LoginResponseDto>.FailedResponse(StatusCode.BadRequest).AddError("Invalid token");

        var user = await unitOfWork.UserRepository.GetFirstOrDefaultByPredicateIncluding(u => u.Email.Equals(email)
                                              && u.ExternalLogins.Count() == 0, asNoTracking: true, u => u.RefreshTokens);
        if (user == null)
            return OperationResponse<LoginResponseDto>.FailedResponse(StatusCode.NotFound).AddError("Password user not found");

        var newPasswordResponse = accessManager.CreateNewPassword(request.Password, user.PasswordHash!);

        if (!newPasswordResponse.IsSuccessful)
            return OperationResponse<LoginResponseDto>.FailedResponse(newPasswordResponse.Code)
                .AddErrors(newPasswordResponse.Errors);

        user.SetPassword(newPasswordResponse.Data);

        foreach (var token in user.RefreshTokens)
        {
            token.Revoke();
            await unitOfWork.RefreshTokenRepository.UpdateAsync(token, cancellationToken);
        }

        var accessToken = tokenProvider.GetAccessToken(user);
        var refreshToken = tokenProvider.GetRefreshToken().SetUser(user);
        user.LastLoginTimeStamp = DateTime.UtcNow;

        await unitOfWork.RefreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

        var commitStatus = await unitOfWork.SaveChangesAsync();
        if (!commitStatus.IsSuccessful)
            return OperationResponse<LoginResponseDto>.FailedResponse(StatusCode.InternalServerError)
                .AddError("Failed to change password");

        var response = new LoginResponseDto(user, accessToken);
        return OperationResponse<LoginResponseDto>.SuccessfulResponse(response);

    }
    private bool ValidateToken(string token, out string email)
    {
        var tokenExpired = tokenProvider.CheckTokenExpiry(token);
        if (tokenExpired)
        {
            email = default!;
            return false;
        }
        email = tokenProvider.ReadClaimValue(token, "email")!;
        return true;
    }
}
