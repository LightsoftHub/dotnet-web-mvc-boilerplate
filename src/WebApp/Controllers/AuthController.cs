using CleanArch.eCode.Shared.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.eCode.WebApp.Controllers;

public class AuthController(ICurrentUser currentUser) : Controller
{
    [DenyDirectAccess]
    public IActionResult GetToken()
    {
        return Ok(currentUser.AccessToken);
    }

    public async Task<IActionResult> Logout(string? returnUrl)
    {
        await SignOutAsync();

        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Login");
        }
    }

    private async Task SignOutAsync()
    {
        foreach (var cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
