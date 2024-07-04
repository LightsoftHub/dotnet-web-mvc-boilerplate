using CleanArch.eCode.HttpApi.Client;
using System.Security.Claims;

namespace CleanArch.eCode.WebApp.Infrastructure.Services;

public class MvcTokenProvider(IHttpContextAccessor httpContextAccessor) :
    ITokenProvider
{
    public string? AccessToken => GetToken();

    public string? GetToken()
    {
        var token = httpContextAccessor.HttpContext?.User?.FindFirstValue(Light.Identity.ClaimTypes.AccessToken);

        //ArgumentNullException.ThrowIfNull(token);

        return token;
    }
}