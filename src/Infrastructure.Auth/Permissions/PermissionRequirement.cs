using Microsoft.AspNetCore.Authorization;

namespace CleanArch.eCode.Infrastructure.Auth.Permissions;

internal class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; private set; } = permission;
}