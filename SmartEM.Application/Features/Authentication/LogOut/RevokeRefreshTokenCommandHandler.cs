using FluentValidation;
using MediatR;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.LogOut;

public class RevokeRefreshTokenCommandHandler(IUnitOfWork unitOfWork, IValidator<RevokeRefreshTokenCommand> validator) 
    : IRequestHandler<RevokeRefreshTokenCommand, OperationResponse>
{
    public async Task<OperationResponse> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
            return OperationResponse<Guid>.FailedResponse(StatusCode.BadRequest)
                .AddErrors(validationResult.Errors.Select(e => e.ErrorMessage).ToList());

        var refreshToken = await unitOfWork.RefreshTokenRepository.GetFirstOrDefaultBySpec(r => r.TokenString == request.RefreshToken, cancellationToken);
        if (refreshToken == null)
            return OperationResponse.SuccessfulResponse();

        if (!refreshToken.IsRevoked)
        {
            refreshToken.Revoke();
            await unitOfWork.RefreshTokenRepository.UpdateAsync(refreshToken);

            var commitStatus = await unitOfWork.SaveChangesAsync();
            if (!commitStatus.IsSuccessful) return OperationResponse.FailedResponse(StatusCode.InternalServerError)
                .AddError("Failed to sign out user");
        }

        return OperationResponse.SuccessfulResponse();
    }
}
