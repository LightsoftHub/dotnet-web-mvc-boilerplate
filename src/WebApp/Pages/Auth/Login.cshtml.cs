using CleanArch.eCode.WebApp.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CleanArch.eCode.WebApp.Pages.Auth;

[AllowAnonymous]
public class LoginModel(TokenHttpService tokenHttpService) : PageModel
{
    [BindProperty]
    [Required]
    public string UserName { get; set; } = default!;

    [BindProperty]
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [BindProperty]
    public bool RememberMe { get; set; }

    public void OnGet()
    {
    }

    public async Task OnPost(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        var getToken = await tokenHttpService.GetTokenAsync(UserName, Password);

        if (getToken.Succeeded)
        {
            var claims = JwtExtensions.ReadClaims(getToken.Data.AccessToken);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.Now.AddHours(3),
                IsPersistent = RememberMe
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

            LocalRedirect(returnUrl);
        }
        else
        {
            ModelState.AddModelError("", getToken.Message);

            foreach (var error in getToken.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
