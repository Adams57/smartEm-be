using SmartEM.Application.Features.Users.Queries.FetchUser;
using SmartEM.Application.Utils;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Application.Contracts.Persistence.UserManagement;

public interface IUserRepository : IPaginationService<User, Guid>
{
    Task<PaginatedResponse<FetchUserDto>> GetPaginatedFetchUserDto(PaginatedRequest paginatedRequest);
}
