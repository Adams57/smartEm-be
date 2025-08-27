using FluentValidation;
using MediatR;
using SmartEM.Application.Contracts.Infrastructure;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Features.Authentication.CustomLogin;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.ChangePassword;

public class ChangePasswordCommandHandler(
    IValidator<ChangePasswordCommand> validation,
    IAccessManager accessManager, 
    IJWTProvider provider,
    IUserIdentity currentUser, 
    IUnitOfWork unitOfWork) : IRequestHandler<ChangePasswordCommand, OperationResponse<LoginResponseDto>>
{
    public async Task<OperationResponse<LoginResponseDto>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var validationResults = validation.Validate(request);
        if (!validationResults.IsValid)
            return OperationResponse<LoginResponseDto>.FailedResponse(StatusCode.BadRequest)
                .AddErrors(validationResults.Errors.Select(x => x.ErrorMessage));

        var user = await unitOfWork.UserRepository.GetByIdIncludeAsync(u => u.Id == currentUser.Id && u.ExternalLogins.Count == 0, u => u.RefreshTokens);
        if (user == null)
            return OperationResponse<LoginResponseDto>.FailedResponse(StatusCode.NotFound)
                .AddError("Password user not found");

        var passwordCorrect = accessManager.VerifyPassword(user.PasswordHash!, request.OldPassword);
        if (!passwordCorrect)
            return OperationResponse<LoginResponseDto>.FailedResponse(StatusCode.BadRequest)
                .AddError("Wrong password");

        var newPassword = accessManager.CreateNewPassword(request.NewPassword, user.PasswordHash!);
        if (!newPassword.IsSuccessful)
            return OperationResponse<LoginResponseDto>.FailedResponse(newPassword.Code)
                .AddErrors(newPassword.Errors);

        user.SetPassword(newPassword.Data);

        foreach (var token in user.RefreshTokens)
        {
            token.Revoke();
            await unitOfWork.RefreshTokenRepository.UpdateAsync(token);
        }

        var accessToken = provider.GetAccessToken(user);
        var refreshToken = provider.GetRefreshToken().SetUser(user);
        user.LastLoginTimeStamp = DateTime.UtcNow;

        await unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
        await unitOfWork.UserRepository.UpdateAsync(user);

        var commitStatus = await unitOfWork.SaveChangesAsync();
        if (!commitStatus.IsSuccessful)
            return OperationResponse<LoginResponseDto>.FailedResponse(StatusCode.InternalServerError)
                .AddError("Failed to change password");

        var response = new LoginResponseDto(user, accessToken);
        return OperationResponse<LoginResponseDto>.SuccessfulResponse(response);
    }
}
