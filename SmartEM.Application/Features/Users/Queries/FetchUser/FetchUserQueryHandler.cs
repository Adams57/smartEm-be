using FluentValidation;
using MediatR;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Utils;
using SmartEM.Application.Utils.ErrorTemplates;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Application.Features.Users.Queries.FetchUser;

public class FetchUserQueryHandler : IRequestHandler<FetchUserByIdQuery, OperationResponse<FetchUserDto>>,
IRequestHandler<FetchUsersQuery, OperationResponse<PaginatedResponse<FetchUserDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<FetchUserByIdQuery> _validator;
    public FetchUserQueryHandler(IUnitOfWork unitOfWork, IValidator<FetchUserByIdQuery> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<OperationResponse<PaginatedResponse<FetchUserDto>>> Handle(FetchUsersQuery request, CancellationToken cancellationToken)
    {
        var paginatedUsers = await _unitOfWork.UserRepository.GetPaginatedFetchUserDto(request?.PaginatedRequest!);
        return OperationResponse<PaginatedResponse<FetchUserDto>>.SuccessfulResponse(paginatedUsers);
    }

    public async Task<OperationResponse<FetchUserDto>> Handle(FetchUserByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
            return OperationResponse<FetchUserDto>.FailedResponse(StatusCode.BadRequest).AddErrors(validationResult.Errors.Select(e => e.ErrorMessage));

        var user = await _unitOfWork.UserRepository.GetByIdIncludeAsync(u => u.Id == request.Id);
        if (user is null)
            return OperationResponse<FetchUserDto>.FailedResponse(StatusCode.NotFound)
                .AddError(Errors.ReturnNotFound<User>("Id", request.Id));

        var userDto = new FetchUserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            AccountType = user.AccountType.ToString(),
        };

        return OperationResponse<FetchUserDto>.SuccessfulResponse(userDto);
    }
}
