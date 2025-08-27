using SmartEM.Application.Contracts.Persistence.UserManagement;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Persistence.Repositories.UserManagement;

public class ExternalLoginRepository : Repository<ExternalLogin>, IExternalLoginRepository
{
    public ExternalLoginRepository(SmartEMDbContext dbContext) : base(dbContext)
    {
    }
}
