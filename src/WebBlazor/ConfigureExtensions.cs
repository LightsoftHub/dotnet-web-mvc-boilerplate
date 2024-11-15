using CleanArch.eCode.HttpApi.Client;
using CleanArch.eCode.Infrastructure.Auth;
using CleanArch.eCode.WebBlazor.Components.Account;
using CleanArch.eCode.WebBlazor.Components.Shared.Settings;
using CleanArch.eCode.WebBlazor.Components.Shared.Spinner;
using CleanArch.eCode.WebBlazor.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;

namespace CleanArch.eCode.WebBlazor;

public static class ServicesExtension
{
    private static readonly Assembly[] assemblies =
        [
            typeof(Program).Assembly,
            typeof(HttpApiClientModule).Assembly,
        ];

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClients(configuration);
        services.AddHttpServices(assemblies);

        services.AddAuth();

        services.AddFluentUIDemoServices();

        services.AddScoped<FuncService>();
        services.AddScoped<SpinnerService>();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddCurrentUser();
        services.AddPermissions();

        services
           .AddAuthentication(options =>
           {
               options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
               options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           })
           .AddCookie(options =>
           {
               options.AccessDeniedPath = new PathString("/access-denied");
               //options.Cookie.Name = "Cookie";
               options.Cookie.HttpOnly = true;
               options.ExpireTimeSpan = TimeSpan.FromHours(1);
               options.LoginPath = new PathString("/account/login");
               options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
               options.SlidingExpiration = true;
           });

        services.AddAuthorization();

        services.AddCascadingAuthenticationState();

        services.AddScoped<IdentityRedirectManager>();

        services.AddScoped<ITokenProvider, MvcTokenProvider>();

        services.AddScoped<LoginService>();

        return services;
    }

    public static IApplicationBuilder ConfigurePipelines(this IApplicationBuilder builder, IConfiguration configuration) =>
    builder
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization()
        .UseWebSockets();
}
