using Microsoft.AspNetCore.Identity;
using SmartEM.Application.Contracts.Infrastructure;
using SmartEM.Application.Contracts.Persistence;
using SmartEM.Domain.UserManagement;

namespace SmartEM.Application.Utils.SecurityProvider;

public class AccessManager : IAccessManager
{
    private readonly IUnitOfWork _unitOfWork;
    public AccessManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public Task<bool> AdminAccountSeeded()
    {
        return _unitOfWork.UserRepository.AnyAsync(u => u.AccountType == AccountTypes.SuperAdmin);
    }

    public async Task<OperationResponse<User>> AuthenticateAsync(string email, string password)
    {
        var user = await _unitOfWork.UserRepository.GetFirstOrDefaultByPredicateIncluding(u => u.Email.Equals(email),
                                    asNoTracking: true, u => u.RefreshTokens);
        if (user == null) return OperationResponse<User>.FailedResponse(StatusCode.NotFound).AddError("Invalid user or password");

        var passwordIsCorrect = VerifyPassword(user.PasswordHash!, password);
        if (!passwordIsCorrect) return OperationResponse<User>.FailedResponse(StatusCode.BadRequest).AddError("Invalid user or password");

        return OperationResponse<User>.SuccessfulResponse(user);
    }

    public OperationResponse<string> CreateNewPassword(string newPassword, string oldPassword)
    {
        var isSamePassword = VerifyPassword(oldPassword, newPassword);
        if (isSamePassword) return OperationResponse<string>.FailedResponse(StatusCode.BadRequest)
                                                           .AddError("Failed to change password");
        return OperationResponse<string>.SuccessfulResponse(HashPassword(newPassword));
    }

    public async Task DeleteInvalidRefreshTokens(ICollection<RefreshToken> refreshTokens)
    {
        var invalidRefreshTokens = refreshTokens.Where(r => r.IsRevoked);
        if (invalidRefreshTokens != null && invalidRefreshTokens.Any())
        {
            await _unitOfWork.RefreshTokenRepository.DeleteRangeAsync([.. invalidRefreshTokens]);
        }
    }

    public string HashPassword(string plainPassword)
    {
        return new PasswordHasher<User>().HashPassword(null!, plainPassword);
    }

    public bool VerifyPassword(string hashed, string plainPassword)
    {
        return new PasswordHasher<User>().VerifyHashedPassword(null!, hashed, plainPassword)
                                == PasswordVerificationResult.Success;
    }
}