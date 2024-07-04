using CleanArch.eCode.Shared.Authorization;
using Light.Identity;
using System.Reflection;

namespace CleanArch.eCode.WebApp.Areas.System.Extensions;

public class ClientClaimsExtensions
{
    public class AppResource
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public List<AppClaim> Claims { get; set; } = [];
    }

    public class AppClaim
    {
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public static IEnumerable<AppResource> GetAllResources()
    {
        var fromClass = typeof(Permissions);

        var permissions = new HashSet<AppResource>();

        // get classes in class
        var modules = fromClass.GetNestedTypes();

        foreach (var module in modules)
        {
            var appResource = new AppResource
            {
                Name = module.GetDisplayName() ?? module.Name,
                Description = module.GetDescription() ?? string.Empty,
            };

            // get props in class
            var fields = module
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(x => x != null);

            foreach (FieldInfo fi in fields)
            {
                if (fi == null)
                    continue;

                var propertyValue = fi.GetValue(null);

                if (propertyValue != null)
                {
                    appResource.Claims.Add(new AppClaim
                    {
                        Type = ClaimTypes.Permission,
                        Value = propertyValue.ToString() ?? string.Empty,
                        Name = fi.GetDisplayName() ?? fi.Name, //$"{module.Name}: {fi.Name}",
                        Description = fi.GetNameOfDisplay() ?? string.Empty
                    });
                }
                //TODO - take descriptions from description attribute
            }

            permissions.Add(appResource);
        }

        return permissions;
    }
}
