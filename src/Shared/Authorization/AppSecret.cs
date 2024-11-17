namespace CleanArch.eCode.Shared.Authorization;

public class DefaultUser
{
    public const string USER_NAME = "super";

    public static List<string> MASTER_USERS => new()
    {
        USER_NAME,
        "minhvd"
    };
}

public class DefaultRole
{
    public const string NAME = "super";

    public static List<string> MASTER_ROLES => new()
    {
        NAME,
        "minhvd"
    };
}
