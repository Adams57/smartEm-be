using SmartEM.Application.Contracts.Persistence.UserManagement;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Persistence.Repositories.UserManagement;

public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(SmartEMDbContext dbContext) : base(dbContext)
    {
    }
}
