using CleanArch.eCode.Infrastructure.Auth;
using CleanArch.eCode.Shared.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CleanArch.eCode.Infrastructure.Auth;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public string? UserId => User?.GetUserId();

    public string? UserName => User?.GetUserName();

    public string? FirstName => User?.GetFirstName();

    public string? LastName => User?.GetLastName();

    public string? FullName => $"{FirstName} {LastName}";

    public string? PhoneNumber => User?.GetPhoneNumber();

    public string? Email => User?.GetEmail();

    public bool IsAuthenticated => User?.IsAuthenticated() is true;

    public bool IsMasterUser => DefaultUser.MASTER_USERS.Any(x => x == UserName);

    public string? AccessToken => User?.FindFirstValue(Light.Identity.ClaimTypes.AccessToken);

    public bool IsInRole(string role) => User?.IsInRole(role) is true;

    public bool HasPermission(string permission) =>
        User?.HasPermission(Light.Identity.ClaimTypes.Permission, permission) is true
        || IsMasterUser;
}