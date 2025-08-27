using SmartEM.Application.Contracts.Persistence.UserManagement;
using SmartEM.Application.Utils;

namespace SmartEM.Application.Contracts.Persistence;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IExternalLoginRepository ExternalLoginRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    Task<OperationResponse> SaveChangesAsync();
}
