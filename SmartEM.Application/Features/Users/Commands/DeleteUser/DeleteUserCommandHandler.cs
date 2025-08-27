using FluentValidation;
using MediatR;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Utils;
using SmartEM.Application.Utils.ErrorTemplates;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteUserCommand> _validator;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IValidator<DeleteUserCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<OperationResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
            return OperationResponse.FailedResponse(StatusCode.BadRequest)
                .AddErrors(validationResult.Errors.Select(x => x.ErrorMessage));

        var users = await _unitOfWork.UserRepository.GetAllByPredicate(u => request.Ids.Any(r => r == u.Id), false);
        if (!users.Any())
            return OperationResponse
                .FailedResponse(StatusCode.NotFound)
                .AddError(Errors.ReturnNotFoundManyIds<User, Guid>("Id", request.Ids));


        await _unitOfWork.UserRepository.DeleteRangeAsync([.. users]);
        var commitStatus = await _unitOfWork.SaveChangesAsync();

        if (!commitStatus.IsSuccessful)
            return commitStatus;

        return OperationResponse.SuccessfulResponse();
    }
}
