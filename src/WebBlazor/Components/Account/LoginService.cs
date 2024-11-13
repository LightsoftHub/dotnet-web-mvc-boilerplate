using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using CleanArch.eCode.HttpApi.Client.Identity;
using Light.Contracts;

namespace CleanArch.eCode.WebBlazor.Components.Account;

public class LoginService(
    IHttpContextAccessor httpContextAccessor,
    TokenHttpService tokenHttpService)
{
    public async Task<Result> LoginAsync(string username, string password, bool rememberMe)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext == null) return Result.Error();

        var getToken = await tokenHttpService.GetTokenAsync(username, password);

        if (getToken.Succeeded)
        {
            var accessToken = getToken.Data.AccessToken;

            var claims = JwtExtensions.ReadClaims(accessToken);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.Now.AddHours(3),
                IsPersistent = rememberMe
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties);

            return Result.Success();
        }
        else
        {
            return Result.Error();
        }
    }

    public async Task LogoutAsync()
    {
        foreach (var cookie in httpContextAccessor.HttpContext!.Request.Cookies.Keys)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete(cookie);
        }

        await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
