namespace CleanArch.eCode.Shared.Authorization;

public interface ICurrentUser
{
    string? UserId { get; }

    string? UserName { get; }

    string? FirstName { get; }

    string? LastName { get; }

    string? FullName { get; }

    string? PhoneNumber { get; }

    string? Email { get; }

    string? AccessToken { get; }

    bool IsAuthenticated { get; }

    bool IsMasterUser { get; }

    bool IsInRole(string role);

    bool HasPermission(string permission);
}
