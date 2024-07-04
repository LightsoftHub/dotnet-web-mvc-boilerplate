using Microsoft.AspNetCore.Authorization;

namespace CleanArch.eCode.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string policy) => Policy = policy;
}