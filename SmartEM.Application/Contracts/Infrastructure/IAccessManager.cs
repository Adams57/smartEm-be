using SmartEM.Application.Utils;
using SmartEM.Domain.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEM.Application.Contracts.Infrastructure;

public interface IAccessManager
{
    Task<OperationResponse<User>> AuthenticateAsync(string email, string password);
    string HashPassword(string plainPassword);
    Task<bool> AdminAccountSeeded();
    bool VerifyPassword(string hashed, string plainPassword);
    OperationResponse<string> CreateNewPassword(string newPassword, string oldPassword);
    Task DeleteInvalidRefreshTokens(ICollection<RefreshToken> refreshTokens);
}
