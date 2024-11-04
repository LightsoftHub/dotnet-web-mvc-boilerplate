using Microsoft.AspNetCore.Mvc;

namespace CleanArch.eCode.WebBlazor.Components.Account
{
    internal static class IdentityComponentsEndpointRouteBuilderExtensions
    {
        // These endpoints are required by the Identity Razor components defined in the /Components/Account/Pages directory of this project.
        public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            var accountGroup = endpoints.MapGroup("/account");

            accountGroup.MapGet("/logout", async (
                [FromServices] LoginService loginService,
                string returnUrl) =>
            {
                await loginService.LogoutAsync();

                return TypedResults.LocalRedirect($"~/{returnUrl}");
            });

            accountGroup.MapPost("/logout", async (
                [FromServices] LoginService loginService,
                [FromForm] string returnUrl) =>
            {
                await loginService.LogoutAsync();

                if (string.IsNullOrEmpty(returnUrl))
                {
                    return TypedResults.LocalRedirect($"/account/login");
                }

                return TypedResults.LocalRedirect($"~/{returnUrl}");
            });

            return accountGroup;
        }
    }
}
