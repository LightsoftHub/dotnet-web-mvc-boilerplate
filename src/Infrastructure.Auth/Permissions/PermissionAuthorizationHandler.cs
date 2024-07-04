using CleanArch.eCode.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace CleanArch.eCode.Infrastructure.Auth.Permissions;

internal class PermissionAuthorizationHandler(ICurrentUser currentUser) :
    AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (currentUser.HasPermission(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        await Task.CompletedTask;
    }
}