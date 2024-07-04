using System.Reflection;

namespace CleanArch.eCode.Shared;

public class AppInfo
{
    private const string appPrefix = "CleanArch";

    public static Assembly[] AppAssemblies =>
        AppDomain.CurrentDomain
        .GetAssemblies()
        .Where(x => x.GetName().Name?.Contains(appPrefix) ?? false)
        .ToArray();

    public static bool IsDebugMode
    {
        get
        {
            bool isDebug = false;
#if DEBUG
            isDebug = true;
#endif
            return isDebug;
        }
    }
}
