using FluentValidation;
using MediatR;
using SmartEM.Application.Contracts.Infrastructure;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.SendPasswordResetToken
{
    public class SendPasswordResetTokenCommandHandler(
        IJWTProvider tokenProvider,
        IUnitOfWork unitOfWork,
        IValidator<SendPasswordResetTokenCommand> validator) : IRequestHandler<SendPasswordResetTokenCommand, OperationResponse>
    {
        private const int TIME_FRAME = 15;

        public async Task<OperationResponse> Handle(SendPasswordResetTokenCommand request, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return OperationResponse.FailedResponse(StatusCode.BadRequest)
                    .AddErrors(errors);
            }

            var user = await unitOfWork.UserRepository.GetFirstOrDefaultBySpec(u => u.Email.ToLower() == request.Email.ToLower()
                                                                            && u.ExternalLogins.Count == 0);

            if (user == null) return OperationResponse.FailedResponse(StatusCode.NotFound).AddError("Password user not found");

            if (user.LastPasswordResetToken > DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(TIME_FRAME)))
                return OperationResponse.FailedResponse(StatusCode.BadRequest).AddError("Token request limit exceeded");

            var passwordResetToken = tokenProvider.GetPasswordResetToken(request.Email);

            user.LastPasswordResetToken = DateTime.UtcNow;
            await unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

            var commitStatus = await unitOfWork.SaveChangesAsync();
            if (!commitStatus.IsSuccessful) return OperationResponse.FailedResponse(StatusCode.InternalServerError).AddError("Failed to update user");

            return OperationResponse.SuccessfulResponse();
        }
    }
}
