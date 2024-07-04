using CleanArch.eCode.Infrastructure.Auth.Permissions;
using CleanArch.eCode.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.eCode.Infrastructure.Auth;

public static class Startup
{
    public static IServiceCollection AddPermissions(this IServiceCollection services) =>
        services
            .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
            .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

    public static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
        services.AddScoped<ICurrentUser, CurrentUser>();
}