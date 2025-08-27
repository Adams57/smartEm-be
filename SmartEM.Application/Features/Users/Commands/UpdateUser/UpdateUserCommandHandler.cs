using FluentValidation;
using MediatR;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Utils;
using SmartEM.Application.Utils.ErrorTemplates;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OperationResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateUserCommand> _validator;
    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateUserCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<OperationResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
            return OperationResponse.FailedResponse(StatusCode.BadRequest)
                .AddErrors(validationResult.Errors.Select(x => x.ErrorMessage));

        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
        if (user == null)
            return OperationResponse.FailedResponse(StatusCode.NotFound)
                .AddError(Errors.ReturnNotFound<User>("Id", request.Id));

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.JobTitle = request.JobTitle;

        await _unitOfWork.UserRepository.UpdateAsync(user);
        var commitStatus = await _unitOfWork.SaveChangesAsync();
        if (!commitStatus.IsSuccessful)
            return OperationResponse.FailedResponse(StatusCode.InternalServerError)
                .AddError(Errors.ReturnCommitFailed<User>(CommitOperation.Update));
        return OperationResponse.SuccessfulResponse();
    }
}
