using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEM.Application.Features.Authentication.ChangePassword;
using SmartEM.Application.Features.Authentication.CustomLogin;
using SmartEM.Application.Features.Authentication.LogOut;
using SmartEM.Application.Features.Authentication.MicrosoftLogin;
using SmartEM.Application.Features.Authentication.RefreshToken;
using SmartEM.Application.Features.Authentication.ResetPassword;
using SmartEM.Application.Features.Authentication.SendPasswordResetToken;
using SmartEM.Infrastructure.Utils;
using SmartEMApi.Extensions;
using System.Security.Claims;

namespace SmartEMApi.Controllers;

[Route("api/authentication")]
public class AuthenticationController : BaseController
{
    private const string REFRESH_TOKEN = "somadiap-refresh-token";
    public AuthenticationController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(CustomLoginCommand request)
    {
        var response = await Mediator.Send(request);
        if (response.IsSuccessful)
        {
            AddToCookie(REFRESH_TOKEN, response.Data);
            return LeanLoginResponse.ReturnLoginDto(response.Data)
            .ResponseResult();
        }
        return response.ResponseResult();

    }

    [Authorize(AuthenticationSchemes = InfrastructureConstants.MICROSOFT_AUTHENTICATION)]
    [HttpPost("login/sso")]
    public async Task<IActionResult> MicrosoftLogin()
    {
        var request = new MicrosoftLoginCommand
        {
            Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value,
            Name = User.FindFirst("name")!.Value,
            ProviderId = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")!.Value
        };
        var response = await Mediator.Send(request);
        if (response.IsSuccessful)
        {
            AddToCookie(REFRESH_TOKEN, response.Data);
            return LeanLoginResponse.ReturnLoginDto(response.Data)
            .ResponseResult();
        }
        return response.ResponseResult();

    }

    [HttpPost("password-reset/token")]
    public async Task<IActionResult> RequestPasswordReset(string email)
    {
        var request = new SendPasswordResetTokenCommand { Email = email };
        var response = await Mediator.Send(request);
        return response.ResponseResult();
    }

    [HttpPut("password-reset")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand request)
    {
        var response = await Mediator.Send(request);
        if (response.IsSuccessful)
        {
            AddToCookie(REFRESH_TOKEN, response.Data);
            return LeanLoginResponse.ReturnLoginDto(response.Data)
            .ResponseResult();
        }
        return response.ResponseResult();
    }

    [HttpGet("refresh-token")]
    public async Task<IActionResult> GetAccessToken()
    {
        var refreshToken = HttpContext.Request.Cookies[REFRESH_TOKEN]!;
        var response = await Mediator.Send(new GetAccessTokenCommand { RefreshToken = refreshToken });
        return response.ResponseResult();
    }

    [Authorize]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand request)
    {
        var response = await Mediator.Send(request);
        if (response.IsSuccessful)
        {
            AddToCookie(REFRESH_TOKEN, response.Data);
            return LeanLoginResponse.ReturnLoginDto(response.Data)
            .ResponseResult();
        }
        return response.ResponseResult();

    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogUserOut()
    {
        var refreshToken = HttpContext.Request.Cookies[REFRESH_TOKEN]!;
        var response = await Mediator.Send(new RevokeRefreshTokenCommand { RefreshToken = refreshToken });

        if (response.IsSuccessful)
            DeleteCookie(REFRESH_TOKEN);

        return response.ResponseResult();
    }

    [NonAction]
    protected void AddToCookie(string cookieName, LoginResponseDto loginData)
    {
        HttpContext.Response.Cookies.Append(cookieName, loginData.RefreshToken,
            new CookieOptions()
            {
                Expires = loginData.RefreshTokenExpiry,
                HttpOnly = true,
                Secure = HttpContext.Request.IsHttps, //TODO: Set to true in production
                SameSite = SameSiteMode.Lax
            });
    }

    [NonAction]
    protected void DeleteCookie(string cookieName)
    {
        if (HttpContext.Request.Cookies[cookieName] != null)
        {
            HttpContext.Response.Cookies.Delete(cookieName);
        }
    }
}
