using SmartEM.Application.Utils;

namespace SmartEM.Application.Features.Authentication.CustomLogin;

public class LeanLoginResponse
{
    public static OperationResponse<LeanLoginResponse> ReturnLoginDto(LoginResponseDto responseDto)
    {
        return OperationResponse<LeanLoginResponse>.SuccessfulResponse(new LeanLoginResponse
        {
            AccessToken = responseDto.AccessToken,
            AccountType = responseDto.AccountType,
            Email = responseDto.Email,
            FirstName = responseDto.FirstName,
            LastName = responseDto.LastName,
            UserId = responseDto.UserId
        });
    }
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string AccountType { get; set; } = default!;
    public string AccessToken { get; set; } = default!;
}
