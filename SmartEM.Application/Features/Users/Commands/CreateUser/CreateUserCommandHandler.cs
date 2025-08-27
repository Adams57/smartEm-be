using FluentValidation;
using MediatR;
using SmartEM.Application.Contracts.Infrastructure;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Utils;
using SmartEM.Application.Utils.ErrorTemplates;
using SmartEM.Domain.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartEM.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler(
    IValidator<CreateUserCommand> validator, 
    DefaultUserConfiguration defaultUserConfiguration,
    IAccessManager accessManager, IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, OperationResponse<Guid>>
{
    public async Task<OperationResponse<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return OperationResponse<Guid>.FailedResponse(StatusCode.BadRequest)
                .AddErrors(errors);
        }

        var userExists = await unitOfWork.UserRepository.AnyAsync(u => u.Email.Equals(request.Email), cancellationToken);
        if (userExists) return OperationResponse<Guid>.FailedResponse(StatusCode.BadRequest)
            .AddError(Errors.ReturnAlreadyExists<User>("Email", request.Email));

        var user = User.New(request.Email, request.FirstName, request.LastName, request.AccountType, request.JobTitle!);
        user.SetPassword(accessManager.HashPassword(defaultUserConfiguration.DefaultPassword));

        await unitOfWork.UserRepository.AddAsync(user);
        var commitStatus = await unitOfWork.SaveChangesAsync();

        if (!commitStatus.IsSuccessful) return OperationResponse<Guid>.FailedResponse(StatusCode.InternalServerError)
            .AddError(Errors.ReturnCommitFailed<User>(CommitOperation.Create));

        return OperationResponse<Guid>.SuccessfulResponse(user.Id);
    }
}
