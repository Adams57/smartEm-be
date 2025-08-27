using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Application.Contracts.Persistence.UserManagement;
using SmartEM.Application.Utils;
using SmartEM.Persistence.Repositories.UserManagement;

namespace SmartEM.Persistence;

public class UnitOfWork(SmartEMDbContext context, ILogger<UnitOfWork> logger) : IUnitOfWork
{
    public async Task<OperationResponse> SaveChangesAsync()
    {
        try
        {
            await context.SaveChangesAsync();
            return OperationResponse.SuccessfulResponse();
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Message.Contains("REFERENCE constraint"))
        {
            var errorMessage = "Entity cannot be deleted, it's currently in use by other records";
            _logger.LogWarning(errorMessage, ex);

            return OperationResponse.FailedResponse(StatusCode.BadRequest)
                .AddError(errorMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = "Error occured while saving changes.";
            _logger.LogWarning(errorMessage, ex);

            return OperationResponse.FailedResponse(StatusCode.InternalServerError)
                .AddError(errorMessage);
        }
    }

    private IUserRepository? _userRepository;
    public IUserRepository UserRepository =>
        _userRepository ??= new UserRepository(context);

    private IExternalLoginRepository? _externalLoginRepository;
    public IExternalLoginRepository ExternalLoginRepository =>
        _externalLoginRepository ??= new ExternalLoginRepository(context);

    private IRefreshTokenRepository? _refreshTokenRepository;
    public IRefreshTokenRepository RefreshTokenRepository =>
        _refreshTokenRepository ??= new RefreshTokenRepository(context);

    private readonly ILogger _logger = logger;
}
