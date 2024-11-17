using System.Reflection;
using System.Security.Claims;

namespace CleanArch.eCode.Infrastructure.Auth;

public class ClaimsStore
{
    private static IEnumerable<Claim>? _allClaims;

    public static IEnumerable<Claim> AllClaims
    {
        get
        {
            _allClaims ??= GetAllClaims();
            return _allClaims;
        }
        set { _allClaims = value; }
    }

    private static List<Claim> GetAllClaims()
    {
        var fromClass = typeof(Shared.Authorization.Permissions);

        var claims = new List<Claim>();

        // get classes in class
        var modules = fromClass.GetNestedTypes();

        foreach (var module in modules)
        {
            // get props in class
            var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (FieldInfo fi in fields)
            {
                var propertyValue = fi.GetValue(null);

                if (propertyValue != null)
                {
                    claims.Add(new Claim(Light.Identity.ClaimTypes.Permission, propertyValue.ToString() ?? string.Empty));
                }
                //TODO - take descriptions from description attribute
            }
        }

        return claims;
    }
}