using CleanArch.eCode.HttpApi.Client;
using CleanArch.eCode.Infrastructure.Auth;
using CleanArch.eCode.WebApp.Infrastructure.Services;
using System.Reflection;

namespace CleanArch.eCode.WebApp;

public static class ServicesExtension
{
    private static readonly Assembly[] assemblies =
        [
            typeof(Program).Assembly,
            typeof(HttpApiClientModule).Assembly,
        ];

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddCurrentUser();
        services.AddPermissions();

        services.AddHttpClients(configuration);
        services.AddHttpServices(assemblies);

        services.AddScoped<ITokenProvider, MvcTokenProvider>();

        return services;
    }

    public static IApplicationBuilder ConfigurePipelines(this IApplicationBuilder builder, IConfiguration configuration) =>
    builder
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization()
        .UseWebSockets();
}
