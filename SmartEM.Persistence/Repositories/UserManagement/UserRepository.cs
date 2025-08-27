using SmartEM.Application.Contracts.Persistence.UserManagement;
using SmartEM.Application.Features.Users.Queries.FetchUser;
using SmartEM.Application.Utils;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Persistence.Repositories.UserManagement;

public class UserRepository(SmartEMDbContext dbContext) : PaginationService<User, Guid>(dbContext), IUserRepository
{
    public async Task<PaginatedResponse<FetchUserDto>> GetPaginatedFetchUserDto(PaginatedRequest paginatedRequest)
    {
        var userQuery = _dbContext.Users.Select(x => new FetchUserDto
        {
            AccountType = x.AccountType.ToString(),
            Email = x.Email,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Id = x.Id,
            SequentialId = x.SequentialId
        });
        return await PaginateResponse<FetchUserDto>(userQuery, paginatedRequest);
    }
}
