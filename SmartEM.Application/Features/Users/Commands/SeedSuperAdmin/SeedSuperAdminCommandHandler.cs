using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartEM.Application.Contracts.Infrastructure;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Features.Users.Commands.CreateUser;
using SmartEM.Application.Utils;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Application.Features.Users.Commands.SeedSuperAdmin
{
    public class SeedSuperAdminCommandHandler(
        IAccessManager accessManager,
        IValidator<CreateUserCommand> validator,
        IUnitOfWork unitOfWork,
        DefaultUserConfiguration applicationSettings,
        ILogger<SeedSuperAdminCommandHandler> logger) : IRequestHandler<SeedSuperAdminCommand, OperationResponse>
    {
        private readonly ILogger _logger = logger;

        public async Task<OperationResponse> Handle(SeedSuperAdminCommand request, CancellationToken cancellationToken)
        {
            var superAdminSeeded = await accessManager.AdminAccountSeeded();
            if (superAdminSeeded)
            {
                _logger.LogInformation("Super admin account already seeded.");
                return OperationResponse.SuccessfulResponse();
            }
            var createUser = new CreateUserCommand
            {
                Email = applicationSettings.Email,
                FirstName = applicationSettings.FirstName,
                LastName = applicationSettings.LastName,
                AccountType = AccountTypes.SuperAdmin,
                JobTitle = null!
            };
            var handler = new CreateUserCommandHandler(validator, applicationSettings, accessManager, unitOfWork);

            _logger.LogInformation("Seeding super admin account with email: {Email}", createUser.Email);
            var response = await handler.Handle(createUser, cancellationToken);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("Super admin account seeded successfully.");
            }
            else
            {
                _logger.LogError($"Failed to seed super admin account. Errors: {string.Join(", ", response.Errors)}");
            }
            return response;
        }
    }
}
