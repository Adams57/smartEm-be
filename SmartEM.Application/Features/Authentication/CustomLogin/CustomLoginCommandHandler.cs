using FluentValidation;
using MediatR;
using SmartEM.Application.Contracts.Infrastructure;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.CustomLogin;

public class CustomLoginCommandHandler(
    IValidator<CustomLoginCommand> validator, 
    IAccessManager accessManager,
    IJWTProvider tokenProvider, 
    IUnitOfWork unitOfWork) : IRequestHandler<CustomLoginCommand, OperationResponse<LoginResponseDto>>
{
    public async Task<OperationResponse<LoginResponseDto>> Handle(CustomLoginCommand request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return OperationResponse<LoginResponseDto>.FailedResponse(StatusCode.BadRequest)
                .AddErrors(errors);
        }

        var authResponse = await accessManager.AuthenticateAsync(request.Email, request.Password);
        if (!authResponse.IsSuccessful) 
            return OperationResponse<LoginResponseDto>.FailedResponse(authResponse.Code)
                .AddErrors(authResponse.Errors);
        var user = authResponse.Data;

        var accessToken = tokenProvider.GetAccessToken(user);
        var refreshToken = tokenProvider.GetRefreshToken().SetUser(user);
        user.LastLoginTimeStamp = DateTime.UtcNow;

        await accessManager.DeleteInvalidRefreshTokens(user.RefreshTokens);
        await unitOfWork.RefreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

        var commitStatus = await unitOfWork.SaveChangesAsync();
        if (!commitStatus.IsSuccessful) 
            return OperationResponse<LoginResponseDto>.FailedResponse(StatusCode.InternalServerError)
                .AddError("Failed to log in user");
        var response = new LoginResponseDto(user, accessToken);
        return OperationResponse<LoginResponseDto>.SuccessfulResponse(response);
    }
}
