using Microsoft.AspNetCore.Http;
using SmartEM.Application.Contracts.Infrastructure;
using System.Security.Claims;

namespace SmartEM.Application.Utils;

public class UserIdentity : IUserIdentity
{
    public UserIdentity(IHttpContextAccessor httpContext)
    {
        Email = httpContext?.HttpContext?
            .User?
            .Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

        Guid.TryParse(httpContext?.HttpContext?
            .User?
            .Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value, out var id);

        Id = id;
    }
    public Guid Id { get; set; }
    public string? Email { get; set; }
}
